using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.DTOs.RegistosClinicos;

namespace ClinicaVeterinaria.Application.Validacao;

/// <summary>
/// Regras do registo clínico de EP-031 e EP-032 (I077, critérios 1 a 3): pelo menos um elemento
/// clínico, tratamentos e prescrições completos e, em EP-032, cada <c>id</c> indicado uma só vez.
/// </summary>
/// <remarks>
/// O ASP.NET Core valida os DTOs aninhados das listas e chama <see cref="ValidarElementoClinico"/>
/// através de <see cref="IValidatableObject"/>. O <c>Validator</c> do .NET não percorre as listas,
/// por isso o serviço usa <see cref="ObterErros"/>, que aplica as mesmas regras a tudo.
/// </remarks>
public static class ValidadorRegistoClinico
{
    public const string MensagemSemElementoClinico =
        "O registo clínico tem de ter pelo menos um elemento clínico: notas clínicas, diagnóstico, um tratamento ou uma prescrição.";

    public const string MensagemTratamentoRepetido = "O mesmo tratamento (id) aparece mais de uma vez.";

    public const string MensagemPrescricaoRepetida = "A mesma prescrição (id) aparece mais de uma vez.";

    /// <summary>
    /// Pelo menos um elemento clínico preenchido. Texto só com espaços não conta (UT-F004-007).
    /// </summary>
    public static IEnumerable<ValidationResult> ValidarElementoClinico(
        string? notasClinicas,
        string? diagnostico,
        IReadOnlyList<CreateTratamentoDto>? tratamentos,
        IReadOnlyList<CreatePrescricaoDto>? prescricoes)
    {
        var temElemento = !string.IsNullOrWhiteSpace(notasClinicas)
            || !string.IsNullOrWhiteSpace(diagnostico)
            || tratamentos is { Count: > 0 }
            || prescricoes is { Count: > 0 };

        if (!temElemento)
        {
            yield return new ValidationResult(MensagemSemElementoClinico);
        }

        if (TemIdRepetido(tratamentos?.Select(t => t.Id)))
        {
            yield return new ValidationResult(MensagemTratamentoRepetido, [nameof(UpdateRegistoClinicoDto.Tratamentos)]);
        }

        if (TemIdRepetido(prescricoes?.Select(p => p.Id)))
        {
            yield return new ValidationResult(MensagemPrescricaoRepetida, [nameof(UpdateRegistoClinicoDto.Prescricoes)]);
        }
    }

    /// <summary>
    /// Todas as mensagens de erro do pedido (campos, listas e elemento clínico); vazia se o pedido
    /// é válido. Usada pelo serviço, que também pode ser chamado sem passar pelo ASP.NET Core.
    /// </summary>
    public static IReadOnlyList<string> ObterErros(UpdateRegistoClinicoDto pedido)
    {
        ArgumentNullException.ThrowIfNull(pedido);

        var erros = new List<ValidationResult>();

        // Campos do próprio pedido e, se estes estiverem bem, ValidarElementoClinico (IValidatableObject).
        Validator.TryValidateObject(pedido, new ValidationContext(pedido), erros, validateAllProperties: true);

        foreach (var tratamento in pedido.Tratamentos ?? [])
        {
            if (tratamento is null)
            {
                erros.Add(new ValidationResult("A lista de tratamentos tem um elemento vazio."));
                continue;
            }

            Validator.TryValidateObject(tratamento, new ValidationContext(tratamento), erros, validateAllProperties: true);
        }

        foreach (var prescricao in pedido.Prescricoes ?? [])
        {
            if (prescricao is null)
            {
                erros.Add(new ValidationResult("A lista de prescrições tem um elemento vazio."));
                continue;
            }

            Validator.TryValidateObject(prescricao, new ValidationContext(prescricao), erros, validateAllProperties: true);
        }

        return erros
            .Select(e => e.ErrorMessage ?? string.Empty)
            .Where(m => m.Length > 0)
            .Distinct(StringComparer.Ordinal)
            .ToList();
    }

    private static bool TemIdRepetido(IEnumerable<int?>? ids)
    {
        if (ids is null)
        {
            return false;
        }

        var indicados = ids.Where(id => id is not null).ToList();

        return indicados.Count != indicados.Distinct().Count();
    }
}
