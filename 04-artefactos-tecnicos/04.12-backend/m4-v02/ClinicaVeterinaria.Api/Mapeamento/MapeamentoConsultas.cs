using ClinicaVeterinaria.Api.DTOs.Consultas;
using ClinicaVeterinaria.Application.Models.Consultas;

namespace ClinicaVeterinaria.Api.Mapeamento;

internal static class MapeamentoConsultas
{
    public static PesquisaConsultas ParaModelo(this ConsultaFiltroQuery origem) => new()
    {
        DataInicio = origem.DataInicio,
        DataFim = origem.DataFim,
        VeterinarioId = origem.VeterinarioId,
        AnimalId = origem.AnimalId,
        Estado = origem.Estado,
        Pagina = origem.Pagina,
        Tamanho = origem.Tamanho,
    };

    public static NovaConsulta ParaModelo(this CreateConsultaDto origem) => new()
    {
        AnimalId = origem.AnimalId,
        VeterinarioId = origem.VeterinarioId,
        DataHora = origem.DataHora,
        Motivo = origem.Motivo,
    };

    public static NovaNotaPreparacao ParaModelo(this CreateNotaPreparacaoDto origem) => new()
    {
        Texto = origem.Texto,
    };

    public static PesquisaMinhasConsultas ParaModelo(this MinhasConsultasQuery origem) => new()
    {
        Tipo = origem.Tipo,
        AnimalId = origem.AnimalId,
    };

    public static AlteracaoConsulta ParaModelo(this UpdateConsultaDto origem) => new()
    {
        VeterinarioId = origem.VeterinarioId,
        DataHora = origem.DataHora,
        Motivo = origem.Motivo,
    };

    public static AlteracaoEstadoConsulta ParaModelo(this UpdateEstadoConsultaDto origem) => new()
    {
        Estado = origem.Estado,
        Observacao = origem.Observacao,
    };

    public static ChegadaDto ParaDto(this ChegadaRegistada origem) => new()
    {
        Id = origem.Id,
        ChegadaEm = origem.ChegadaEm,
    };

    public static ConsultaDto ParaDto(this DetalheConsulta origem) => new()
    {
        Id = origem.Id,
        DataHora = origem.DataHora,
        DuracaoMinutos = origem.DuracaoMinutos,
        Estado = origem.Estado,
        Motivo = origem.Motivo,
        Observacao = origem.Observacao,
        Animal = origem.Animal.ParaDto(),
        Cliente = origem.Cliente.ParaDto(),
        Veterinario = origem.Veterinario.ParaDto(),
        ChegadaEm = origem.ChegadaEm,
        NotasPreparacao = origem.NotasPreparacao.Select(item => item.ParaDto()).ToArray(),
    };

    public static ConsultaListItemDto ParaDto(this ResumoConsulta origem) => new()
    {
        Id = origem.Id,
        DataHora = origem.DataHora,
        DuracaoMinutos = origem.DuracaoMinutos,
        Estado = origem.Estado,
        Motivo = origem.Motivo,
        Animal = origem.Animal.ParaDto(),
        Cliente = origem.Cliente.ParaDto(),
        Veterinario = origem.Veterinario.ParaDto(),
        ChegadaEm = origem.ChegadaEm,
    };

    public static MinhaConsultaDto ParaDto(this MinhaConsulta origem) => new()
    {
        Id = origem.Id,
        DataHora = origem.DataHora,
        Estado = origem.Estado,
        Animal = origem.Animal.ParaDto(),
        Veterinario = origem.Veterinario,
    };

    public static NotaPreparacaoDto ParaDto(this NotaPreparacao origem) => new()
    {
        Id = origem.Id,
        ConsultaId = origem.ConsultaId,
        Texto = origem.Texto,
        Autor = origem.Autor,
        CriadaEm = origem.CriadaEm,
    };
}
