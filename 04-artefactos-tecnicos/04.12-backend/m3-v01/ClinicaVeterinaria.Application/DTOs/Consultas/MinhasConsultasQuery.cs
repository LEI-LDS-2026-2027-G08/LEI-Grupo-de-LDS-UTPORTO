using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Agenda;

namespace ClinicaVeterinaria.Application.DTOs.Consultas;

/// <summary>Parâmetros de EP-030 (GET /me/consultas).</summary>
/// <remarks>
/// As duas regras estão em <see cref="Validate"/> e não num atributo <c>[Range]</c>: o validador só
/// chama <see cref="IValidatableObject.Validate"/> quando os atributos passam, e assim um pedido com
/// os dois parâmetros errados recebe os dois erros no mesmo 400.
/// </remarks>
public class MinhasConsultasQuery : IValidatableObject
{
    public const string MensagemAnimalIdInvalido = "O animalId deve ser superior a 0.";

    /// <summary><c>futuras</c> (por omissão) ou <c>historico</c>. Outro valor: 400.</summary>
    public string? Tipo { get; set; }

    /// <summary>Filtra por um animal do Cliente autenticado. Inferior a 1: 400. Animal de outro Cliente: 403.</summary>
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
