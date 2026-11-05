using ClinicaVeterinaria.Application.Agenda;

namespace ClinicaVeterinaria.Application.Validacao;

/// <summary>
/// Regras dos parâmetros de EP-023 (GET /consultas) que as anotações de dados não cobrem: o
/// intervalo de datas e o estado. Um pedido que não as cumpre dá 400.
/// </summary>
/// <remarks>
/// <para>As duas datas são opcionais. Quando vêm as duas, <c>dataFim</c> não pode ser anterior a
/// <c>dataInicio</c> (um só dia é válido). Ao contrário de EP-022, a agenda é paginada e não tem um
/// limite de dias.</para>
/// <para>Como em EP-022, só se aceitam datas entre 2000-01-01 e 9998-12-31
/// (<see cref="ValidadorPeriodoDisponibilidade"/>), para as conversões para UTC não saírem dos
/// limites de <see cref="DateTime"/>.</para>
/// </remarks>
public static class ValidadorFiltroConsultas
{
    /// <summary>Devolve a regra que o intervalo de datas não cumpre, ou <c>null</c> quando é válido.</summary>
    public static string? ObterErroPeriodo(DateOnly? dataInicio, DateOnly? dataFim)
    {
        if (ForaDosLimites(dataInicio) || ForaDosLimites(dataFim))
        {
            return ValidadorPeriodoDisponibilidade.MensagemForaDosLimites;
        }

        return dataInicio is { } inicio && dataFim is { } fim && fim < inicio
            ? ValidadorPeriodoDisponibilidade.MensagemDataFimAnterior
            : null;
    }

    /// <summary>
    /// Valor de <c>EstadoConsulta</c> correspondente ao texto recebido, sem distinguir maiúsculas
    /// nem espaços nos extremos; <c>null</c> quando o filtro não foi indicado.
    /// </summary>
    /// <exception cref="ArgumentException">O texto não é um estado válido.</exception>
    public static string? NormalizarEstado(string? estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
        {
            return null;
        }

        var texto = estado.Trim();

        return EstadosConsulta.Todos.FirstOrDefault(e => string.Equals(e, texto, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException(MensagemEstadoInvalido(texto));
    }

    /// <summary>Indica se o texto é vazio (sem filtro) ou um estado válido.</summary>
    public static bool EstadoValido(string? estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
        {
            return true;
        }

        var texto = estado.Trim();

        return EstadosConsulta.Todos.Any(e => string.Equals(e, texto, StringComparison.OrdinalIgnoreCase));
    }

    public static string MensagemEstadoInvalido(string estado) =>
        $"O estado '{estado}' é inválido. Valores permitidos: {string.Join(", ", EstadosConsulta.Todos)}.";

    private static bool ForaDosLimites(DateOnly? data) =>
        data is { } valor
        && (valor < ValidadorPeriodoDisponibilidade.DataMinima || valor > ValidadorPeriodoDisponibilidade.DataMaxima);
}
