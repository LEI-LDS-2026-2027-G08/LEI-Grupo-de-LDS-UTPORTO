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
/// Agenda, consulta e criação de marcações (EP-023 a EP-025). Só lê e grava dados administrativos
/// da marcação, do animal, do proprietário e do Veterinário, e lê as notas de preparação
/// (<c>InformacaoApoio</c>); nunca lê o registo clínico, os tratamentos nem as prescrições.
/// </summary>
public sealed class ConsultasService : IConsultasService
{
    private const string MensagemNaoEncontrada = "A marcação indicada não existe.";
    private const string MensagemNaoAtribuida = "A marcação não está atribuída ao Veterinário autenticado.";

    /// <summary>EP-025 — data e hora que não é estritamente futura (400).</summary>
    public const string MensagemDataNaoFutura = "A data da marcação deve ser futura.";

    /// <summary>EP-025 — animal inexistente (404).</summary>
    public const string MensagemAnimalNaoEncontrado = "O animal indicado não existe.";

    /// <summary>EP-025 — Veterinário inexistente, de outro perfil ou bloqueado (404).</summary>
    public const string MensagemVeterinarioNaoEncontrado = "O veterinário indicado não existe ou não está ativo.";

    /// <summary>EP-025 — a consulta não cabe inteira num período do horário de funcionamento (422).</summary>
    public const string MensagemForaDoHorario =
        "A marcação está fora do horário de funcionamento da clínica: a consulta tem de começar e acabar dentro de um período.";

    /// <summary>EP-025 — o intervalo sobrepõe-se a outra marcação não cancelada do Veterinário (422).</summary>
    public const string MensagemVeterinarioIndisponivel =
        "O veterinário já tem uma marcação que se sobrepõe a esse horário.";

    private readonly ClinicaDbContext _db;
    private readonly TimeProvider _relogio;

    /// <summary>Serviço com o relógio do sistema (usado pelos testes de EP-023 e EP-024).</summary>
    public ConsultasService(ClinicaDbContext db)
        : this(db, TimeProvider.System)
    {
    }

    public ConsultasService(ClinicaDbContext db, TimeProvider relogio)
    {
        ArgumentNullException.ThrowIfNull(relogio);

        _db = db;
        _relogio = relogio;
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

    public async Task<ConsultaComVersao> CriarConsultaAsync(CreateConsultaDto pedido, int criadaPorId)
    {
        ArgumentNullException.ThrowIfNull(pedido);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(criadaPorId);

        // Mesmas regras da validação automática do pedido (400), para o serviço também as aplicar
        // quando é chamado diretamente (UT-F003-022).
        Validar(pedido);

        var animalId = pedido.AnimalId!.Value;
        var veterinarioId = pedido.VeterinarioId!.Value;
        var inicioUtc = ParaUtcAoSegundo(pedido.DataHora!.Value);
        var agoraUtc = _relogio.GetUtcNow().UtcDateTime;

        // Estritamente futura: a hora atual já não serve (UT-F003-020, UT-F003-021).
        if (inicioUtc <= agoraUtc)
        {
            throw new ArgumentException(MensagemDataNaoFutura);
        }

        if (!await _db.Pacientes.AsNoTracking().AnyAsync(p => p.Id == animalId))
        {
            throw new KeyNotFoundException(MensagemAnimalNaoEncontrado);
        }

        // Mesma regra de EP-021 e EP-022: conta ativa, perfil Veterinário e registo de funcionário.
        var veterinarioAtivo = await (
                from u in _db.Utilizadores.AsNoTracking()
                join f in _db.Funcionarios.AsNoTracking() on u.Id equals f.UtilizadorId
                where u.Id == veterinarioId
                      && u.Perfil == Perfis.Veterinario
                      && u.Estado == EstadosUtilizador.Ativo
                select u.Id)
            .AnyAsync();

        if (!veterinarioAtivo)
        {
            throw new KeyNotFoundException(MensagemVeterinarioNaoEncontrado);
        }

        var duracaoMinutos = DuracaoConsulta.Ler(await _db.ConfiguracaoSistema.AsNoTracking()
            .Where(c => c.Chave == DuracaoConsulta.Chave)
            .Select(c => c.Valor)
            .FirstOrDefaultAsync());

        var horario = await _db.HorariosFuncionamento.AsNoTracking()
            .Where(h => h.Ativo)
            .Select(h => new PeriodoValidado(h.DiaSemana, h.HoraAbertura, h.HoraFecho))
            .ToListAsync();

        var fuso = FusoHorarioClinica.Obter();

        // Primeiro o horário, sem marcações: distingue «fora do horário» de «indisponível».
        if (!CalculadoraDisponibilidade.IntervaloDisponivel(inicioUtc, duracaoMinutos, horario, [], fuso))
        {
            throw new UnprocessableEntityException(MensagemForaDoHorario);
        }

        // Critério 2: a leitura da agenda e a gravação são feitas com a agenda do Veterinário
        // bloqueada, para que dois pedidos simultâneos para a mesma vaga não sejam ambos aceites.
        // A base em memória dos testes unitários não tem transações.
        var relacional = _db.Database.IsRelational();
        await using var transacao = relacional ? await _db.Database.BeginTransactionAsync() : null;

        await BloqueioAgendaVeterinario.AdquirirAsync(_db, veterinarioId);

        var fimUtc = inicioUtc.AddMinutes(duracaoMinutos);

        var marcacoes = await _db.Marcacoes.AsNoTracking()
            .Where(m => m.VeterinarioId == veterinarioId
                        && m.Estado != EstadosConsulta.Cancelada
                        && m.DataHoraInicio < fimUtc
                        && m.DataHoraInicio.AddMinutes(m.DuracaoMinutos) > inicioUtc)
            .Select(m => new MarcacaoAgendada(m.DataHoraInicio, m.DuracaoMinutos, m.Estado))
            .ToListAsync();

        if (!CalculadoraDisponibilidade.IntervaloDisponivel(inicioUtc, duracaoMinutos, horario, marcacoes, fuso))
        {
            throw new UnprocessableEntityException(MensagemVeterinarioIndisponivel);
        }

        var marcacao = new Marcacao
        {
            PacienteId = animalId,
            VeterinarioId = veterinarioId,
            DataHoraInicio = inicioUtc,
            DuracaoMinutos = checked((short)duracaoMinutos),
            Motivo = string.IsNullOrWhiteSpace(pedido.Motivo) ? null : pedido.Motivo.Trim(),
            Estado = EstadosConsulta.Pendente,
            CriadaPorId = criadaPorId,
            DataCriacao = ParaUtcAoSegundo(agoraUtc)
        };

        _db.Marcacoes.Add(marcacao);
        await _db.SaveChangesAsync();

        if (transacao is not null)
        {
            await transacao.CommitAsync();
        }

        // Resposta no formato de EP-024, já com a versão gerada pela base de dados.
        return await ObterConsultaAsync(marcacao.Id, AcessoConsultas.Rececionista);
    }

    public async Task<ConsultaComVersao> AtualizarConsultaAsync(
    int id,
    UpdateConsultaDto pedido,
    string? ifMatch)
    {
        ArgumentNullException.ThrowIfNull(pedido);

        // Aplica também as DataAnnotations quando o serviço é chamado diretamente.
        Validar(pedido);

        var inicioUtc = ParaUtcAoSegundo(pedido.DataHora!.Value);
        var agoraUtc = _relogio.GetUtcNow().UtcDateTime;

        if (inicioUtc <= agoraUtc)
        {
            throw new ArgumentException(MensagemDataNaoFutura);
        }

        var marcacao = await _db.Marcacoes
            .FirstOrDefaultAsync(m => m.Id == id)
            ?? throw new KeyNotFoundException(MensagemNaoEncontrada);

        // If-Match é opcional. Quando vem indicado, tem de corresponder
        // fortemente à RowVersion atual.
        if (!VersaoMarcacao.CorrespondeIfMatch(marcacao.RowVersion, ifMatch))
        {
            throw new PreconditionFailedException(
                "A versão indicada em If-Match não corresponde à versão atual da marcação.");
        }

        // Realizada e Cancelada são estados finais para efeitos de alteração.
        if (marcacao.Estado is EstadosConsulta.Realizada or EstadosConsulta.Cancelada)
        {
            throw new UnprocessableEntityException(
                "Uma marcação Realizada ou Cancelada não pode ser alterada.");
        }

        var veterinarioId = pedido.VeterinarioId!.Value;

        // Mesma regra de EP-025: tem de existir um funcionário ativo
        // com perfil de Veterinário.
        var veterinarioAtivo = await (
                from u in _db.Utilizadores.AsNoTracking()
                join f in _db.Funcionarios.AsNoTracking()
                    on u.Id equals f.UtilizadorId
                where u.Id == veterinarioId
                      && u.Perfil == Perfis.Veterinario
                      && u.Estado == EstadosUtilizador.Ativo
                select u.Id)
            .AnyAsync();

        if (!veterinarioAtivo)
        {
            throw new KeyNotFoundException(MensagemVeterinarioNaoEncontrado);
        }

        var horario = await _db.HorariosFuncionamento.AsNoTracking()
            .Where(h => h.Ativo)
            .Select(h => new PeriodoValidado(
                h.DiaSemana,
                h.HoraAbertura,
                h.HoraFecho))
            .ToListAsync();

        var fuso = FusoHorarioClinica.Obter();

        // Primeiro distinguimos "fora do horário" de "Veterinário indisponível".
        if (!CalculadoraDisponibilidade.IntervaloDisponivel(
                inicioUtc,
                marcacao.DuracaoMinutos,
                horario,
                [],
                fuso))
        {
            throw new UnprocessableEntityException(MensagemForaDoHorario);
        }

        var relacional = _db.Database.IsRelational();

        await using var transacao = relacional
            ? await _db.Database.BeginTransactionAsync()
            : null;

        // A I073 deixou este bloqueio precisamente para ser reutilizado pelo EP-026.
        await BloqueioAgendaVeterinario.AdquirirAsync(_db, veterinarioId);

        var fimUtc = inicioUtc.AddMinutes(marcacao.DuracaoMinutos);

        var marcacoes = await _db.Marcacoes.AsNoTracking()
            .Where(m =>
                m.Id != id
                && m.VeterinarioId == veterinarioId
                && m.Estado != EstadosConsulta.Cancelada
                && m.DataHoraInicio < fimUtc
                && m.DataHoraInicio.AddMinutes(m.DuracaoMinutos) > inicioUtc)
            .Select(m => new MarcacaoAgendada(
                m.DataHoraInicio,
                m.DuracaoMinutos,
                m.Estado))
            .ToListAsync();

        if (!CalculadoraDisponibilidade.IntervaloDisponivel(
                inicioUtc,
                marcacao.DuracaoMinutos,
                horario,
                marcacoes,
                fuso))
        {
            throw new UnprocessableEntityException(
                MensagemVeterinarioIndisponivel);
        }

        marcacao.VeterinarioId = veterinarioId;
        marcacao.DataHoraInicio = inicioUtc;
        marcacao.Motivo = string.IsNullOrWhiteSpace(pedido.Motivo)
            ? null
            : pedido.Motivo.Trim();

        marcacao.DataUltimaAlteracao = ParaUtcAoSegundo(agoraUtc);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException erro)
        {
            throw new PreconditionFailedException(
                "A marcação foi alterada por outro pedido. Obtenha a versão atual e tente novamente.",
                erro);
        }

        if (transacao is not null)
        {
            await transacao.CommitAsync();
        }

        return await ObterConsultaAsync(
            marcacao.Id,
            AcessoConsultas.Rececionista);
    }

    public async Task<string> AtualizarEstadoAsync(
        int id,
        UpdateEstadoConsultaDto pedido,
        string? ifMatch)
    {
        ArgumentNullException.ThrowIfNull(pedido);

        Validar(pedido);

        var novoEstado = MaquinaEstadosMarcacao.NormalizarDestino(
            pedido.Estado);

        var marcacao = await _db.Marcacoes
            .FirstOrDefaultAsync(m => m.Id == id)
            ?? throw new KeyNotFoundException(MensagemNaoEncontrada);

        if (!VersaoMarcacao.CorrespondeIfMatch(
                marcacao.RowVersion,
                ifMatch))
        {
            throw new PreconditionFailedException(
                "A versão indicada em If-Match não corresponde à versão atual da marcação.");
        }

        MaquinaEstadosMarcacao.ValidarTransicao(
            marcacao.Estado,
            novoEstado);

        marcacao.Estado = novoEstado;
        marcacao.DataUltimaAlteracao =
            ParaUtcAoSegundo(_relogio.GetUtcNow().UtcDateTime);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException erro)
        {
            throw new PreconditionFailedException(
                "A marcação foi alterada por outro pedido. Obtenha a versão atual e tente novamente.",
                erro);
        }

        return VersaoMarcacao.ParaETag(marcacao.RowVersion);

        // pedido.Observacao faz parte do contrato de EP-027,
        // mas o Modelo de Dados atual não tem campo onde a persistir.
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

    private static void Validar(object pedido)
    {
        var erros = new List<ValidationResult>();

        if (!Validator.TryValidateObject(pedido, new ValidationContext(pedido), erros, validateAllProperties: true))
        {
            throw new ArgumentException(string.Join(" ", erros.Select(e => e.ErrorMessage)));
        }
    }

    // Uma data sem fuso é lida como UTC; uma data com fuso é convertida. Os segundos fracionários
    // são retirados, como na coluna datetime2(0), para a verificação e a gravação usarem o mesmo valor.
    private static DateTime ParaUtcAoSegundo(DateTime valor)
    {
        var utc = valor.Kind switch
        {
            DateTimeKind.Local => valor.ToUniversalTime(),
            _ => DateTime.SpecifyKind(valor, DateTimeKind.Utc)
        };

        return new DateTime(utc.Ticks - (utc.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc);
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
