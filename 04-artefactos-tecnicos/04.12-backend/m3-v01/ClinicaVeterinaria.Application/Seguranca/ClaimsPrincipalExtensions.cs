using System.Globalization;
using System.Security.Claims;

namespace ClinicaVeterinaria.Application.Seguranca;

/// <summary>
/// Leitura das claims do utilizador autenticado. Os identificadores usados
/// para aplicar regras de propriedade vêm sempre daqui (do token) e nunca
/// de parâmetros do pedido (RNF002).
/// </summary>
public static class ClaimsPrincipalExtensions
{
    // 9999-12-31T23:59:59Z, o maior valor aceite por DateTimeOffset.FromUnixTimeSeconds.
    private const long MaximoSegundosUnix = 253_402_300_799;

    public static int? ObterUtilizadorId(this ClaimsPrincipal utilizador) =>
        LerIdentificador(utilizador, ClaimsClinica.UtilizadorId);

    /// <summary>Identificador do Cliente; só existe em tokens do perfil Cliente.</summary>
    public static int? ObterClienteId(this ClaimsPrincipal utilizador) =>
        LerIdentificador(utilizador, ClaimsClinica.ClienteId);

    /// <summary>Identificador do Veterinário; só existe em tokens do perfil Veterinário.</summary>
    public static int? ObterVeterinarioId(this ClaimsPrincipal utilizador) =>
        LerIdentificador(utilizador, ClaimsClinica.VeterinarioId);

    public static string? ObterPerfil(this ClaimsPrincipal utilizador)
    {
        ArgumentNullException.ThrowIfNull(utilizador);
        return utilizador.FindFirst(ClaimsClinica.Perfil)?.Value;
    }

    public static string? ObterJti(this ClaimsPrincipal utilizador)
    {
        ArgumentNullException.ThrowIfNull(utilizador);
        return utilizador.FindFirst(ClaimsClinica.Jti)?.Value;
    }

    /// <summary>
    /// Data de expiração do token (claim <c>exp</c>), em UTC. Usada no fim de sessão
    /// para manter a revogação até ao momento em que o token deixaria de ser aceite (I064).
    /// </summary>
    public static DateTime? ObterExpiracao(this ClaimsPrincipal utilizador)
    {
        ArgumentNullException.ThrowIfNull(utilizador);

        var valor = utilizador.FindFirst(ClaimsClinica.Expiracao)?.Value;

        if (long.TryParse(valor, NumberStyles.None, CultureInfo.InvariantCulture, out var segundos)
            && segundos <= MaximoSegundosUnix)
        {
            return DateTimeOffset.FromUnixTimeSeconds(segundos).UtcDateTime;
        }

        return null;
    }

    private static int? LerIdentificador(ClaimsPrincipal utilizador, string tipoClaim)
    {
        ArgumentNullException.ThrowIfNull(utilizador);

        var valor = utilizador.FindFirst(tipoClaim)?.Value;

        if (int.TryParse(valor, NumberStyles.None, CultureInfo.InvariantCulture, out var id) && id > 0)
        {
            return id;
        }

        return null;
    }
}
