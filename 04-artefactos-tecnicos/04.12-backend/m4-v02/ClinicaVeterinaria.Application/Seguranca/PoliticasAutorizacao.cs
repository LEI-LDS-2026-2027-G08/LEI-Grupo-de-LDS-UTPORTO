namespace ClinicaVeterinaria.Application.Seguranca;

/// <summary>
/// Nomes das políticas de autorização registadas na API.
/// Uso: <c>[Authorize(Policy = PoliticasAutorizacao.Administrador)]</c>.
/// </summary>
public static class PoliticasAutorizacao
{
    public const string Administrador = "Perfil.Administrador";
    public const string Rececionista = "Perfil.Rececionista";
    public const string Veterinario = "Perfil.Veterinario";
    public const string AuxiliarVeterinario = "Perfil.AuxiliarVeterinario";
    public const string Cliente = "Perfil.Cliente";

    /// <summary>Qualquer um dos quatro perfis internos (frontend web).</summary>
    public const string Funcionario = "Perfil.Funcionario";

    /// <summary>
    /// Rotas <c>/me/*</c>: perfil Cliente com o identificador do Cliente no token.
    /// O identificador usado para filtrar os dados vem sempre do token e nunca
    /// do pedido (RNF002).
    /// </summary>
    public const string ClienteProprietario = "Propriedade.Cliente";
}
