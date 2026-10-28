using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Application.DTOs.RegistosClinicos;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Validacao;
using ClinicaVeterinaria.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Infrastructure;

/// <summary>
/// Registo clínico das consultas (EP-031, EP-032). Só o Veterinário a quem a consulta está
/// atribuída cria ou altera o registo (Documentação de API, secção 3.3; UT-F004-017). Cada consulta
/// tem no máximo um registo (índice único <c>UQ_RegistoClinico_Marcacao</c>).
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

    /// <summary>EP-032 — registo inexistente (404).</summary>
    public const string MensagemRegistoNaoEncontrado = "O registo clínico indicado não existe.";

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
            throw new InvalidOperationException(MensagemJaTemRegisto);
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
            throw new InvalidOperationException(MensagemJaTemRegisto, erro);
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
            throw new ArgumentException(MensagemTratamentoDeOutroRegisto);
        }

        if (prescricoesPedido.Any(p => p.Id is int pid && prescricoesAtuais.TrueForAll(a => a.Id != pid)))
        {
            throw new ArgumentException(MensagemPrescricaoDeOutroRegisto);
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
            throw new ArgumentException(string.Join(" ", erros));
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
}
