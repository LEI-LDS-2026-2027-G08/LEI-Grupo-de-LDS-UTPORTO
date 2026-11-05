using System.Globalization;
using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Application.DTOs.Consultas;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Application.Validacao;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;

namespace ClinicaVeterinaria.UnitTests.Consultas;

/// <summary>
/// I072 — <see cref="ConsultasService"/> (EP-023, EP-024) com o <c>ClinicaDbContext</c> em memória
/// (I028, secção 4.2). Casos UT-F003-050 a UT-F003-055 da Especificação de Testes Unitários
/// (I028, secção 7.3.2); os restantes são próprios desta tarefa.
/// </summary>
public class ConsultasServiceTests
{
    private static readonly DateTime Criacao = new(2026, 10, 10, 9, 0, 0, DateTimeKind.Utc);

    private static readonly DateOnly Quinta = new(2026, 10, 15);

    // RowVersion do caso UT-F003-085: 00 00 00 00 00 00 07 D1.
    private static readonly byte[] VersaoD1 = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0xD1];

    // ----- EP-023 — Consultar agenda -----

    [Fact(DisplayName = "UT-F003-050 — o Veterinário que envia o veterinarioId de outro só recebe as suas marcações")]
    public async Task VeterinarioIgnoraFiltroDeOutroVeterinario()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var a1 = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z", EstadosConsulta.Confirmada);
        await AdicionarMarcacaoAsync(db, d.AnimalB1, d.VetB, "2026-10-15T10:00:00Z", EstadosConsulta.Pendente);
        var a2 = await AdicionarMarcacaoAsync(db, d.AnimalA2, d.VetA, "2026-10-15T13:00:00Z", EstadosConsulta.Pendente);

        var filtro = new ConsultaFiltroQuery { DataInicio = Quinta, DataFim = Quinta, VeterinarioId = d.VetB };
        var agenda = await CriarServico(db).ObterConsultasAsync(filtro, AcessoConsultas.Veterinario(d.VetA));

        Assert.Equal(new[] { a1, a2 }, agenda.Itens.Select(c => c.Id));
        Assert.All(agenda.Itens, c => Assert.Equal(d.VetA, c.Veterinario.Id));
        Assert.Equal(2, agenda.Total);
    }

    [Fact(DisplayName = "UT-F003-051 — a Rececionista filtra por estado e recebe as marcações dos dois Veterinários")]
    public async Task RececionistaFiltraPorEstado()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var a = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z", EstadosConsulta.Confirmada);
        var b = await AdicionarMarcacaoAsync(db, d.AnimalB1, d.VetB, "2026-10-15T10:00:00Z", EstadosConsulta.Confirmada);
        await AdicionarMarcacaoAsync(db, d.AnimalA2, d.VetA, "2026-10-15T11:00:00Z", EstadosConsulta.Pendente);
        await AdicionarMarcacaoAsync(db, d.AnimalB1, d.VetB, "2026-10-15T12:00:00Z", EstadosConsulta.Cancelada);

        var filtro = new ConsultaFiltroQuery { DataInicio = Quinta, DataFim = Quinta, Estado = EstadosConsulta.Confirmada };
        var agenda = await CriarServico(db).ObterConsultasAsync(filtro, AcessoConsultas.Rececionista);

        Assert.Equal(new[] { a, b }, agenda.Itens.Select(c => c.Id));
        Assert.All(agenda.Itens, c => Assert.Equal(EstadosConsulta.Confirmada, c.Estado));
        Assert.Equal(new[] { d.VetA, d.VetB }, agenda.Itens.Select(c => c.Veterinario.Id));
    }

    [Fact(DisplayName = "UT-F003-052 — dataFim anterior a dataInicio devolve 400")]
    public async Task DataFimAnteriorDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var filtro = new ConsultaFiltroQuery { DataInicio = Quinta, DataFim = Quinta.AddDays(-1) };

        var erro = await Assert.ThrowsAsync<ArgumentException>(
            () => CriarServico(db).ObterConsultasAsync(filtro, AcessoConsultas.Rececionista));

        Assert.Equal(ValidadorPeriodoDisponibilidade.MensagemDataFimAnterior, erro.Message);
    }

    [Fact(DisplayName = "I072 — a agenda é por dia de Lisboa: no dia da mudança de hora conta a meia-noite local")]
    public async Task PeriodoEmDiasDeLisboa()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        // 2026-10-25 (domingo) tem 25 horas: começa às 23:00 UTC do dia 24 (hora de verão) e
        // acaba às 00:00 UTC do dia 26 (hora de inverno).
        await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-10-24T22:59:00Z", EstadosConsulta.Pendente); // 24/10 23:59
        var primeira = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-10-24T23:00:00Z", EstadosConsulta.Pendente); // 25/10 00:00
        var ultima = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-10-25T23:59:00Z", EstadosConsulta.Pendente); // 25/10 23:59
        await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-10-26T00:00:00Z", EstadosConsulta.Pendente); // 26/10 00:00

        var dia = new DateOnly(2026, 10, 25);
        var agenda = await CriarServico(db).ObterConsultasAsync(
            new ConsultaFiltroQuery { DataInicio = dia, DataFim = dia },
            AcessoConsultas.Rececionista);

        Assert.Equal(new[] { primeira, ultima }, agenda.Itens.Select(c => c.Id));
    }

    [Fact(DisplayName = "I072 — só dataInicio: dessa data em diante; só dataFim: até ao fim desse dia")]
    public async Task DatasOpcionais()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var antes = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-10-14T09:00:00Z", EstadosConsulta.Realizada);
        var durante = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z", EstadosConsulta.Confirmada);
        var depois = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-11-20T09:00:00Z", EstadosConsulta.Pendente);
        var servico = CriarServico(db);

        var desde = await servico.ObterConsultasAsync(new ConsultaFiltroQuery { DataInicio = Quinta }, AcessoConsultas.Rececionista);
        var ate = await servico.ObterConsultasAsync(new ConsultaFiltroQuery { DataFim = Quinta }, AcessoConsultas.Rececionista);
        var tudo = await servico.ObterConsultasAsync(new ConsultaFiltroQuery(), AcessoConsultas.Rececionista);

        Assert.Equal(new[] { durante, depois }, desde.Itens.Select(c => c.Id));
        Assert.Equal(new[] { antes, durante }, ate.Itens.Select(c => c.Id));
        Assert.Equal(new[] { antes, durante, depois }, tudo.Itens.Select(c => c.Id));
    }

    [Fact(DisplayName = "I072 — a Rececionista filtra por Veterinário e por animal; sem estado vêm todos, incluindo as canceladas")]
    public async Task RececionistaFiltraPorVeterinarioEAnimal()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var a1 = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z", EstadosConsulta.Cancelada);
        var a2 = await AdicionarMarcacaoAsync(db, d.AnimalA2, d.VetA, "2026-10-15T10:00:00Z", EstadosConsulta.Pendente);
        var b1 = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetB, "2026-10-15T11:00:00Z", EstadosConsulta.Pendente);
        var servico = CriarServico(db);

        var doVetA = await servico.ObterConsultasAsync(new ConsultaFiltroQuery { VeterinarioId = d.VetA }, AcessoConsultas.Rececionista);
        var doAnimal = await servico.ObterConsultasAsync(new ConsultaFiltroQuery { AnimalId = d.AnimalA1 }, AcessoConsultas.Rececionista);

        Assert.Equal(new[] { a1, a2 }, doVetA.Itens.Select(c => c.Id));
        Assert.Equal(new[] { a1, b1 }, doAnimal.Itens.Select(c => c.Id));
    }

    [Fact(DisplayName = "I072 — o Auxiliar Veterinário vê a agenda de todos os Veterinários")]
    public async Task AuxiliarVeTodaAAgenda()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var a = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z", EstadosConsulta.Pendente);
        var b = await AdicionarMarcacaoAsync(db, d.AnimalB1, d.VetB, "2026-10-15T10:00:00Z", EstadosConsulta.Pendente);

        var agenda = await CriarServico(db).ObterConsultasAsync(new ConsultaFiltroQuery(), AcessoConsultas.AuxiliarVeterinario);

        Assert.Equal(new[] { a, b }, agenda.Itens.Select(c => c.Id));
    }

    [Theory(DisplayName = "I072 — o estado não distingue maiúsculas nem espaços nos extremos")]
    [InlineData("confirmada")]
    [InlineData(" CONFIRMADA ")]
    public async Task EstadoSemDistinguirMaiusculas(string estado)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var confirmada = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z", EstadosConsulta.Confirmada);
        await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-10-15T10:00:00Z", EstadosConsulta.Pendente);

        var agenda = await CriarServico(db).ObterConsultasAsync(new ConsultaFiltroQuery { Estado = estado }, AcessoConsultas.Rececionista);

        Assert.Equal(confirmada, Assert.Single(agenda.Itens).Id);
    }

    [Theory(DisplayName = "I072 — estado inválido ou datas fora de 2000-01-01 a 9998-12-31 devolvem 400")]
    [InlineData("Faturada", null, null)]
    [InlineData(null, "1999-12-31", null)]
    [InlineData(null, null, "9999-12-31")]
    public async Task FiltroInvalidoDevolve400(string? estado, string? dataInicio, string? dataFim)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var filtro = new ConsultaFiltroQuery
        {
            Estado = estado,
            DataInicio = dataInicio is null ? null : Data(dataInicio),
            DataFim = dataFim is null ? null : Data(dataFim)
        };

        await Assert.ThrowsAsync<ArgumentException>(
            () => CriarServico(db).ObterConsultasAsync(filtro, AcessoConsultas.Rececionista));
    }

    [Fact(DisplayName = "I072 — paginação: ordem por data e hora e depois por id, com o total de todas as páginas")]
    public async Task PaginacaoComOrdemEstavel()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var tarde = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-10-15T14:00:00Z", EstadosConsulta.Pendente);
        var manhaB = await AdicionarMarcacaoAsync(db, d.AnimalB1, d.VetB, "2026-10-15T09:00:00Z", EstadosConsulta.Pendente);
        var manhaA = await AdicionarMarcacaoAsync(db, d.AnimalA2, d.VetA, "2026-10-15T09:00:00Z", EstadosConsulta.Pendente);
        var noite = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetB, "2026-10-15T17:00:00Z", EstadosConsulta.Pendente);
        var servico = CriarServico(db);

        var pagina1 = await servico.ObterConsultasAsync(new ConsultaFiltroQuery { Tamanho = 2 }, AcessoConsultas.Rececionista);
        var pagina2 = await servico.ObterConsultasAsync(new ConsultaFiltroQuery { Tamanho = 2, Pagina = 2 }, AcessoConsultas.Rececionista);

        // Às 09:00 há duas marcações: a criada primeiro (id menor) vem antes.
        Assert.Equal(new[] { manhaB, manhaA }, pagina1.Itens.Select(c => c.Id));
        Assert.Equal(new[] { tarde, noite }, pagina2.Itens.Select(c => c.Id));
        Assert.Equal(4, pagina1.Total);
        Assert.Equal(4, pagina2.Total);
        Assert.Equal(2, pagina2.Pagina);
        Assert.Equal(2, pagina2.Tamanho);
    }

    [Fact(DisplayName = "I072 — uma página depois do fim, mesmo a maior possível, vem vazia e com o total")]
    public async Task PaginaDepoisDoFimVemVazia()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z", EstadosConsulta.Pendente);

        var agenda = await CriarServico(db).ObterConsultasAsync(
            new ConsultaFiltroQuery { Pagina = int.MaxValue, Tamanho = 100 },
            AcessoConsultas.Rececionista);

        Assert.Empty(agenda.Itens);
        Assert.Equal(1, agenda.Total);
    }

    [Fact(DisplayName = "I072 — cada item tem as datas em UTC e as referências ao animal, ao proprietário e ao Veterinário")]
    public async Task ItemComReferenciasEDatasUtc()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        await AdicionarMarcacaoAsync(
            db, d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z", EstadosConsulta.Confirmada,
            motivo: "Vacinação anual", chegada: "2026-10-15T08:52:00Z");

        var item = Assert.Single((await CriarServico(db).ObterConsultasAsync(new ConsultaFiltroQuery(), AcessoConsultas.Rececionista)).Itens);

        Assert.Equal(Utc("2026-10-15T09:00:00Z"), item.DataHora);
        Assert.Equal(DateTimeKind.Utc, item.DataHora.Kind);
        Assert.Equal(Utc("2026-10-15T08:52:00Z"), item.ChegadaEm);
        Assert.Equal(DateTimeKind.Utc, item.ChegadaEm!.Value.Kind);
        Assert.Equal(30, item.DuracaoMinutos);
        Assert.Equal("Vacinação anual", item.Motivo);
        Assert.Equal(d.AnimalA1, item.Animal.Id);
        Assert.Equal("Bobi", item.Animal.Nome);
        Assert.Equal("Cão", item.Animal.Especie);
        Assert.Equal(d.ClienteA, item.Cliente.Id);
        Assert.Equal("Joana Cliente", item.Cliente.Nome);
        Assert.Null(item.Cliente.Telefone);
        Assert.Equal(d.VetA, item.Veterinario.Id);
        Assert.Equal("Ana Vet", item.Veterinario.Nome);
    }

    // ----- EP-024 — Consultar marcação -----

    [Fact(DisplayName = "UT-F003-053 — o Auxiliar sem autorização de apoio lê a marcação com a nota de preparação")]
    public async Task AuxiliarSemApoioLeNotaDePreparacao()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var consulta = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z", EstadosConsulta.Confirmada);
        var nota = await AdicionarNotaAsync(db, consulta, d.AuxComApoio, "Sala 2 preparada.", "2026-10-15T08:55:00Z");

        // O AUX_SEM_APOIO tem o mesmo acesso de leitura de qualquer Auxiliar (RF026).
        var lida = await CriarServico(db).ObterConsultaAsync(consulta, AcessoConsultas.AuxiliarVeterinario);

        var notaLida = Assert.Single(lida.Consulta.NotasPreparacao);
        Assert.Equal(nota, notaLida.Id);
        Assert.Equal("Sala 2 preparada.", notaLida.Texto);
        Assert.Equal("Carla Aux", notaLida.Autor);
        Assert.Equal(Utc("2026-10-15T08:55:00Z"), notaLida.CriadaEm);
        Assert.Equal(DateTimeKind.Utc, notaLida.CriadaEm.Kind);
        Assert.Null(notaLida.ConsultaId);
    }

    [Fact(DisplayName = "UT-F003-054 — o Veterinário que lê a marcação de outro Veterinário recebe 403")]
    public async Task VeterinarioNaoLeMarcacaoDeOutro()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var doVetB = await AdicionarMarcacaoAsync(db, d.AnimalB1, d.VetB, "2026-10-15T09:00:00Z", EstadosConsulta.Confirmada);

        await Assert.ThrowsAsync<ForbiddenException>(
            () => CriarServico(db).ObterConsultaAsync(doVetB, AcessoConsultas.Veterinario(d.VetA)));
    }

    [Fact(DisplayName = "UT-F003-055 — o Veterinário lê a marcação que lhe está atribuída")]
    public async Task VeterinarioLeAPropriaMarcacao()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var doVetA = await AdicionarMarcacaoAsync(
            db, d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z", EstadosConsulta.Confirmada, motivo: "Consulta de rotina");

        var lida = await CriarServico(db).ObterConsultaAsync(doVetA, AcessoConsultas.Veterinario(d.VetA));

        Assert.Equal(doVetA, lida.Consulta.Id);
        Assert.Equal(EstadosConsulta.Confirmada, lida.Consulta.Estado);
        Assert.Equal("Consulta de rotina", lida.Consulta.Motivo);
        Assert.Equal(Utc("2026-10-15T09:00:00Z"), lida.Consulta.DataHora);
        Assert.Equal(d.VetA, lida.Consulta.Veterinario.Id);
        Assert.Equal("Ana Vet", lida.Consulta.Veterinario.Nome);
        Assert.Empty(lida.Consulta.NotasPreparacao);
    }

    [Fact(DisplayName = "I072 — marcação inexistente devolve 404, também para um Veterinário")]
    public async Task MarcacaoInexistenteDevolve404()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var servico = CriarServico(db);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => servico.ObterConsultaAsync(999, AcessoConsultas.Rececionista));
        await Assert.ThrowsAsync<KeyNotFoundException>(() => servico.ObterConsultaAsync(999, AcessoConsultas.Veterinario(d.VetA)));
    }

    [Fact(DisplayName = "I072 — EP-024 devolve a versão da marcação como ETag (RowVersion em Base64, entre aspas)")]
    public async Task DevolveVersaoComoETag()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var consulta = await AdicionarMarcacaoAsync(
            db, d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z", EstadosConsulta.Pendente,
            rowVersion: VersaoD1);

        var lida = await CriarServico(db).ObterConsultaAsync(consulta, AcessoConsultas.Rececionista);

        Assert.Equal("\"AAAAAAAAB9E=\"", lida.ETag);
    }

    [Fact(DisplayName = "I072 — só as notas da própria marcação, da mais antiga para a mais recente")]
    public async Task NotasDaMarcacaoPorOrdem()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var consulta = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z", EstadosConsulta.Confirmada);
        var outra = await AdicionarMarcacaoAsync(db, d.AnimalB1, d.VetB, "2026-10-15T09:00:00Z", EstadosConsulta.Confirmada);
        var segunda = await AdicionarNotaAsync(db, consulta, d.AuxComApoio, "Segunda", "2026-10-15T08:50:00Z");
        await AdicionarNotaAsync(db, outra, d.AuxComApoio, "De outra marcação", "2026-10-15T08:00:00Z");
        var primeira = await AdicionarNotaAsync(db, consulta, d.AuxComApoio, "Primeira", "2026-10-15T08:40:00Z");

        var lida = await CriarServico(db).ObterConsultaAsync(consulta, AcessoConsultas.Rececionista);

        Assert.Equal(new[] { primeira, segunda }, lida.Consulta.NotasPreparacao.Select(n => n.Id));
    }

    // ----- Preparação dos dados -----

    private sealed record DadosBase(
        int VetA,
        int VetB,
        int AuxComApoio,
        int ClienteA,
        int AnimalA1,
        int AnimalA2,
        int AnimalB1);

    private static ConsultasService CriarServico(ClinicaDbContext db) => new(db);

    private static DateOnly Data(string texto) => DateOnly.Parse(texto, CultureInfo.InvariantCulture);

    private static DateTime Utc(string texto) => DateTime.Parse(
        texto, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);

    /// <summary>
    /// Dados de referência da I028 (secção 5): VET_A, VET_B, REC_A, AUX_COM_APOIO, AUX_SEM_APOIO,
    /// CLIENTE_A com ANIMAL_A1 e ANIMAL_A2, e CLIENTE_B com ANIMAL_B1.
    /// </summary>
    private static async Task<DadosBase> CriarBaseAsync(ClinicaDbContext db)
    {
        var vetA = await AdicionarFuncionarioAsync(db, "Ana Vet", Perfis.Veterinario);
        var vetB = await AdicionarFuncionarioAsync(db, "Bruno Vet", Perfis.Veterinario);
        await AdicionarFuncionarioAsync(db, "Rita Rececao", Perfis.Rececionista);
        var auxComApoio = await AdicionarFuncionarioAsync(db, "Carla Aux", Perfis.AuxiliarVeterinario, podeRegistarApoio: true);
        await AdicionarFuncionarioAsync(db, "Diogo Aux", Perfis.AuxiliarVeterinario);

        var especie = new Especie { Nome = "Cão" };
        db.Especies.Add(especie);
        await db.SaveChangesAsync();

        var clienteA = await AdicionarClienteAsync(db, "Joana Cliente", "960000001");
        var clienteB = await AdicionarClienteAsync(db, "Bernardo Cliente", "960000002");

        var animalA1 = await AdicionarAnimalAsync(db, clienteA, especie.Id, "Bobi");
        var animalA2 = await AdicionarAnimalAsync(db, clienteA, especie.Id, "Mia");
        var animalB1 = await AdicionarAnimalAsync(db, clienteB, especie.Id, "Rex");

        return new DadosBase(vetA, vetB, auxComApoio, clienteA, animalA1, animalA2, animalB1);
    }

    private static async Task<int> AdicionarFuncionarioAsync(
        ClinicaDbContext db,
        string nome,
        string perfil,
        bool podeRegistarApoio = false)
    {
        var utilizador = new Utilizador
        {
            Email = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "@clinica-g08.test",
            PasswordHash = "hash-nao-usado-nestes-testes",
            Perfil = perfil,
            Estado = EstadosUtilizador.Ativo,
            DataCriacao = Criacao
        };
        db.Utilizadores.Add(utilizador);
        await db.SaveChangesAsync();

        db.Funcionarios.Add(new Funcionario
        {
            UtilizadorId = utilizador.Id,
            Nome = nome,
            PodeRegistarApoio = podeRegistarApoio
        });
        await db.SaveChangesAsync();

        return utilizador.Id;
    }

    private static async Task<int> AdicionarClienteAsync(ClinicaDbContext db, string nome, string telefone)
    {
        var utilizador = new Utilizador
        {
            Email = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "@clinica-g08.test",
            PasswordHash = "hash-nao-usado-nestes-testes",
            Perfil = Perfis.Cliente,
            Estado = EstadosUtilizador.Ativo,
            DataCriacao = Criacao
        };
        db.Utilizadores.Add(utilizador);
        await db.SaveChangesAsync();

        var cliente = new Cliente
        {
            UtilizadorId = utilizador.Id,
            Nome = nome,
            Telefone = telefone,
            DataRegisto = Criacao
        };
        db.Clientes.Add(cliente);
        await db.SaveChangesAsync();

        return cliente.Id;
    }

    private static async Task<int> AdicionarAnimalAsync(ClinicaDbContext db, int clienteId, int especieId, string nome)
    {
        var paciente = new Paciente
        {
            ClienteId = clienteId,
            Nome = nome,
            EspecieId = especieId,
            DataRegisto = Criacao
        };
        db.Pacientes.Add(paciente);
        await db.SaveChangesAsync();

        return paciente.Id;
    }

    private static async Task<int> AdicionarMarcacaoAsync(
        ClinicaDbContext db,
        int pacienteId,
        int veterinarioId,
        string inicioUtc,
        string estado,
        string? motivo = null,
        string? chegada = null,
        byte[]? rowVersion = null)
    {
        // A base em memória devolve as datas como foram gravadas; o SQL Server devolve-as sem
        // indicação de fuso. Gravá-las como Unspecified verifica que o serviço as marca como UTC.
        var marcacao = new Marcacao
        {
            PacienteId = pacienteId,
            VeterinarioId = veterinarioId,
            DataHoraInicio = DateTime.SpecifyKind(Utc(inicioUtc), DateTimeKind.Unspecified),
            DuracaoMinutos = 30,
            Motivo = motivo,
            Estado = estado,
            DataHoraChegada = chegada is null ? null : DateTime.SpecifyKind(Utc(chegada), DateTimeKind.Unspecified),
            CriadaPorId = 1,
            DataCriacao = Criacao,
            RowVersion = rowVersion ?? new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }
        };
        db.Marcacoes.Add(marcacao);
        await db.SaveChangesAsync();

        return marcacao.Id;
    }

    private static async Task<int> AdicionarNotaAsync(
        ClinicaDbContext db,
        int marcacaoId,
        int auxiliarId,
        string descricao,
        string registoUtc)
    {
        var nota = new InformacaoApoio
        {
            MarcacaoId = marcacaoId,
            AuxiliarId = auxiliarId,
            Descricao = descricao,
            DataRegisto = DateTime.SpecifyKind(Utc(registoUtc), DateTimeKind.Unspecified)
        };
        db.InformacoesApoio.Add(nota);
        await db.SaveChangesAsync();

        return nota.Id;
    }
}
