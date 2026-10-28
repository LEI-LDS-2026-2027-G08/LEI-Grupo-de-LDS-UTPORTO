using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.Application.DTOs.RegistosClinicos;

/// <summary>
/// Corpo de EP-032 (PUT /registos-clinicos/{id}): o mesmo de EP-031 sem <c>consultaId</c>, porque
/// o registo mantém-se associado à consulta e ao animal. As listas substituem as anteriores: um
/// elemento existente mantém-se indicando o seu <c>id</c>; os que não vierem são removidos.
/// </summary>
public class UpdateRegistoClinicoDto : IValidatableObject
{
    [MaxLength(4000, ErrorMessage = "As notas clínicas não podem ter mais de 4000 caracteres.")]
    public string? NotasClinicas { get; set; }

    [MaxLength(1000, ErrorMessage = "O diagnóstico não pode ter mais de 1000 caracteres.")]
    public string? Diagnostico { get; set; }

    public IReadOnlyList<CreateTratamentoDto>? Tratamentos { get; set; }

    public IReadOnlyList<CreatePrescricaoDto>? Prescricoes { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
        ValidadorRegistoClinico.ValidarElementoClinico(NotasClinicas, Diagnostico, Tratamentos, Prescricoes);
}
