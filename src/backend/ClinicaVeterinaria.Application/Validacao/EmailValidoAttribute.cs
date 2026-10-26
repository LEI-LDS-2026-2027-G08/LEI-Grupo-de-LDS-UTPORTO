using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.Validacao;

/// <summary>
/// Valida o e-mail segundo a regra R-EMAIL da Especificação de Testes Unitários (I028, secção 4.4):
/// até 256 caracteres (coluna <c>Utilizador.Email</c>), sem espaços, com exatamente um <c>@</c>,
/// parte local não vazia e domínio com pelo menos um ponto e sem partes vazias
/// (<c>mail.pt</c> é aceite; <c>mail</c>, <c>.pt</c>, <c>mail.</c> e <c>mail..pt</c> não).
/// Um valor vazio é aceite; para o tornar obrigatório, juntar <c>[Required]</c>.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class EmailValidoAttribute : ValidationAttribute
{
    public const int ComprimentoMaximo = 256;

    public EmailValidoAttribute()
        : base("O e-mail é inválido: deve ter a forma nome@dominio.pt, com um único @, sem espaços e até 256 caracteres.")
    {
    }

    public override bool IsValid(object? value) =>
        value is null
        || (value is string texto && (texto.Length == 0 || EValido(texto)));

    public static bool EValido(string? email)
    {
        if (string.IsNullOrEmpty(email) || email.Length > ComprimentoMaximo || email.Any(char.IsWhiteSpace))
        {
            return false;
        }

        var arroba = email.IndexOf('@', StringComparison.Ordinal);

        // Parte local não vazia e um único @.
        if (arroba <= 0 || arroba != email.LastIndexOf('@'))
        {
            return false;
        }

        var partesDominio = email[(arroba + 1)..].Split('.');

        return partesDominio.Length >= 2 && partesDominio.All(parte => parte.Length > 0);
    }
}
