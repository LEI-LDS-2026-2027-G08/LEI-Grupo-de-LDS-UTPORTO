using System.Globalization;
using System.Security.Claims;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Seguranca;

namespace ClinicaVeterinaria.UnitTests.Consultas;

/// <summary>
/// I072 — acesso à agenda lido do token (EP-023, EP-024): o Veterinário é sempre identificado pela
/// claim <c>veterinarioId</c> (RNF002); a Rececionista e o Auxiliar veem todas as marcações.
/// </summary>
public class AcessoConsultasTests
{
    [Theory(DisplayName = "I072 — a Rececionista e o Auxiliar Veterinário não ficam restringidos a um Veterinário")]
    [InlineData(Perfis.Rececionista)]
    [InlineData(Perfis.AuxiliarVeterinario)]
    public void RececionistaEAuxiliarVeemTudo(string perfil)
    {
        var acesso = AcessoConsultas.DoUtilizador(Principal(perfil));

        Assert.Equal(perfil, acesso.Perfil);
        Assert.Null(acesso.VeterinarioId);
    }

    [Fact(DisplayName = "I072 — o Veterinário fica restringido ao identificador da claim veterinarioId")]
    public void VeterinarioUsaAClaim()
    {
        var acesso = AcessoConsultas.DoUtilizador(Principal(Perfis.Veterinario, veterinarioId: 4));

        Assert.Equal(Perfis.Veterinario, acesso.Perfil);
        Assert.Equal(4, acesso.VeterinarioId);
    }

    [Fact(DisplayName = "I072 — um token de Veterinário sem a claim veterinarioId dá 403")]
    public void VeterinarioSemClaimDevolve403()
    {
        Assert.Throws<ForbiddenException>(() => AcessoConsultas.DoUtilizador(Principal(Perfis.Veterinario)));
    }

    [Theory(DisplayName = "I072 — o Administrador e o Cliente não têm acesso à agenda (403)")]
    [InlineData(Perfis.Administrador)]
    [InlineData(Perfis.Cliente)]
    public void OutrosPerfisDevolvem403(string perfil)
    {
        Assert.Throws<ForbiddenException>(() => AcessoConsultas.DoUtilizador(Principal(perfil)));
    }

    /// <summary>Utilizador autenticado tal como o JwtBearer o constrói a partir do token.</summary>
    internal static ClaimsPrincipal Principal(string perfil, int? veterinarioId = null)
    {
        var claims = new List<Claim>
        {
            new(ClaimsClinica.UtilizadorId, (veterinarioId ?? 1).ToString(CultureInfo.InvariantCulture)),
            new(ClaimsClinica.Perfil, perfil),
            new(ClaimsClinica.Jti, "jti-de-teste")
        };

        if (veterinarioId is int id)
        {
            claims.Add(new Claim(ClaimsClinica.VeterinarioId, id.ToString(CultureInfo.InvariantCulture)));
        }

        return new ClaimsPrincipal(new ClaimsIdentity(
            claims,
            authenticationType: "Bearer",
            nameType: ClaimsClinica.UtilizadorId,
            roleType: ClaimsClinica.Perfil));
    }
}
