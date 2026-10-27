using System.Security.Claims;
using ClinicaVeterinaria.Application.Exceptions;

namespace ClinicaVeterinaria.Application.Seguranca;

/// <summary>
/// Quem consulta a agenda e as marcações (EP-023, EP-024), lido do token. A Rececionista e o
/// Auxiliar Veterinário veem todas as marcações; o Veterinário só as que lhe estão atribuídas,
/// com o identificador da claim <c>veterinarioId</c> e nunca o do pedido (RNF002).
/// </summary>
public sealed class AcessoConsultas
{
    private AcessoConsultas(string perfil, int? veterinarioId)
    {
        Perfil = perfil;
        VeterinarioId = veterinarioId;
    }

    public string Perfil { get; }

    /// <summary>
    /// Só no perfil Veterinário: o Veterinário autenticado, a quem a agenda e a leitura de
    /// marcações ficam restringidas. <c>null</c> nos outros perfis.
    /// </summary>
    public int? VeterinarioId { get; }

    public static AcessoConsultas Rececionista { get; } = new(Perfis.Rececionista, null);

    public static AcessoConsultas AuxiliarVeterinario { get; } = new(Perfis.AuxiliarVeterinario, null);

    public static AcessoConsultas Veterinario(int veterinarioId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(veterinarioId);

        return new AcessoConsultas(Perfis.Veterinario, veterinarioId);
    }

    /// <summary>Acesso do utilizador autenticado.</summary>
    /// <exception cref="ForbiddenException">
    /// Perfil sem acesso à agenda, ou token de Veterinário sem a claim <c>veterinarioId</c>.
    /// </exception>
    public static AcessoConsultas DoUtilizador(ClaimsPrincipal utilizador)
    {
        ArgumentNullException.ThrowIfNull(utilizador);

        return utilizador.ObterPerfil() switch
        {
            Perfis.Rececionista => Rececionista,
            Perfis.AuxiliarVeterinario => AuxiliarVeterinario,
            Perfis.Veterinario => utilizador.ObterVeterinarioId() is int id
                ? Veterinario(id)
                : throw new ForbiddenException("O utilizador autenticado não está associado a um Veterinário."),
            _ => throw new ForbiddenException("O perfil autenticado não tem acesso à agenda.")
        };
    }
}
