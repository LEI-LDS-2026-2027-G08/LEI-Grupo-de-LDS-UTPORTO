using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ClinicaVeterinaria.Api.Seguranca;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ClinicaVeterinaria.IntegrationTests.Autenticacao;

/// <summary>
/// I092 — POST /auth/login (EP-001), POST /auth/logout (EP-002) e GET /auth/me (EP-003),
/// casos IT-EP001-01 a 06, IT-EP002-01 a 03 e IT-EP003-02 a 04 da Especificação de Testes
/// de Integração (I030). O IT-EP003-01 está em AutenticacaoInfraestruturaTests (I091).
/// As palavras-passe dos dados de demonstração não estão no repositório: cada teste define
/// uma palavra-passe de teste na conta que usa; a base é reposta antes de cada teste.
/// </summary>
public sealed class AutenticacaoEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
  private const string PalavraPasseTeste = "Teste#I092-2026";
  private const string EmailRececao = "rececao@clinica-g08.test";
  private const string EmailBloqueada = "rececao.bloqueada@clinica-g08.test";
  private const string EmailVetAna = "vet.ana@clinica-g08.test";
  private const string EmailClienteJoao = "cliente.joao@clinica-g08.test";

  public AutenticacaoEndpointsTests(IntegracaoFixture fixture)
      : base(fixture)
  {
  }

  // ----- EP-001 -----

  [Fact(DisplayName = "IT-EP001-01 — conta ativa com credenciais válidas devolve 200 e JWT com sub, email, role, jti e exp")]
  public async Task LoginValidoDevolveTokenComClaims()
  {
    await DefinirPalavraPasseAsync(EmailRececao);

    using var cliente = Fixture.Factory.CreateClient();
    using var resposta = await cliente.PostAsJsonAsync("/auth/login", new { email = EmailRececao, password = PalavraPasseTeste });

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
    var raiz = documento.RootElement;
    var utilizador = raiz.GetProperty("utilizador");

    Assert.Equal(EmailRececao, utilizador.GetProperty("email").GetString());
    Assert.Equal("Rececionista", utilizador.GetProperty("perfil").GetString());
    Assert.True(raiz.GetProperty("expiraEm").GetDateTime() > DateTime.UtcNow);

    using var payload = LerPayload(raiz.GetProperty("token").GetString()!);
    var claims = payload.RootElement;

    Assert.Equal(utilizador.GetProperty("id").GetInt32().ToString(CultureInfo.InvariantCulture), claims.GetProperty("sub").GetString());
    Assert.Equal(EmailRececao, claims.GetProperty("email").GetString());
    Assert.Equal("Rececionista", claims.GetProperty("role").GetString());
    Assert.False(string.IsNullOrWhiteSpace(claims.GetProperty("jti").GetString()));
    Assert.True(claims.GetProperty("exp").GetInt64() > DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    Assert.False(claims.TryGetProperty("clienteId", out _));
  }

  [Theory(DisplayName = "IT-EP001-02 — e-mail ou palavra-passe em falta devolve 400 sem token")]
  [InlineData("", PalavraPasseTeste)]
  [InlineData(EmailRececao, "")]
  [InlineData(null, PalavraPasseTeste)]
  [InlineData(EmailRececao, null)]
  public async Task CamposEmFaltaDevolvem400(string? email, string? password)
  {
    using var cliente = Fixture.Factory.CreateClient();
    using var resposta = await cliente.PostAsJsonAsync("/auth/login", new { email, password });

    await AssertErroSemTokenAsync(resposta, HttpStatusCode.BadRequest);
  }

  [Theory(DisplayName = "IT-EP001-03 — palavra-passe incorreta (ou e-mail inexistente) devolve 401 sem token")]
  [InlineData(EmailRececao)]
  [InlineData("nao.existe@clinica-g08.test")]
  public async Task CredenciaisIncorretasDevolvem401(string email)
  {
    await DefinirPalavraPasseAsync(EmailRececao);

    using var cliente = Fixture.Factory.CreateClient();
    using var resposta = await cliente.PostAsJsonAsync("/auth/login", new { email, password = "Errada#2026x" });

    await AssertErroSemTokenAsync(resposta, HttpStatusCode.Unauthorized);
  }

  [Fact(DisplayName = "IT-EP001-04 — conta bloqueada com a palavra-passe certa devolve 403 sem token")]
  public async Task ContaBloqueadaDevolve403()
  {
    await DefinirPalavraPasseAsync(EmailBloqueada);

    using var cliente = Fixture.Factory.CreateClient();
    using var resposta = await cliente.PostAsJsonAsync("/auth/login", new { email = EmailBloqueada, password = PalavraPasseTeste });

    await AssertErroSemTokenAsync(resposta, HttpStatusCode.Forbidden);
  }

  [Fact(DisplayName = "IT-EP001-05 — conta Cliente válida devolve 200 e o JWT inclui clienteId")]
  public async Task LoginClienteIncluiClienteId()
  {
    await DefinirPalavraPasseAsync(EmailClienteJoao);
    var clienteIdEsperado = await ObterClienteIdAsync(EmailClienteJoao);

    using var cliente = Fixture.Factory.CreateClient();
    using var resposta = await cliente.PostAsJsonAsync("/auth/login", new { email = EmailClienteJoao, password = PalavraPasseTeste });

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
    using var payload = LerPayload(documento.RootElement.GetProperty("token").GetString()!);

    Assert.Equal("Cliente", payload.RootElement.GetProperty("role").GetString());
    Assert.Equal(clienteIdEsperado.ToString(CultureInfo.InvariantCulture), payload.RootElement.GetProperty("clienteId").GetString());
    Assert.Equal(clienteIdEsperado, documento.RootElement.GetProperty("utilizador").GetProperty("clienteId").GetInt32());
  }

  [Fact(DisplayName = "IT-EP001-06 — conta Veterinário válida devolve 200 e o JWT inclui veterinarioId")]
  public async Task LoginVeterinarioIncluiVeterinarioId()
  {
    await DefinirPalavraPasseAsync(EmailVetAna);
    var idVet = await ObterIdAsync(EmailVetAna);

    using var cliente = Fixture.Factory.CreateClient();
    using var resposta = await cliente.PostAsJsonAsync("/auth/login", new { email = EmailVetAna, password = PalavraPasseTeste });

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
    using var payload = LerPayload(documento.RootElement.GetProperty("token").GetString()!);

    Assert.Equal("Veterinario", payload.RootElement.GetProperty("role").GetString());
    Assert.Equal(idVet.ToString(CultureInfo.InvariantCulture), payload.RootElement.GetProperty("veterinarioId").GetString());
  }

  // ----- EP-002 -----

  [Fact(DisplayName = "IT-EP002-01 — logout com JWT válido devolve 204 e o jti fica revogado")]
  public async Task LogoutRevogaJti()
  {
    var token = await IniciarSessaoAsync(EmailRececao);
    using var payload = LerPayload(token);
    var jti = payload.RootElement.GetProperty("jti").GetString()!;
    var id = await ObterIdAsync(EmailRececao);

    using var cliente = CriarClienteComToken(token);
    using var resposta = await cliente.PostAsync("/auth/logout", null);

    Assert.Equal(HttpStatusCode.NoContent, resposta.StatusCode);

    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
    var revogado = await db.TokensRevogados.AsNoTracking().SingleAsync(t => t.Jti == jti);

    Assert.Equal(id, revogado.UtilizadorId);
    Assert.True(revogado.DataExpiracao > DateTime.UtcNow);
  }

  [Fact(DisplayName = "IT-EP002-02 — token revogado pelo logout devolve 401 num endpoint protegido")]
  public async Task TokenRevogadoDevolve401()
  {
    var token = await IniciarSessaoAsync(EmailRececao);

    using var cliente = CriarClienteComToken(token);

    using var antes = await cliente.GetAsync("/auth/me");
    Assert.Equal(HttpStatusCode.OK, antes.StatusCode);

    using var logout = await cliente.PostAsync("/auth/logout", null);
    Assert.Equal(HttpStatusCode.NoContent, logout.StatusCode);

    using var me = await cliente.GetAsync("/auth/me");
    using var clientes = await cliente.GetAsync("/clientes");
    using var segundoLogout = await cliente.PostAsync("/auth/logout", null);

    await AssertErroAsync(me, HttpStatusCode.Unauthorized);
    await AssertErroAsync(clientes, HttpStatusCode.Unauthorized);
    await AssertErroAsync(segundoLogout, HttpStatusCode.Unauthorized);
  }

  [Fact(DisplayName = "IT-EP002-03 — logout sem token ou com token inválido devolve 401")]
  public async Task LogoutSemTokenOuInvalidoDevolve401()
  {
    using var semToken = Fixture.Factory.CreateClient();
    using var invalido = CriarClienteComToken("isto.nao.e-um-jwt");

    using var respostaSemToken = await semToken.PostAsync("/auth/logout", null);
    using var respostaInvalido = await invalido.PostAsync("/auth/logout", null);

    await AssertErroAsync(respostaSemToken, HttpStatusCode.Unauthorized);
    await AssertErroAsync(respostaInvalido, HttpStatusCode.Unauthorized);
  }

  // ----- EP-003 (o IT-EP003-01 está em AutenticacaoInfraestruturaTests) -----

  [Fact(DisplayName = "IT-EP003-02 — /auth/me sem token devolve 401")]
  public async Task MeSemTokenDevolve401()
  {
    using var cliente = Fixture.Factory.CreateClient();
    using var resposta = await cliente.GetAsync("/auth/me");

    await AssertErroAsync(resposta, HttpStatusCode.Unauthorized);
  }

  [Fact(DisplayName = "IT-EP003-03 — /auth/me com JWT adulterado (perfil alterado, assinatura original) devolve 401")]
  public async Task MeComTokenAdulteradoDevolve401()
  {
    var token = await IniciarSessaoAsync(EmailRececao);
    var partes = token.Split('.');

    using var payload = LerPayload(token);
    var claims = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(payload.RootElement.GetRawText())!;
    claims["role"] = JsonSerializer.SerializeToElement("Administrador");

    var adulterado = $"{partes[0]}.{CodificarBase64Url(JsonSerializer.SerializeToUtf8Bytes(claims))}.{partes[2]}";

    using var cliente = CriarClienteComToken(adulterado);
    using var resposta = await cliente.GetAsync("/auth/me");

    await AssertErroAsync(resposta, HttpStatusCode.Unauthorized);
  }

  [Fact(DisplayName = "IT-EP003-04 — /auth/me com JWT expirado, assinado com a chave certa, devolve 401")]
  public async Task MeComTokenExpiradoDevolve401()
  {
    using var scope = Fixture.Factory.Services.CreateScope();

    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
    var opcoes = scope.ServiceProvider.GetRequiredService<IOptions<JwtOptions>>();

    // Emitido há 9 horas com a validade configurada de 8 horas: expirou há 1 hora.
    var geradorNoPassado = new JwtTokenService(opcoes, new RelogioFixo(DateTimeOffset.UtcNow.AddHours(-9)));
    var expirado = geradorNoPassado.GerarToken(await authService.ObterAutenticadoAsync(await ObterIdAsync(EmailRececao)));

    Assert.True(expirado.ExpiraEm < DateTime.UtcNow);

    using var cliente = CriarClienteComToken(expirado.Token);
    using var resposta = await cliente.GetAsync("/auth/me");

    await AssertErroAsync(resposta, HttpStatusCode.Unauthorized);
  }

  // ----- Auxiliares -----

  private async Task DefinirPalavraPasseAsync(string email)
  {
    using var scope = Fixture.Factory.Services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Utilizador>>();

    var utilizador = await db.Utilizadores.SingleAsync(u => u.Email == email);
    utilizador.PasswordHash = hasher.HashPassword(utilizador, PalavraPasseTeste);

    await db.SaveChangesAsync();
  }

  private async Task<string> IniciarSessaoAsync(string email)
  {
    await DefinirPalavraPasseAsync(email);

    using var cliente = Fixture.Factory.CreateClient();
    using var resposta = await cliente.PostAsJsonAsync("/auth/login", new { email, password = PalavraPasseTeste });

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());

    return documento.RootElement.GetProperty("token").GetString()!;
  }

  private HttpClient CriarClienteComToken(string token)
  {
    var cliente = Fixture.Factory.CreateClient();
    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    return cliente;
  }

  private async Task<int> ObterIdAsync(string email)
  {
    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

    return await db.Utilizadores.AsNoTracking().Where(u => u.Email == email).Select(u => u.Id).SingleAsync();
  }

  private async Task<int> ObterClienteIdAsync(string email)
  {
    var utilizadorId = await ObterIdAsync(email);

    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

    return await db.Clientes.AsNoTracking().Where(c => c.UtilizadorId == utilizadorId).Select(c => c.Id).SingleAsync();
  }

  private static JsonDocument LerPayload(string token)
  {
    var parte = token.Split('.')[1].Replace('-', '+').Replace('_', '/');
    parte = parte.PadRight(parte.Length + ((4 - (parte.Length % 4)) % 4), '=');

    return JsonDocument.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(parte)));
  }

  private static string CodificarBase64Url(byte[] bytes) =>
      Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');

  /// <summary>Formato único de erro da infraestrutura (I058): <c>code</c> igual ao código HTTP.</summary>
  private static async Task AssertErroAsync(HttpResponseMessage resposta, HttpStatusCode esperado)
  {
    Assert.Equal(esperado, resposta.StatusCode);

    var corpo = await resposta.Content.ReadAsStringAsync();
    Assert.False(string.IsNullOrWhiteSpace(corpo), $"A resposta {(int)esperado} não tem corpo.");

    using var documento = JsonDocument.Parse(corpo);
    Assert.Equal((int)esperado, documento.RootElement.GetProperty("code").GetInt32());
  }

  private static async Task AssertErroSemTokenAsync(HttpResponseMessage resposta, HttpStatusCode esperado)
  {
    await AssertErroAsync(resposta, esperado);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
    Assert.False(documento.RootElement.TryGetProperty("token", out _));
  }

  private sealed class RelogioFixo(DateTimeOffset agora) : TimeProvider
  {
    public override DateTimeOffset GetUtcNow() => agora;
  }
}
