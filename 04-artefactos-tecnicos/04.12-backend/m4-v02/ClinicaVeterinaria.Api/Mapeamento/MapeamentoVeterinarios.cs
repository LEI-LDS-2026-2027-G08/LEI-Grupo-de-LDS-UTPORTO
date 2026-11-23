using ClinicaVeterinaria.Api.DTOs.Veterinarios;
using ClinicaVeterinaria.Application.Models.Veterinarios;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.Api.Mapeamento;

internal static class MapeamentoVeterinarios
{
    public static PeriodoDisponibilidade ParaModelo(this DisponibilidadeQuery origem) => new()
    {
        DataInicio = origem.DataInicio,
        DataFim = origem.DataFim,
    };

    public static DiaDisponivelDto ParaDto(this DiaDisponivel origem) => new()
    {
        Data = origem.Data,
        HorariosLivres = origem.HorariosLivres
            .Select(ValidadorHorario.FormatarHora)
            .ToArray(),
    };

    public static DisponibilidadeDto ParaDto(this DisponibilidadeVeterinario origem) => new()
    {
        VeterinarioId = origem.VeterinarioId,
        DuracaoConsultaMinutos = origem.DuracaoConsultaMinutos,
        Dias = origem.Dias.Select(item => item.ParaDto()).ToArray(),
    };
}
