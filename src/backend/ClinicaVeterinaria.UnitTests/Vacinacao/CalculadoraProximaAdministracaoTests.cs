using ClinicaVeterinaria.Application.Vacinacao;

namespace ClinicaVeterinaria.UnitTests.Vacinacao;

/// <summary>
/// I082 — <see cref="CalculadoraProximaAdministracao"/> (RF045): casos UT-F006-001 a UT-F006-007 da
/// Especificação de Testes Unitários (I028, secção 7.5.1).
/// </summary>
public class CalculadoraProximaAdministracaoTests
{
    [Theory(DisplayName = "UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário")]
    [InlineData("90 dias")]
    [InlineData("fim de mês")]
    [InlineData("dia bissexto")]
    [InlineData("fevereiro comum")]
    [InlineData("fim de ano")]
    [InlineData("intervalo de 365 dias")]
    public void SomaOIntervalo(string caso)
    {
        var (dataAdministracao, intervaloDias, esperada) = DatasVacinacaoTeste.CasoDeCalendario(caso);

        var proxima = CalculadoraProximaAdministracao.Calcular(dataAdministracao, intervaloDias);

        Assert.Equal(esperada, proxima);
    }

    [Fact(DisplayName = "UT-F006-002 — sem intervalo não há próxima administração (null)")]
    public void SemIntervaloDevolveNull()
    {
        Assert.Null(CalculadoraProximaAdministracao.Calcular(DatasVacinacaoTeste.Hoje, null));
    }

    [Theory(DisplayName = "I082 — intervalo indicado e não superior a 0 é recusado")]
    [InlineData(0)]
    [InlineData(-30)]
    public void IntervaloNaoPositivoERecusado(int intervaloDias)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => CalculadoraProximaAdministracao.Calcular(DatasVacinacaoTeste.Hoje, intervaloDias));
    }
}
