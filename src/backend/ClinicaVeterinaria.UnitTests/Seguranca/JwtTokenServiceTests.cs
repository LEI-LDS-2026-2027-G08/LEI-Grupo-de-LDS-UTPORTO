using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace ClinicaVeterinaria.UnitTests.Seguranca;

/// <summary>I063 — critério 4: conteúdo, algoritmo, validade e rejeição dos tokens.</summary>
public class JwtTokenServiceTests
{
    private static readonly DateTimeOffset Agora = new(2026, 10, 25, 10, 0, 0, 567, TimeSpan.Zero);

    [Fact]
    public void TokenDeClienteTemAsClaimsDoContrato()
    {
        var servico = AjudaSeguranca.CriarTokenService(AjudaSeguranca.NovaChave(), new RelogioFixo(Agora));

        var emitido = servico.GerarToken(AjudaSeguranca.Utilizador(12, Perfis.Cliente, clienteId: 7));
        var token = new JsonWebToken(emitido.Token);

        Assert.Equal(SecurityAlgorithms.HmacSha256, token.Alg);
        Assert.Equal("12", token.GetClaim(ClaimsClinica.UtilizadorId).Value);
        Assert.Equal("teste12@clinica-g08.test", token.GetClaim(ClaimsClinica.Email).Value);
        Assert.Equal(Perfis.Cliente, token.GetClaim(ClaimsClinica.Perfil).Value);
        Assert.Equal("7", token.GetClaim(ClaimsClinica.ClienteId).Value);
        Assert.Equal(emitido.Jti, token.GetClaim(ClaimsClinica.Jti).Value);
        Assert.False(token.TryGetClaim(ClaimsClinica.VeterinarioId, out _));
    }

    [Fact]
    public void TokenDeVeterinarioTemVeterinarioIdESemClienteId()
    {
        var servico = AjudaSeguranca.CriarTokenService(AjudaSeguranca.NovaChave(), new RelogioFixo(Agora));

        var token = new JsonWebToken(servico.GerarToken(AjudaSeguranca.Utilizador(4, Perfis.Veterinario)).Token);

        Assert.Equal("4", token.GetClaim(ClaimsClinica.VeterinarioId).Value);
        Assert.False(token.TryGetClaim(ClaimsClinica.ClienteId, out _));
    }

    [Fact]
    public void TokenExpiraOitoHorasDepoisDaEmissao()
    {
        var servico = AjudaSeguranca.CriarTokenService(AjudaSeguranca.NovaChave(), new RelogioFixo(Agora));

        var emitido = servico.GerarToken(AjudaSeguranca.Utilizador(1, Perfis.Administrador));
        var token = new JsonWebToken(emitido.Token);

        var esperado = new DateTime(2026, 10, 25, 18, 0, 0, DateTimeKind.Utc);
        Assert.Equal(esperado, emitido.ExpiraEm);
        Assert.Equal(esperado, token.ValidTo);
    }

    [Fact]
    public void CadaTokenTemUmJtiDiferente()
    {
        var servico = AjudaSeguranca.CriarTokenService(AjudaSeguranca.NovaChave(), new RelogioFixo(Agora));
        var utilizador = AjudaSeguranca.Utilizador(1, Perfis.Administrador);

        Assert.NotEqual(servico.GerarToken(utilizador).Jti, servico.GerarToken(utilizador).Jti);
    }

    [Fact]
    public async Task TokenAtualEAceitePelaConfiguracaoDaApi()
    {
        var chave = AjudaSeguranca.NovaChave();
        var token = AjudaSeguranca.CriarTokenService(chave, TimeProvider.System)
            .GerarToken(AjudaSeguranca.Utilizador(1, Perfis.Administrador)).Token;

        var resultado = await ValidarComConfiguracaoDaApiAsync(chave, token);

        Assert.True(resultado.IsValid);
        Assert.Equal(Perfis.Administrador, resultado.ClaimsIdentity.FindFirst(ClaimsClinica.Perfil)?.Value);
    }

    [Fact]
    public async Task TokenExpiradoERejeitado()
    {
        var chave = AjudaSeguranca.NovaChave();
        var haNoveHoras = new RelogioFixo(DateTimeOffset.UtcNow.AddHours(-9));
        var token = AjudaSeguranca.CriarTokenService(chave, haNoveHoras)
            .GerarToken(AjudaSeguranca.Utilizador(1, Perfis.Administrador)).Token;

        var resultado = await ValidarComConfiguracaoDaApiAsync(chave, token);

        Assert.False(resultado.IsValid);
        Assert.IsType<SecurityTokenExpiredException>(resultado.Exception);
    }

    [Fact]
    public async Task TokenAssinadoComOutraChaveERejeitado()
    {
        var token = AjudaSeguranca.CriarTokenService(AjudaSeguranca.NovaChave(), TimeProvider.System)
            .GerarToken(AjudaSeguranca.Utilizador(1, Perfis.Administrador)).Token;

        var resultado = await ValidarComConfiguracaoDaApiAsync(AjudaSeguranca.NovaChave(), token);

        Assert.False(resultado.IsValid);
    }

    [Fact]
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

    private static async Task<TokenValidationResult> ValidarComConfiguracaoDaApiAsync(string chave, string token)
    {
        using var servicos = AjudaSeguranca.CriarServicos(chave);
        var opcoes = servicos
            .GetRequiredService<IOptionsMonitor<JwtBearerOptions>>()
            .Get(JwtBearerDefaults.AuthenticationScheme);

        return await new JsonWebTokenHandler().ValidateTokenAsync(token, opcoes.TokenValidationParameters);
    }
}
