using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.DTOs.Consultas;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Application.Validacao;
using ClinicaVeterinaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Infrastructure;

/// <summary>
/// Agenda e consulta de marcações (EP-023, EP-024). Só lê dados administrativos da marcação, do
/// animal, do proprietário e do Veterinário, e as notas de preparação (<c>InformacaoApoio</c>);
/// nunca lê o registo clínico, os tratamentos nem as prescrições.
/// </summary>
public sealed class ConsultasService : IConsultasService
{
    private const string MensagemNaoEncontrada = "A marcação indicada não existe.";
    private const string MensagemNaoAtribuida = "A marcação não está atribuída ao Veterinário autenticado.";

    private readonly ClinicaDbContext _db;

    public ConsultasService(ClinicaDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResultDto<ConsultaListItemDto>> ObterConsultasAsync(
        ConsultaFiltroQuery filtro,
        AcessoConsultas acesso)
    {
        ArgumentNullException.ThrowIfNull(filtro);
        ArgumentNullException.ThrowIfNull(acesso);

        // Mesmas regras da validação automática do pedido (400), para o serviço também as aplicar
        // quando é chamado diretamente.
        Validar(filtro);

        var estado = ValidadorFiltroConsultas.NormalizarEstado(filtro.Estado);

        // Para o Veterinário vale sempre o identificador do token (EP-023; UT-F003-050).
        var veterinarioId = acesso.VeterinarioId ?? filtro.VeterinarioId;

        var marcacoes = _db.Marcacoes.AsNoTracking();

        if (veterinarioId is int vet)
        {
            marcacoes = marcacoes.Where(m => m.VeterinarioId == vet);
        }

        if (filtro.AnimalId is int animal)
        {
            marcacoes = marcacoes.Where(m => m.PacienteId == animal);
        }

        if (estado is not null)
        {
            marcacoes = marcacoes.Where(m => m.Estado == estado);
        }

        // As datas são dias em hora de Lisboa e as marcações estão em UTC (R-FUSO): conta o início
        // da marcação, da meia-noite de dataInicio à meia-noite seguinte a dataFim.
        var fuso = FusoHorarioClinica.Obter();

        if (filtro.DataInicio is DateOnly dataInicio)
        {
            var desde = InicioDoDiaUtc(dataInicio, fuso);
            marcacoes = marcacoes.Where(m => m.DataHoraInicio >= desde);
        }

        if (filtro.DataFim is DateOnly dataFim)
        {
            var ate = InicioDoDiaUtc(dataFim.AddDays(1), fuso);
            marcacoes = marcacoes.Where(m => m.DataHoraInicio < ate);
        }

        var total = await marcacoes.CountAsync();

        // Em long, como em EP-004: uma página muito alta não pode dar um salto negativo.
        var salto = (long)(filtro.Pagina - 1) * filtro.Tamanho;

        List<ConsultaListItemDto> itens = [];

        if (salto < total)
        {
            var linhas = await ComDetalhes(marcacoes)
                .Skip((int)salto)
                .Take(filtro.Tamanho)
                .ToListAsync();

            itens = linhas.Select(ParaItem).ToList();
        }

        return new PagedResultDto<ConsultaListItemDto>
        {
            Itens = itens,
            Pagina = filtro.Pagina,
            Tamanho = filtro.Tamanho,
            Total = total
        };
    }

    public async Task<ConsultaComVersao> ObterConsultaAsync(int id, AcessoConsultas acesso)
    {
        ArgumentNullException.ThrowIfNull(acesso);

        var linha = await ComDetalhes(_db.Marcacoes.AsNoTracking().Where(m => m.Id == id))
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException(MensagemNaoEncontrada);

        if (acesso.VeterinarioId is int veterinarioId && linha.VeterinarioId != veterinarioId)
        {
            throw new ForbiddenException(MensagemNaoAtribuida);
        }

        var notas = await (
                from i in _db.InformacoesApoio.AsNoTracking()
                join f in _db.Funcionarios.AsNoTracking() on i.AuxiliarId equals f.UtilizadorId
                where i.MarcacaoId == id
                orderby i.DataRegisto, i.Id
                select new { i.Id, i.Descricao, Autor = f.Nome, i.DataRegisto })
            .ToListAsync();

        var consulta = new ConsultaDto
        {
            Id = linha.Id,
            DataHora = Utc(linha.DataHoraInicio),
            DuracaoMinutos = linha.DuracaoMinutos,
            Estado = linha.Estado,
            Motivo = linha.Motivo,
            Animal = Animal(linha),
            Cliente = Cliente(linha),
            Veterinario = Veterinario(linha),
            ChegadaEm = Utc(linha.DataHoraChegada),
            NotasPreparacao = notas
                .Select(n => new NotaPreparacaoDto
                {
                    Id = n.Id,
                    Texto = n.Descricao,
                    Autor = n.Autor,
                    CriadaEm = Utc(n.DataRegisto)
                })
                .ToList()
        };

        return new ConsultaComVersao(consulta, VersaoMarcacao.ParaETag(linha.RowVersion));
    }

    /// <summary>
    /// Marcações com o animal, a espécie, o proprietário e o nome do Veterinário, por data e hora
    /// e, em caso de empate, por identificador (ordem estável para a paginação).
    /// </summary>
    private IQueryable<LinhaMarcacao> ComDetalhes(IQueryable<Marcacao> marcacoes) =>
        from m in marcacoes
        join p in _db.Pacientes.AsNoTracking() on m.PacienteId equals p.Id
        join e in _db.Especies.AsNoTracking() on p.EspecieId equals e.Id
        join c in _db.Clientes.AsNoTracking() on p.ClienteId equals c.Id
        join f in _db.Funcionarios.AsNoTracking() on m.VeterinarioId equals f.UtilizadorId
        orderby m.DataHoraInicio, m.Id
        select new LinhaMarcacao
        {
            Id = m.Id,
            DataHoraInicio = m.DataHoraInicio,
            DuracaoMinutos = m.DuracaoMinutos,
            Estado = m.Estado,
            Motivo = m.Motivo,
            DataHoraChegada = m.DataHoraChegada,
            RowVersion = m.RowVersion,
            AnimalId = p.Id,
            AnimalNome = p.Nome,
            Especie = e.Nome,
            ClienteId = c.Id,
            ClienteNome = c.Nome,
            VeterinarioId = m.VeterinarioId,
            VeterinarioNome = f.Nome
        };

    private static void Validar(ConsultaFiltroQuery filtro)
    {
        var erros = new List<ValidationResult>();

        if (!Validator.TryValidateObject(filtro, new ValidationContext(filtro), erros, validateAllProperties: true))
        {
            throw new ArgumentException(string.Join(" ", erros.Select(e => e.ErrorMessage)));
        }
    }

    private static ConsultaListItemDto ParaItem(LinhaMarcacao linha) => new()
    {
        Id = linha.Id,
        DataHora = Utc(linha.DataHoraInicio),
        DuracaoMinutos = linha.DuracaoMinutos,
        Estado = linha.Estado,
        Motivo = linha.Motivo,
        Animal = Animal(linha),
        Cliente = Cliente(linha),
        Veterinario = Veterinario(linha),
        ChegadaEm = Utc(linha.DataHoraChegada)
    };

    private static AnimalRefDto Animal(LinhaMarcacao linha) =>
        new() { Id = linha.AnimalId, Nome = linha.AnimalNome, Especie = linha.Especie };

    // O telefone do proprietário só é devolvido na ficha do animal (EP-016).
    private static ClienteRefDto Cliente(LinhaMarcacao linha) =>
        new() { Id = linha.ClienteId, Nome = linha.ClienteNome };

    private static VeterinarioRefDto Veterinario(LinhaMarcacao linha) =>
        new() { Id = linha.VeterinarioId, Nome = linha.VeterinarioNome };

    // As colunas datetime2 guardam UTC (Modelo de Dados, D15), mas são lidas sem indicação de fuso.
    private static DateTime Utc(DateTime valor) => DateTime.SpecifyKind(valor, DateTimeKind.Utc);

    private static DateTime? Utc(DateTime? valor) => valor is DateTime data ? Utc(data) : null;

    // A meia-noite existe sempre em Lisboa (as mudanças de hora são à 01:00 UTC).
    private static DateTime InicioDoDiaUtc(DateOnly data, TimeZoneInfo fuso) =>
        FusoHorarioClinica.ParaUtc(data, TimeOnly.MinValue, fuso)
        ?? DateTime.SpecifyKind(data.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);

    private sealed class LinhaMarcacao
    {
        public int Id { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public short DuracaoMinutos { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Motivo { get; set; }
        public DateTime? DataHoraChegada { get; set; }
        public byte[] RowVersion { get; set; } = [];
        public int AnimalId { get; set; }
        public string AnimalNome { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public int ClienteId { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public int VeterinarioId { get; set; }
        public string VeterinarioNome { get; set; } = string.Empty;
    }
}
