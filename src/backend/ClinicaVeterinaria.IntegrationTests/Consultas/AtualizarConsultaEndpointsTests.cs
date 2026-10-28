using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.Consultas;

/// <summary>
/// I074 — EP-026 (PUT /consultas/{id}) com API real e SQL Server.
/// Casos IT-EP026-01 a IT-EP026-08 da especificação de integração.
/// </summary>
public sealed class AtualizarConsultaEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
  private const string EmailRececao = "rececao@clinica-g08.test";
  private const string EmailVetAna = "vet.ana@clinica-g08.test";
  private const string EmailVetBruno = "vet.bruno@clinica-g08.test";

  public AtualizarConsultaEndpointsTests(
      IntegracaoFixture fixture)
      : base(fixture)
  {
  }

  [Theory(DisplayName = "IT-EP026-01 — Pendente ou Confirmada é alterada para horário livre, com novo ETag")]
  [InlineData(EmailVetBruno, "Pendente")]
  [InlineData(EmailVetAna, "Confirmada")]
  public async Task AtualizacaoValida(
      string emailVeterinarioAtual,
      string estadoEsperado)
  {
    var marcacao = await ObterMarcacaoAsync(
        emailVeterinarioAtual,
        estadoEsperado);

    var novoVeterinario =
        await ObterUtilizadorIdAsync(
            emailVeterinarioAtual == EmailVetAna
                ? EmailVetBruno
                : EmailVetAna);

    var novaData =
        InicioUtc(
            QuartaFeiraFutura(),
            estadoEsperado == "Pendente" ? 10 : 11,
            0);

    using var cliente =
        await ClienteDeAsync(EmailRececao);

    using var antes =
        await cliente.GetAsync(
            $"/consultas/{marcacao.Id}");

    Assert.Equal(
        HttpStatusCode.OK,
        antes.StatusCode);

    var etagAnterior =
        Assert.IsType<EntityTagHeaderValue>(
            antes.Headers.ETag).Tag;

    using var resposta =
        await EnviarPutAsync(
            cliente,
            marcacao.Id,
            Corpo(
                novoVeterinario,
                novaData,
                "Consulta reagendada"));

    Assert.Equal(
        HttpStatusCode.OK,
        resposta.StatusCode);

    var novoETag =
        Assert.IsType<EntityTagHeaderValue>(
            resposta.Headers.ETag).Tag;

    Assert.NotEqual(
        etagAnterior,
        novoETag);

    using var documento =
        await LerAsync(resposta);

    var raiz = documento.RootElement;

    Assert.Equal(
        Texto(novaData),
        raiz.GetProperty("dataHora").GetString());

    Assert.Equal(
        novoVeterinario,
        raiz.GetProperty("veterinario")
            .GetProperty("id")
            .GetInt32());

    Assert.Equal(
        "Consulta reagendada",
        raiz.GetProperty("motivo").GetString());

    Assert.Equal(
        estadoEsperado,
        raiz.GetProperty("estado").GetString());

    using var persistida =
        await cliente.GetAsync(
            $"/consultas/{marcacao.Id}");

    Assert.Equal(
        HttpStatusCode.OK,
        persistida.StatusCode);

    Assert.Equal(
        novoETag,
        persistida.Headers.ETag!.Tag);

    using var documentoPersistido =
        await LerAsync(persistida);

    Assert.Equal(
        Texto(novaData),
        documentoPersistido.RootElement
            .GetProperty("dataHora")
            .GetString());
  }

  [Fact(DisplayName = "IT-EP026-02 — dados inválidos devolvem 400")]
  public async Task DadosInvalidosDevolvem400()
  {
    var marcacao =
        await ObterMarcacaoAsync(
            EmailVetBruno,
            "Pendente");

    using var cliente =
        await ClienteDeAsync(EmailRececao);

    using var resposta =
        await cliente.PutAsJsonAsync(
            $"/consultas/{marcacao.Id}",
            new
            {
              dataHora = Texto(
                    InicioUtc(
                        QuartaFeiraFutura(),
                        10,
                        0))
            });

    await AssertErroAsync(
        resposta,
        HttpStatusCode.BadRequest);
  }

  [Fact(DisplayName = "IT-EP026-03 — consulta inexistente devolve 404")]
  public async Task ConsultaInexistenteDevolve404()
  {
    using var cliente =
        await ClienteDeAsync(EmailRececao);

    using var resposta =
        await EnviarPutAsync(
            cliente,
            999999,
            Corpo(
                await ObterUtilizadorIdAsync(
                    EmailVetAna),
                InicioUtc(
                    QuartaFeiraFutura(),
                    10,
                    0)));

    await AssertErroAsync(
        resposta,
        HttpStatusCode.NotFound);
  }

  [Fact(DisplayName = "IT-EP026-04 — If-Match desatualizado devolve 412 e não altera")]
  public async Task IfMatchDesatualizadoDevolve412()
  {
    var marcacao =
        await ObterMarcacaoAsync(
            EmailVetBruno,
            "Pendente");

    var vetAna =
        await ObterUtilizadorIdAsync(
            EmailVetAna);

    var dia =
        QuartaFeiraFutura();

    using var cliente =
        await ClienteDeAsync(EmailRececao);

    using var leituraInicial =
        await cliente.GetAsync(
            $"/consultas/{marcacao.Id}");

    var etagAntigo =
        leituraInicial.Headers.ETag!.Tag;

    var primeiraData =
        InicioUtc(dia, 10, 0);

    using var primeira =
        await EnviarPutAsync(
            cliente,
            marcacao.Id,
            Corpo(
                vetAna,
                primeiraData,
                "Primeira alteração"));

    Assert.Equal(
        HttpStatusCode.OK,
        primeira.StatusCode);

    using var segunda =
        await EnviarPutAsync(
            cliente,
            marcacao.Id,
            Corpo(
                vetAna,
                InicioUtc(dia, 11, 0),
                "Alteração indevida"),
            etagAntigo);

    await AssertErroAsync(
        segunda,
        HttpStatusCode.PreconditionFailed);

    using var atual =
        await cliente.GetAsync(
            $"/consultas/{marcacao.Id}");

    using var documento =
        await LerAsync(atual);

    Assert.Equal(
        Texto(primeiraData),
        documento.RootElement
            .GetProperty("dataHora")
            .GetString());

    Assert.Equal(
        "Primeira alteração",
        documento.RootElement
            .GetProperty("motivo")
            .GetString());
  }

  [Fact(DisplayName = "IT-EP026-05 — consulta Realizada devolve 422")]
  public async Task RealizadaDevolve422()
  {
    var marcacao =
        await ObterMarcacaoAsync(
            EmailVetAna,
            "Realizada");

    using var cliente =
        await ClienteDeAsync(EmailRececao);

    using var resposta =
        await EnviarPutAsync(
            cliente,
            marcacao.Id,
            Corpo(
                await ObterUtilizadorIdAsync(
                    EmailVetAna),
                InicioUtc(
                    QuartaFeiraFutura(),
                    10,
                    0)));

    await AssertErroAsync(
        resposta,
        HttpStatusCode.UnprocessableEntity);
  }

  [Fact(DisplayName = "IT-EP026-06 — consulta Cancelada devolve 422")]
  public async Task CanceladaDevolve422()
  {
    var marcacao =
        await ObterMarcacaoAsync(
            EmailVetBruno,
            "Cancelada");

    using var cliente =
        await ClienteDeAsync(EmailRececao);

    using var resposta =
        await EnviarPutAsync(
            cliente,
            marcacao.Id,
            Corpo(
                await ObterUtilizadorIdAsync(
                    EmailVetBruno),
                InicioUtc(
                    QuartaFeiraFutura(),
                    10,
                    0)));

    await AssertErroAsync(
        resposta,
        HttpStatusCode.UnprocessableEntity);
  }

  [Fact(DisplayName = "IT-EP026-07 — novo horário indisponível devolve 422")]
  public async Task HorarioIndisponivelDevolve422()
  {
    var vetAna =
        await ObterUtilizadorIdAsync(
            EmailVetAna);

    var bobi =
        await ObterAnimalIdAsync("Bobi");

    var kiko =
        await ObterAnimalIdAsync("Kiko");

    var dia =
        QuartaFeiraFutura();

    using var cliente =
        await ClienteDeAsync(EmailRececao);

    var inicioPrimeira =
        InicioUtc(dia, 10, 0);

    using var primeira =
        await cliente.PostAsJsonAsync(
            "/consultas",
            CorpoCriar(
                bobi,
                vetAna,
                inicioPrimeira));

    Assert.Equal(
        HttpStatusCode.Created,
        primeira.StatusCode);

    using var segunda =
        await cliente.PostAsJsonAsync(
            "/consultas",
            CorpoCriar(
                kiko,
                vetAna,
                InicioUtc(dia, 11, 0)));

    Assert.Equal(
        HttpStatusCode.Created,
        segunda.StatusCode);

    using var documentoSegunda =
        await LerAsync(segunda);

    var idSegunda =
        documentoSegunda.RootElement
            .GetProperty("id")
            .GetInt32();

    using var sobreposta =
        await EnviarPutAsync(
            cliente,
            idSegunda,
            Corpo(
                vetAna,
                InicioUtc(dia, 10, 15)));

    await AssertErroAsync(
        sobreposta,
        HttpStatusCode.UnprocessableEntity);

    using var atual =
        await cliente.GetAsync(
            $"/consultas/{idSegunda}");

    using var documentoAtual =
        await LerAsync(atual);

    Assert.Equal(
        Texto(InicioUtc(dia, 11, 0)),
        documentoAtual.RootElement
            .GetProperty("dataHora")
            .GetString());
  }

  [Fact(DisplayName = "IT-EP026-08 — If-Match atual permite atualizar e emite novo ETag")]
  public async Task IfMatchAtualEAceite()
  {
    var marcacao =
        await ObterMarcacaoAsync(
            EmailVetBruno,
            "Pendente");

    var vetAna =
        await ObterUtilizadorIdAsync(
            EmailVetAna);

    using var cliente =
        await ClienteDeAsync(EmailRececao);

    using var leitura =
        await cliente.GetAsync(
            $"/consultas/{marcacao.Id}");

    var etagAtual =
        leitura.Headers.ETag!.Tag;

    using var resposta =
        await EnviarPutAsync(
            cliente,
            marcacao.Id,
            Corpo(
                vetAna,
                InicioUtc(
                    QuartaFeiraFutura(),
                    10,
                    0)),
            etagAtual);

    Assert.Equal(
        HttpStatusCode.OK,
        resposta.StatusCode);

    var novoETag =
        Assert.IsType<EntityTagHeaderValue>(
            resposta.Headers.ETag).Tag;

    Assert.NotEqual(
        etagAtual,
        novoETag);
  }

  [Fact(DisplayName = "I074 — EP-026 sem token devolve 401")]
  public async Task SemTokenDevolve401()
  {
    using var cliente =
        Fixture.Factory.CreateClient();

    using var resposta =
        await EnviarPutAsync(
            cliente,
            999999,
            Corpo(
                1,
                InicioUtc(
                    QuartaFeiraFutura(),
                    10,
                    0)));

    await AssertErroAsync(
        resposta,
        HttpStatusCode.Unauthorized);
  }

  [Theory(DisplayName = "I074 — EP-026 só pode ser usado pela Rececionista")]
  [InlineData("admin@clinica-g08.test")]
  [InlineData(EmailVetAna)]
  [InlineData("aux.carla@clinica-g08.test")]
  [InlineData("cliente.joao@clinica-g08.test")]
  public async Task OutrosPerfisRecebem403(
      string email)
  {
    var marcacao =
        await ObterMarcacaoAsync(
            EmailVetBruno,
            "Pendente");

    using var cliente =
        await ClienteDeAsync(email);

    using var resposta =
        await EnviarPutAsync(
            cliente,
            marcacao.Id,
            Corpo(
                await ObterUtilizadorIdAsync(
                    EmailVetAna),
                InicioUtc(
                    QuartaFeiraFutura(),
                    10,
                    0)));

    await AssertErroAsync(
        resposta,
        HttpStatusCode.Forbidden);
  }

  // ----- Auxiliares -----

  private sealed record MarcacaoDemo(
      int Id,
      byte[] RowVersion);

  private static Dictionary<string, object?> Corpo(
      int veterinarioId,
      DateTime dataHora,
      string? motivo = null) =>
      new()
      {
        ["veterinarioId"] = veterinarioId,
        ["dataHora"] = Texto(dataHora),
        ["motivo"] = motivo
      };

  private static Dictionary<string, object?> CorpoCriar(
      int animalId,
      int veterinarioId,
      DateTime dataHora) =>
      new()
      {
        ["animalId"] = animalId,
        ["veterinarioId"] = veterinarioId,
        ["dataHora"] = Texto(dataHora),
        ["motivo"] = "Teste de integração I074"
      };

  private static async Task<HttpResponseMessage> EnviarPutAsync(
      HttpClient cliente,
      int id,
      object corpo,
      string? ifMatch = null)
  {
    using var pedido =
        new HttpRequestMessage(
            HttpMethod.Put,
            $"/consultas/{id}")
        {
          Content = JsonContent.Create(corpo)
        };

    if (ifMatch is not null)
    {
      pedido.Headers.TryAddWithoutValidation(
          "If-Match",
          ifMatch);
    }

    return await cliente.SendAsync(pedido);
  }

  private static DateOnly QuartaFeiraFutura()
  {
    var hoje =
        DateOnly.FromDateTime(
            FusoHorarioClinica.ParaLocal(
                DateTime.UtcNow,
                FusoHorarioClinica.Obter()));

    var dia =
        hoje.AddDays(28);

    while (dia.DayOfWeek != DayOfWeek.Wednesday)
    {
      dia = dia.AddDays(1);
    }

    return dia;
  }

  private static DateTime InicioUtc(
      DateOnly dia,
      int hora,
      int minuto) =>
      FusoHorarioClinica.ParaUtc(
          dia,
          new TimeOnly(hora, minuto),
          FusoHorarioClinica.Obter())!.Value;

  private static string Texto(DateTime utc) =>
      utc.ToString(
          "yyyy-MM-dd'T'HH:mm:ss'Z'",
          CultureInfo.InvariantCulture);

  private static async Task<JsonDocument> LerAsync(
      HttpResponseMessage resposta) =>
      JsonDocument.Parse(
          await resposta.Content.ReadAsStringAsync());

  private static async Task AssertErroAsync(
      HttpResponseMessage resposta,
      HttpStatusCode esperado)
  {
    Assert.Equal(
        esperado,
        resposta.StatusCode);

    using var documento =
        await LerAsync(resposta);

    Assert.Equal(
        (int)esperado,
        documento.RootElement
            .GetProperty("code")
            .GetInt32());

    Assert.False(
        string.IsNullOrWhiteSpace(
            documento.RootElement
                .GetProperty("message")
                .GetString()));

    Assert.False(
        string.IsNullOrWhiteSpace(
            documento.RootElement
                .GetProperty("detail")
                .GetString()));
  }

  private async Task<HttpClient> ClienteDeAsync(
      string email)
  {
    using var scope =
        Fixture.Factory.Services.CreateScope();

    var authService =
        scope.ServiceProvider
            .GetRequiredService<IAuthService>();

    var jwtTokenService =
        scope.ServiceProvider
            .GetRequiredService<IJwtTokenService>();

    var utilizador =
        await authService.ObterAutenticadoAsync(
            await ObterUtilizadorIdAsync(email));

    var token =
        jwtTokenService.GerarToken(utilizador);

    var cliente =
        Fixture.Factory.CreateClient();

    cliente.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue(
            "Bearer",
            token.Token);

    return cliente;
  }

  private async Task<int> ObterUtilizadorIdAsync(
      string email)
  {
    using var scope =
        Fixture.Factory.Services.CreateScope();

    var db =
        scope.ServiceProvider
            .GetRequiredService<ClinicaDbContext>();

    return await db.Utilizadores
        .AsNoTracking()
        .Where(u => u.Email == email)
        .Select(u => u.Id)
        .SingleAsync();
  }

  private async Task<int> ObterAnimalIdAsync(
      string nome)
  {
    using var scope =
        Fixture.Factory.Services.CreateScope();

    var db =
        scope.ServiceProvider
            .GetRequiredService<ClinicaDbContext>();

    return await db.Pacientes
        .AsNoTracking()
        .Where(p => p.Nome == nome)
        .Select(p => p.Id)
        .SingleAsync();
  }

  private async Task<MarcacaoDemo> ObterMarcacaoAsync(
      string emailVeterinario,
      string estado)
  {
    var veterinarioId =
        await ObterUtilizadorIdAsync(
            emailVeterinario);

    using var scope =
        Fixture.Factory.Services.CreateScope();

    var db =
        scope.ServiceProvider
            .GetRequiredService<ClinicaDbContext>();

    return await db.Marcacoes
        .AsNoTracking()
        .Where(
            m =>
                m.VeterinarioId == veterinarioId
                && m.Estado == estado)
        .Select(
            m => new MarcacaoDemo(
                m.Id,
                m.RowVersion))
        .SingleAsync();
  }
}
