using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.Application.Models.Consultas;

public class PesquisaConsultas : IValidatableObject
{
    public DateOnly? DataInicio { get; set; }

    public DateOnly? DataFim { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O veterinarioId deve ser superior a 0.")]
    public int? VeterinarioId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O animalId deve ser superior a 0.")]
    public int? AnimalId { get; set; }

    public string? Estado { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A página deve ser igual ou superior a 1.")]
    public int Pagina { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "O tamanho da página deve estar entre 1 e 100.")]
    public int Tamanho { get; set; } = 20;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var erroPeriodo = ValidadorFiltroConsultas.ObterErroPeriodo(DataInicio, DataFim);

        if (erroPeriodo is not null)
        {
            yield return new ValidationResult(erroPeriodo, [nameof(DataFim)]);
        }

        if (!ValidadorFiltroConsultas.EstadoValido(Estado))
        {
            yield return new ValidationResult(
                ValidadorFiltroConsultas.MensagemEstadoInvalido(Estado!.Trim()),
                [nameof(Estado)]);
        }
    }
}
