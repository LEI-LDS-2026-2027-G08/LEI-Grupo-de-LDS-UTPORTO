using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.Utilizadores;

/// <summary>
/// I092 — GET /utilizadores (EP-004), GET /utilizadores/{id} (EP-005) e POST /utilizadores (EP-006),
/// casos IT-EP004-01 a 03, IT-EP005-01 e 02 e IT-EP006-01 a 05 da Especificação de Testes de
/// Integração (I030), mais os 401 e 403 previstos no contrato da API v02.
/// Dados de demonstração da I061: Administrador, Rececionistas Rita (ativa) e Rui (bloqueado),
/// Veterinários Ana e Bruno, Auxiliares Carla (com apoio) e Diogo, e Clientes.
/// </summary>
public sealed class FuncionariosEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
  private const string EmailVetAna = "vet.ana@clinica-g08.test";
  private const string EmailClienteJoao = "cliente.joao@clinica-g08.test";

  public FuncionariosEndpointsTests(IntegracaoFixture fixture)
      : base(fixture)
  {
  }

  // ----- EP-004 -----

  [Fact(DisplayName = "IT-EP004-01 — Administrador lista os funcionários: 200 com envelope paginado e sem contas de Cliente")]
  public async Task ListarDevolveEnvelopePaginado()
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

    using var resposta = await cliente.GetAsync("/utilizadores");

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
    var raiz = documento.RootElement;
    var itens = raiz.GetProperty("itens").EnumerateArray().ToList();

    Assert.Equal(1, raiz.GetProperty("pagina").GetInt32());
    Assert.Equal(20, raiz.GetProperty("tamanho").GetInt32());
    Assert.Equal(await ContarFuncionariosAsync(), raiz.GetProperty("total").GetInt32());
    Assert.NotEmpty(itens);
    Assert.All(itens, item => Assert.NotEqual("Cliente", item.GetProperty("perfil").GetString()));
    Assert.All(itens, item => Assert.False(item.TryGetProperty("passwordHash", out _)));
  }

  [Fact(DisplayName = "IT-EP004-02 — perfil=Veterinario&estado=Ativo devolve 200 só com Veterinários ativos")]
  public async Task FiltroPorPerfilEEstado()
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

    using var resposta = await cliente.GetAsync("/utilizadores?perfil=Veterinario&estado=Ativo");

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
    var itens = documento.RootElement.GetProperty("itens").EnumerateArray().ToList();

    Assert.NotEmpty(itens);
    Assert.All(itens, item =>
    {
      Assert.Equal("Veterinario", item.GetProperty("perfil").GetString());
      Assert.Equal("Ativo", item.GetProperty("estado").GetString());
    });
    Assert.Contains(itens, item => item.GetProperty("email").GetString() == EmailVetAna);
    Assert.Equal(itens.Count, documento.RootElement.GetProperty("total").GetInt32());
  }

  [Theory(DisplayName = "IT-EP004-03 — perfil ou estado fora da enumeração devolve 400")]
  [InlineData("/utilizadores?perfil=Gestor")]
  [InlineData("/utilizadores?estado=Suspenso")]
  public async Task FiltroInvalidoDevolve400(string url)
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

    using var resposta = await cliente.GetAsync(url);

    await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
  }

  // ----- EP-005 -----

  [Fact(DisplayName = "IT-EP005-01 — funcionário existente devolve 200 com o UtilizadorDto correto e sem hash")]
  public async Task ObterFuncionarioExistente()
  {
    var id = await ObterIdAsync(EmailVetAna);

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

    using var resposta = await cliente.GetAsync($"/utilizadores/{id}");

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
    var raiz = documento.RootElement;

    Assert.Equal(id, raiz.GetProperty("id").GetInt32());
    Assert.Equal(EmailVetAna, raiz.GetProperty("email").GetString());
    Assert.Equal("Veterinario", raiz.GetProperty("perfil").GetString());
    Assert.Equal("Ativo", raiz.GetProperty("estado").GetString());
    Assert.False(string.IsNullOrWhiteSpace(raiz.GetProperty("nome").GetString()));
    Assert.True(raiz.TryGetProperty("criadoEm", out _));
    Assert.False(raiz.TryGetProperty("passwordHash", out _));
  }

  [Fact(DisplayName = "IT-EP005-02 — identificador inexistente (ou de uma conta de Cliente) devolve 404")]
  public async Task ObterInexistenteOuClienteDevolve404()
  {
    var idInexistente = await ObterIdInexistenteAsync();
    var idCliente = await ObterIdAsync(EmailClienteJoao);

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

    using var inexistente = await cliente.GetAsync($"/utilizadores/{idInexistente}");
    using var contaCliente = await cliente.GetAsync($"/utilizadores/{idCliente}");

    await AssertErroAsync(inexistente, HttpStatusCode.NotFound);
    await AssertErroAsync(contaCliente, HttpStatusCode.NotFound);
  }

  // ----- EP-006 -----

  [Fact(DisplayName = "IT-EP006-01 — dados válidos e e-mail livre devolvem 201 com Location e a conta fica persistida e ativa")]
  public async Task CriarFuncionarioValido()
  {
    var email = DadosTeste.EmailUnico("rui.lopes");

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

    using var resposta = await cliente.PostAsJsonAsync("/utilizadores", Pedido(email, "AuxiliarVeterinario", podeRegistarApoio: true));

    Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
    var raiz = documento.RootElement;
    var id = raiz.GetProperty("id").GetInt32();

    Assert.NotNull(resposta.Headers.Location);
    Assert.EndsWith($"/utilizadores/{id}", resposta.Headers.Location!.ToString(), StringComparison.Ordinal);
    Assert.Equal(email, raiz.GetProperty("email").GetString());
    Assert.Equal("AuxiliarVeterinario", raiz.GetProperty("perfil").GetString());
    Assert.Equal("Ativo", raiz.GetProperty("estado").GetString());
    Assert.True(raiz.GetProperty("podeRegistarApoio").GetBoolean());
    Assert.False(raiz.TryGetProperty("passwordHash", out _));

    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

    var utilizador = await db.Utilizadores.AsNoTracking().SingleAsync(u => u.Id == id);
    var funcionario = await db.Funcionarios.AsNoTracking().SingleAsync(f => f.UtilizadorId == id);

    Assert.Equal(email, utilizador.Email);
    Assert.Equal("AuxiliarVeterinario", utilizador.Perfil);
    Assert.Equal("Ativo", utilizador.Estado);
    Assert.NotEqual("Temp#2026x", utilizador.PasswordHash);
    Assert.True(funcionario.PodeRegistarApoio);
  }

  [Theory(DisplayName = "IT-EP006-02 — campo obrigatório inválido ou palavra-passe fraca devolve 400 e nenhuma conta é criada")]
  [InlineData("", "Rececionista", "Temp#2026x")]
  [InlineData("Rui Lopes", "", "Temp#2026x")]
  [InlineData("Rui Lopes", "Gestor", "Temp#2026x")]
  [InlineData("Rui Lopes", "Rececionista", "fraca")]
  [InlineData("Rui Lopes", "Rececionista", "")]
  public async Task DadosInvalidosDevolvem400(string nome, string perfil, string passwordInicial)
  {
    var email = DadosTeste.EmailUnico("rui.lopes");

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

    using var resposta = await cliente.PostAsJsonAsync("/utilizadores", new
    {
      nome,
      email,
      telefone = "934567890",
      perfil,
      podeRegistarApoio = false,
      passwordInicial
    });

    await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    Assert.Equal(0, await ContarContasAsync(email));
  }

  [Fact(DisplayName = "IT-EP006-02 — e-mail com formato inválido devolve 400 e nenhuma conta é criada")]
  public async Task EmailInvalidoDevolve400()
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

    using var resposta = await cliente.PostAsJsonAsync("/utilizadores", Pedido("rui.lopes@mail", "Rececionista"));

    await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    Assert.Equal(0, await ContarContasAsync("rui.lopes@mail"));
  }

  [Fact(DisplayName = "IT-EP006-03 — e-mail já registado (sem distinguir maiúsculas) devolve 409 sem duplicar a conta")]
  public async Task EmailRegistadoDevolve409()
  {
    var total = await ContarTodasAsContasAsync();

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

    using var mesmo = await cliente.PostAsJsonAsync("/utilizadores", Pedido(EmailVetAna, "Rececionista"));
    using var maiusculas = await cliente.PostAsJsonAsync("/utilizadores", Pedido(EmailVetAna.ToUpperInvariant(), "Rececionista"));

    await AssertErroAsync(mesmo, HttpStatusCode.Conflict);
    await AssertErroAsync(maiusculas, HttpStatusCode.Conflict);
    Assert.Equal(1, await ContarContasAsync(EmailVetAna));
    Assert.Equal(total, await ContarTodasAsContasAsync());
  }

  [Fact(DisplayName = "IT-EP006-04 — perfil Cliente devolve 422 e nenhuma conta é criada")]
  public async Task PerfilClienteDevolve422()
  {
    var email = DadosTeste.EmailUnico("cliente.novo");

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

    using var resposta = await cliente.PostAsJsonAsync("/utilizadores", Pedido(email, "Cliente"));

    await AssertErroAsync(resposta, HttpStatusCode.UnprocessableEntity);
    Assert.Equal(0, await ContarContasAsync(email));
  }

  [Theory(DisplayName = "IT-EP006-05 — podeRegistarApoio=true num perfil que não é Auxiliar devolve 422 e nenhuma conta é criada")]
  [InlineData("Rececionista")]
  [InlineData("Veterinario")]
  [InlineData("Administrador")]
  public async Task ApoioForaDoAuxiliarDevolve422(string perfil)
  {
    var email = DadosTeste.EmailUnico("apoio");

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

    using var resposta = await cliente.PostAsJsonAsync("/utilizadores", Pedido(email, perfil, podeRegistarApoio: true));

    await AssertErroAsync(resposta, HttpStatusCode.UnprocessableEntity);
    Assert.Equal(0, await ContarContasAsync(email));
  }

  // ----- 401 e 403 (contrato da API v02) -----

  [Theory(DisplayName = "I092 — EP-004 a EP-006: perfis que não são Administrador recebem 403 e nada é criado")]
  [InlineData("Rececionista")]
  [InlineData("Veterinario")]
  [InlineData("AuxiliarVeterinario")]
  [InlineData("Cliente")]
  public async Task OutrosPerfisRecebem403(string perfil)
  {
    var id = await ObterIdAsync(EmailVetAna);
    var email = DadosTeste.EmailUnico("sem.permissao");

    using var cliente = await Fixture.CriarClienteAutenticadoAsync(perfil);

    using var listar = await cliente.GetAsync("/utilizadores");
    using var obter = await cliente.GetAsync($"/utilizadores/{id}");
    using var criar = await cliente.PostAsJsonAsync("/utilizadores", Pedido(email, "Rececionista"));

    await AssertErroAsync(listar, HttpStatusCode.Forbidden);
    await AssertErroAsync(obter, HttpStatusCode.Forbidden);
    await AssertErroAsync(criar, HttpStatusCode.Forbidden);
    Assert.Equal(0, await ContarContasAsync(email));
  }

  [Fact(DisplayName = "I092 — EP-004 a EP-006: sem token devolvem 401 e nada é criado")]
  public async Task SemTokenDevolve401()
  {
    var id = await ObterIdAsync(EmailVetAna);
    var email = DadosTeste.EmailUnico("sem.token");

    using var cliente = Fixture.Factory.CreateClient();

    using var listar = await cliente.GetAsync("/utilizadores");
    using var obter = await cliente.GetAsync($"/utilizadores/{id}");
    using var criar = await cliente.PostAsJsonAsync("/utilizadores", Pedido(email, "Rececionista"));

    await AssertErroAsync(listar, HttpStatusCode.Unauthorized);
    await AssertErroAsync(obter, HttpStatusCode.Unauthorized);
    await AssertErroAsync(criar, HttpStatusCode.Unauthorized);
    Assert.Equal(0, await ContarContasAsync(email));
  }

  // ----- Auxiliares -----

  private static object Pedido(string email, string perfil, bool podeRegistarApoio = false) => new
  {
    nome = "Rui Lopes",
    email,
    telefone = "934567890",
    perfil,
    podeRegistarApoio,
    passwordInicial = "Temp#2026x"
  };

  /// <summary>Formato único de erro da infraestrutura (I058): <c>code</c> igual ao código HTTP.</summary>
  private static async Task AssertErroAsync(HttpResponseMessage resposta, HttpStatusCode esperado)
  {
    Assert.Equal(esperado, resposta.StatusCode);

    var corpo = await resposta.Content.ReadAsStringAsync();
    Assert.False(string.IsNullOrWhiteSpace(corpo), $"A resposta {(int)esperado} não tem corpo.");

    using var documento = JsonDocument.Parse(corpo);
    Assert.Equal((int)esperado, documento.RootElement.GetProperty("code").GetInt32());
  }

  private async Task<int> ObterIdAsync(string email)
  {
    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

    return await db.Utilizadores.AsNoTracking().Where(u => u.Email == email).Select(u => u.Id).SingleAsync();
  }

  private async Task<int> ObterIdInexistenteAsync()
  {
    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

    return (await db.Utilizadores.AsNoTracking().MaxAsync(u => (int?)u.Id) ?? 0) + 1000;
  }

  private async Task<int> ContarContasAsync(string email)
  {
    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

    // A collation da base (SQL Server) não distingue maiúsculas de minúsculas.
    return await db.Utilizadores.AsNoTracking().CountAsync(u => u.Email == email);
  }

  private async Task<int> ContarTodasAsContasAsync()
  {
    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

    return await db.Utilizadores.AsNoTracking().CountAsync();
  }

  private async Task<int> ContarFuncionariosAsync()
  {
    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

    return await (
            from u in db.Utilizadores.AsNoTracking()
            join f in db.Funcionarios.AsNoTracking() on u.Id equals f.UtilizadorId
            where u.Perfil != "Cliente"
            select u.Id)
        .CountAsync();
  }
}
