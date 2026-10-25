using Microsoft.IdentityModel.Tokens;

namespace ClinicaVeterinaria.Api.Seguranca;

/// <summary>
/// Configuração da secção <c>Jwt</c>. A chave (<c>Jwt:Key</c>) nunca fica nos
/// ficheiros versionados: em desenvolvimento vem dos User Secrets e nos
/// restantes ambientes de variáveis de ambiente (ver README do backend).
/// </summary>
public sealed class JwtOptions
{
    public const string Seccao = "Jwt";

    /// <summary>HS256 exige uma chave de pelo menos 256 bits.</summary>
    public const int TamanhoMinimoChaveBytes = 32;

    /// <summary>Chave de assinatura em Base64.</summary>
    public string Key { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public int ExpirationHours { get; set; }

    public IReadOnlyList<string> ValidarConfiguracao()
    {
        var erros = new List<string>();

        var erroChave = ValidarChave(out _);
        if (erroChave is not null)
        {
            erros.Add(erroChave);
        }

        if (string.IsNullOrWhiteSpace(Issuer))
        {
            erros.Add("O Issuer JWT (Jwt:Issuer) não está configurado.");
        }

        if (string.IsNullOrWhiteSpace(Audience))
        {
            erros.Add("O Audience JWT (Jwt:Audience) não está configurado.");
        }

        if (ExpirationHours <= 0)
        {
            erros.Add("A validade do token JWT (Jwt:ExpirationHours) não está corretamente configurada.");
        }

        return erros;
    }

    public SymmetricSecurityKey CriarChaveAssinatura()
    {
        var erro = ValidarChave(out var bytes);

        if (erro is not null)
        {
            throw new InvalidOperationException(erro);
        }

        return new SymmetricSecurityKey(bytes);
    }

    private string? ValidarChave(out byte[] bytes)
    {
        bytes = [];

        if (string.IsNullOrWhiteSpace(Key))
        {
            return "A chave JWT (Jwt:Key) não está configurada. Configure-a nos User Secrets (ver README do backend).";
        }

        try
        {
            bytes = Convert.FromBase64String(Key);
        }
        catch (FormatException)
        {
            return "A chave JWT (Jwt:Key) tem de estar codificada em Base64.";
        }

        if (bytes.Length < TamanhoMinimoChaveBytes)
        {
            return "A chave JWT (Jwt:Key) tem de ter pelo menos 32 bytes (256 bits) depois de descodificada.";
        }

        return null;
    }
}
