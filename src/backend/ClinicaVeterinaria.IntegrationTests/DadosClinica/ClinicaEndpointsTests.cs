using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.DadosClinica;

/// <summary>
/// I092 — GET /clinica (EP-009) e PUT /clinica (EP-010), casos IT-EP009-01 e 02 e
/// IT-EP010-01 a 05 da Especificação de Testes de Integração (I030), mais os 401 e 403
/// previstos no contrato da API v02.
/// As tabelas Clinica e ConfiguracaoSistema são dados de referência que o Respawn não repõe
/// (I091): por isso, cada teste que altera a clínica repõe no fim os dados lidos no início.
/// Horário da I061: segunda a sexta 09:00–13:00 e 14:00–19:00; sábado 09:00–13:00.
/// </summary>
public sealed class ClinicaEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
  public ClinicaEndpointsTests(IntegracaoFixture fixture)
      : base(fixture)
  {
  }

  // ----- EP-009 -----

  [Fact(DisplayName = "IT-EP009-01 — configuração existente: 200 e o ClinicaDto coincide com a persistência")]
  public async Task ObterCoincideComPersistencia()
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

    using var resposta = await cliente.GetAsync("/clinica");

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
    var raiz = documento.RootElement;

    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

    var clinica = await db.Clinica.AsNoTracking().SingleAsync();
    var periodosAtivos = await db.HorariosFuncionamento.AsNoTracking().CountAsync(h => h.Ativo);

    Assert.Equal(clinica.Nome, raiz.GetProperty("nome").GetString());
    Assert.Equal(clinica.Email, raiz.GetProperty("email").GetString());
    Assert.Equal(clinica.Telefone, raiz.GetProperty("telefone").GetString());
    Assert.True(raiz.GetProperty("duracaoConsultaMinutos").GetInt32() > 0);
    Assert.Equal(periodosAtivos, raiz.GetProperty("horarioFuncionamento").GetArrayLength());
  }

  [Fact(DisplayName = "IT-EP009-02 — dia com dois períodos: 200 e ambos são devolvidos corretamente")]
  public async Task ObterDevolveDoisPeriodosNoMesmoDia()
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

    using var resposta = await cliente.GetAsync("/clinica");

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());

    var segunda = documento.RootElement.GetProperty("horarioFuncionamento").EnumerateArray()
        .Where(p => p.GetProperty("diaSemana").GetString() == "Segunda")
        .Select(p => $"{p.GetProperty("abertura").GetString()}-{p.GetProperty("fecho").GetString()}");

    Assert.Equal("09:00-13:00|14:00-19:00", string.Join('|', segunda));
  }

  // ----- EP-010 -----

  [Fact(DisplayName = "IT-EP010-01 — dados e horário válidos: 200 e os dados ficam persistidos (relidos em EP-009)")]
  public async Task AtualizarDadosValidos()
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");
    var original = await LerClinicaAsync(cliente);

    try
    {
      var pedido = original.DeepClone().AsObject();
      pedido["nome"] = "Clínica Veterinária G08 (teste I092)";
      pedido["telefone"] = "229111222";
      pedido["duracaoConsultaMinutos"] = 45;
      pedido["horarioFuncionamento"] = new JsonArray(
          Periodo("Terca", "10:00", "18:00"),
          Periodo("Sabado", "09:00", "12:00"));

      using var resposta = await cliente.PutAsJsonAsync("/clinica", pedido);

      Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

      var relida = await LerClinicaAsync(cliente);

      Assert.Equal("Clínica Veterinária G08 (teste I092)", (string?)relida["nome"]);
      Assert.Equal("229111222", (string?)relida["telefone"]);
      Assert.Equal(45, (int)relida["duracaoConsultaMinutos"]!);
      Assert.Equal("Terca 10:00-18:00|Sabado 09:00-12:00", Periodos(relida));
    }
    finally
    {
      await ReporAsync(cliente, original);
    }
  }

  [Theory(DisplayName = "IT-EP010-02 — campos inválidos devolvem 400 e o estado anterior mantém-se")]
  [InlineData("nome", "")]
  [InlineData("nif", "123456780")]
  [InlineData("telefone", "abc")]
  [InlineData("email", "geral-sem-arroba")]
  [InlineData("duracaoConsultaMinutos", "0")]
  [InlineData("diaSemana", "Feriado")]
  [InlineData("abertura", "9h")]
  public async Task CamposInvalidosDevolvem400(string campo, string valor)
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");
    var original = await LerClinicaAsync(cliente);

    try
    {
      var pedido = original.DeepClone().AsObject();

      switch (campo)
      {
        case "duracaoConsultaMinutos":
          pedido[campo] = int.Parse(valor, CultureInfo.InvariantCulture);
          break;
        case "diaSemana":
        case "abertura":
          pedido["horarioFuncionamento"]![0]![campo] = valor;
          break;
        default:
          pedido[campo] = valor;
          break;
      }

      using var resposta = await cliente.PutAsJsonAsync("/clinica", pedido);

      await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
      Assert.Equal(original.ToJsonString(), (await LerClinicaAsync(cliente)).ToJsonString());
    }
    finally
    {
      await ReporAsync(cliente, original);
    }
  }

  [Theory(DisplayName = "IT-EP010-03 — hora de fecho igual ou anterior à de abertura devolve 400 e o horário mantém-se")]
  [InlineData("14:00", "14:00")]
  [InlineData("14:00", "13:00")]
  public async Task FechoNaoPosteriorDevolve400(string abertura, string fecho)
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");
    var original = await LerClinicaAsync(cliente);

    try
    {
      var pedido = original.DeepClone().AsObject();
      pedido["horarioFuncionamento"] = new JsonArray(Periodo("Quarta", abertura, fecho));

      using var resposta = await cliente.PutAsJsonAsync("/clinica", pedido);

      await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
      Assert.Equal(original.ToJsonString(), (await LerClinicaAsync(cliente)).ToJsonString());
    }
    finally
    {
      await ReporAsync(cliente, original);
    }
  }

  [Fact(DisplayName = "IT-EP010-04 — períodos sobrepostos no mesmo dia devolvem 400 e o horário mantém-se")]
  public async Task PeriodosSobrepostosDevolvem400()
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");
    var original = await LerClinicaAsync(cliente);

    try
    {
      var pedido = original.DeepClone().AsObject();
      pedido["horarioFuncionamento"] = new JsonArray(
          Periodo("Quinta", "09:00", "13:00"),
          Periodo("Quinta", "12:30", "18:00"));

      using var resposta = await cliente.PutAsJsonAsync("/clinica", pedido);

      await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
      Assert.Equal(original.ToJsonString(), (await LerClinicaAsync(cliente)).ToJsonString());
    }
    finally
    {
      await ReporAsync(cliente, original);
    }
  }

  [Fact(DisplayName = "IT-EP010-05 — dois períodos não sobrepostos no mesmo dia: 200 e ambos ficam persistidos")]
  public async Task DoisPeriodosNaoSobrepostos()
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");
    var original = await LerClinicaAsync(cliente);

    try
    {
      var pedido = original.DeepClone().AsObject();
      pedido["horarioFuncionamento"] = new JsonArray(
          Periodo("Sexta", "14:00", "18:00"),
          Periodo("Sexta", "08:30", "12:30"));

      using var resposta = await cliente.PutAsJsonAsync("/clinica", pedido);

      Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
      Assert.Equal("Sexta 08:30-12:30|Sexta 14:00-18:00", Periodos(await LerClinicaAsync(cliente)));

      using var scope = Fixture.Factory.Services.CreateScope();
      var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

      Assert.Equal(2, await db.HorariosFuncionamento.AsNoTracking().CountAsync(h => h.Ativo && h.DiaSemana == 5));
    }
    finally
    {
      await ReporAsync(cliente, original);
    }
  }

  // ----- 401 e 403 (contrato da API v02) -----

  [Theory(DisplayName = "I092 — EP-009 e EP-010: perfis que não são Administrador recebem 403 e nada é alterado")]
  [InlineData("Rececionista")]
  [InlineData("Veterinario")]
  [InlineData("AuxiliarVeterinario")]
  [InlineData("Cliente")]
  public async Task OutrosPerfisRecebem403(string perfil)
  {
    using var admin = await Fixture.CriarClienteAutenticadoAsync("Administrador");
    var original = await LerClinicaAsync(admin);

    var pedido = original.DeepClone().AsObject();
    pedido["nome"] = "Alteração sem permissão";

    using var cliente = await Fixture.CriarClienteAutenticadoAsync(perfil);

    using var obter = await cliente.GetAsync("/clinica");
    using var atualizar = await cliente.PutAsJsonAsync("/clinica", pedido);

    await AssertErroAsync(obter, HttpStatusCode.Forbidden);
    await AssertErroAsync(atualizar, HttpStatusCode.Forbidden);
    Assert.Equal(original.ToJsonString(), (await LerClinicaAsync(admin)).ToJsonString());
  }

  [Fact(DisplayName = "I092 — EP-009 e EP-010: sem token devolvem 401 e nada é alterado")]
  public async Task SemTokenDevolve401()
  {
    using var admin = await Fixture.CriarClienteAutenticadoAsync("Administrador");
    var original = await LerClinicaAsync(admin);

    var pedido = original.DeepClone().AsObject();
    pedido["nome"] = "Alteração sem token";

    using var cliente = Fixture.Factory.CreateClient();

    using var obter = await cliente.GetAsync("/clinica");
    using var atualizar = await cliente.PutAsJsonAsync("/clinica", pedido);

    await AssertErroAsync(obter, HttpStatusCode.Unauthorized);
    await AssertErroAsync(atualizar, HttpStatusCode.Unauthorized);
    Assert.Equal(original.ToJsonString(), (await LerClinicaAsync(admin)).ToJsonString());
  }

  // ----- Auxiliares -----

  private static JsonObject Periodo(string diaSemana, string abertura, string fecho) => new()
  {
    ["diaSemana"] = diaSemana,
    ["abertura"] = abertura,
    ["fecho"] = fecho
  };

  private static string Periodos(JsonObject clinica) =>
      string.Join('|', clinica["horarioFuncionamento"]!.AsArray()
          .Select(p => $"{(string?)p!["diaSemana"]} {(string?)p["abertura"]}-{(string?)p["fecho"]}"));

  private static async Task<JsonObject> LerClinicaAsync(HttpClient cliente)
  {
    using var resposta = await cliente.GetAsync("/clinica");

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

    return JsonNode.Parse(await resposta.Content.ReadAsStringAsync())!.AsObject();
  }

  private static async Task ReporAsync(HttpClient cliente, JsonObject original)
  {
    using var resposta = await cliente.PutAsJsonAsync("/clinica", original);

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
  }

  /// <summary>Formato único de erro da infraestrutura (I058): <c>code</c> igual ao código HTTP.</summary>
  private static async Task AssertErroAsync(HttpResponseMessage resposta, HttpStatusCode esperado)
  {
    Assert.Equal(esperado, resposta.StatusCode);

    var corpo = await resposta.Content.ReadAsStringAsync();
    Assert.False(string.IsNullOrWhiteSpace(corpo), $"A resposta {(int)esperado} não tem corpo.");

    using var documento = JsonDocument.Parse(corpo);
    Assert.Equal((int)esperado, documento.RootElement.GetProperty("code").GetInt32());
  }
}
