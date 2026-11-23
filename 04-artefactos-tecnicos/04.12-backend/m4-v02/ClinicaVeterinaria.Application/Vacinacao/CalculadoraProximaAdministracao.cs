namespace ClinicaVeterinaria.Application.Vacinacao;

/// <summary>
/// Data prevista da próxima administração (RF045; Documentação de API v02, EP-043):
/// <c>dataProximaPrevista = dataAdministracao + intervaloDias</c>; sem intervalo, não há próxima.
/// </summary>
/// <remarks>
/// É a mesma regra da coluna calculada <c>RegistoProfilaxia.DataProximaAdministracao</c>
/// (<c>DATEADD(DAY, IntervaloDias, DataAdministracao)</c>). O serviço usa esta classe para devolver
/// a data também nos testes com a base em memória, que não calcula colunas.
/// </remarks>
public static class CalculadoraProximaAdministracao
{
    /// <summary>
    /// Devolve a data de administração somada do intervalo em dias, ou <c>null</c> sem intervalo.
    /// Os dias são de calendário: atravessam os fins de mês e os 29 de fevereiro (UT-F006-003 a 007).
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Intervalo indicado e não superior a 0.</exception>
    public static DateOnly? Calcular(DateOnly dataAdministracao, int? intervaloDias)
    {
        if (intervaloDias is not int dias)
        {
            return null;
        }

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dias, nameof(intervaloDias));

        return dataAdministracao.AddDays(dias);
    }
}
