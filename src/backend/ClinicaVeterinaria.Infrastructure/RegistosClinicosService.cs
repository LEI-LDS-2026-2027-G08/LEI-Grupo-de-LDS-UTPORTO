using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.DTOs.RegistosClinicos;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Validacao;
using ClinicaVeterinaria.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Infrastructure;

/// <summary>
/// Registo clínico das consultas (EP-031 a EP-034). Só o Veterinário a quem a consulta está
/// atribuída cria ou altera o registo (Documentação de API, secção 3.3; UT-F004-017). Cada consulta
/// tem no máximo um registo (índice único <c>UQ_RegistoClinico_Marcacao</c>). A leitura do registo
/// e do histórico do paciente (EP-033, EP-034) é do Veterinário e do Auxiliar Veterinário; o perfil
/// é verificado no controlador e estas operações não alteram nada.
/// </summary>
public sealed class RegistosClinicosService : IRegistosClinicosService
{
    /// <summary>EP-031 — consulta inexistente (404).</summary>
    public const string MensagemConsultaNaoEncontrada = "A consulta indicada não existe.";

    /// <summary>EP-031 — consulta atribuída a outro Veterinário (403).</summary>
    public const string MensagemConsultaDeOutroVeterinario =
        "A consulta não está atribuída ao Veterinário autenticado.";

    /// <summary>EP-031 — consulta Pendente ou Cancelada (422).</summary>
    public const string MensagemEstadoNaoAdmitido =
        "Só é possível registar o ato clínico de uma consulta Confirmada ou Realizada.";

    /// <summary>EP-031 — a consulta já tem registo clínico (409).</summary>
    public const string MensagemJaTemRegisto =
        "A consulta já tem registo clínico. Para o alterar, use PUT /registos-clinicos/{id}.";

    /// <summary>EP-032 e EP-033 — registo inexistente (404).</summary>
    public const string MensagemRegistoNaoEncontrado = "O registo clínico indicado não existe.";

    /// <summary>EP-034 — animal inexistente (404).</summary>
    public const string MensagemAnimalNaoEncontrado = "O animal indicado não existe.";

    /// <summary>EP-032 — registo de outro Veterinário (403).</summary>
    public const string MensagemRegistoDeOutroVeterinario =
        "O registo clínico não pertence a uma consulta do Veterinário autenticado.";

    /// <summary>EP-032 — id de tratamento que não é deste registo (400; PA-10 da I028).</summary>
    public const string MensagemTratamentoDeOutroRegisto =
        "O tratamento indicado (id) não pertence a este registo clínico.";

    /// <summary>EP-032 — id de prescrição que não é deste registo (400; PA-10 da I028).</summary>
    public const string MensagemPrescricaoDeOutroRegisto =
        "A prescrição indicada (id) não pertence a este registo clínico.";

    // Erros do SQL Server para uma chave única violada (UQ_RegistoClinico_Marcacao).
    private const int ErroIndiceUnico = 2601;
    private const int ErroRestricaoUnica = 2627;

    private readonly ClinicaDbContext _db;
    private readonly TimeProvider _relogio;

    public RegistosClinicosService(ClinicaDbContext db, TimeProvider relogio)
    {
        ArgumentNullException.ThrowIfNull(relogio);

        _db = db;
        _relogio = relogio;
    }

    public async Task<RegistoClinicoDto> CriarAsync(CreateRegistoClinicoDto pedido, int veterinarioId)
    {
        ArgumentNullException.ThrowIfNull(pedido);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(veterinarioId);

        // (1) Campos, listas e «pelo menos um elemento clínico» → 400.
        Validar(pedido);

        var consultaId = pedido.ConsultaId!.Value;

        // (2) Consulta existe → 404.
        var marcacao = await _db.Marcacoes.AsNoTracking()
            .Where(m => m.Id == consultaId)
            .Select(m => new { m.VeterinarioId, m.Estado })
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException(MensagemConsultaNaoEncontrada);

        // (3) Só o Veterinário da consulta → 403.
        if (marcacao.VeterinarioId != veterinarioId)
        {
            throw new ForbiddenException(MensagemConsultaDeOutroVeterinario);
        }

        // (4) Só Confirmada ou Realizada → 422 (UT-F004-004, 014, 015).
        if (marcacao.Estado is not (EstadosConsulta.Confirmada or EstadosConsulta.Realizada))
        {
            throw new UnprocessableEntityException(MensagemEstadoNaoAdmitido);
        }

        // (5) No máximo um registo por consulta → 409 (UT-F004-013).
        if (await _db.RegistosClinicos.AnyAsync(r => r.MarcacaoId == consultaId))
        {
            throw new ConflitoException(MensagemJaTemRegisto);
        }

        var agora = AgoraAoSegundo();

        var registo = new RegistoClinico
        {
            MarcacaoId = consultaId,
            VeterinarioId = veterinarioId,
            NotasClinicas = Normalizar(pedido.NotasClinicas),
            Diagnostico = Normalizar(pedido.Diagnostico),
            CriadoEm = agora
        };

        // (6) Registo, tratamentos e prescrições gravados juntos ou nada é gravado (IT-EP031-09).
        // A base em memória dos testes unitários não tem transações.
        var relacional = _db.Database.IsRelational();
        await using var transacao = relacional ? await _db.Database.BeginTransactionAsync() : null;

        try
        {
            _db.RegistosClinicos.Add(registo);
            await _db.SaveChangesAsync();

            _db.Tratamentos.AddRange((pedido.Tratamentos ?? []).Select(t => NovoTratamento(registo.Id, t)));
            _db.Prescricoes.AddRange((pedido.Prescricoes ?? []).Select(p => NovaPrescricao(registo.Id, p)));
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException erro) when (ChaveUnicaViolada(erro))
        {
            // Dois pedidos simultâneos para a mesma consulta: o segundo bate no índice único.
            throw new ConflitoException(MensagemJaTemRegisto, erro);
        }

        if (transacao is not null)
        {
            await transacao.CommitAsync();
        }

        return await ObterDtoAsync(registo.Id);
    }

    public async Task<RegistoClinicoDto> AtualizarAsync(int id, UpdateRegistoClinicoDto pedido, int veterinarioId)
    {
        ArgumentNullException.ThrowIfNull(pedido);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(veterinarioId);

        // (1) Campos, listas e «pelo menos um elemento clínico» → 400 (UT-F004-020).
        Validar(pedido);

        // (2) Registo existe → 404 (UT-F004-021).
        var registo = await _db.RegistosClinicos.FirstOrDefaultAsync(r => r.Id == id)
            ?? throw new KeyNotFoundException(MensagemRegistoNaoEncontrado);

        // (3) Só o Veterinário que fez o registo → 403.
        if (registo.VeterinarioId != veterinarioId)
        {
            throw new ForbiddenException(MensagemRegistoDeOutroVeterinario);
        }

        var tratamentosAtuais = await _db.Tratamentos.Where(t => t.RegistoClinicoId == id).ToListAsync();
        var prescricoesAtuais = await _db.Prescricoes.Where(p => p.RegistoClinicoId == id).ToListAsync();

        var tratamentosPedido = pedido.Tratamentos ?? [];
        var prescricoesPedido = pedido.Prescricoes ?? [];

        // (4) Um id indicado tem de ser deste registo → 400 (UT-F004-019; PA-10 da I028).
        if (tratamentosPedido.Any(t => t.Id is int tid && tratamentosAtuais.TrueForAll(a => a.Id != tid)))
        {
            throw new ValidacaoException(MensagemTratamentoDeOutroRegisto);
        }

        if (prescricoesPedido.Any(p => p.Id is int pid && prescricoesAtuais.TrueForAll(a => a.Id != pid)))
        {
            throw new ValidacaoException(MensagemPrescricaoDeOutroRegisto);
        }

        // (5) As listas substituem as anteriores: atualizar os indicados, acrescentar os novos e
        // remover os que não vieram (UT-F004-018). A consulta (MarcacaoId) não muda.
        registo.NotasClinicas = Normalizar(pedido.NotasClinicas);
        registo.Diagnostico = Normalizar(pedido.Diagnostico);
        registo.AtualizadoEm = AgoraAoSegundo();
        registo.AtualizadoPorId = veterinarioId;

        foreach (var atual in tratamentosAtuais)
        {
            var novo = tratamentosPedido.FirstOrDefault(t => t.Id == atual.Id);

            if (novo is null)
            {
                _db.Tratamentos.Remove(atual);
                continue;
            }

            atual.Descricao = novo.Descricao.Trim();
            atual.DataAplicacao = novo.DataAplicacao!.Value;
        }

        foreach (var atual in prescricoesAtuais)
        {
            var nova = prescricoesPedido.FirstOrDefault(p => p.Id == atual.Id);

            if (nova is null)
            {
                _db.Prescricoes.Remove(atual);
                continue;
            }

            atual.Medicamento = nova.Medicamento.Trim();
            atual.Dosagem = nova.Dosagem.Trim();
            atual.Frequencia = nova.Frequencia.Trim();
            atual.DuracaoDias = (short?)nova.DuracaoDias;
        }

        _db.Tratamentos.AddRange(tratamentosPedido.Where(t => t.Id is null).Select(t => NovoTratamento(id, t)));
        _db.Prescricoes.AddRange(prescricoesPedido.Where(p => p.Id is null).Select(p => NovaPrescricao(id, p)));

        // Uma só gravação: o EF Core aplica as alterações numa transação.
        await _db.SaveChangesAsync();

        return await ObterDtoAsync(id);
    }

    public async Task<RegistoClinicoDto> ObterAsync(int id)
    {
        // Registo existe → 404 (IT-EP033-03). A leitura não depende do Veterinário que fez o
        // registo: o histórico clínico do paciente inclui os registos de todos os Veterinários.
        if (!await _db.RegistosClinicos.AsNoTracking().AnyAsync(r => r.Id == id))
        {
            throw new KeyNotFoundException(MensagemRegistoNaoEncontrado);
        }

        return await ObterDtoAsync(id);
    }

    public async Task<HistoricoClinicoDto> ObterHistoricoAsync(int animalId, HistoricoClinicoQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        // (1) Intervalo de datas → 400 (UT-F004-026). As mesmas regras da validação automática do
        // pedido, para o serviço também as aplicar quando é chamado diretamente.
        var erroPeriodo = ValidadorFiltroConsultas.ObterErroPeriodo(query.DataInicio, query.DataFim);

        if (erroPeriodo is not null)
        {
            throw new ValidacaoException(erroPeriodo);
        }

        // (2) Animal existe → 404 (UT-F004-027).
        var animal = await (
                from p in _db.Pacientes.AsNoTracking()
                join e in _db.Especies.AsNoTracking() on p.EspecieId equals e.Id
                where p.Id == animalId
                select new AnimalRefDto { Id = p.Id, Nome = p.Nome, Especie = e.Nome })
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException(MensagemAnimalNaoEncontrado);

        // (3) Registos das consultas do animal. As datas são dias em hora de Lisboa e as marcações
        // estão em UTC (R-FUSO): conta o início da consulta, da meia-noite de dataInicio à
        // meia-noite seguinte a dataFim, como na agenda (EP-023).
        var fuso = FusoHorarioClinica.Obter();
        var marcacoes = _db.Marcacoes.AsNoTracking().Where(m => m.PacienteId == animalId);

        if (query.DataInicio is DateOnly dataInicio)
        {
            var desde = InicioDoDiaUtc(dataInicio, fuso);
            marcacoes = marcacoes.Where(m => m.DataHoraInicio >= desde);
        }

        if (query.DataFim is DateOnly dataFim)
        {
            var ate = InicioDoDiaUtc(dataFim.AddDays(1), fuso);
            marcacoes = marcacoes.Where(m => m.DataHoraInicio < ate);
        }

        // (4) Do mais recente para o mais antigo (UT-F004-023), com o número de tratamentos e de
        // prescrições de cada registo (UT-F004-024).
        var linhas = await (
                from m in marcacoes
                join r in _db.RegistosClinicos.AsNoTracking() on m.Id equals r.MarcacaoId
                join v in _db.Funcionarios.AsNoTracking() on r.VeterinarioId equals v.UtilizadorId
                orderby m.DataHoraInicio descending, r.Id descending
                select new
                {
                    r.Id,
                    r.MarcacaoId,
                    m.DataHoraInicio,
                    Veterinario = v.Nome,
                    r.Diagnostico,
                    NumeroTratamentos = _db.Tratamentos.Count(t => t.RegistoClinicoId == r.Id),
                    NumeroPrescricoes = _db.Prescricoes.Count(p => p.RegistoClinicoId == r.Id)
                })
            .ToListAsync();

        var registos = linhas
            .Select(l => new RegistoClinicoResumoDto
            {
                Id = l.Id,
                ConsultaId = l.MarcacaoId,
                Data = DateOnly.FromDateTime(FusoHorarioClinica.ParaLocal(l.DataHoraInicio, fuso)),
                Veterinario = l.Veterinario,
                Diagnostico = l.Diagnostico,
                NumeroTratamentos = l.NumeroTratamentos,
                NumeroPrescricoes = l.NumeroPrescricoes
            })
            .ToList();

        return new HistoricoClinicoDto { Animal = animal, Registos = registos };
    }

    /// <summary>
    /// Registo com o paciente da consulta, o nome do Veterinário, o autor da última alteração e as
    /// listas, por identificador.
    /// </summary>
    private async Task<RegistoClinicoDto> ObterDtoAsync(int id)
    {
        var linha = await (
                from r in _db.RegistosClinicos.AsNoTracking()
                join m in _db.Marcacoes.AsNoTracking() on r.MarcacaoId equals m.Id
                join v in _db.Funcionarios.AsNoTracking() on r.VeterinarioId equals v.UtilizadorId
                where r.Id == id
                select new
                {
                    r.Id,
                    r.MarcacaoId,
                    m.PacienteId,
                    Veterinario = v.Nome,
                    r.CriadoEm,
                    r.NotasClinicas,
                    r.Diagnostico,
                    r.AtualizadoEm,
                    r.AtualizadoPorId
                })
            .FirstAsync();

        string? atualizadoPor = null;

        if (linha.AtualizadoPorId is int autorId)
        {
            atualizadoPor = await _db.Funcionarios.AsNoTracking()
                .Where(f => f.UtilizadorId == autorId)
                .Select(f => f.Nome)
                .FirstOrDefaultAsync();
        }

        var tratamentos = await _db.Tratamentos.AsNoTracking()
            .Where(t => t.RegistoClinicoId == id)
            .OrderBy(t => t.Id)
            .Select(t => new TratamentoDto { Id = t.Id, Descricao = t.Descricao, DataAplicacao = t.DataAplicacao })
            .ToListAsync();

        var prescricoes = await _db.Prescricoes.AsNoTracking()
            .Where(p => p.RegistoClinicoId == id)
            .OrderBy(p => p.Id)
            .Select(p => new PrescricaoDto
            {
                Id = p.Id,
                Medicamento = p.Medicamento,
                Dosagem = p.Dosagem,
                Frequencia = p.Frequencia,
                DuracaoDias = p.DuracaoDias
            })
            .ToListAsync();

        return new RegistoClinicoDto
        {
            Id = linha.Id,
            ConsultaId = linha.MarcacaoId,
            AnimalId = linha.PacienteId,
            Veterinario = linha.Veterinario,
            CriadoEm = Utc(linha.CriadoEm),
            NotasClinicas = linha.NotasClinicas,
            Diagnostico = linha.Diagnostico,
            Tratamentos = tratamentos,
            Prescricoes = prescricoes,
            AtualizadoEm = linha.AtualizadoEm is DateTime atualizado ? Utc(atualizado) : null,
            AtualizadoPor = atualizadoPor
        };
    }

    private static void Validar(UpdateRegistoClinicoDto pedido)
    {
        var erros = ValidadorRegistoClinico.ObterErros(pedido);

        if (erros.Count > 0)
        {
            throw new ValidacaoException(string.Join(" ", erros));
        }
    }

    private static Tratamento NovoTratamento(int registoId, CreateTratamentoDto tratamento) => new()
    {
        RegistoClinicoId = registoId,
        Descricao = tratamento.Descricao.Trim(),
        DataAplicacao = tratamento.DataAplicacao!.Value
    };

    private static Prescricao NovaPrescricao(int registoId, CreatePrescricaoDto prescricao) => new()
    {
        RegistoClinicoId = registoId,
        Medicamento = prescricao.Medicamento.Trim(),
        Dosagem = prescricao.Dosagem.Trim(),
        Frequencia = prescricao.Frequencia.Trim(),
        DuracaoDias = (short?)prescricao.DuracaoDias
    };

    // Texto só com espaços conta como vazio (UT-F004-007) e fica nulo na base de dados.
    private static string? Normalizar(string? texto) =>
        string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();

    private static bool ChaveUnicaViolada(DbUpdateException erro) =>
        erro.InnerException is SqlException sql && sql.Number is ErroIndiceUnico or ErroRestricaoUnica;

    // Sem segundos fracionários, como na coluna datetime2(0).
    private DateTime AgoraAoSegundo()
    {
        var agora = _relogio.GetUtcNow().UtcDateTime;

        return new DateTime(agora.Ticks - (agora.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc);
    }

    // As colunas datetime2 guardam UTC (Modelo de Dados, D15), mas são lidas sem indicação de fuso.
    private static DateTime Utc(DateTime valor) => DateTime.SpecifyKind(valor, DateTimeKind.Utc);

    // A meia-noite existe sempre em Lisboa (as mudanças de hora são à 01:00 UTC).
    private static DateTime InicioDoDiaUtc(DateOnly data, TimeZoneInfo fuso) =>
        FusoHorarioClinica.ParaUtc(data, TimeOnly.MinValue, fuso)
        ?? DateTime.SpecifyKind(data.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
}
