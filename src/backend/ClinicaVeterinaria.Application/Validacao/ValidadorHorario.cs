using System.Globalization;
using ClinicaVeterinaria.Application.DTOs.Clinica;

namespace ClinicaVeterinaria.Application.Validacao;

/// <summary>Período já validado, com o dia da semana como na base de dados (1 = segunda-feira).</summary>
public sealed record PeriodoValidado(byte DiaSemana, TimeOnly Abertura, TimeOnly Fecho);

/// <summary>
/// Regras do horário de funcionamento (RF019, I067): dia da semana válido,
/// horas no formato HH:mm, fecho posterior à abertura e sem períodos sobrepostos
/// no mesmo dia. Períodos contíguos (13:00–14:00 e 14:00–19:00) são aceites.
/// </summary>
public static class ValidadorHorario
{
    private const string FormatoHora = "HH:mm";

    /// <summary>Nomes da enumeração <c>DiaSemana</c> (Documentação de API, 2.2), pela ordem 1..7.</summary>
    public static readonly IReadOnlyList<string> DiasSemana =
        ["Segunda", "Terca", "Quarta", "Quinta", "Sexta", "Sabado", "Domingo"];

    public static string NomeDia(byte diaSemana) => DiasSemana[diaSemana - 1];

    public static string FormatarHora(TimeOnly hora) => hora.ToString(FormatoHora, CultureInfo.InvariantCulture);

    /// <summary>
    /// Valida todos os períodos e devolve-os ordenados por dia e hora de abertura.
    /// Reúne todos os erros numa única <see cref="ArgumentException"/> (400).
    /// </summary>
    public static IReadOnlyList<PeriodoValidado> Validar(IReadOnlyList<PeriodoHorarioDto> periodos)
    {
        ArgumentNullException.ThrowIfNull(periodos);

        var erros = new List<string>();
        var validos = new List<PeriodoValidado>();

        for (var i = 0; i < periodos.Count; i++)
        {
            var periodo = periodos[i];
            var posicao = i + 1;

            if (periodo is null)
            {
                erros.Add($"Período {posicao}: está vazio.");
                continue;
            }

            var indiceDia = IndiceDia(periodo.DiaSemana);
            var abertura = LerHora(periodo.Abertura);
            var fecho = LerHora(periodo.Fecho);

            if (indiceDia < 0)
            {
                erros.Add($"Período {posicao}: dia da semana inválido ({string.Join(", ", DiasSemana)}).");
            }

            if (abertura is null)
            {
                erros.Add($"Período {posicao}: a hora de abertura deve estar no formato HH:mm.");
            }

            if (fecho is null)
            {
                erros.Add($"Período {posicao}: a hora de fecho deve estar no formato HH:mm.");
            }

            if (indiceDia < 0 || abertura is null || fecho is null)
            {
                continue;
            }

            if (fecho <= abertura)
            {
                erros.Add($"Período {posicao}: a hora de fecho deve ser posterior à de abertura.");
                continue;
            }

            validos.Add(new PeriodoValidado((byte)(indiceDia + 1), abertura.Value, fecho.Value));
        }

        var ordenados = validos
            .OrderBy(p => p.DiaSemana)
            .ThenBy(p => p.Abertura)
            .ToList();

        for (var i = 1; i < ordenados.Count; i++)
        {
            var anterior = ordenados[i - 1];
            var atual = ordenados[i];

            if (atual.DiaSemana == anterior.DiaSemana && atual.Abertura < anterior.Fecho)
            {
                erros.Add(
                    $"{NomeDia(atual.DiaSemana)}: os períodos {FormatarHora(anterior.Abertura)}–{FormatarHora(anterior.Fecho)} " +
                    $"e {FormatarHora(atual.Abertura)}–{FormatarHora(atual.Fecho)} sobrepõem-se.");
            }
        }

        if (erros.Count > 0)
        {
            throw new ArgumentException(string.Join(" ", erros));
        }

        return ordenados;
    }

    private static int IndiceDia(string? nome)
    {
        for (var i = 0; i < DiasSemana.Count; i++)
        {
            if (string.Equals(DiasSemana[i], nome?.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }

    private static TimeOnly? LerHora(string? valor) =>
        TimeOnly.TryParseExact(valor?.Trim(), FormatoHora, CultureInfo.InvariantCulture, DateTimeStyles.None, out var hora)
            ? hora
            : null;
}
