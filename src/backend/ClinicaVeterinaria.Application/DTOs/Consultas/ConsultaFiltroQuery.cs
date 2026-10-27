using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.Application.DTOs.Consultas;

/// <summary>
/// Parâmetros de EP-023 (GET /consultas). Todos os filtros são opcionais; as datas são dias em
/// hora de Lisboa (AAAA-MM-DD) e incluem os dois extremos.
/// </summary>
/// <remarks>
/// Quando o perfil é Veterinário, o servidor ignora <see cref="VeterinarioId"/> e usa o
/// identificador do token (Documentação de API v02, EP-023). <see cref="Estado"/> é texto, como os
/// restantes filtros de enumerações da API; um valor fora de <c>EstadoConsulta</c> dá 400.
/// </remarks>
public class ConsultaFiltroQuery : IValidatableObject
{
    /// <summary>Primeiro dia da agenda (hora de Lisboa), no formato AAAA-MM-DD.</summary>
    public DateOnly? DataInicio { get; set; }

    /// <summary>Último dia da agenda (hora de Lisboa), no formato AAAA-MM-DD. Não pode ser anterior a dataInicio.</summary>
    public DateOnly? DataFim { get; set; }

    /// <summary>Filtra pelo Veterinário. Ignorado quando o perfil autenticado é Veterinário.</summary>
    [Range(1, int.MaxValue, ErrorMessage = "O veterinarioId deve ser superior a 0.")]
    public int? VeterinarioId { get; set; }

    /// <summary>Filtra pelo animal.</summary>
    [Range(1, int.MaxValue, ErrorMessage = "O animalId deve ser superior a 0.")]
    public int? AnimalId { get; set; }

    /// <summary>Pendente, Confirmada, Realizada ou Cancelada.</summary>
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
