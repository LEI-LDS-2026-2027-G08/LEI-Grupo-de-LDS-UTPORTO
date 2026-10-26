using System.Security.Claims;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ClinicaVeterinaria.UnitTests.Seguranca;

/// <summary>
/// I064 — leitura da expiração do token (claim <c>exp</c>), usada no fim de sessão para
/// manter a revogação até ao instante em que o token deixaria de ser aceite.
/// </summary>
public class ExpiracaoTokenTests
{
    [Fact(DisplayName = "I064 — A expiração lida do token validado pela API é a do token emitido")]
    public async Task ExpiracaoDoTokenValidadoEADoTokenEmitido()
    {
        var chave = AjudaSeguranca.NovaChave();
        var emitido = AjudaSeguranca.CriarTokenService(chave, TimeProvider.System)
            .GerarToken(AjudaSeguranca.Utilizador(1, Perfis.Administrador));

        using var servicos = AjudaSeguranca.CriarServicos(chave);
        var opcoes = servicos
            .GetRequiredService<IOptionsMonitor<JwtBearerOptions>>()
            .Get(JwtBearerDefaults.AuthenticationScheme);
        var resultado = await new JsonWebTokenHandler().ValidateTokenAsync(emitido.Token, opcoes.TokenValidationParameters);

        Assert.True(resultado.IsValid);
        Assert.Equal<DateTime?>(emitido.ExpiraEm, new ClaimsPrincipal(resultado.ClaimsIdentity).ObterExpiracao());
    }

    [Fact(DisplayName = "I064 — A claim exp, em segundos desde 1970, é convertida para UTC")]
    public void ExpiracaoEmSegundosUnixEConvertidaParaUtc()
    {
        // 1791993600 = 2026-10-14T16:00:00Z (RELOGIO da I028 + 8 horas).
        var expiracao = PrincipalComExpiracao("1791993600").ObterExpiracao();

        Assert.Equal<DateTime?>(new DateTime(2026, 10, 14, 16, 0, 0, DateTimeKind.Utc), expiracao);
        Assert.Equal(DateTimeKind.Utc, expiracao!.Value.Kind);
    }

    [Theory(DisplayName = "I064 — Uma claim exp em falta ou inválida não dá data de expiração")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("-1")]
    [InlineData("1791993600.5")]
    [InlineData("253402300800")]
    public void ExpiracaoEmFaltaOuInvalidaDevolveNull(string? valor)
    {
        Assert.Null(PrincipalComExpiracao(valor).ObterExpiracao());
    }

    private static ClaimsPrincipal PrincipalComExpiracao(string? valor)
    {
        var claims = new List<Claim> { new(ClaimsClinica.UtilizadorId, "1") };

        if (valor is not null)
        {
            claims.Add(new Claim(ClaimsClinica.Expiracao, valor));
        }

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer"));
    }
}
