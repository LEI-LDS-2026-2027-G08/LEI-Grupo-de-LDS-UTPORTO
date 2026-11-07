using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.Validacao;

/// <summary>
/// Valida um NIF português: 9 dígitos e dígito de controlo (módulo 11).
/// Um valor vazio é aceite; para o tornar obrigatório, juntar <c>[Required]</c>.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NifValidoAttribute : ValidationAttribute
{
    public NifValidoAttribute()
        : base("O NIF é inválido: deve ter 9 dígitos e um dígito de controlo correto.")
    {
    }

    public override bool IsValid(object? value) =>
        value is null
        || (value is string texto && (string.IsNullOrWhiteSpace(texto) || EValido(texto.Trim())));

    public static bool EValido(string? nif)
    {
        if (nif is null || nif.Length != 9 || !nif.All(char.IsAsciiDigit))
        {
            return false;
        }

        var soma = 0;
        for (var i = 0; i < 8; i++)
        {
            soma += (nif[i] - '0') * (9 - i);
        }

        var resto = soma % 11;
        var controlo = resto < 2 ? 0 : 11 - resto;

        return nif[8] - '0' == controlo;
    }
}
