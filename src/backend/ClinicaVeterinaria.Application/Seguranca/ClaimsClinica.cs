namespace ClinicaVeterinaria.Application.Seguranca;

/// <summary>
/// Nomes das claims do JWT, conforme a Documentação de API (3.2).
/// </summary>
public static class ClaimsClinica
{
    public const string UtilizadorId = "sub";
    public const string Email = "email";
    public const string Perfil = "role";
    public const string Jti = "jti";
    public const string ClienteId = "clienteId";
    public const string VeterinarioId = "veterinarioId";
}
