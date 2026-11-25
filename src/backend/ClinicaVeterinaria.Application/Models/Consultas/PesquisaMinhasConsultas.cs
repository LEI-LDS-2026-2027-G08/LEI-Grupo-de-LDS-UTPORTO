using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Agenda;

namespace ClinicaVeterinaria.Application.Models.Consultas;

public class PesquisaMinhasConsultas : IValidatableObject
{
    public const string MensagemAnimalIdInvalido = "O animalId deve ser superior a 0.";

    public string? Tipo { get; set; }

    public int? AnimalId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TiposMinhasConsultas.Normalizar(Tipo) is null)
        {
            yield return new ValidationResult(TiposMinhasConsultas.MensagemTipoInvalido, [nameof(Tipo)]);
        }

        if (AnimalId is < 1)
        {
            yield return new ValidationResult(MensagemAnimalIdInvalido, [nameof(AnimalId)]);
        }
    }
}
