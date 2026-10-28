using System.Globalization;
using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Application.DTOs.RegistosClinicos;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Application.Validacao;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.UnitTests.Seguranca;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.UnitTests.RegistosClinicos;

/// <summary>
/// I077 — <see cref="RegistosClinicosService"/> (EP-031, EP-032) com o <c>ClinicaDbContext</c> em
/// memória (I028, secção 4.2). Casos UT-F004-001 a UT-F004-021 da Especificação de Testes
/// Unitários (I028, secção 7.4); os restantes são próprios desta tarefa.
/// </summary>
/// <remarks>
/// <c>RELOGIO</c> é 2026-10-14T08:00:00Z. A base em memória não tem o índice único
/// <c>UQ_RegistoClinico_Marcacao</c> nem transações: a criação simultânea e a ausência de gravações
/// parciais são verificadas nos testes de integração, com o SQL Server.
/// </remarks>
public class RegistosClinicosServiceTests
{
    private static readonly DateTimeOffset Relogio = new(2026, 10, 14, 8, 0, 0, TimeSpan.Zero);

    private static readonly DateOnly Hoje = new(2026, 10, 14);

    // ----- EP-031 — Criar registo clínico -----

    [Fact(DisplayName = "UT-F004-001 — cria o registo com tratamento e prescrição; o estado da marcação não muda")]
    public async Task CriaRegistoCompleto()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var registo = await CriarServico(db).CriarAsync(PedidoExemplo(d.ConsultaConfirmada), d.VetA);

        Assert.Equal(d.ConsultaConfirmada, registo.ConsultaId);
        Assert.Equal(d.AnimalA1, registo.AnimalId);
        Assert.Equal("Ana Vet", registo.Veterinario);
        Assert.Equal(Relogio.UtcDateTime, registo.CriadoEm);
        Assert.Equal(DateTimeKind.Utc, registo.CriadoEm.Kind);
        Assert.Equal("Animal ativo; ligeira inflamação no ouvido esquerdo.", registo.NotasClinicas);
        Assert.Equal("Otite externa", registo.Diagnostico);
        Assert.Null(registo.AtualizadoEm);
        Assert.Null(registo.AtualizadoPor);

        var tratamento = Assert.Single(registo.Tratamentos);
        Assert.Equal("Limpeza auricular", tratamento.Descricao);
        Assert.Equal(Hoje, tratamento.DataAplicacao);

        var prescricao = Assert.Single(registo.Prescricoes);
        Assert.Equal("Gotas auriculares X", prescricao.Medicamento);
        Assert.Equal(7, prescricao.DuracaoDias);

        var gravado = await db.RegistosClinicos.AsNoTracking().SingleAsync(r => r.Id == registo.Id);
        Assert.Equal(d.VetA, gravado.VeterinarioId);
        Assert.Equal(d.ConsultaConfirmada, gravado.MarcacaoId);

        var marcacao = await db.Marcacoes.AsNoTracking().SingleAsync(m => m.Id == d.ConsultaConfirmada);
        Assert.Equal(EstadosConsulta.Confirmada, marcacao.Estado);
    }

    [Fact(DisplayName = "UT-F004-002 — sem notas nem diagnóstico, só com uma prescrição completa, é aceite")]
    public async Task SoPrescricaoEAceite()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var pedido = new CreateRegistoClinicoDto { ConsultaId = d.ConsultaConfirmada, Prescricoes = [Prescricao()] };

        var registo = await CriarServico(db).CriarAsync(pedido, d.VetA);

        Assert.Null(registo.NotasClinicas);
        Assert.Null(registo.Diagnostico);
        Assert.Single(registo.Prescricoes);
        Assert.Empty(registo.Tratamentos);
    }

    [Fact(DisplayName = "UT-F004-003 — sem notas, diagnóstico nem prescrições, só com um tratamento, é aceite")]
    public async Task SoTratamentoEAceite()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var pedido = new CreateRegistoClinicoDto { ConsultaId = d.ConsultaConfirmada, Tratamentos = [Tratamento()] };

        var registo = await CriarServico(db).CriarAsync(pedido, d.VetA);

        Assert.Single(registo.Tratamentos);
        Assert.Empty(registo.Prescricoes);
    }

    [Fact(DisplayName = "UT-F004-004 — consulta Realizada sem registo é aceite (Confirmada e Realizada são admitidas)")]
    public async Task ConsultaRealizadaEAceite()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var registo = await CriarServico(db).CriarAsync(PedidoExemplo(d.ConsultaRealizadaSemRegisto), d.VetA);

        Assert.Equal(d.ConsultaRealizadaSemRegisto, registo.ConsultaId);
    }

    [Theory(DisplayName = "UT-F004-005 — consultaId = 0, ou em falta, devolve 400")]
    [InlineData(0)]
    [InlineData(null)]
    public async Task ConsultaIdInvalidoDevolve400(int? consultaId)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var pedido = new CreateRegistoClinicoDto { ConsultaId = consultaId, Diagnostico = "Otite externa" };

        var erro = await Assert.ThrowsAsync<ArgumentException>(() => CriarServico(db).CriarAsync(pedido, 1));

        Assert.Contains("consultaId", erro.Message, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "UT-F004-006 — notas e diagnóstico vazios e listas vazias devolvem 400 (nenhum elemento clínico)")]
    public async Task SemElementoClinicoDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var pedido = new CreateRegistoClinicoDto
        {
            ConsultaId = d.ConsultaConfirmada,
            NotasClinicas = "",
            Diagnostico = "",
            Tratamentos = [],
            Prescricoes = []
        };

        var erro = await Assert.ThrowsAsync<ArgumentException>(() => CriarServico(db).CriarAsync(pedido, d.VetA));

        Assert.Equal(ValidadorRegistoClinico.MensagemSemElementoClinico, erro.Message);
        Assert.False(await db.RegistosClinicos.AnyAsync(r => r.MarcacaoId == d.ConsultaConfirmada));
    }

    [Fact(DisplayName = "UT-F004-007 — notas só com espaços e listas vazias devolvem 400")]
    public async Task NotasSoComEspacosDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var pedido = new CreateRegistoClinicoDto { ConsultaId = d.ConsultaConfirmada, NotasClinicas = "   " };

        var erro = await Assert.ThrowsAsync<ArgumentException>(() => CriarServico(db).CriarAsync(pedido, d.VetA));

        Assert.Equal(ValidadorRegistoClinico.MensagemSemElementoClinico, erro.Message);
    }

    [Theory(DisplayName = "UT-F004-008 — prescrição sem medicamento, sem dosagem ou sem frequência devolve 400")]
    [InlineData("medicamento")]
    [InlineData("dosagem")]
    [InlineData("frequência")]
    public async Task PrescricaoIncompletaDevolve400(string campoEmFalta)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var prescricao = Prescricao();

        switch (campoEmFalta)
        {
            case "medicamento":
                prescricao.Medicamento = "";
                break;
            case "dosagem":
                prescricao.Dosagem = "";
                break;
            default:
                prescricao.Frequencia = "";
                break;
        }

        var pedido = new CreateRegistoClinicoDto { ConsultaId = d.ConsultaConfirmada, Prescricoes = [prescricao] };

        var erro = await Assert.ThrowsAsync<ArgumentException>(() => CriarServico(db).CriarAsync(pedido, d.VetA));

        Assert.Contains(campoEmFalta, erro.Message, StringComparison.Ordinal);
        Assert.False(await db.RegistosClinicos.AnyAsync(r => r.MarcacaoId == d.ConsultaConfirmada));
    }

    [Fact(DisplayName = "UT-F004-009 — prescrição com duracaoDias = 0 devolve 400")]
    public async Task DuracaoZeroDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var prescricao = Prescricao();
        prescricao.DuracaoDias = 0;
        var pedido = new CreateRegistoClinicoDto { ConsultaId = d.ConsultaConfirmada, Prescricoes = [prescricao] };

        await Assert.ThrowsAsync<ArgumentException>(() => CriarServico(db).CriarAsync(pedido, d.VetA));
    }

    [Fact(DisplayName = "UT-F004-010 — prescrição sem duracaoDias é aceite")]
    public async Task PrescricaoSemDuracaoEAceite()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var prescricao = Prescricao();
        prescricao.DuracaoDias = null;
        var pedido = new CreateRegistoClinicoDto { ConsultaId = d.ConsultaConfirmada, Prescricoes = [prescricao] };

        var registo = await CriarServico(db).CriarAsync(pedido, d.VetA);

        Assert.Null(Assert.Single(registo.Prescricoes).DuracaoDias);
    }

    [Fact(DisplayName = "UT-F004-011 — tratamento com descrição vazia devolve 400")]
    public async Task TratamentoSemDescricaoDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var tratamento = Tratamento();
        tratamento.Descricao = "";
        var pedido = new CreateRegistoClinicoDto { ConsultaId = d.ConsultaConfirmada, Tratamentos = [tratamento] };

        await Assert.ThrowsAsync<ArgumentException>(() => CriarServico(db).CriarAsync(pedido, d.VetA));
    }

    [Fact(DisplayName = "UT-F004-012 — diagnóstico com 1001 caracteres devolve 400 (máximo 1000)")]
    public async Task DiagnosticoDemasiadoLongoDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var pedido = new CreateRegistoClinicoDto { ConsultaId = d.ConsultaConfirmada, Diagnostico = new string('x', 1001) };

        await Assert.ThrowsAsync<ArgumentException>(() => CriarServico(db).CriarAsync(pedido, d.VetA));
    }

    [Fact(DisplayName = "UT-F004-013 — segundo registo para a mesma consulta devolve 409 e nada é gravado")]
    public async Task SegundoRegistoDevolve409()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<InvalidOperationException>(
            () => CriarServico(db).CriarAsync(PedidoExemplo(d.ConsultaRealizada), d.VetA));

        Assert.Equal(RegistosClinicosService.MensagemJaTemRegisto, erro.Message);
        Assert.Equal(1, await db.RegistosClinicos.CountAsync(r => r.MarcacaoId == d.ConsultaRealizada));
        Assert.Equal(2, await db.Tratamentos.CountAsync(t => t.RegistoClinicoId == d.RegistoA));
        Assert.Equal(1, await db.Prescricoes.CountAsync(p => p.RegistoClinicoId == d.RegistoA));
    }

    [Fact(DisplayName = "UT-F004-014 — consulta Pendente devolve 422")]
    public async Task ConsultaPendenteDevolve422()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<UnprocessableEntityException>(
            () => CriarServico(db).CriarAsync(PedidoExemplo(d.ConsultaPendente), d.VetA));

        Assert.Equal(RegistosClinicosService.MensagemEstadoNaoAdmitido, erro.Message);
    }

    [Fact(DisplayName = "UT-F004-015 — consulta Cancelada devolve 422")]
    public async Task ConsultaCanceladaDevolve422()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<UnprocessableEntityException>(
            () => CriarServico(db).CriarAsync(PedidoExemplo(d.ConsultaCancelada), d.VetA));

        Assert.Equal(RegistosClinicosService.MensagemEstadoNaoAdmitido, erro.Message);
    }

    [Fact(DisplayName = "UT-F004-016 — consulta inexistente devolve 404")]
    public async Task ConsultaInexistenteDevolve404()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => CriarServico(db).CriarAsync(PedidoExemplo(999), d.VetA));

        Assert.Equal(RegistosClinicosService.MensagemConsultaNaoEncontrada, erro.Message);
    }

    [Fact(DisplayName = "UT-F004-017 — consulta de outro Veterinário devolve 403")]
    public async Task ConsultaDeOutroVeterinarioDevolve403()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<ForbiddenException>(
            () => CriarServico(db).CriarAsync(PedidoExemplo(d.ConsultaConfirmadaVetB), d.VetA));

        Assert.Equal(RegistosClinicosService.MensagemConsultaDeOutroVeterinario, erro.Message);
        Assert.False(await db.RegistosClinicos.AnyAsync(r => r.MarcacaoId == d.ConsultaConfirmadaVetB));
    }

    [Fact(DisplayName = "I077 — os textos são aparados e as notas só com espaços ficam nulas")]
    public async Task TextosAparados()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var tratamento = Tratamento();
        tratamento.Descricao = "  Limpeza auricular  ";
        var pedido = new CreateRegistoClinicoDto
        {
            ConsultaId = d.ConsultaConfirmada,
            NotasClinicas = "   ",
            Diagnostico = "  Otite externa ",
            Tratamentos = [tratamento]
        };

        var registo = await CriarServico(db).CriarAsync(pedido, d.VetA);

        Assert.Null(registo.NotasClinicas);
        Assert.Equal("Otite externa", registo.Diagnostico);
        Assert.Equal("Limpeza auricular", Assert.Single(registo.Tratamentos).Descricao);
    }

    // ----- EP-032 — Atualizar registo clínico -----

    [Fact(DisplayName = "UT-F004-018 — mantém T1 atualizado, acrescenta um novo, remove T2 e P1; regista autor e data")]
    public async Task AtualizaListasERegistaAutor()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var pedido = new UpdateRegistoClinicoDto
        {
            NotasClinicas = "Melhoria evidente.",
            Diagnostico = "Otite externa (em resolução)",
            Tratamentos =
            [
                new CreateTratamentoDto { Id = d.T1, Descricao = "Limpeza auricular (repetida)", DataAplicacao = Hoje },
                new CreateTratamentoDto { Descricao = "Aplicação de pomada", DataAplicacao = Hoje }
            ],
            Prescricoes = []
        };

        var registo = await CriarServico(db).AtualizarAsync(d.RegistoA, pedido, d.VetA);

        Assert.Equal(d.ConsultaRealizada, registo.ConsultaId);
        Assert.Equal(d.AnimalA1, registo.AnimalId);
        Assert.Equal("Melhoria evidente.", registo.NotasClinicas);
        Assert.Equal(Relogio.UtcDateTime, registo.AtualizadoEm);
        Assert.Equal("Ana Vet", registo.AtualizadoPor);
        Assert.Empty(registo.Prescricoes);
        Assert.Collection(
            registo.Tratamentos,
            t =>
            {
                Assert.Equal(d.T1, t.Id);
                Assert.Equal("Limpeza auricular (repetida)", t.Descricao);
            },
            t =>
            {
                Assert.NotEqual(d.T2, t.Id);
                Assert.Equal("Aplicação de pomada", t.Descricao);
            });

        var gravado = await db.RegistosClinicos.AsNoTracking().SingleAsync(r => r.Id == d.RegistoA);
        Assert.Equal(d.ConsultaRealizada, gravado.MarcacaoId);
        Assert.Equal(d.VetA, gravado.AtualizadoPorId);
        Assert.False(await db.Tratamentos.AnyAsync(t => t.Id == d.T2));
        Assert.False(await db.Prescricoes.AnyAsync(p => p.Id == d.P1));
    }

    [Fact(DisplayName = "UT-F004-019 — id de tratamento que pertence a outro registo devolve 400")]
    public async Task TratamentoDeOutroRegistoDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var pedido = new UpdateRegistoClinicoDto
        {
            Diagnostico = "Otite externa",
            Tratamentos = [new CreateTratamentoDto { Id = d.T9, Descricao = "Outro", DataAplicacao = Hoje }]
        };

        var erro = await Assert.ThrowsAsync<ArgumentException>(
            () => CriarServico(db).AtualizarAsync(d.RegistoA, pedido, d.VetA));

        Assert.Equal(RegistosClinicosService.MensagemTratamentoDeOutroRegisto, erro.Message);
        Assert.Equal(2, await db.Tratamentos.CountAsync(t => t.RegistoClinicoId == d.RegistoA));
    }

    [Fact(DisplayName = "UT-F004-020 — atualização com tudo vazio devolve 400 e o registo fica como estava")]
    public async Task AtualizacaoVaziaDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<ArgumentException>(
            () => CriarServico(db).AtualizarAsync(d.RegistoA, new UpdateRegistoClinicoDto(), d.VetA));

        Assert.Equal(ValidadorRegistoClinico.MensagemSemElementoClinico, erro.Message);
        var gravado = await db.RegistosClinicos.AsNoTracking().SingleAsync(r => r.Id == d.RegistoA);
        Assert.Equal("Otite externa", gravado.Diagnostico);
        Assert.Null(gravado.AtualizadoEm);
    }

    [Fact(DisplayName = "UT-F004-021 — registo inexistente devolve 404")]
    public async Task RegistoInexistenteDevolve404()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => CriarServico(db).AtualizarAsync(999, new UpdateRegistoClinicoDto { Diagnostico = "x" }, d.VetA));

        Assert.Equal(RegistosClinicosService.MensagemRegistoNaoEncontrado, erro.Message);
    }

    [Fact(DisplayName = "I077 — registo de outro Veterinário devolve 403 e não é alterado")]
    public async Task RegistoDeOutroVeterinarioDevolve403()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<ForbiddenException>(() => CriarServico(db).AtualizarAsync(
            d.RegistoA, new UpdateRegistoClinicoDto { Diagnostico = "Outro" }, d.VetB));

        Assert.Equal(RegistosClinicosService.MensagemRegistoDeOutroVeterinario, erro.Message);
        Assert.Equal("Otite externa", (await db.RegistosClinicos.AsNoTracking().SingleAsync(r => r.Id == d.RegistoA)).Diagnostico);
    }

    [Fact(DisplayName = "I077 — o mesmo id de tratamento repetido no pedido devolve 400")]
    public async Task IdRepetidoDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var pedido = new UpdateRegistoClinicoDto
        {
            Tratamentos =
            [
                new CreateTratamentoDto { Id = d.T1, Descricao = "A", DataAplicacao = Hoje },
                new CreateTratamentoDto { Id = d.T1, Descricao = "B", DataAplicacao = Hoje }
            ]
        };

        var erro = await Assert.ThrowsAsync<ArgumentException>(
            () => CriarServico(db).AtualizarAsync(d.RegistoA, pedido, d.VetA));

        Assert.Equal(ValidadorRegistoClinico.MensagemTratamentoRepetido, erro.Message);
    }

    [Fact(DisplayName = "I077 — uma prescrição existente mantém-se indicando o id e é atualizada")]
    public async Task PrescricaoExistenteAtualizada()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var pedido = new UpdateRegistoClinicoDto
        {
            Prescricoes =
            [
                new CreatePrescricaoDto { Id = d.P1, Medicamento = "Gotas auriculares X", Dosagem = "2 gotas", Frequencia = "1 vez por dia", DuracaoDias = 10 }
            ]
        };

        var registo = await CriarServico(db).AtualizarAsync(d.RegistoA, pedido, d.VetA);

        var prescricao = Assert.Single(registo.Prescricoes);
        Assert.Equal(d.P1, prescricao.Id);
        Assert.Equal("2 gotas", prescricao.Dosagem);
        Assert.Equal(10, prescricao.DuracaoDias);
        Assert.Empty(registo.Tratamentos);
    }

    // ----- Auxiliares -----

    private static RegistosClinicosService CriarServico(ClinicaDbContext db) => new(db, new RelogioFixo(Relogio));

    private static CreateTratamentoDto Tratamento() =>
        new() { Descricao = "Limpeza auricular", DataAplicacao = Hoje };

    private static CreatePrescricaoDto Prescricao() =>
        new() { Medicamento = "Gotas auriculares X", Dosagem = "3 gotas", Frequencia = "2 vezes por dia", DuracaoDias = 7 };

    /// <summary>O pedido de exemplo da Documentação de API (EP-031).</summary>
    private static CreateRegistoClinicoDto PedidoExemplo(int consultaId) => new()
    {
        ConsultaId = consultaId,
        NotasClinicas = "Animal ativo; ligeira inflamação no ouvido esquerdo.",
        Diagnostico = "Otite externa",
        Tratamentos = [Tratamento()],
        Prescricoes = [Prescricao()]
    };

    /// <summary>
    /// Dados de referência da I028 (secção 5): VET_A e VET_B, ANIMAL_A1, as consultas de VET_A em
    /// cada estado, uma consulta Confirmada de VET_B, e REGISTO_CLINICO_A (consulta Realizada) com
    /// os tratamentos T1 e T2 e a prescrição P1. T9 é um tratamento de outro registo.
    /// </summary>
    private static async Task<DadosBase> CriarBaseAsync(ClinicaDbContext db)
    {
        var vetA = await AdicionarFuncionarioAsync(db, "Ana Vet", Perfis.Veterinario);
        var vetB = await AdicionarFuncionarioAsync(db, "Bruno Vet", Perfis.Veterinario);

        var especie = new Especie { Nome = "Cão" };
        db.Especies.Add(especie);
        await db.SaveChangesAsync();

        var utilizadorCliente = new Utilizador
        {
            Email = "cliente.a@clinica-g08.test",
            PasswordHash = "hash-nao-usado-nestes-testes",
            Perfil = Perfis.Cliente,
            Estado = EstadosUtilizador.Ativo,
            DataCriacao = Relogio.UtcDateTime
        };
        db.Utilizadores.Add(utilizadorCliente);
        await db.SaveChangesAsync();

        var cliente = new Cliente
        {
            UtilizadorId = utilizadorCliente.Id,
            Nome = "Joana Cliente",
            Telefone = "960000001",
            DataRegisto = Relogio.UtcDateTime
        };
        db.Clientes.Add(cliente);
        await db.SaveChangesAsync();

        var animal = new Paciente { ClienteId = cliente.Id, Nome = "Bobi", EspecieId = especie.Id, DataRegisto = Relogio.UtcDateTime };
        db.Pacientes.Add(animal);
        await db.SaveChangesAsync();

        var pendente = await AdicionarMarcacaoAsync(db, animal.Id, vetA, EstadosConsulta.Pendente);
        var confirmada = await AdicionarMarcacaoAsync(db, animal.Id, vetA, EstadosConsulta.Confirmada);
        var realizada = await AdicionarMarcacaoAsync(db, animal.Id, vetA, EstadosConsulta.Realizada);
        var realizadaSemRegisto = await AdicionarMarcacaoAsync(db, animal.Id, vetA, EstadosConsulta.Realizada);
        var cancelada = await AdicionarMarcacaoAsync(db, animal.Id, vetA, EstadosConsulta.Cancelada);
        var confirmadaVetB = await AdicionarMarcacaoAsync(db, animal.Id, vetB, EstadosConsulta.Confirmada);
        var realizadaVetB = await AdicionarMarcacaoAsync(db, animal.Id, vetB, EstadosConsulta.Realizada);

        var registoA = new RegistoClinico
        {
            MarcacaoId = realizada,
            VeterinarioId = vetA,
            Diagnostico = "Otite externa",
            CriadoEm = Relogio.UtcDateTime.AddDays(-1)
        };
        var registoB = new RegistoClinico
        {
            MarcacaoId = realizadaVetB,
            VeterinarioId = vetB,
            Diagnostico = "Dermatite",
            CriadoEm = Relogio.UtcDateTime.AddDays(-1)
        };
        db.RegistosClinicos.AddRange(registoA, registoB);
        await db.SaveChangesAsync();

        var t1 = new Tratamento { RegistoClinicoId = registoA.Id, Descricao = "Limpeza auricular", DataAplicacao = Hoje.AddDays(-1) };
        var t2 = new Tratamento { RegistoClinicoId = registoA.Id, Descricao = "Corte de pelo", DataAplicacao = Hoje.AddDays(-1) };
        var t9 = new Tratamento { RegistoClinicoId = registoB.Id, Descricao = "Banho", DataAplicacao = Hoje.AddDays(-1) };
        var p1 = new Prescricao { RegistoClinicoId = registoA.Id, Medicamento = "Gotas X", Dosagem = "3 gotas", Frequencia = "2 vezes por dia", DuracaoDias = 7 };
        db.Tratamentos.AddRange(t1, t2, t9);
        db.Prescricoes.Add(p1);
        await db.SaveChangesAsync();

        db.ChangeTracker.Clear();

        return new DadosBase(
            vetA, vetB, animal.Id, pendente, confirmada, realizada, realizadaSemRegisto, cancelada, confirmadaVetB,
            registoA.Id, t1.Id, t2.Id, t9.Id, p1.Id);
    }

    private static async Task<int> AdicionarFuncionarioAsync(ClinicaDbContext db, string nome, string perfil)
    {
        var utilizador = new Utilizador
        {
            Email = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "@clinica-g08.test",
            PasswordHash = "hash-nao-usado-nestes-testes",
            Perfil = perfil,
            Estado = EstadosUtilizador.Ativo,
            DataCriacao = Relogio.UtcDateTime
        };
        db.Utilizadores.Add(utilizador);
        await db.SaveChangesAsync();

        db.Funcionarios.Add(new Funcionario { UtilizadorId = utilizador.Id, Nome = nome });
        await db.SaveChangesAsync();

        return utilizador.Id;
    }

    private static async Task<int> AdicionarMarcacaoAsync(ClinicaDbContext db, int pacienteId, int veterinarioId, string estado)
    {
        var marcacao = new Marcacao
        {
            PacienteId = pacienteId,
            VeterinarioId = veterinarioId,
            DataHoraInicio = Relogio.UtcDateTime.AddHours(1),
            DuracaoMinutos = 30,
            Estado = estado,
            CriadaPorId = 1,
            DataCriacao = Relogio.UtcDateTime
        };
        db.Marcacoes.Add(marcacao);
        await db.SaveChangesAsync();

        return marcacao.Id;
    }

    private sealed record DadosBase(
        int VetA,
        int VetB,
        int AnimalA1,
        int ConsultaPendente,
        int ConsultaConfirmada,
        int ConsultaRealizada,
        int ConsultaRealizadaSemRegisto,
        int ConsultaCancelada,
        int ConsultaConfirmadaVetB,
        int RegistoA,
        int T1,
        int T2,
        int T9,
        int P1);
}
