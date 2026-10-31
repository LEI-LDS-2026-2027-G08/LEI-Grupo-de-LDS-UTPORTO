namespace ClinicaVeterinaria.Application.Vacinacao;

/// <summary>
/// Tipos de administração, com os valores da enumeração <c>TipoAdministracao</c> da Documentação
/// de API v02 (2.2) e da restrição CK_Profilaxia_Tipo.
/// </summary>
public static class TiposAdministracao
{
    public const string Vacina = "Vacina";
    public const string Desparasitacao = "Desparasitacao";

    /// <summary>Expressão regular dos valores aceites, para as anotações de validação dos DTOs.</summary>
    public const string ExpressaoValida = "^(Vacina|Desparasitacao)$";

    public const string MensagemTipoInvalido = "O tipo deve ser Vacina ou Desparasitacao.";

    public static readonly IReadOnlyList<string> Todos =
    [
        Vacina,
        Desparasitacao
    ];
}
