using System.Globalization;
using ClinicaVeterinaria.Application.Vacinacao;

namespace ClinicaVeterinaria.UnitTests.Vacinacao;

/// <summary>
/// I082 — <see cref="CalculadoraProximaAdministracao"/> (RF045): casos UT-F006-001 a UT-F006-007 da
/// Especificação de Testes Unitários (I028, secção 7.5.1).
/// </summary>
public class CalculadoraProximaAdministracaoTests
{
    [Theory(DisplayName = "UT-F006-001 e 003 a 007 — data de administração + intervalo em dias de calendário")]
    [InlineData("2026-10-14", 90, "2027-01-12")]
    [InlineData("2026-01-31", 1, "2026-02-01")]
    [InlineData("2028-02-28", 1, "2028-02-29")]
    [InlineData("2027-02-28", 1, "2027-03-01")]
    [InlineData("2026-12-31", 1, "2027-01-01")]
    [InlineData("2027-03-01", 365, "2028-02-29")]
    public void SomaOIntervalo(string dataAdministracao, int intervaloDias, string esperada)
    {
        var proxima = CalculadoraProximaAdministracao.Calcular(Data(dataAdministracao), intervaloDias);

        Assert.Equal(Data(esperada), proxima);
    }

    [Fact(DisplayName = "UT-F006-002 — sem intervalo não há próxima administração (null)")]
    public void SemIntervaloDevolveNull()
    {
        Assert.Null(CalculadoraProximaAdministracao.Calcular(Data("2026-10-14"), null));
    }

    [Theory(DisplayName = "I082 — intervalo indicado e não superior a 0 é recusado")]
    [InlineData(0)]
    [InlineData(-30)]
    public void IntervaloNaoPositivoERecusado(int intervaloDias)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => CalculadoraProximaAdministracao.Calcular(Data("2026-10-14"), intervaloDias));
    }

    private static DateOnly Data(string texto) => DateOnly.Parse(texto, CultureInfo.InvariantCulture);
}
