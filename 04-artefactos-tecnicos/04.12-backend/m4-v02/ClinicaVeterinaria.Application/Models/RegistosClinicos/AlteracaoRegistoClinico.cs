using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.Application.Models.RegistosClinicos;

public class AlteracaoRegistoClinico : IValidatableObject
{
    [MaxLength(4000, ErrorMessage = "As notas clínicas não podem ter mais de 4000 caracteres.")]
    public string? NotasClinicas { get; set; }

    [MaxLength(1000, ErrorMessage = "O diagnóstico não pode ter mais de 1000 caracteres.")]
    public string? Diagnostico { get; set; }

    public IReadOnlyList<DadosTratamento>? Tratamentos { get; set; }

    public IReadOnlyList<DadosPrescricao>? Prescricoes { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
        ValidadorRegistoClinico.ValidarElementoClinico(
            NotasClinicas,
            Diagnostico,
            Tratamentos?.Select(tratamento => tratamento?.Id).ToArray(),
            Prescricoes?.Select(prescricao => prescricao?.Id).ToArray());
}
