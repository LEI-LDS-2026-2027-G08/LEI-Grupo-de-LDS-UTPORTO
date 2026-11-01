using ClinicaVeterinaria.Api.Seguranca;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace ClinicaVeterinaria.UnitTests.Seguranca;

/// <summary>
/// I063 — critério 4: conteúdo, algoritmo, validade e rejeição dos tokens. Casos UT-F001-023 a
/// UT-F001-034 da Especificação de Testes Unitários (I028, secção 7.1.2), revistos na I086.
/// </summary>
public class JwtTokenServiceTests
{
    // RELOGIO_JWT da I028 (com milissegundos).
    private static readonly DateTimeOffset Agora = new(2026, 10, 25, 10, 0, 0, 567, TimeSpan.Zero);

    [Fact(DisplayName = "UT-F001-023 — O token de um Cliente tem sub, email, role, clienteId e jti, sem veterinarioId")]
    public void TokenDeClienteTemAsClaimsDoContrato()
    {
        var servico = AjudaSeguranca.CriarTokenService(AjudaSeguranca.NovaChave(), new RelogioFixo(Agora));

        var emitido = servico.GerarToken(AjudaSeguranca.Utilizador(12, Perfis.Cliente, clienteId: 7));
        var token = new JsonWebToken(emitido.Token);

        Assert.Equal("12", token.GetClaim(ClaimsClinica.UtilizadorId).Value);
        Assert.Equal("teste12@clinica-g08.test", token.GetClaim(ClaimsClinica.Email).Value);
        Assert.Equal(Perfis.Cliente, token.GetClaim(ClaimsClinica.Perfil).Value);
        Assert.Equal("7", token.GetClaim(ClaimsClinica.ClienteId).Value);
        Assert.Equal(emitido.Jti, token.GetClaim(ClaimsClinica.Jti).Value);
        Assert.False(token.TryGetClaim(ClaimsClinica.VeterinarioId, out _));
    }

    [Fact(DisplayName = "UT-F001-024 — O token de um Veterinário tem veterinarioId e não tem clienteId")]
    public void TokenDeVeterinarioTemVeterinarioIdESemClienteId()
    {
        var servico = AjudaSeguranca.CriarTokenService(AjudaSeguranca.NovaChave(), new RelogioFixo(Agora));

        var token = new JsonWebToken(servico.GerarToken(AjudaSeguranca.Utilizador(4, Perfis.Veterinario)).Token);

        Assert.Equal("4", token.GetClaim(ClaimsClinica.VeterinarioId).Value);
        Assert.False(token.TryGetClaim(ClaimsClinica.ClienteId, out _));
    }

    [Fact(DisplayName = "UT-F001-025 — O token expira exatamente 8 horas depois, sem frações de segundo, e expiraEm é igual a exp")]
    public void TokenExpiraOitoHorasDepoisDaEmissao()
    {
        var servico = AjudaSeguranca.CriarTokenService(AjudaSeguranca.NovaChave(), new RelogioFixo(Agora));

        var emitido = servico.GerarToken(AjudaSeguranca.Utilizador(1, Perfis.Administrador));
        var token = new JsonWebToken(emitido.Token);

        var esperado = new DateTime(2026, 10, 25, 18, 0, 0, DateTimeKind.Utc);
        Assert.Equal(esperado, emitido.ExpiraEm);
        Assert.Equal(esperado, token.ValidTo);
    }

    [Fact(DisplayName = "UT-F001-026 — Dois tokens do mesmo utilizador têm jti diferentes")]
    public void CadaTokenTemUmJtiDiferente()
    {
        var servico = AjudaSeguranca.CriarTokenService(AjudaSeguranca.NovaChave(), new RelogioFixo(Agora));
        var utilizador = AjudaSeguranca.Utilizador(1, Perfis.Administrador);

        Assert.NotEqual(servico.GerarToken(utilizador).Jti, servico.GerarToken(utilizador).Jti);
    }

    [Fact(DisplayName = "UT-F001-027 — O cabeçalho do token indica o algoritmo HS256")]
    public void TokenUsaHs256()
    {
        var servico = AjudaSeguranca.CriarTokenService(AjudaSeguranca.NovaChave(), new RelogioFixo(Agora));

        var token = new JsonWebToken(servico.GerarToken(AjudaSeguranca.Utilizador(1, Perfis.Administrador)).Token);

        Assert.Equal(SecurityAlgorithms.HmacSha256, token.Alg);
    }

    [Fact(DisplayName = "UT-F001-028 — Um token emitido agora é aceite pela configuração da API e a claim role é lida com o nome curto")]
    public async Task TokenAtualEAceitePelaConfiguracaoDaApi()
    {
        var chave = AjudaSeguranca.NovaChave();
        var token = AjudaSeguranca.CriarTokenService(chave, TimeProvider.System)
            .GerarToken(AjudaSeguranca.Utilizador(1, Perfis.Administrador)).Token;

        var resultado = await ValidarComConfiguracaoDaApiAsync(chave, token);

        Assert.True(resultado.IsValid);
        Assert.Equal(Perfis.Administrador, resultado.ClaimsIdentity.FindFirst(ClaimsClinica.Perfil)?.Value);
    }

    [Fact(DisplayName = "UT-F001-029 — Um token emitido há 8 h 01 min é rejeitado por ter expirado")]
    public async Task TokenExpiradoERejeitado()
    {
        var chave = AjudaSeguranca.NovaChave();
        var token = TokenEmitidoHa(chave, TimeSpan.FromHours(8) + TimeSpan.FromMinutes(1));

        var resultado = await ValidarComConfiguracaoDaApiAsync(chave, token);

        Assert.IsType<SecurityTokenExpiredException>(resultado.Exception);
    }

    [Fact(DisplayName = "UT-F001-030 — Um token emitido há 7 h 59 min ainda é aceite (sem tolerância de relógio)")]
    public async Task TokenQuaseAExpirarAindaEAceite()
    {
        var chave = AjudaSeguranca.NovaChave();
        var token = TokenEmitidoHa(chave, TimeSpan.FromHours(8) - TimeSpan.FromMinutes(1));

        var resultado = await ValidarComConfiguracaoDaApiAsync(chave, token);

        Assert.True(resultado.IsValid);
    }

    [Fact(DisplayName = "UT-F001-031 — Um token assinado com outra chave de 256 bits é rejeitado")]
    public async Task TokenAssinadoComOutraChaveERejeitado()
    {
        var token = AjudaSeguranca.CriarTokenService(AjudaSeguranca.NovaChave(), TimeProvider.System)
            .GerarToken(AjudaSeguranca.Utilizador(1, Perfis.Administrador)).Token;

        var resultado = await ValidarComConfiguracaoDaApiAsync(AjudaSeguranca.NovaChave(), token);

        Assert.False(resultado.IsValid);
    }

    [Fact(DisplayName = "UT-F001-032 — Um token de Cliente com o payload de um Administrador e a assinatura original é rejeitado")]
    public async Task TokenAdulteradoERejeitado()
    {
        var chave = AjudaSeguranca.NovaChave();
        var token = AjudaSeguranca.CriarTokenService(chave, TimeProvider.System)
            .GerarToken(AjudaSeguranca.Utilizador(1, Perfis.Cliente, clienteId: 3)).Token;

        // Troca o payload pelo de um token de Administrador, mantendo a assinatura original.
        var outro = AjudaSeguranca.CriarTokenService(chave, TimeProvider.System)
            .GerarToken(AjudaSeguranca.Utilizador(1, Perfis.Administrador)).Token;
        var partes = token.Split('.');
        var adulterado = partes[0] + "." + outro.Split('.')[1] + "." + partes[2];

        var resultado = await ValidarComConfiguracaoDaApiAsync(chave, adulterado);

        Assert.False(resultado.IsValid);
    }

    [Fact(DisplayName = "UT-F001-033 — Um token sem assinatura (alg = none) é rejeitado: só HS256 é aceite")]
    public async Task TokenSemAssinaturaERejeitado()
    {
        var chave = AjudaSeguranca.NovaChave();
        var semAssinatura = new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = AjudaSeguranca.Issuer,
            Audience = AjudaSeguranca.Audience,
            Expires = DateTime.UtcNow.AddHours(1),
            Claims = new Dictionary<string, object>
            {
                [ClaimsClinica.UtilizadorId] = "1",
                [ClaimsClinica.Perfil] = Perfis.Administrador,
                [ClaimsClinica.Jti] = "sem-assinatura"
            }
        });

        var resultado = await ValidarComConfiguracaoDaApiAsync(chave, semAssinatura);

        Assert.False(resultado.IsValid);
    }

    [Fact(DisplayName = "UT-F001-034 — Um token assinado com a chave certa mas com outro emissor é rejeitado")]
    public async Task TokenDeOutroEmissorERejeitado()
    {
        var chave = AjudaSeguranca.NovaChave();
        var opcoes = AjudaSeguranca.CriarOpcoes(chave);
        opcoes.Issuer = "OutroEmissor";
        var token = new JwtTokenService(Options.Create(opcoes), TimeProvider.System)
            .GerarToken(AjudaSeguranca.Utilizador(1, Perfis.Administrador)).Token;

        var resultado = await ValidarComConfiguracaoDaApiAsync(chave, token);

        Assert.False(resultado.IsValid);
    }

    /// <summary>
    /// Token emitido há <paramref name="idade"/>. A validação usa a hora atual, por isso a emissão
    /// também parte dela: o resultado depende só da idade do token.
    /// </summary>
    private static string TokenEmitidoHa(string chave, TimeSpan idade) =>
        AjudaSeguranca.CriarTokenService(chave, new RelogioFixo(DateTimeOffset.UtcNow - idade))
            .GerarToken(AjudaSeguranca.Utilizador(1, Perfis.Administrador)).Token;

    private static async Task<TokenValidationResult> ValidarComConfiguracaoDaApiAsync(string chave, string token)
    {
        using var servicos = AjudaSeguranca.CriarServicos(chave);
        var opcoes = servicos
            .GetRequiredService<IOptionsMonitor<JwtBearerOptions>>()
            .Get(JwtBearerDefaults.AuthenticationScheme);

        return await new JsonWebTokenHandler().ValidateTokenAsync(token, opcoes.TokenValidationParameters);
    }
}
