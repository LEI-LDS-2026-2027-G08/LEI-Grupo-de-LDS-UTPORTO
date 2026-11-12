using System.Globalization;
using ClinicaVeterinaria.Application.DTOs.Vacinacoes;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Application.Vacinacao;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.UnitTests.Seguranca;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.UnitTests.Vacinacao;

/// <summary>
/// I082 — <see cref="VacinacoesService"/> (EP-042, EP-043, EP-044 e a atualização do RF056) com o
/// <c>ClinicaDbContext</c> em memória (I028, secção 4.2). Casos UT-F006-008 a UT-F006-024 da
/// Especificação de Testes Unitários (I028, secção 7.5.2); os restantes são próprios desta tarefa.
/// </summary>
/// <remarks>
/// <c>RELOGIO</c> é 2026-10-14T08:00:00Z (09:00 em Lisboa), por isso «hoje» é 2026-10-14. A base em
/// memória não calcula a coluna <c>DataProximaAdministracao</c>: a data prevista vem sempre da
/// <see cref="CalculadoraProximaAdministracao"/>, que os testes de integração confrontam com a coluna
/// calculada do SQL Server.
/// </remarks>
public class VacinacoesServiceTests
{
    private static readonly DateTimeOffset Relogio = new(2026, 10, 14, 8, 0, 0, TimeSpan.Zero);

    private static readonly DateOnly Hoje = new(2026, 10, 14);

    // ----- EP-043 — Registar vacinação ou desparasitação -----

    [Fact(DisplayName = "UT-F006-008 — vacina com intervalo de 365 dias: registo do VET_A e próxima a 2027-10-14")]
    public async Task RegistaVacinaComIntervalo()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var administracao = await CriarServico(db).RegistarAsync(d.AnimalA1, Pedido(), d.VetA);

        Assert.Equal(TiposAdministracao.Vacina, administracao.Tipo);
        Assert.Equal("Vacina antirrábica", administracao.Produto);
        Assert.Equal("RB-2291", administracao.Lote);
        Assert.Equal(Hoje, administracao.DataAdministracao);
        Assert.Equal(365, administracao.IntervaloDias);
        Assert.Equal(new DateOnly(2027, 10, 14), administracao.DataProximaPrevista);
        Assert.Equal("Ana Vet", administracao.Veterinario);

        var gravado = await db.RegistosProfilaxias.AsNoTracking().SingleAsync(r => r.Id == administracao.Id);
        Assert.Equal(d.VetA, gravado.VeterinarioId);
        Assert.Equal(d.AnimalA1, gravado.PacienteId);
        Assert.Equal((short)365, gravado.IntervaloDias);
        Assert.Equal(Relogio.UtcDateTime, gravado.DataRegisto);
    }

    [Fact(DisplayName = "UT-F006-009 — desparasitação sem intervalo: dataProximaPrevista = null")]
    public async Task RegistaDesparasitacaoSemIntervalo()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var pedido = Pedido(tipo: TiposAdministracao.Desparasitacao, produto: "Desparasitante interno Y", lote: "DP-0042", intervaloDias: null);

        var administracao = await CriarServico(db).RegistarAsync(d.AnimalA1, pedido, d.VetA);

        Assert.Equal(TiposAdministracao.Desparasitacao, administracao.Tipo);
        Assert.Null(administracao.IntervaloDias);
        Assert.Null(administracao.DataProximaPrevista);
    }

    [Theory(DisplayName = "UT-F006-010 — sem tipo, produto, lote ou data de administração devolve 400 com o campo em falta")]
    [InlineData("tipo", "O tipo é obrigatório.")]
    [InlineData("produto", "O produto é obrigatório.")]
    [InlineData("lote", "O lote é obrigatório.")]
    [InlineData("dataAdministracao", "A data de administração é obrigatória.")]
    public async Task CampoEmFaltaDevolve400(string campo, string mensagem)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var antes = await db.RegistosProfilaxias.CountAsync();
        var pedido = Pedido();

        switch (campo)
        {
            case "tipo":
                pedido.Tipo = "";
                break;
            case "produto":
                pedido.Produto = "";
                break;
            case "lote":
                pedido.Lote = "   ";
                break;
            default:
                pedido.DataAdministracao = null;
                break;
        }

        var erro = await Assert.ThrowsAsync<ValidacaoException>(() => CriarServico(db).RegistarAsync(d.AnimalA1, pedido, d.VetA));

        Assert.Contains(mensagem, erro.Message, StringComparison.Ordinal);
        Assert.Equal(antes, await db.RegistosProfilaxias.CountAsync());
    }

    [Fact(DisplayName = "UT-F006-011 — data de administração de amanhã (2026-10-15) devolve 400")]
    public async Task DataFuturaDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<ValidacaoException>(
            () => CriarServico(db).RegistarAsync(d.AnimalA1, Pedido(data: Hoje.AddDays(1)), d.VetA));

        Assert.Equal(VacinacoesService.MensagemDataFutura, erro.Message);
    }

    [Fact(DisplayName = "UT-F006-012 — data de administração de hoje (2026-10-14) é aceite")]
    public async Task DataDeHojeEAceite()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var administracao = await CriarServico(db).RegistarAsync(d.AnimalA1, Pedido(data: Hoje), d.VetA);

        Assert.Equal(Hoje, administracao.DataAdministracao);
    }

    [Fact(DisplayName = "I082 — «hoje» é o dia em Lisboa: às 23:30 UTC de 13/10 (00:30 de 14/10 em Lisboa) aceita 2026-10-14")]
    public async Task HojeEmHoraDeLisboa()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var servico = new VacinacoesService(db, new RelogioFixo(new DateTimeOffset(2026, 10, 13, 23, 30, 0, TimeSpan.Zero)));

        var administracao = await servico.RegistarAsync(d.AnimalA1, Pedido(data: Hoje), d.VetA);

        Assert.Equal(Hoje, administracao.DataAdministracao);
    }

    [Theory(DisplayName = "UT-F006-013 e UT-F006-015 — intervaloDias = 0 ou negativo devolve 400")]
    [InlineData(0)]
    [InlineData(-30)]
    public async Task IntervaloNaoPositivoDevolve400(int intervaloDias)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<ValidacaoException>(
            () => CriarServico(db).RegistarAsync(d.AnimalA1, Pedido(intervaloDias: intervaloDias), d.VetA));

        Assert.Contains("intervaloDias", erro.Message, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "UT-F006-014 — intervaloDias = 1 é aceite e a próxima é 2026-10-15")]
    public async Task IntervaloDeUmDia()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var administracao = await CriarServico(db).RegistarAsync(d.AnimalA1, Pedido(intervaloDias: 1), d.VetA);

        Assert.Equal(new DateOnly(2026, 10, 15), administracao.DataProximaPrevista);
    }

    [Fact(DisplayName = "UT-F006-016 — lote com 51 caracteres devolve 400 (máximo 50)")]
    public async Task LoteDemasiadoLongoDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<ValidacaoException>(
            () => CriarServico(db).RegistarAsync(d.AnimalA1, Pedido(lote: new string('L', 51)), d.VetA));

        Assert.Contains("lote", erro.Message, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "I082 — tipo fora de Vacina/Desparasitacao devolve 400")]
    public async Task TipoInvalidoNoRegistoDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<ValidacaoException>(
            () => CriarServico(db).RegistarAsync(d.AnimalA1, Pedido(tipo: "Vacinas"), d.VetA));

        Assert.Equal(TiposAdministracao.MensagemTipoInvalido, erro.Message);
    }

    [Fact(DisplayName = "UT-F006-017 — animal inexistente (id = 999) devolve 404")]
    public async Task AnimalInexistenteNoRegistoDevolve404()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<KeyNotFoundException>(() => CriarServico(db).RegistarAsync(999, Pedido(), d.VetA));

        Assert.Equal(VacinacoesService.MensagemAnimalNaoEncontrado, erro.Message);
    }

    [Fact(DisplayName = "I082 — produto e lote são guardados sem espaços nos extremos")]
    public async Task TextosAparados()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var administracao = await CriarServico(db).RegistarAsync(
            d.AnimalA1, Pedido(produto: "  Vacina antirrábica  ", lote: " RB-2291 "), d.VetA);

        Assert.Equal("Vacina antirrábica", administracao.Produto);
        Assert.Equal("RB-2291", administracao.Lote);
    }

    // ----- RF056 — Atualizar vacinação ou desparasitação (I082, critério 4) -----

    [Fact(DisplayName = "I082 — atualizar a data de administração recalcula a próxima (2026-09-01 + 365 = 2027-09-01)")]
    public async Task AtualizarDataRecalculaProxima()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var administracao = await CriarServico(db).AtualizarAsync(
            d.AnimalA1, d.VacinaA1, Pedido(data: new DateOnly(2026, 9, 1)), d.VetA);

        Assert.Equal(d.VacinaA1, administracao.Id);
        Assert.Equal(new DateOnly(2026, 9, 1), administracao.DataAdministracao);
        Assert.Equal(new DateOnly(2027, 9, 1), administracao.DataProximaPrevista);

        var gravado = await db.RegistosProfilaxias.AsNoTracking().SingleAsync(r => r.Id == d.VacinaA1);
        Assert.Equal(new DateOnly(2026, 9, 1), gravado.DataAdministracao);
        Assert.Equal(d.AnimalA1, gravado.PacienteId);
    }

    [Fact(DisplayName = "I082 — atualizar o intervalo recalcula a próxima, e retirá-lo deixa-a nula")]
    public async Task AtualizarIntervaloRecalculaProxima()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var servico = CriarServico(db);

        var com90 = await servico.AtualizarAsync(d.AnimalA1, d.VacinaA1, Pedido(intervaloDias: 90), d.VetA);
        var semIntervalo = await servico.AtualizarAsync(d.AnimalA1, d.VacinaA1, Pedido(intervaloDias: null), d.VetA);

        Assert.Equal(new DateOnly(2027, 1, 12), com90.DataProximaPrevista);
        Assert.Null(semIntervalo.IntervaloDias);
        Assert.Null(semIntervalo.DataProximaPrevista);
    }

    [Fact(DisplayName = "I082 — a atualização substitui produto e lote e mantém o animal e o Veterinário")]
    public async Task AtualizarSubstituiCampos()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var administracao = await CriarServico(db).AtualizarAsync(
            d.AnimalA1, d.VacinaA1, Pedido(produto: "Vacina antirrábica (corrigida)", lote: "RB-2292"), d.VetA);

        Assert.Equal("Vacina antirrábica (corrigida)", administracao.Produto);
        Assert.Equal("RB-2292", administracao.Lote);
        Assert.Equal("Ana Vet", administracao.Veterinario);

        var gravado = await db.RegistosProfilaxias.AsNoTracking().SingleAsync(r => r.Id == d.VacinaA1);
        Assert.Equal(d.VetA, gravado.VeterinarioId);
        Assert.Equal(d.AnimalA1, gravado.PacienteId);
    }

    [Fact(DisplayName = "I082 — a atualização com dados inválidos devolve 400 e o registo fica como estava")]
    public async Task AtualizarComDadosInvalidosDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        await Assert.ThrowsAsync<ValidacaoException>(
            () => CriarServico(db).AtualizarAsync(d.AnimalA1, d.VacinaA1, Pedido(data: Hoje.AddDays(1), intervaloDias: 0), d.VetA));

        var gravado = await db.RegistosProfilaxias.AsNoTracking().SingleAsync(r => r.Id == d.VacinaA1);
        Assert.Equal(Hoje, gravado.DataAdministracao);
        Assert.Equal((short)365, gravado.IntervaloDias);
    }

    [Fact(DisplayName = "I082 — atualizar um registo de outro animal, ou inexistente, devolve 404")]
    public async Task AtualizarRegistoDeOutroAnimalDevolve404()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var servico = CriarServico(db);

        var deOutroAnimal = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => servico.AtualizarAsync(d.AnimalA2, d.VacinaA1, Pedido(), d.VetA));
        var inexistente = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => servico.AtualizarAsync(d.AnimalA1, 999, Pedido(), d.VetA));
        var animalInexistente = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => servico.AtualizarAsync(999, d.VacinaA1, Pedido(), d.VetA));

        Assert.Equal(VacinacoesService.MensagemAdministracaoNaoEncontrada, deOutroAnimal.Message);
        Assert.Equal(VacinacoesService.MensagemAdministracaoNaoEncontrada, inexistente.Message);
        Assert.Equal(VacinacoesService.MensagemAnimalNaoEncontrado, animalInexistente.Message);
    }

    [Fact(DisplayName = "I082 — atualizar um registo feito por outro Veterinário devolve 403")]
    public async Task AtualizarRegistoDeOutroVeterinarioDevolve403()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<ForbiddenException>(
            () => CriarServico(db).AtualizarAsync(d.AnimalA1, d.DesparasitacaoA1, Pedido(), d.VetA));

        Assert.Equal(VacinacoesService.MensagemAdministracaoDeOutroVeterinario, erro.Message);
    }

    // ----- I089 — casos além da I028 (critérios do Backlog de Projeto v03) -----

    [Theory(DisplayName = "I089 — vacinação sem intervalo e desparasitação com intervalo: a próxima depende só do intervalo, não do tipo")]
    [InlineData(TiposAdministracao.Vacina, null, null)]
    [InlineData(TiposAdministracao.Desparasitacao, 90, "2027-01-12")]
    public async Task ProximaDependeSoDoIntervalo(string tipo, int? intervaloDias, string? esperada)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        DateOnly? proximaEsperada = esperada is null ? null : Data(esperada);

        var administracao = await CriarServico(db).RegistarAsync(d.AnimalA1, Pedido(tipo: tipo, intervaloDias: intervaloDias), d.VetA);

        Assert.Equal(tipo, administracao.Tipo);
        Assert.Equal(proximaEsperada, administracao.DataProximaPrevista);
    }

    [Theory(DisplayName = "I089 — a atualização recalcula a próxima nos casos de calendário: fim de mês, fim de ano e 29 de fevereiro")]
    [InlineData("2026-01-31", 1, "2026-02-01")]
    [InlineData("2025-12-31", 1, "2026-01-01")]
    [InlineData("2024-02-28", 1, "2024-02-29")]
    [InlineData("2023-03-01", 365, "2024-02-29")]
    public async Task AtualizacaoRecalculaNosCasosDeCalendario(string dataAdministracao, int intervaloDias, string esperada)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var administracao = await CriarServico(db).AtualizarAsync(
            d.AnimalA1, d.VacinaA1, Pedido(data: Data(dataAdministracao), intervaloDias: intervaloDias), d.VetA);

        Assert.Equal(Data(esperada), administracao.DataProximaPrevista);

        var gravado = await db.RegistosProfilaxias.AsNoTracking().SingleAsync(r => r.Id == d.VacinaA1);
        Assert.Equal(Data(dataAdministracao), gravado.DataAdministracao);
        Assert.Equal((short)intervaloDias, gravado.IntervaloDias);
    }

    [Fact(DisplayName = "I089 — dar intervalo a uma desparasitação sem próxima (2026-08-01 + 90) cria a próxima de 2026-10-30, que entra nas próximas do boletim")]
    public async Task AtualizarDesparasitacaoComIntervaloCriaProxima()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var servico = CriarServico(db);
        var pedido = Pedido(
            tipo: TiposAdministracao.Desparasitacao,
            produto: "Desparasitante externo Z",
            lote: "DZ-101",
            data: new DateOnly(2026, 8, 1),
            intervaloDias: 90);

        var administracao = await servico.AtualizarAsync(d.AnimalA1, d.DesparasitacaoA1, pedido, d.VetB);
        var boletim = await servico.ObterBoletimAsync(d.AnimalA1, new BoletimQuery());

        Assert.Equal(new DateOnly(2026, 10, 30), administracao.DataProximaPrevista);
        Assert.Contains(
            boletim.Proximas,
            p => p.Tipo == TiposAdministracao.Desparasitacao && p.DataProximaPrevista == new DateOnly(2026, 10, 30));
    }

    [Fact(DisplayName = "I089 — o Veterinário que não fez o registo recebe 403 e o registo fica como estava")]
    public async Task AtualizacaoDeOutroVeterinarioNaoAlteraORegisto()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        await Assert.ThrowsAsync<ForbiddenException>(
            () => CriarServico(db).AtualizarAsync(
                d.AnimalA1, d.DesparasitacaoA1, Pedido(data: new DateOnly(2026, 9, 1), intervaloDias: 30), d.VetA));

        var gravado = await db.RegistosProfilaxias.AsNoTracking().SingleAsync(r => r.Id == d.DesparasitacaoA1);
        Assert.Equal(TiposAdministracao.Desparasitacao, gravado.Tipo);
        Assert.Equal(new DateOnly(2026, 8, 1), gravado.DataAdministracao);
        Assert.Null(gravado.IntervaloDias);
        Assert.Equal(d.VetB, gravado.VeterinarioId);
    }

    // ----- EP-042 — Consultar boletim do paciente (web) -----

    [Fact(DisplayName = "UT-F006-018 — administrações da mais recente para a mais antiga; próximas só com a vacina de 2027-10-14")]
    public async Task BoletimCompleto()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var boletim = await CriarServico(db).ObterBoletimAsync(d.AnimalA1, new BoletimQuery());

        Assert.Equal(d.AnimalA1, boletim.Animal.Id);
        Assert.Equal("Bobi", boletim.Animal.Nome);
        Assert.Equal("Cão", boletim.Animal.Especie);
        Assert.Equal(new[] { d.VacinaA1, d.DesparasitacaoA1, d.VacinaJulhoA1 }, boletim.Administracoes.Select(a => a.Id));

        var desparasitacao = boletim.Administracoes.Single(a => a.Id == d.DesparasitacaoA1);
        Assert.Null(desparasitacao.DataProximaPrevista);
        Assert.Equal("Bruno Vet", desparasitacao.Veterinario);
        Assert.Equal("DZ-101", desparasitacao.Lote);

        var proxima = Assert.Single(boletim.Proximas);
        Assert.Equal(TiposAdministracao.Vacina, proxima.Tipo);
        Assert.Equal("Vacina antirrábica", proxima.Produto);
        Assert.Equal(new DateOnly(2027, 10, 14), proxima.DataProximaPrevista);
    }

    [Theory(DisplayName = "UT-F006-019 — tipo filtra as administrações e as próximas")]
    [InlineData(TiposAdministracao.Vacina)]
    [InlineData(TiposAdministracao.Desparasitacao)]
    public async Task BoletimFiltradoPorTipo(string tipo)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var boletim = await CriarServico(db).ObterBoletimAsync(d.AnimalA1, new BoletimQuery { Tipo = tipo });

        Assert.NotEmpty(boletim.Administracoes);
        Assert.All(boletim.Administracoes, a => Assert.Equal(tipo, a.Tipo));
        Assert.All(boletim.Proximas, p => Assert.Equal(tipo, p.Tipo));
    }

    [Fact(DisplayName = "UT-F006-020 — tipo = \"Vacinas\" devolve 400")]
    public async Task TipoInvalidoNoBoletimDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<ValidacaoException>(
            () => CriarServico(db).ObterBoletimAsync(d.AnimalA1, new BoletimQuery { Tipo = "Vacinas" }));

        Assert.Equal(TiposAdministracao.MensagemTipoInvalido, erro.Message);
    }

    [Fact(DisplayName = "UT-F006-021 — próxima prevista para ontem (2026-10-13) não aparece em proximas")]
    public async Task ProximaPassadaNaoAparece()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var boletim = await CriarServico(db).ObterBoletimAsync(d.AnimalA1, new BoletimQuery());

        var julho = boletim.Administracoes.Single(a => a.Id == d.VacinaJulhoA1);
        Assert.Equal(new DateOnly(2026, 10, 13), julho.DataProximaPrevista);
        Assert.DoesNotContain(boletim.Proximas, p => p.DataProximaPrevista == new DateOnly(2026, 10, 13));
    }

    [Fact(DisplayName = "I082 — próximas por ordem crescente, incluindo a de hoje")]
    public async Task ProximasPorOrdemCrescente()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var servico = CriarServico(db);
        await servico.RegistarAsync(d.AnimalA1, Pedido(tipo: TiposAdministracao.Desparasitacao, produto: "Desparasitante interno Y", data: new DateOnly(2026, 10, 1), intervaloDias: 13), d.VetA);
        await servico.RegistarAsync(d.AnimalA1, Pedido(tipo: TiposAdministracao.Desparasitacao, produto: "Desparasitante interno Y", data: Hoje, intervaloDias: 90), d.VetA);

        var boletim = await servico.ObterBoletimAsync(d.AnimalA1, new BoletimQuery());

        Assert.Equal(
            new[] { Hoje, new DateOnly(2027, 1, 12), new DateOnly(2027, 10, 14) },
            boletim.Proximas.Select(p => p.DataProximaPrevista));
    }

    [Fact(DisplayName = "I082 — animal sem registos: boletim com as duas listas vazias")]
    public async Task BoletimSemRegistos()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var boletim = await CriarServico(db).ObterBoletimAsync(d.AnimalA2, new BoletimQuery());

        Assert.Equal("Mia", boletim.Animal.Nome);
        Assert.Empty(boletim.Administracoes);
        Assert.Empty(boletim.Proximas);
    }

    [Fact(DisplayName = "I082 — EP-042 com animal inexistente devolve 404")]
    public async Task BoletimDeAnimalInexistenteDevolve404()
    {
        using var db = AjudaBaseDados.CriarContexto();
        await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => CriarServico(db).ObterBoletimAsync(999, new BoletimQuery()));

        Assert.Equal(VacinacoesService.MensagemAnimalNaoEncontrado, erro.Message);
    }

    [Fact(DisplayName = "I082 — o boletim não inclui registos de outros animais")]
    public async Task BoletimSoDoAnimal()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var boletim = await CriarServico(db).ObterBoletimAsync(d.AnimalA1, new BoletimQuery());

        Assert.DoesNotContain(boletim.Administracoes, a => a.Id == d.VacinaB1);
    }

    // ----- EP-044 — Consultar boletim do meu animal (Cliente) -----

    [Fact(DisplayName = "UT-F006-022 — o Cliente vê o mesmo histórico e as mesmas próximas do boletim web")]
    public async Task BoletimDoCliente()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var servico = CriarServico(db);

        var web = await servico.ObterBoletimAsync(d.AnimalA1, new BoletimQuery());
        var meu = await servico.ObterBoletimDoClienteAsync(d.ClienteA, d.AnimalA1);

        Assert.Equal(
            web.Administracoes.Select(a => (a.Tipo, a.Produto, a.DataAdministracao)),
            meu.Historico.Select(h => (h.Tipo, h.Produto, h.DataAdministracao)));
        Assert.Equal(
            web.Proximas.Select(p => (p.Tipo, p.Produto, p.DataProximaPrevista)),
            meu.Proximas.Select(p => (p.Tipo, p.Produto, p.DataProximaPrevista)));
    }

    [Fact(DisplayName = "UT-F006-023 — o CLIENTE_A pede o boletim do ANIMAL_B1: 403")]
    public async Task BoletimDeAnimalDeOutroClienteDevolve403()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<ForbiddenException>(
            () => CriarServico(db).ObterBoletimDoClienteAsync(d.ClienteA, d.AnimalB1));

        Assert.Equal(VacinacoesService.MensagemAnimalDeOutroCliente, erro.Message);
    }

    [Fact(DisplayName = "UT-F006-024 — ANIMAL_A2 sem registos: historico = [] e proximas = []")]
    public async Task BoletimDoClienteSemRegistos()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var meu = await CriarServico(db).ObterBoletimDoClienteAsync(d.ClienteA, d.AnimalA2);

        Assert.Empty(meu.Historico);
        Assert.Empty(meu.Proximas);
    }

    [Fact(DisplayName = "I082 — EP-044 com animal inexistente devolve 404")]
    public async Task BoletimDoClienteDeAnimalInexistenteDevolve404()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => CriarServico(db).ObterBoletimDoClienteAsync(d.ClienteA, 999));

        Assert.Equal(VacinacoesService.MensagemAnimalNaoEncontrado, erro.Message);
    }

    // ----- Auxiliares -----

    private static VacinacoesService CriarServico(ClinicaDbContext db) => new(db, new RelogioFixo(Relogio));

    private static DateOnly Data(string texto) => DateOnly.Parse(texto, CultureInfo.InvariantCulture);

    /// <summary>O pedido do exemplo UT-F006-008, com os campos que cada caso altera.</summary>
    private static CreateAdministracaoDto Pedido(
        string tipo = TiposAdministracao.Vacina,
        string produto = "Vacina antirrábica",
        string lote = "RB-2291",
        DateOnly? data = null,
        int? intervaloDias = 365) => new()
        {
            Tipo = tipo,
            Produto = produto,
            Lote = lote,
            DataAdministracao = data ?? Hoje,
            IntervaloDias = intervaloDias
        };

    /// <summary>
    /// Dados de referência da I028 (secção 5): VET_A (Ana Vet) e VET_B (Bruno Vet); CLIENTE_A com
    /// ANIMAL_A1 (Bobi) e ANIMAL_A2 (Mia, sem registos); CLIENTE_B com ANIMAL_B1. ANIMAL_A1 tem a
    /// vacina de 2026-10-14 (VET_A, 365 dias, próxima 2027-10-14), a desparasitação de 2026-08-01
    /// (VET_B, sem próxima) e uma vacina de 2026-07-15 (VET_A, 90 dias, próxima ontem, 2026-10-13).
    /// </summary>
    private static async Task<DadosBase> CriarBaseAsync(ClinicaDbContext db)
    {
        var vetA = await AdicionarFuncionarioAsync(db, "Ana Vet");
        var vetB = await AdicionarFuncionarioAsync(db, "Bruno Vet");

        var cao = new Especie { Nome = "Cão" };
        var gato = new Especie { Nome = "Gato" };
        db.Especies.AddRange(cao, gato);
        await db.SaveChangesAsync();

        var clienteA = await AdicionarClienteAsync(db, "Joana Cliente");
        var clienteB = await AdicionarClienteAsync(db, "Rui Cliente");

        var animalA1 = new Paciente { ClienteId = clienteA, Nome = "Bobi", EspecieId = cao.Id, DataRegisto = Relogio.UtcDateTime };
        var animalA2 = new Paciente { ClienteId = clienteA, Nome = "Mia", EspecieId = gato.Id, DataRegisto = Relogio.UtcDateTime };
        var animalB1 = new Paciente { ClienteId = clienteB, Nome = "Luna", EspecieId = gato.Id, DataRegisto = Relogio.UtcDateTime };
        db.Pacientes.AddRange(animalA1, animalA2, animalB1);
        await db.SaveChangesAsync();

        var vacina = Registo(animalA1.Id, vetA, TiposAdministracao.Vacina, "Vacina antirrábica", "RB-2291", Hoje, 365);
        var desparasitacao = Registo(animalA1.Id, vetB, TiposAdministracao.Desparasitacao, "Desparasitante externo Z", "DZ-101", new DateOnly(2026, 8, 1), null);
        var vacinaJulho = Registo(animalA1.Id, vetA, TiposAdministracao.Vacina, "Vacina polivalente", "PV-1187", new DateOnly(2026, 7, 15), 90);
        var vacinaB1 = Registo(animalB1.Id, vetA, TiposAdministracao.Vacina, "Vacina felina", "VF-0001", Hoje, 365);
        db.RegistosProfilaxias.AddRange(vacina, desparasitacao, vacinaJulho, vacinaB1);
        await db.SaveChangesAsync();

        db.ChangeTracker.Clear();

        return new DadosBase(
            vetA, vetB, clienteA, clienteB, animalA1.Id, animalA2.Id, animalB1.Id,
            vacina.Id, desparasitacao.Id, vacinaJulho.Id, vacinaB1.Id);
    }

    private static RegistoProfilaxia Registo(
        int pacienteId,
        int veterinarioId,
        string tipo,
        string produto,
        string lote,
        DateOnly data,
        short? intervaloDias) => new()
        {
            PacienteId = pacienteId,
            VeterinarioId = veterinarioId,
            Tipo = tipo,
            Produto = produto,
            Lote = lote,
            DataAdministracao = data,
            IntervaloDias = intervaloDias,
            DataRegisto = Relogio.UtcDateTime.AddDays(-1)
        };

    private static async Task<int> AdicionarFuncionarioAsync(ClinicaDbContext db, string nome)
    {
        var utilizador = new Utilizador
        {
            Email = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "@clinica-g08.test",
            PasswordHash = "hash-nao-usado-nestes-testes",
            Perfil = Perfis.Veterinario,
            Estado = EstadosUtilizador.Ativo,
            DataCriacao = Relogio.UtcDateTime
        };
        db.Utilizadores.Add(utilizador);
        await db.SaveChangesAsync();

        db.Funcionarios.Add(new Funcionario { UtilizadorId = utilizador.Id, Nome = nome });
        await db.SaveChangesAsync();

        return utilizador.Id;
    }

    private static async Task<int> AdicionarClienteAsync(ClinicaDbContext db, string nome)
    {
        var utilizador = new Utilizador
        {
            Email = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "@clinica-g08.test",
            PasswordHash = "hash-nao-usado-nestes-testes",
            Perfil = Perfis.Cliente,
            Estado = EstadosUtilizador.Ativo,
            DataCriacao = Relogio.UtcDateTime
        };
        db.Utilizadores.Add(utilizador);
        await db.SaveChangesAsync();

        var cliente = new Cliente
        {
            UtilizadorId = utilizador.Id,
            Nome = nome,
            Telefone = "960000001",
            DataRegisto = Relogio.UtcDateTime
        };
        db.Clientes.Add(cliente);
        await db.SaveChangesAsync();

        return cliente.Id;
    }

    private sealed record DadosBase(
        int VetA,
        int VetB,
        int ClienteA,
        int ClienteB,
        int AnimalA1,
        int AnimalA2,
        int AnimalB1,
        int VacinaA1,
        int DesparasitacaoA1,
        int VacinaJulhoA1,
        int VacinaB1);
}
