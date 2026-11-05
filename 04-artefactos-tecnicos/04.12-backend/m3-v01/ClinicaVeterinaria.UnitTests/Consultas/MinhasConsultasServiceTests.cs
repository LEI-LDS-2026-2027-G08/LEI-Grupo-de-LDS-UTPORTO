using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Application.DTOs.Consultas;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.UnitTests.Seguranca;

namespace ClinicaVeterinaria.UnitTests.Consultas;

/// <summary>
/// I076 — <see cref="MinhasConsultasService"/> (EP-030; RF028, RF029) com o <c>ClinicaDbContext</c>
/// em memória (I028, secção 4.2). Casos UT-F003-056 a UT-F003-061 da Especificação de Testes
/// Unitários (I028, secção 7.3.2), com a regra decidida na I076: uma marcação Cancelada ou Realizada
/// vai para o histórico mesmo que a data ainda não tenha chegado (a I028 separava só pela data).
/// </summary>
public class MinhasConsultasServiceTests
{
    // RELOGIO: terça-feira, 2026-10-20 10:00 UTC.
    private static readonly DateTimeOffset Relogio = new(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);

    // Propriedades de MinhaConsultaDto, por ordem alfabética: só dados administrativos.
    private static readonly string[] CamposDaResposta = ["Animal", "DataHora", "Estado", "Id", "Veterinario"];

    private static readonly DateTime Criacao = new(2026, 10, 1, 9, 0, 0, DateTimeKind.Utc);

    // ----- tipo = futuras -----

    [Fact(DisplayName = "UT-F003-056 — futuras: só marcações Pendentes ou Confirmadas futuras dos animais do Cliente, por data crescente")]
    public async Task FuturasSoDoClientePorDataCrescente()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var a2Confirmada = await AdicionarMarcacaoAsync(db, d.AnimalA2, d.Vet, "2026-10-25T09:00:00Z", EstadosConsulta.Confirmada);
        var a1Pendente = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.Vet, "2026-10-22T09:00:00Z", EstadosConsulta.Pendente);
        await AdicionarMarcacaoAsync(db, d.AnimalA1, d.Vet, "2026-10-15T09:00:00Z", EstadosConsulta.Realizada);
        await AdicionarMarcacaoAsync(db, d.AnimalA1, d.Vet, "2026-10-23T09:00:00Z", EstadosConsulta.Cancelada);
        await AdicionarMarcacaoAsync(db, d.AnimalB1, d.Vet, "2026-10-21T09:00:00Z", EstadosConsulta.Pendente);

        var futuras = await CriarServico(db).ListarAsync(d.ClienteA, new MinhasConsultasQuery { Tipo = TiposMinhasConsultas.Futuras });

        Assert.Equal(new[] { a1Pendente, a2Confirmada }, futuras.Select(c => c.Id));
        Assert.All(futuras, c => Assert.Contains(c.Animal.Id, new[] { d.AnimalA1, d.AnimalA2 }));
    }

    [Fact(DisplayName = "UT-F003-057 — fronteira: uma marcação que começa exatamente agora ainda é futura")]
    public async Task MarcacaoQueComecaAgoraEFutura()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var agora = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.Vet, "2026-10-20T10:00:00Z", EstadosConsulta.Confirmada);
        var servico = CriarServico(db);

        var futuras = await servico.ListarAsync(d.ClienteA, new MinhasConsultasQuery { Tipo = TiposMinhasConsultas.Futuras });
        var historico = await servico.ListarAsync(d.ClienteA, new MinhasConsultasQuery { Tipo = TiposMinhasConsultas.Historico });

        Assert.Equal(agora, Assert.Single(futuras).Id);
        Assert.Empty(historico);
    }

    [Fact(DisplayName = "UT-F003-058 — historico: marcações passadas e Canceladas, mesmo futuras, por data decrescente")]
    public async Task HistoricoPassadasECanceladasPorDataDecrescente()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var realizada = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.Vet, "2026-10-14T09:30:00Z", EstadosConsulta.Realizada);
        var canceladaPassada = await AdicionarMarcacaoAsync(db, d.AnimalA2, d.Vet, "2026-10-19T10:00:00Z", EstadosConsulta.Cancelada);
        var canceladaFutura = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.Vet, "2026-11-03T09:00:00Z", EstadosConsulta.Cancelada);
        var pendentePassada = await AdicionarMarcacaoAsync(db, d.AnimalA2, d.Vet, "2026-10-20T09:59:00Z", EstadosConsulta.Pendente);
        await AdicionarMarcacaoAsync(db, d.AnimalA1, d.Vet, "2026-10-28T09:00:00Z", EstadosConsulta.Pendente);
        await AdicionarMarcacaoAsync(db, d.AnimalB1, d.Vet, "2026-10-14T11:00:00Z", EstadosConsulta.Realizada);

        var historico = await CriarServico(db).ListarAsync(d.ClienteA, new MinhasConsultasQuery { Tipo = TiposMinhasConsultas.Historico });

        Assert.Equal(new[] { canceladaFutura, pendentePassada, canceladaPassada, realizada }, historico.Select(c => c.Id));
    }

    [Fact(DisplayName = "UT-F003-059 — sem tipo é igual a tipo = futuras")]
    public async Task SemTipoEIgualAFuturas()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        await AdicionarMarcacaoAsync(db, d.AnimalA1, d.Vet, "2026-10-15T09:00:00Z", EstadosConsulta.Realizada);
        await AdicionarMarcacaoAsync(db, d.AnimalA1, d.Vet, "2026-10-22T09:00:00Z", EstadosConsulta.Pendente);
        var servico = CriarServico(db);

        var semTipo = await servico.ListarAsync(d.ClienteA, new MinhasConsultasQuery());
        var futuras = await servico.ListarAsync(d.ClienteA, new MinhasConsultasQuery { Tipo = TiposMinhasConsultas.Futuras });

        Assert.Equal(futuras.Select(c => c.Id), semTipo.Select(c => c.Id));
        Assert.Single(semTipo);
    }

    [Theory(DisplayName = "UT-F003-060 — tipo inválido devolve 400")]
    [InlineData("todas")]
    [InlineData("passadas")]
    [InlineData("futura")]
    public async Task TipoInvalidoDevolve400(string tipo)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<ArgumentException>(
            () => CriarServico(db).ListarAsync(d.ClienteA, new MinhasConsultasQuery { Tipo = tipo }));

        Assert.Equal(TiposMinhasConsultas.MensagemTipoInvalido, erro.Message);
    }

    [Fact(DisplayName = "UT-F003-061 — animalId de um animal de outro Cliente devolve 403")]
    public async Task AnimalDeOutroClienteDevolve403()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        await AdicionarMarcacaoAsync(db, d.AnimalB1, d.Vet, "2026-10-22T09:00:00Z", EstadosConsulta.Pendente);

        var erro = await Assert.ThrowsAsync<ForbiddenException>(
            () => CriarServico(db).ListarAsync(d.ClienteA, new MinhasConsultasQuery { AnimalId = d.AnimalB1 }));

        Assert.Equal(MinhasConsultasService.MensagemAnimalDeOutroCliente, erro.Message);
    }

    // ----- Casos próprios da I076 -----

    [Fact(DisplayName = "I076 — animalId inexistente também devolve 403, sem revelar se o animal existe")]
    public async Task AnimalInexistenteDevolve403()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        await Assert.ThrowsAsync<ForbiddenException>(
            () => CriarServico(db).ListarAsync(d.ClienteA, new MinhasConsultasQuery { AnimalId = d.AnimalB1 + 1000 }));
    }

    [Fact(DisplayName = "I076 — animalId próprio devolve só as marcações desse animal")]
    public async Task AnimalProprioFiltra()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var a1 = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.Vet, "2026-10-22T09:00:00Z", EstadosConsulta.Pendente);
        await AdicionarMarcacaoAsync(db, d.AnimalA2, d.Vet, "2026-10-23T09:00:00Z", EstadosConsulta.Pendente);
        var servico = CriarServico(db);

        var doA1 = await servico.ListarAsync(d.ClienteA, new MinhasConsultasQuery { AnimalId = d.AnimalA1 });
        var historicoDoA2 = await servico.ListarAsync(d.ClienteA, new MinhasConsultasQuery { Tipo = TiposMinhasConsultas.Historico, AnimalId = d.AnimalA2 });

        Assert.Equal(a1, Assert.Single(doA1).Id);
        Assert.Empty(historicoDoA2);
    }

    [Fact(DisplayName = "I076 — uma marcação Realizada com data futura vai para o histórico")]
    public async Task RealizadaFuturaVaiParaHistorico()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var realizada = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.Vet, "2026-10-20T11:00:00Z", EstadosConsulta.Realizada);
        var servico = CriarServico(db);

        var futuras = await servico.ListarAsync(d.ClienteA, new MinhasConsultasQuery());
        var historico = await servico.ListarAsync(d.ClienteA, new MinhasConsultasQuery { Tipo = TiposMinhasConsultas.Historico });

        Assert.Empty(futuras);
        Assert.Equal(realizada, Assert.Single(historico).Id);
    }

    [Fact(DisplayName = "I076 — cada marcação do Cliente aparece numa só das duas vistas")]
    public async Task CadaMarcacaoNumaSoVista()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var ids = new List<int>();
        foreach (var estado in EstadosConsulta.Todos)
        {
            ids.Add(await AdicionarMarcacaoAsync(db, d.AnimalA1, d.Vet, "2026-10-19T09:00:00Z", estado));
            ids.Add(await AdicionarMarcacaoAsync(db, d.AnimalA2, d.Vet, "2026-10-21T09:00:00Z", estado));
        }

        var servico = CriarServico(db);
        var futuras = await servico.ListarAsync(d.ClienteA, new MinhasConsultasQuery());
        var historico = await servico.ListarAsync(d.ClienteA, new MinhasConsultasQuery { Tipo = TiposMinhasConsultas.Historico });

        Assert.Empty(futuras.Select(c => c.Id).Intersect(historico.Select(c => c.Id)));
        Assert.Equal(ids.Order(), futuras.Concat(historico).Select(c => c.Id).Order());
        Assert.All(futuras, c => Assert.True(TiposMinhasConsultas.EFutura(c.DataHora, c.Estado, Relogio.UtcDateTime)));
        Assert.All(historico, c => Assert.False(TiposMinhasConsultas.EFutura(c.DataHora, c.Estado, Relogio.UtcDateTime)));
    }

    [Theory(DisplayName = "I076 — o tipo não distingue maiúsculas nem espaços à volta")]
    [InlineData("HISTORICO")]
    [InlineData(" Historico ")]
    public async Task TipoSemDistinguirMaiusculas(string tipo)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var passada = await AdicionarMarcacaoAsync(db, d.AnimalA1, d.Vet, "2026-10-15T09:00:00Z", EstadosConsulta.Realizada);

        var historico = await CriarServico(db).ListarAsync(d.ClienteA, new MinhasConsultasQuery { Tipo = tipo });

        Assert.Equal(passada, Assert.Single(historico).Id);
    }

    [Fact(DisplayName = "I076 — a resposta só tem dados administrativos: data em UTC, estado, animal e nome do Veterinário")]
    public async Task RespostaSoComDadosAdministrativos()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        await AdicionarMarcacaoAsync(db, d.AnimalA1, d.Vet, "2026-10-22T09:30:00Z", EstadosConsulta.Confirmada, motivo: "Comichão no ouvido");

        var consulta = Assert.Single(await CriarServico(db).ListarAsync(d.ClienteA, new MinhasConsultasQuery()));

        Assert.Equal(new DateTime(2026, 10, 22, 9, 30, 0, DateTimeKind.Utc), consulta.DataHora);
        Assert.Equal(DateTimeKind.Utc, consulta.DataHora.Kind);
        Assert.Equal(EstadosConsulta.Confirmada, consulta.Estado);
        Assert.Equal(d.AnimalA1, consulta.Animal.Id);
        Assert.Equal("Bobi", consulta.Animal.Nome);
        Assert.Equal("Cão", consulta.Animal.Especie);
        Assert.Equal("Ana Vet", consulta.Veterinario);
        Assert.Equal(
            CamposDaResposta,
            typeof(MinhaConsultaDto).GetProperties().Select(p => p.Name).Order());
    }

    [Fact(DisplayName = "I076 — Cliente sem marcações recebe listas vazias")]
    public async Task ClienteSemMarcacoesRecebeListasVazias()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        await AdicionarMarcacaoAsync(db, d.AnimalB1, d.Vet, "2026-10-22T09:00:00Z", EstadosConsulta.Pendente);
        var servico = CriarServico(db);

        Assert.Empty(await servico.ListarAsync(d.ClienteA, new MinhasConsultasQuery()));
        Assert.Empty(await servico.ListarAsync(d.ClienteA, new MinhasConsultasQuery { Tipo = TiposMinhasConsultas.Historico }));
    }

    // ----- MinhasConsultasQuery (validação automática do pedido, 400) -----

    [Theory(DisplayName = "I076 — MinhasConsultasQuery aceita futuras, historico ou nada")]
    [InlineData(null)]
    [InlineData("futuras")]
    [InlineData("historico")]
    public void QueryValida(string? tipo)
    {
        Assert.Empty(Validar(new MinhasConsultasQuery { Tipo = tipo, AnimalId = 1 }));
    }

    [Fact(DisplayName = "I076 — MinhasConsultasQuery recusa tipo desconhecido e animalId inferior a 1 no mesmo pedido")]
    public void QueryInvalida()
    {
        var erros = Validar(new MinhasConsultasQuery { Tipo = "todas", AnimalId = 0 });

        Assert.Contains(erros, e => e.ErrorMessage == TiposMinhasConsultas.MensagemTipoInvalido
            && e.MemberNames.Contains(nameof(MinhasConsultasQuery.Tipo)));
        Assert.Contains(erros, e => e.ErrorMessage == MinhasConsultasQuery.MensagemAnimalIdInvalido
            && e.MemberNames.Contains(nameof(MinhasConsultasQuery.AnimalId)));
    }

    private static List<ValidationResult> Validar(MinhasConsultasQuery query)
    {
        var erros = new List<ValidationResult>();
        _ = Validator.TryValidateObject(query, new ValidationContext(query), erros, validateAllProperties: true);
        return erros;
    }

    // ----- Dados -----

    private static MinhasConsultasService CriarServico(ClinicaDbContext db) => new(db, new RelogioFixo(Relogio));

    private sealed record DadosBase(int Vet, int ClienteA, int AnimalA1, int AnimalA2, int AnimalB1);

    private static async Task<DadosBase> CriarBaseAsync(ClinicaDbContext db)
    {
        var vet = await AdicionarFuncionarioAsync(db, "Ana Vet", Perfis.Veterinario);

        var especie = new Especie { Nome = "Cão" };
        db.Especies.Add(especie);
        await db.SaveChangesAsync();

        var clienteA = await AdicionarClienteAsync(db, "Joana Cliente");
        var clienteB = await AdicionarClienteAsync(db, "Bernardo Cliente");

        var animalA1 = await AdicionarAnimalAsync(db, clienteA, especie.Id, "Bobi");
        var animalA2 = await AdicionarAnimalAsync(db, clienteA, especie.Id, "Mia");
        var animalB1 = await AdicionarAnimalAsync(db, clienteB, especie.Id, "Rex");

        return new DadosBase(vet, clienteA, animalA1, animalA2, animalB1);
    }

    private static async Task<int> AdicionarFuncionarioAsync(ClinicaDbContext db, string nome, string perfil)
    {
        var utilizador = NovoUtilizador(perfil);
        db.Utilizadores.Add(utilizador);
        await db.SaveChangesAsync();

        db.Funcionarios.Add(new Funcionario { UtilizadorId = utilizador.Id, Nome = nome });
        await db.SaveChangesAsync();

        return utilizador.Id;
    }

    private static async Task<int> AdicionarClienteAsync(ClinicaDbContext db, string nome)
    {
        var utilizador = NovoUtilizador(Perfis.Cliente);
        db.Utilizadores.Add(utilizador);
        await db.SaveChangesAsync();

        var cliente = new Cliente
        {
            UtilizadorId = utilizador.Id,
            Nome = nome,
            Telefone = "960000001",
            DataRegisto = Criacao
        };
        db.Clientes.Add(cliente);
        await db.SaveChangesAsync();

        return cliente.Id;
    }

    private static Utilizador NovoUtilizador(string perfil) => new()
    {
        Email = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "@clinica-g08.test",
        PasswordHash = "hash-nao-usado-nestes-testes",
        Perfil = perfil,
        Estado = EstadosUtilizador.Ativo,
        DataCriacao = Criacao
    };

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
        string? motivo = null)
    {
        var marcacao = new Marcacao
        {
            PacienteId = pacienteId,
            VeterinarioId = veterinarioId,
            DataHoraInicio = DateTime.Parse(inicioUtc, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
            DuracaoMinutos = 30,
            Motivo = motivo,
            Estado = estado,
            CriadaPorId = veterinarioId,
            DataCriacao = Criacao
        };
        db.Marcacoes.Add(marcacao);
        await db.SaveChangesAsync();

        return marcacao.Id;
    }
}
