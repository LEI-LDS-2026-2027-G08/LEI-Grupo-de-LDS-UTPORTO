using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.Application.DTOs.RegistosClinicos;

/// <summary>
/// Parâmetros de EP-034 (GET /animais/{id}/historico-clinico). As duas datas são opcionais e
/// são dias em hora de Lisboa (AAAA-MM-DD), com os dois extremos incluídos. Conta a data da
/// consulta a que o registo pertence.
/// </summary>
/// <remarks>
/// As regras do intervalo são as da agenda (EP-023, <see cref="ValidadorFiltroConsultas"/>):
/// <c>dataFim</c> não pode ser anterior a <c>dataInicio</c> e as datas têm de estar entre
/// 2000-01-01 e 9998-12-31. Um intervalo inválido dá 400.
/// </remarks>
public class HistoricoClinicoQuery : IValidatableObject
{
    /// <summary>Primeiro dia (hora de Lisboa), no formato AAAA-MM-DD.</summary>
    public DateOnly? DataInicio { get; set; }

    /// <summary>Último dia (hora de Lisboa), no formato AAAA-MM-DD. Não pode ser anterior a dataInicio.</summary>
    public DateOnly? DataFim { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var erro = ValidadorFiltroConsultas.ObterErroPeriodo(DataInicio, DataFim);

        if (erro is not null)
        {
            yield return new ValidationResult(erro, [nameof(DataFim)]);
        }
    }
}
