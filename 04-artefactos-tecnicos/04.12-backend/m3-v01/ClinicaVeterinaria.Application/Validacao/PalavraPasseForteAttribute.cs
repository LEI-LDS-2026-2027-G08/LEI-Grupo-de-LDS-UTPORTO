using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.Validacao;

/// <summary>
/// Política de palavra-passe das contas de funcionário (I065, critério 4):
/// entre <see cref="ComprimentoMinimo"/> e <see cref="ComprimentoMaximo"/> caracteres,
/// com pelo menos uma letra minúscula, uma letra maiúscula, um algarismo e um símbolo.
/// Um valor vazio é aceite; para o tornar obrigatório, juntar <c>[Required]</c>.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PalavraPasseForteAttribute : ValidationAttribute
{
    public const int ComprimentoMinimo = 8;
    public const int ComprimentoMaximo = 128;

    public PalavraPasseForteAttribute()
        : base($"A palavra-passe deve ter entre {ComprimentoMinimo} e {ComprimentoMaximo} caracteres, " +
               "com pelo menos uma letra minúscula, uma letra maiúscula, um algarismo e um símbolo.")
    {
    }

    public override bool IsValid(object? value) =>
        value is null
        || (value is string texto && (texto.Length == 0 || EForte(texto)));

    public static bool EForte(string? palavraPasse) =>
        palavraPasse is not null
        && palavraPasse.Length >= ComprimentoMinimo
        && palavraPasse.Length <= ComprimentoMaximo
        && palavraPasse.Any(char.IsLower)
        && palavraPasse.Any(char.IsUpper)
        && palavraPasse.Any(char.IsDigit)
        && palavraPasse.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c));
}
