using System.ComponentModel.DataAnnotations;
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
/// I078 — leitura do registo clínico e do histórico do paciente (EP-033, EP-034) no
/// <see cref="RegistosClinicosService"/>, com o <c>ClinicaDbContext</c> em memória (I028, secção
/// 4.2). Casos UT-F004-022 a UT-F004-027 da Especificação de Testes Unitários (I028, secção 7.4);
/// os restantes são próprios desta tarefa.
/// </summary>
/// <remarks>
/// O perfil (Veterinário ou Auxiliar Veterinário) é verificado no controlador
/// (<c>RegistosClinicosControllerTests</c>); o serviço devolve o mesmo registo aos dois, como
/// prevê o contrato v02 (PA-08 da I028). As datas das consultas estão em UTC e as do histórico são
/// dias em hora de Lisboa (R-FUSO).
/// </remarks>
public class RegistosClinicosLeituraServiceTests
{
    private static readonly DateTimeOffset Relogio = new(2026, 10, 20, 8, 0, 0, TimeSpan.Zero);

    // 2026-09-02 09:00 e 2026-10-14 09:30 em Lisboa (hora de verão, UTC+1).
    private static readonly DateTime InicioSetembro = new(2026, 9, 2, 8, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime InicioOutubro = new(2026, 10, 14, 8, 30, 0, DateTimeKind.Utc);

    // ----- EP-033 — Consultar registo clínico -----

    [Fact(DisplayName = "UT-F004-022 — devolve o RegistoClinicoDto completo (notas, diagnóstico, tratamentos e prescrições)")]
    public async Task ObterDevolveRegistoCompleto()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var registo = await CriarServico(db).ObterAsync(d.RegistoOutubro);

        Assert.Equal(d.RegistoOutubro, registo.Id);
        Assert.Equal(d.ConsultaOutubro, registo.ConsultaId);
        Assert.Equal(d.Bobi, registo.AnimalId);
        Assert.Equal("Bruno Vet", registo.Veterinario);
        Assert.Equal("Canal auditivo esquerdo eritematoso.", registo.NotasClinicas);
        Assert.Equal("Otite externa", registo.Diagnostico);
        Assert.Equal(DateTimeKind.Utc, registo.CriadoEm.Kind);
        Assert.Equal("Limpeza auricular", Assert.Single(registo.Tratamentos).Descricao);
        Assert.Equal(2, registo.Prescricoes.Count);
        Assert.Null(registo.AtualizadoEm);
        Assert.Null(registo.AtualizadoPor);
    }

    [Fact(DisplayName = "I078 — EP-033 com id inexistente devolve 404")]
    public async Task ObterRegistoInexistenteDevolve404()
    {
        using var db = AjudaBaseDados.CriarContexto();
        await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<NaoEncontradoException>(() => CriarServico(db).ObterAsync(999));

        Assert.Equal(RegistosClinicosService.MensagemRegistoNaoEncontrado, erro.Message);
    }

    [Fact(DisplayName = "I078 — EP-033 não altera o registo (só leitura)")]
    public async Task ObterNaoAlteraORegisto()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        await CriarServico(db).ObterAsync(d.RegistoOutubro);

        var gravado = await db.RegistosClinicos.AsNoTracking().SingleAsync(r => r.Id == d.RegistoOutubro);
        Assert.Null(gravado.AtualizadoEm);
        Assert.Null(gravado.AtualizadoPorId);
        Assert.Equal("Otite externa", gravado.Diagnostico);
        Assert.Equal(1, await db.Tratamentos.CountAsync(t => t.RegistoClinicoId == d.RegistoOutubro));
        Assert.Equal(2, await db.Prescricoes.CountAsync(p => p.RegistoClinicoId == d.RegistoOutubro));
    }

    // ----- EP-034 — Consultar histórico clínico do paciente -----

    [Fact(DisplayName = "UT-F004-023 — sem datas, devolve os dois registos do animal: primeiro o de 2026-10-14, depois o de 2026-09-02")]
    public async Task HistoricoPorOrdemDescendente()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var historico = await CriarServico(db).ObterHistoricoAsync(d.Bobi, new HistoricoClinicoQuery());

        Assert.Equal(d.Bobi, historico.Animal.Id);
        Assert.Equal("Bobi", historico.Animal.Nome);
        Assert.Equal("Cão", historico.Animal.Especie);
        Assert.Equal(new[] { d.RegistoOutubro, d.RegistoSetembro }, historico.Registos.Select(r => r.Id));
        Assert.Equal(new DateOnly(2026, 10, 14), historico.Registos[0].Data);
        Assert.Equal(new DateOnly(2026, 9, 2), historico.Registos[1].Data);
    }

    [Fact(DisplayName = "UT-F004-024 — o resumo indica numeroTratamentos = 1 e numeroPrescricoes = 2")]
    public async Task ResumoContaTratamentosEPrescricoes()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var historico = await CriarServico(db).ObterHistoricoAsync(d.Bobi, new HistoricoClinicoQuery());

        var outubro = historico.Registos.Single(r => r.Id == d.RegistoOutubro);
        Assert.Equal(d.ConsultaOutubro, outubro.ConsultaId);
        Assert.Equal("Bruno Vet", outubro.Veterinario);
        Assert.Equal("Otite externa", outubro.Diagnostico);
        Assert.Equal(1, outubro.NumeroTratamentos);
        Assert.Equal(2, outubro.NumeroPrescricoes);

        var setembro = historico.Registos.Single(r => r.Id == d.RegistoSetembro);
        Assert.Equal("Ana Vet", setembro.Veterinario);
        Assert.Equal(0, setembro.NumeroTratamentos);
        Assert.Equal(0, setembro.NumeroPrescricoes);
    }

    [Fact(DisplayName = "UT-F004-025 — dataInicio = dataFim = 2026-10-14 devolve só o registo de 2026-10-14")]
    public async Task HistoricoFiltradoPorUmDia()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var dia = new DateOnly(2026, 10, 14);

        var historico = await CriarServico(db).ObterHistoricoAsync(
            d.Bobi,
            new HistoricoClinicoQuery { DataInicio = dia, DataFim = dia });

        Assert.Equal(d.RegistoOutubro, Assert.Single(historico.Registos).Id);
    }

    [Fact(DisplayName = "UT-F004-026 — dataInicio = 2026-10-14 e dataFim = 2026-09-01 devolve 400")]
    public async Task IntervaloInvalidoDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var query = new HistoricoClinicoQuery { DataInicio = new DateOnly(2026, 10, 14), DataFim = new DateOnly(2026, 9, 1) };

        var erro = await Assert.ThrowsAsync<ValidacaoException>(() => CriarServico(db).ObterHistoricoAsync(1, query));

        Assert.Equal(ValidadorPeriodoDisponibilidade.MensagemDataFimAnterior, erro.Message);
    }

    [Fact(DisplayName = "UT-F004-027 — animal inexistente (id = 999) devolve 404")]
    public async Task AnimalInexistenteDevolve404()
    {
        using var db = AjudaBaseDados.CriarContexto();
        await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<NaoEncontradoException>(
            () => CriarServico(db).ObterHistoricoAsync(999, new HistoricoClinicoQuery()));

        Assert.Equal(RegistosClinicosService.MensagemAnimalNaoEncontrado, erro.Message);
    }

    [Fact(DisplayName = "I078 — animal sem registos devolve 200 com a lista vazia (IT-EP034-05)")]
    public async Task AnimalSemRegistosDevolveListaVazia()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var historico = await CriarServico(db).ObterHistoricoAsync(d.Kiko, new HistoricoClinicoQuery());

        Assert.Equal("Kiko", historico.Animal.Nome);
        Assert.Empty(historico.Registos);
    }

    [Fact(DisplayName = "I078 — o histórico não inclui registos de outros animais nem consultas sem registo")]
    public async Task HistoricoSoDoAnimal()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var historico = await CriarServico(db).ObterHistoricoAsync(d.Bobi, new HistoricoClinicoQuery());

        Assert.Equal(2, historico.Registos.Count);
        Assert.DoesNotContain(historico.Registos, r => r.Id == d.RegistoMia);
        Assert.DoesNotContain(historico.Registos, r => r.ConsultaId == d.ConsultaBobiSemRegisto);
    }

    [Fact(DisplayName = "I078 — inclui os registos de todos os Veterinários do animal (RF035)")]
    public async Task HistoricoComRegistosDeVariosVeterinarios()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var historico = await CriarServico(db).ObterHistoricoAsync(d.Bobi, new HistoricoClinicoQuery());

        Assert.Contains(historico.Registos, r => r.Veterinario == "Ana Vet");
        Assert.Contains(historico.Registos, r => r.Veterinario == "Bruno Vet");
    }

    [Fact(DisplayName = "I078 — a data é o dia da consulta em hora de Lisboa: 2026-10-13T23:30Z conta como 2026-10-14 (R-FUSO)")]
    public async Task DataEmHoraDeLisboa()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var registo = await AdicionarRegistoAsync(
            db, d.Kiko, d.VetA, new DateTime(2026, 10, 13, 23, 30, 0, DateTimeKind.Utc), "Vómitos", 0, 0);
        var servico = CriarServico(db);

        var dia13 = await servico.ObterHistoricoAsync(
            d.Kiko,
            new HistoricoClinicoQuery { DataInicio = new DateOnly(2026, 10, 13), DataFim = new DateOnly(2026, 10, 13) });
        var dia14 = await servico.ObterHistoricoAsync(
            d.Kiko,
            new HistoricoClinicoQuery { DataInicio = new DateOnly(2026, 10, 14), DataFim = new DateOnly(2026, 10, 14) });

        Assert.Empty(dia13.Registos);
        var resumo = Assert.Single(dia14.Registos);
        Assert.Equal(registo.RegistoId, resumo.Id);
        Assert.Equal(new DateOnly(2026, 10, 14), resumo.Data);
    }

    [Fact(DisplayName = "I078 — só dataInicio ou só dataFim também filtram")]
    public async Task HistoricoComUmaSoData()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var servico = CriarServico(db);

        var desde = await servico.ObterHistoricoAsync(d.Bobi, new HistoricoClinicoQuery { DataInicio = new DateOnly(2026, 10, 1) });
        var ate = await servico.ObterHistoricoAsync(d.Bobi, new HistoricoClinicoQuery { DataFim = new DateOnly(2026, 9, 30) });

        Assert.Equal(d.RegistoOutubro, Assert.Single(desde.Registos).Id);
        Assert.Equal(d.RegistoSetembro, Assert.Single(ate.Registos).Id);
    }

    [Fact(DisplayName = "I078 — datas fora de 2000-01-01 a 9998-12-31 devolvem 400")]
    public async Task DataForaDosLimitesDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var query = new HistoricoClinicoQuery { DataFim = new DateOnly(9999, 12, 31) };

        var erro = await Assert.ThrowsAsync<ValidacaoException>(() => CriarServico(db).ObterHistoricoAsync(1, query));

        Assert.Equal(ValidadorPeriodoDisponibilidade.MensagemForaDosLimites, erro.Message);
    }

    [Theory(DisplayName = "I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio")]
    [InlineData(null, null, true)]
    [InlineData("2026-10-14", null, true)]
    [InlineData(null, "2026-10-14", true)]
    [InlineData("2026-10-14", "2026-10-14", true)]
    [InlineData("2026-10-14", "2026-09-01", false)]
    [InlineData("1999-12-31", null, false)]
    public void ValidacaoDaQuery(string? dataInicio, string? dataFim, bool valido)
    {
        var query = new HistoricoClinicoQuery
        {
            DataInicio = dataInicio is null ? null : DateOnly.Parse(dataInicio, CultureInfo.InvariantCulture),
            DataFim = dataFim is null ? null : DateOnly.Parse(dataFim, CultureInfo.InvariantCulture)
        };
        var resultados = new List<ValidationResult>();

        var resultado = Validator.TryValidateObject(query, new ValidationContext(query), resultados, validateAllProperties: true);

        Assert.Equal(valido, resultado);
    }

    // ----- Auxiliares -----

    private static RegistosClinicosService CriarServico(ClinicaDbContext db) => new(db, new RelogioFixo(Relogio));

    /// <summary>
    /// Dados de referência: VET_A (Ana Vet) e VET_B (Bruno Vet); Bobi com um registo de VET_A a
    /// 2026-09-02 (sem tratamentos nem prescrições), um registo de VET_B a 2026-10-14 (1 tratamento
    /// e 2 prescrições) e uma consulta Confirmada sem registo; Mia (outro animal) com um registo a
    /// 2026-10-14; Kiko sem registos.
    /// </summary>
    private static async Task<DadosBase> CriarBaseAsync(ClinicaDbContext db)
    {
        var vetA = await AdicionarFuncionarioAsync(db, "Ana Vet", Perfis.Veterinario);
        var vetB = await AdicionarFuncionarioAsync(db, "Bruno Vet", Perfis.Veterinario);

        var cao = new Especie { Nome = "Cão" };
        var gato = new Especie { Nome = "Gato" };
        db.Especies.AddRange(cao, gato);
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

        var bobi = new Paciente { ClienteId = cliente.Id, Nome = "Bobi", EspecieId = cao.Id, DataRegisto = Relogio.UtcDateTime };
        var mia = new Paciente { ClienteId = cliente.Id, Nome = "Mia", EspecieId = gato.Id, DataRegisto = Relogio.UtcDateTime };
        var kiko = new Paciente { ClienteId = cliente.Id, Nome = "Kiko", EspecieId = cao.Id, DataRegisto = Relogio.UtcDateTime };
        db.Pacientes.AddRange(bobi, mia, kiko);
        await db.SaveChangesAsync();

        var setembro = await AdicionarRegistoAsync(db, bobi.Id, vetA, InicioSetembro, "Check-up sem alterações", 0, 0);
        var outubro = await AdicionarRegistoAsync(db, bobi.Id, vetB, InicioOutubro, "Otite externa", 1, 2);
        var registoMia = await AdicionarRegistoAsync(db, mia.Id, vetA, InicioOutubro.AddHours(1), "Dermatite", 0, 0);
        var semRegisto = await AdicionarMarcacaoAsync(db, bobi.Id, vetA, Relogio.UtcDateTime.AddDays(10), EstadosConsulta.Confirmada);

        db.ChangeTracker.Clear();

        return new DadosBase(
            vetA, bobi.Id, mia.Id, kiko.Id, setembro.RegistoId, outubro.RegistoId, outubro.MarcacaoId,
            registoMia.RegistoId, semRegisto);
    }

    /// <summary>Consulta Realizada do animal, com o registo clínico e os tratamentos e prescrições indicados.</summary>
    private static async Task<(int MarcacaoId, int RegistoId)> AdicionarRegistoAsync(
        ClinicaDbContext db,
        int pacienteId,
        int veterinarioId,
        DateTime inicioUtc,
        string diagnostico,
        int tratamentos,
        int prescricoes)
    {
        var marcacaoId = await AdicionarMarcacaoAsync(db, pacienteId, veterinarioId, inicioUtc, EstadosConsulta.Realizada);

        var registo = new RegistoClinico
        {
            MarcacaoId = marcacaoId,
            VeterinarioId = veterinarioId,
            NotasClinicas = "Canal auditivo esquerdo eritematoso.",
            Diagnostico = diagnostico,
            CriadoEm = inicioUtc.AddMinutes(30)
        };
        db.RegistosClinicos.Add(registo);
        await db.SaveChangesAsync();

        for (var i = 0; i < tratamentos; i++)
        {
            db.Tratamentos.Add(new Tratamento
            {
                RegistoClinicoId = registo.Id,
                Descricao = "Limpeza auricular",
                DataAplicacao = DateOnly.FromDateTime(inicioUtc)
            });
        }

        for (var i = 0; i < prescricoes; i++)
        {
            db.Prescricoes.Add(new Prescricao
            {
                RegistoClinicoId = registo.Id,
                Medicamento = "Gotas auriculares X",
                Dosagem = "3 gotas",
                Frequencia = "2 vezes por dia",
                DuracaoDias = 7
            });
        }

        await db.SaveChangesAsync();

        return (marcacaoId, registo.Id);
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

    private static async Task<int> AdicionarMarcacaoAsync(
        ClinicaDbContext db,
        int pacienteId,
        int veterinarioId,
        DateTime inicioUtc,
        string estado)
    {
        var marcacao = new Marcacao
        {
            PacienteId = pacienteId,
            VeterinarioId = veterinarioId,
            DataHoraInicio = inicioUtc,
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
        int Bobi,
        int Mia,
        int Kiko,
        int RegistoSetembro,
        int RegistoOutubro,
        int ConsultaOutubro,
        int RegistoMia,
        int ConsultaBobiSemRegisto);
}
