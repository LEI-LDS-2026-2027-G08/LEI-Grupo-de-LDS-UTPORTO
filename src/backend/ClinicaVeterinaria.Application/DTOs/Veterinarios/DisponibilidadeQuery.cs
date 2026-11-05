using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.Application.DTOs.Veterinarios;

/// <summary>
/// Parâmetros de EP-022 (GET /veterinarios/{id}/disponibilidade). As regras do período estão em
/// <see cref="ValidadorPeriodoDisponibilidade"/>; um pedido que não as cumpre dá 400.
/// </summary>
/// <remarks>
/// <c>DataInicio</c> é <c>DateOnly?</c> (no Modelo de Classes, <c>DateOnly</c>) só para distinguir
/// um parâmetro em falta, que tem de dar 400, da data mínima.
/// </remarks>
public class DisponibilidadeQuery : IValidatableObject
{
    /// <summary>Primeiro dia, no formato AAAA-MM-DD. Obrigatório.</summary>
    [Display(Name = "dataInicio")]
    [Required(ErrorMessage = ValidadorPeriodoDisponibilidade.MensagemDataInicioObrigatoria)]
    public DateOnly? DataInicio { get; set; }

    /// <summary>Último dia, no formato AAAA-MM-DD. Opcional: por omissão, igual a <c>dataInicio</c>; no máximo 31 dias.</summary>
    [Display(Name = "dataFim")]
    public DateOnly? DataFim { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // A data de início em falta já é indicada pelo [Required].
        if (DataInicio is null)
        {
            yield break;
        }

        var erro = ValidadorPeriodoDisponibilidade.ObterErro(DataInicio, DataFim);

        if (erro is not null)
        {
            yield return new ValidationResult(erro, [nameof(DataFim)]);
        }
    }
}
