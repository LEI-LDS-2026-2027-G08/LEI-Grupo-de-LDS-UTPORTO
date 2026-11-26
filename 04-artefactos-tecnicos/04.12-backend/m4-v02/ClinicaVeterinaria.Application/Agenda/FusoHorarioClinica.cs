namespace ClinicaVeterinaria.Application.Agenda;

/// <summary>
/// Fuso horário da clínica — regra R-FUSO da Especificação de Testes Unitários (I028, secção 4.4):
/// as datas e horas com instante são guardadas em UTC (Modelo de Dados, D15), mas o horário de
/// funcionamento e os horários livres de EP-022 estão na hora local de Lisboa, que muda com a
/// hora de verão.
/// </summary>
public static class FusoHorarioClinica
{
    /// <summary>Identificador IANA do fuso da clínica.</summary>
    public const string Identificador = "Europe/Lisbon";

    // Identificador do Windows com as mesmas regras, para sistemas sem conversão de IANA.
    private const string IdentificadorWindows = "GMT Standard Time";

    private static readonly Lazy<TimeZoneInfo> Fuso = new(Carregar);

    /// <summary>Fuso <c>Europe/Lisbon</c>.</summary>
    public static TimeZoneInfo Obter() => Fuso.Value;

    /// <summary>
    /// Converte uma data e hora local de Lisboa para UTC. Devolve <c>null</c> para uma hora que
    /// não existe nesse dia (o salto da mudança para a hora de verão). Numa hora repetida (mudança
    /// para a hora de inverno), usa a primeira ocorrência, ainda na hora de verão.
    /// </summary>
    public static DateTime? ParaUtc(DateOnly data, TimeOnly hora, TimeZoneInfo fuso)
    {
        ArgumentNullException.ThrowIfNull(fuso);

        var local = data.ToDateTime(hora, DateTimeKind.Unspecified);

        if (fuso.IsInvalidTime(local))
        {
            return null;
        }

        var deslocamento = fuso.IsAmbiguousTime(local)
            ? fuso.GetAmbiguousTimeOffsets(local).Max()
            : fuso.GetUtcOffset(local);

        return DateTime.SpecifyKind(local - deslocamento, DateTimeKind.Utc);
    }

    /// <summary>Converte um instante UTC para a data e hora local de Lisboa.</summary>
    public static DateTime ParaLocal(DateTime instanteUtc, TimeZoneInfo fuso)
    {
        ArgumentNullException.ThrowIfNull(fuso);

        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(instanteUtc, DateTimeKind.Utc), fuso);
    }

    private static TimeZoneInfo Carregar()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(Identificador);
        }
        catch (TimeZoneNotFoundException)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(IdentificadorWindows);
        }
    }
}
