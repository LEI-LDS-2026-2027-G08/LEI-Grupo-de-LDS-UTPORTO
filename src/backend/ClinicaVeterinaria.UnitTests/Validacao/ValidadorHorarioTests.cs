using ClinicaVeterinaria.Application.DTOs.Clinica;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.UnitTests.Validacao;

public class ValidadorHorarioTests
{
    private static PeriodoHorarioDto P(string dia, string abertura, string fecho) =>
        new() { DiaSemana = dia, Abertura = abertura, Fecho = fecho };

    [Fact]
    public void VariosPeriodosNoMesmoDiaSaoOrdenadosPorDiaEHora()
    {
        var resultado = ValidadorHorario.Validar(
        [
            P("Sabado", "09:00", "13:00"),
            P("Segunda", "14:00", "19:00"),
            P("Segunda", "09:00", "13:00")
        ]);

        Assert.Equal(
        [
            new PeriodoValidado(1, new TimeOnly(9, 0), new TimeOnly(13, 0)),
            new PeriodoValidado(1, new TimeOnly(14, 0), new TimeOnly(19, 0)),
            new PeriodoValidado(6, new TimeOnly(9, 0), new TimeOnly(13, 0))
        ], resultado);
    }

    [Fact]
    public void PeriodosContiguosSaoAceites()
    {
        var resultado = ValidadorHorario.Validar([P("Terca", "09:00", "13:00"), P("Terca", "13:00", "19:00")]);

        Assert.Equal(2, resultado.Count);
    }

    [Fact]
    public void HorarioVazioSignificaClinicaFechada()
    {
        Assert.Empty(ValidadorHorario.Validar([]));
    }

    [Fact]
    public void PeriodosSobrepostosNoMesmoDiaSaoRejeitados()
    {
        var erro = Assert.Throws<ArgumentException>(() =>
            ValidadorHorario.Validar([P("Segunda", "09:00", "13:00"), P("Segunda", "12:00", "15:00")]));

        Assert.Contains("sobrepõem-se", erro.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void MesmasHorasEmDiasDiferentesNaoSeSobrepoem()
    {
        var resultado = ValidadorHorario.Validar([P("Segunda", "09:00", "13:00"), P("Terca", "09:00", "13:00")]);

        Assert.Equal(2, resultado.Count);
    }

    [Theory]
    [InlineData("13:00", "13:00")]
    [InlineData("13:00", "09:00")]
    public void FechoIgualOuAnteriorAAberturaERejeitado(string abertura, string fecho)
    {
        var erro = Assert.Throws<ArgumentException>(() => ValidadorHorario.Validar([P("Quarta", abertura, fecho)]));

        Assert.Contains("posterior à de abertura", erro.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("Feriado", "09:00", "13:00")]
    [InlineData("Segunda", "9h", "13:00")]
    [InlineData("Segunda", "09:00", "25:00")]
    [InlineData("", "09:00", "13:00")]
    public void DiaOuHoraInvalidosSaoRejeitados(string dia, string abertura, string fecho)
    {
        Assert.Throws<ArgumentException>(() => ValidadorHorario.Validar([P(dia, abertura, fecho)]));
    }
}
