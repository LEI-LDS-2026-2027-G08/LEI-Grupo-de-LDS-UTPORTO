namespace ClinicaVeterinaria.Application.Seguranca;

/// <summary>
/// Perfis de utilizador, com os mesmos valores da enumeração <c>Perfil</c>
/// da Documentação de API (2.2) e da restrição CK_Utilizador_Perfil.
/// </summary>
/// <remarks>
/// Para restringir um endpoint a vários perfis, usar as constantes no atributo
/// <c>[Authorize(Roles = Perfis.Administrador + "," + Perfis.Rececionista)]</c>.
/// Para um único perfil, preferir as políticas de <see cref="PoliticasAutorizacao"/>.
/// </remarks>
public static class Perfis
{
    public const string Administrador = "Administrador";
    public const string Rececionista = "Rececionista";
    public const string Veterinario = "Veterinario";
    public const string AuxiliarVeterinario = "AuxiliarVeterinario";
    public const string Cliente = "Cliente";

    /// <summary>Os quatro perfis internos que usam o frontend web.</summary>
    public const string Funcionarios =
        Administrador + "," + Rececionista + "," + Veterinario + "," + AuxiliarVeterinario;

    /// <summary>Os quatro perfis internos, em lista (para RequireRole).</summary>
    public static readonly IReadOnlyList<string> ListaFuncionarios =
    [
        Administrador,
        Rececionista,
        Veterinario,
        AuxiliarVeterinario
    ];

    public static readonly IReadOnlyList<string> Todos =
    [
        Administrador,
        Rececionista,
        Veterinario,
        AuxiliarVeterinario,
        Cliente
    ];
}
