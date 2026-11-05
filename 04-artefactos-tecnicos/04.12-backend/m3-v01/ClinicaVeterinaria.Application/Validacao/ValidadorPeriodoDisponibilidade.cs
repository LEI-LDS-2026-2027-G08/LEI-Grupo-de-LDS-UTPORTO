namespace ClinicaVeterinaria.Application.Validacao;

/// <summary>
/// Regras do período de EP-022 (Documentação de API v02): <c>dataInicio</c> obrigatória;
/// <c>dataFim</c> opcional (por omissão igual a <c>dataInicio</c>), não anterior a
/// <c>dataInicio</c> e com um período de no máximo 31 dias, contando os dois extremos.
/// </summary>
/// <remarks>
/// Aceitam-se apenas datas entre 2000-01-01 e 9998-12-31: fora disso o pedido não tem
/// sentido para a agenda da clínica e os cálculos em UTC sairiam dos limites de <see cref="DateTime"/>.
/// </remarks>
public static class ValidadorPeriodoDisponibilidade
{
    public const int MaximoDias = 31;

    public const string MensagemDataInicioObrigatoria =
        "A data de início (dataInicio) é obrigatória, no formato AAAA-MM-DD.";
    public const string MensagemDataFimAnterior =
        "A data de fim (dataFim) não pode ser anterior à data de início (dataInicio).";
    public const string MensagemPeriodoDemasiadoLongo =
        "O período não pode ter mais de 31 dias.";
    public const string MensagemForaDosLimites =
        "As datas têm de estar entre 2000-01-01 e 9998-12-31.";

    public static readonly DateOnly DataMinima = new(2000, 1, 1);
    public static readonly DateOnly DataMaxima = new(9998, 12, 31);

    /// <summary>Data de fim efetiva: a indicada ou, sem ela, a data de início.</summary>
    public static DateOnly DataFimEfetiva(DateOnly dataInicio, DateOnly? dataFim) => dataFim ?? dataInicio;

    /// <summary>Devolve a regra que o período não cumpre, ou <c>null</c> quando é válido.</summary>
    public static string? ObterErro(DateOnly? dataInicio, DateOnly? dataFim)
    {
        if (dataInicio is null)
        {
            return MensagemDataInicioObrigatoria;
        }

        var inicio = dataInicio.Value;
        var fim = DataFimEfetiva(inicio, dataFim);

        if (inicio < DataMinima || inicio > DataMaxima || fim < DataMinima || fim > DataMaxima)
        {
            return MensagemForaDosLimites;
        }

        if (fim < inicio)
        {
            return MensagemDataFimAnterior;
        }

        // 2026-10-01 a 2026-10-31 são 31 dias: a diferença entre os dois é 30.
        return fim.DayNumber - inicio.DayNumber + 1 > MaximoDias
            ? MensagemPeriodoDemasiadoLongo
            : null;
    }
}
