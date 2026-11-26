using ClinicaVeterinaria.Api.DTOs.Clinica;
using ClinicaVeterinaria.Application.Models.Clinica;

namespace ClinicaVeterinaria.Api.Mapeamento;

internal static class MapeamentoClinica
{
    public static PeriodoHorario ParaModelo(this PeriodoHorarioDto origem) => new()
    {
        DiaSemana = origem.DiaSemana,
        Abertura = origem.Abertura,
        Fecho = origem.Fecho,
    };

    public static AlteracaoClinica ParaModelo(this UpdateClinicaDto origem) => new()
    {
        Nome = origem.Nome,
        Nif = origem.Nif,
        Morada = origem.Morada,
        Telefone = origem.Telefone,
        Email = origem.Email,
        DuracaoConsultaMinutos = origem.DuracaoConsultaMinutos,
        HorarioFuncionamento = origem.HorarioFuncionamento?.Select(item => item is null ? null! : item.ParaModelo()).ToList(),
    };

    public static ClinicaDto ParaDto(this DadosClinica origem) => new()
    {
        Nome = origem.Nome,
        Nif = origem.Nif,
        Morada = origem.Morada,
        Telefone = origem.Telefone,
        Email = origem.Email,
        DuracaoConsultaMinutos = origem.DuracaoConsultaMinutos,
        HorarioFuncionamento = origem.HorarioFuncionamento.Select(item => item.ParaDto()).ToArray(),
    };

    public static PeriodoHorarioDto ParaDto(this PeriodoHorario origem) => new()
    {
        DiaSemana = origem.DiaSemana,
        Abertura = origem.Abertura,
        Fecho = origem.Fecho,
    };
}
