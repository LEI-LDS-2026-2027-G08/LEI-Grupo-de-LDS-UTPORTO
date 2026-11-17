using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClinicaVeterinaria.Application.Validacao;

/// <summary>
/// Política de palavra-passe das contas criadas pela clínica, de funcionário (I065, critério 4)
/// e de Cliente (EP-013):
/// entre <see cref="ComprimentoMinimo"/> e <see cref="ComprimentoMaximo"/> caracteres,
/// com pelo menos uma letra minúscula, uma letra maiúscula, um algarismo
/// e um carácter de pontuação ou símbolo Unicode.
/// Um valor vazio é aceite; para o tornar obrigatório, juntar <c>[Required]</c>.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PalavraPasseForteAttribute : ValidationAttribute
{
    public const int ComprimentoMinimo = 8;
    public const int ComprimentoMaximo = 128;

    public PalavraPasseForteAttribute()
        : base($"A palavra-passe deve ter entre {ComprimentoMinimo} e {ComprimentoMaximo} caracteres, " +
               "com pelo menos uma letra minúscula, uma letra maiúscula, um algarismo " +
               "e um carácter de pontuação ou símbolo.")
    {
    }

    public override bool IsValid(object? value) =>
        value is null
        || (value is string texto && (texto.Length == 0 || EForte(texto)));

    public static bool EForte(string? palavraPasse)
    {
        if (palavraPasse is null || palavraPasse.Length is < ComprimentoMinimo or > ComprimentoMaximo)
        {
            return false;
        }

        var temMinuscula = false;
        var temMaiuscula = false;
        var temAlgarismo = false;
        var temSimbolo = false;

        for (var indice = 0; indice < palavraPasse.Length;)
        {
            if (!Rune.TryGetRuneAt(palavraPasse, indice, out var caracter))
            {
                return false;
            }

            temMinuscula |= Rune.IsLower(caracter);
            temMaiuscula |= Rune.IsUpper(caracter);
            temAlgarismo |= Rune.IsDigit(caracter);
            temSimbolo |= Rune.IsPunctuation(caracter) || Rune.IsSymbol(caracter);
            indice += caracter.Utf16SequenceLength;
        }

        return temMinuscula && temMaiuscula && temAlgarismo && temSimbolo;
    }
}
