using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.Application.Agenda;

/// <summary>Marcação de um Veterinário, com o início em UTC, tal como está guardada.</summary>
public sealed record MarcacaoAgendada(DateTime InicioUtc, int DuracaoMinutos, string Estado);

/// <summary>Horários livres de um dia, em hora local da clínica.</summary>
public sealed record DiaDisponivel(DateOnly Data, IReadOnlyList<TimeOnly> HorariosLivres);

/// <summary>
/// Cálculo da disponibilidade de um Veterinário (RF020, EP-022), isolado da base de dados:
/// recebe o horário de funcionamento, a duração das consultas, as marcações do Veterinário
/// e o instante atual, e devolve os horários livres de cada dia (critério 6 da I071).
/// </summary>
/// <remarks>
/// Regras:
/// <list type="bullet">
/// <item>Os horários de cada período começam na abertura e repetem-se a cada <c>duracaoMinutos</c>;
/// um horário só é devolvido se a consulta acabar até ao fecho do período.</item>
/// <item>Cada marcação não cancelada ocupa <c>[início, início + a sua duração)</c>. Um horário está
/// ocupado se se sobrepuser a uma marcação; intervalos que só se tocam não se sobrepõem (R-HORARIO).</item>
/// <item>As marcações <c>Cancelada</c> não ocupam horário.</item>
/// <item>O horário de funcionamento está em hora local de Lisboa e as marcações em UTC (R-FUSO).</item>
/// <item>Os horários que começam antes do instante atual não são devolvidos (PA-05 da I028).</item>
/// <item>Um dia sem períodos (por exemplo, domingo) é devolvido com a lista vazia.</item>
/// </list>
/// <see cref="IntervaloDisponivel"/> aplica as mesmas regras a um intervalo qualquer, para ser
/// reutilizado na criação e alteração de marcações (EP-025, EP-026).
/// </remarks>
public static class CalculadoraDisponibilidade
{
    /// <summary>Estado de marcação que não ocupa horário.</summary>
    public const string EstadoCancelada = "Cancelada";

    /// <summary>Horários livres de cada dia de <paramref name="dataInicio"/> a <paramref name="dataFim"/>, inclusive.</summary>
    public static IReadOnlyList<DiaDisponivel> Calcular(
        DateOnly dataInicio,
        DateOnly dataFim,
        IReadOnlyList<PeriodoValidado> horario,
        int duracaoMinutos,
        IReadOnlyList<MarcacaoAgendada> marcacoes,
        DateTime agoraUtc,
        TimeZoneInfo fuso)
    {
        ArgumentNullException.ThrowIfNull(horario);
        ArgumentNullException.ThrowIfNull(marcacoes);
        ArgumentNullException.ThrowIfNull(fuso);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(duracaoMinutos);

        if (dataFim < dataInicio)
        {
            throw new ArgumentException("A data de fim não pode ser anterior à data de início.", nameof(dataFim));
        }

        var duracao = TimeSpan.FromMinutes(duracaoMinutos);
        var ocupadas = Ocupadas(marcacoes);
        var dias = new List<DiaDisponivel>();

        // Por número de dias, para nunca somar um dia a DateOnly.MaxValue.
        var numeroDias = dataFim.DayNumber - dataInicio.DayNumber + 1;

        for (var indice = 0; indice < numeroDias; indice++)
        {
            var data = dataInicio.AddDays(indice);
            var livres = new List<TimeOnly>();

            foreach (var periodo in PeriodosDoDia(horario, data))
            {
                var fecho = periodo.Fecho.ToTimeSpan();

                for (var inicio = periodo.Abertura.ToTimeSpan(); inicio + duracao <= fecho; inicio += duracao)
                {
                    var hora = TimeOnly.FromTimeSpan(inicio);
                    var inicioUtc = FusoHorarioClinica.ParaUtc(data, hora, fuso);

                    // Uma hora que não existe nesse dia (mudança para a hora de verão) não é um horário.
                    if (inicioUtc is null || inicioUtc.Value < agoraUtc)
                    {
                        continue;
                    }

                    if (!SobrepoeAlguma(inicioUtc.Value, inicioUtc.Value + duracao, ocupadas))
                    {
                        livres.Add(hora);
                    }
                }
            }

            dias.Add(new DiaDisponivel(data, livres));
        }

        return dias;
    }

    /// <summary>
    /// Indica se a consulta <c>[inicioUtc, inicioUtc + duracaoMinutos)</c> cabe inteira num período
    /// do horário desse dia (em hora local) e não se sobrepõe a nenhuma marcação não cancelada
    /// (regra R-HORARIO). Não exige que o início coincida com a grelha de EP-022 (PA-12 da I028).
    /// </summary>
    public static bool IntervaloDisponivel(
        DateTime inicioUtc,
        int duracaoMinutos,
        IReadOnlyList<PeriodoValidado> horario,
        IReadOnlyList<MarcacaoAgendada> marcacoes,
        TimeZoneInfo fuso)
    {
        ArgumentNullException.ThrowIfNull(horario);
        ArgumentNullException.ThrowIfNull(marcacoes);
        ArgumentNullException.ThrowIfNull(fuso);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(duracaoMinutos);

        var fimUtc = inicioUtc + TimeSpan.FromMinutes(duracaoMinutos);
        var inicioLocal = FusoHorarioClinica.ParaLocal(inicioUtc, fuso);
        var fimLocal = FusoHorarioClinica.ParaLocal(fimUtc, fuso);
        var data = DateOnly.FromDateTime(inicioLocal);

        var dentroDoHorario = PeriodosDoDia(horario, data).Any(periodo =>
            inicioLocal >= data.ToDateTime(periodo.Abertura)
            && fimLocal <= data.ToDateTime(periodo.Fecho));

        return dentroDoHorario && !SobrepoeAlguma(inicioUtc, fimUtc, Ocupadas(marcacoes));
    }

    /// <summary>Dia da semana como na base de dados e em <see cref="PeriodoValidado"/>: 1 = segunda … 7 = domingo.</summary>
    public static byte DiaSemana(DateOnly data) =>
        data.DayOfWeek == DayOfWeek.Sunday ? (byte)7 : (byte)data.DayOfWeek;

    private static IEnumerable<PeriodoValidado> PeriodosDoDia(IReadOnlyList<PeriodoValidado> horario, DateOnly data)
    {
        var diaSemana = DiaSemana(data);

        return horario
            .Where(periodo => periodo.DiaSemana == diaSemana)
            .OrderBy(periodo => periodo.Abertura);
    }

    private static List<(DateTime Inicio, DateTime Fim)> Ocupadas(IReadOnlyList<MarcacaoAgendada> marcacoes) =>
        marcacoes
            .Where(marcacao => !string.Equals(marcacao.Estado, EstadoCancelada, StringComparison.Ordinal))
            .Select(marcacao =>
            {
                var inicio = DateTime.SpecifyKind(marcacao.InicioUtc, DateTimeKind.Utc);
                return (Inicio: inicio, Fim: inicio.AddMinutes(marcacao.DuracaoMinutos));
            })
            .ToList();

    // [inicio, fim) sobrepõe [o.Inicio, o.Fim) se cada um começar antes de o outro acabar.
    private static bool SobrepoeAlguma(DateTime inicio, DateTime fim, List<(DateTime Inicio, DateTime Fim)> ocupadas) =>
        ocupadas.Exists(ocupada => inicio < ocupada.Fim && ocupada.Inicio < fim);
}
