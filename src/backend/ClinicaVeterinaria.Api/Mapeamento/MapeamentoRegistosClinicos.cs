using ClinicaVeterinaria.Api.DTOs.RegistosClinicos;
using ClinicaVeterinaria.Application.Models.RegistosClinicos;

namespace ClinicaVeterinaria.Api.Mapeamento;

internal static class MapeamentoRegistosClinicos
{
    public static DadosPrescricao ParaModelo(this CreatePrescricaoDto origem) => new()
    {
        Id = origem.Id,
        Medicamento = origem.Medicamento,
        Dosagem = origem.Dosagem,
        Frequencia = origem.Frequencia,
        DuracaoDias = origem.DuracaoDias,
    };

    public static NovoRegistoClinico ParaModelo(this CreateRegistoClinicoDto origem) => new()
    {
        NotasClinicas = origem.NotasClinicas,
        Diagnostico = origem.Diagnostico,
        Tratamentos = origem.Tratamentos?.Select(item => item is null ? null! : item.ParaModelo()).ToArray(),
        Prescricoes = origem.Prescricoes?.Select(item => item is null ? null! : item.ParaModelo()).ToArray(),
        ConsultaId = origem.ConsultaId,
    };

    public static DadosTratamento ParaModelo(this CreateTratamentoDto origem) => new()
    {
        Id = origem.Id,
        Descricao = origem.Descricao,
        DataAplicacao = origem.DataAplicacao,
    };

    public static PeriodoHistoricoClinico ParaModelo(this HistoricoClinicoQuery origem) => new()
    {
        DataInicio = origem.DataInicio,
        DataFim = origem.DataFim,
    };

    public static AlteracaoRegistoClinico ParaModelo(this UpdateRegistoClinicoDto origem) => new()
    {
        NotasClinicas = origem.NotasClinicas,
        Diagnostico = origem.Diagnostico,
        Tratamentos = origem.Tratamentos?.Select(item => item is null ? null! : item.ParaModelo()).ToArray(),
        Prescricoes = origem.Prescricoes?.Select(item => item is null ? null! : item.ParaModelo()).ToArray(),
    };

    public static HistoricoClinicoDto ParaDto(this HistoricoClinico origem) => new()
    {
        Animal = origem.Animal.ParaDto(),
        Registos = origem.Registos.Select(item => item.ParaDto()).ToArray(),
    };

    public static PrescricaoDto ParaDto(this PrescricaoRegistada origem) => new()
    {
        Id = origem.Id,
        Medicamento = origem.Medicamento,
        Dosagem = origem.Dosagem,
        Frequencia = origem.Frequencia,
        DuracaoDias = origem.DuracaoDias,
    };

    public static RegistoClinicoDto ParaDto(this DetalheRegistoClinico origem) => new()
    {
        Id = origem.Id,
        ConsultaId = origem.ConsultaId,
        AnimalId = origem.AnimalId,
        Veterinario = origem.Veterinario,
        CriadoEm = origem.CriadoEm,
        NotasClinicas = origem.NotasClinicas,
        Diagnostico = origem.Diagnostico,
        Tratamentos = origem.Tratamentos.Select(item => item.ParaDto()).ToArray(),
        Prescricoes = origem.Prescricoes.Select(item => item.ParaDto()).ToArray(),
        AtualizadoEm = origem.AtualizadoEm,
        AtualizadoPor = origem.AtualizadoPor,
    };

    public static RegistoClinicoResumoDto ParaDto(this ResumoRegistoClinico origem) => new()
    {
        Id = origem.Id,
        ConsultaId = origem.ConsultaId,
        Data = origem.Data,
        Veterinario = origem.Veterinario,
        Diagnostico = origem.Diagnostico,
        NumeroTratamentos = origem.NumeroTratamentos,
        NumeroPrescricoes = origem.NumeroPrescricoes,
    };

    public static TratamentoDto ParaDto(this TratamentoRegistado origem) => new()
    {
        Id = origem.Id,
        Descricao = origem.Descricao,
        DataAplicacao = origem.DataAplicacao,
    };

    public static RegistoClinicoAuxiliarDto ParaDtoAuxiliar(
        this DetalheRegistoClinico origem) => new()
        {
            Id = origem.Id,
            ConsultaId = origem.ConsultaId,
            AnimalId = origem.AnimalId,
            Veterinario = origem.Veterinario,
            CriadoEm = origem.CriadoEm,
            Tratamentos = origem.Tratamentos.Select(item => item.ParaDto()).ToArray(),
            Prescricoes = origem.Prescricoes.Select(item => item.ParaDto()).ToArray(),
            AtualizadoEm = origem.AtualizadoEm,
            AtualizadoPor = origem.AtualizadoPor,
        };
    public static HistoricoClinicoAuxiliarDto ParaDtoAuxiliar(this HistoricoClinico origem) => new()
    {
        Animal = origem.Animal.ParaDto(),
        Registos = origem.Registos.Select(item => new RegistoClinicoResumoAuxiliarDto
        {
            Id = item.Id,
            ConsultaId = item.ConsultaId,
            Data = item.Data,
            Veterinario = item.Veterinario,
            NumeroTratamentos = item.NumeroTratamentos,
            NumeroPrescricoes = item.NumeroPrescricoes,
        }).ToArray(),
    };
}
