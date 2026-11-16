using ClinicaVeterinaria.Application.Agenda;

namespace ClinicaVeterinaria.UnitTests.Vacinacao;

internal static class DatasVacinacaoTeste
{
    public static DateTimeOffset Relogio { get; } =
        DateTimeOffset.FromUnixTimeSeconds(TimeProvider.System.GetUtcNow().ToUnixTimeSeconds());

    public static DateOnly Hoje { get; } =
        DateOnly.FromDateTime(FusoHorarioClinica.ParaLocal(Relogio.UtcDateTime, FusoHorarioClinica.Obter()));

    public static (DateOnly Administracao, int Intervalo, DateOnly Proxima) CasoDeCalendario(string caso)
    {
        var ano = Hoje.Year - 1;
        var anoBissexto = Enumerable.Range(ano - 7, 8).Last(DateTime.IsLeapYear);

        return caso switch
        {
            "90 dias" => (Hoje.AddDays(-30), 90, Hoje.AddDays(60)),
            "fim de mês" => (new DateOnly(ano, 1, 31), 1, new DateOnly(ano, 2, 1)),
            "dia bissexto" => (new DateOnly(anoBissexto, 2, 28), 1, new DateOnly(anoBissexto, 2, 29)),
            "fevereiro comum" => (new DateOnly(anoBissexto - 1, 2, 28), 1, new DateOnly(anoBissexto - 1, 3, 1)),
            "fim de ano" => (new DateOnly(ano, 12, 31), 1, new DateOnly(ano + 1, 1, 1)),
            "intervalo de 365 dias" => (new DateOnly(anoBissexto - 1, 3, 1), 365, new DateOnly(anoBissexto, 2, 29)),
            _ => throw new ArgumentOutOfRangeException(nameof(caso)),
        };
    }
}
