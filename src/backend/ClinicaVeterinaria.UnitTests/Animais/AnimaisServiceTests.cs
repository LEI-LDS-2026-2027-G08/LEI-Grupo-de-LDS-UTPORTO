using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.DTOs.Animais;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.UnitTests.Seguranca;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.UnitTests.Animais;

/// <summary>
/// I086 — pacientes (EP-015 a EP-020) no <see cref="AnimaisService"/>: casos UT-F002-001 a
/// UT-F002-030 da Especificação de Testes Unitários (I028, secções 7.2.1 e 7.2.2). A idade
/// (casos 025 a 030) é calculada dentro do serviço, por isso é verificada através da ficha do
/// animal com o relógio injetado. As regras de formato do pedido (clienteId e nome) estão no
/// <see cref="UpdateAnimalDto"/> e dão 400 na validação automática do [ApiController].
/// </summary>
public class AnimaisServiceTests
{
    // RELOGIO da Especificação de Testes Unitários (I028, secção 5): hoje é 2026-10-14.
    private static readonly DateTimeOffset Relogio = new(2026, 10, 14, 8, 0, 0, TimeSpan.Zero);

    // CLIENTE_A (ANIMAL_A1 Bobi e ANIMAL_A2 Mia), CLIENTE_B (ANIMAL_B1 Kiko) e CLIENTE_C, sem animais.
    private const int ClienteA = 1;
    private const int ClienteB = 2;
    private const int ClienteC = 3;
    private const int AnimalA1 = 1;
    private const int AnimalA2 = 2;
    private const int AnimalB1 = 3;

    private const int EspecieCao = 1;
    private const int EspecieGato = 2;

    // ----- EP-017 — Registar paciente -----

    [Fact(DisplayName = "UT-F002-001 — Registar o Bobi cria o paciente do CLIENTE_A com DataRegisto = RELOGIO e idadeAnos = 4")]
    public async Task RegistarAnimalValido()
    {
        using var db = CriarBase();

        var criado = await CriarServico(db).CriarAsync(PedidoBobi());

        var paciente = await db.Pacientes.AsNoTracking().SingleAsync(p => p.Id == criado.Id);
        Assert.Equal(ClienteA, paciente.ClienteId);
        Assert.Equal(Relogio.UtcDateTime, paciente.DataRegisto);
        Assert.Equal(4, criado.IdadeAnos);
    }

    [Fact(DisplayName = "UT-F002-002 — Sem raça, data de nascimento e peso, o paciente é registado com esses campos vazios")]
    public async Task RegistarSemCamposOpcionais()
    {
        using var db = CriarBase();
        var pedido = PedidoBobi();
        pedido.Raca = null;
        pedido.DataNascimento = null;
        pedido.Peso = null;

        var criado = await CriarServico(db).CriarAsync(pedido);

        var paciente = await db.Pacientes.AsNoTracking().SingleAsync(p => p.Id == criado.Id);
        Assert.Null(paciente.RacaId);
        Assert.Null(paciente.DataNascimento);
        Assert.Null(paciente.Peso);
    }

    [Fact(DisplayName = "UT-F002-003 — clienteId = 0 (em falta) é recusado no pedido (400)")]
    public void ClienteIdEmFaltaEInvalido()
    {
        var pedido = PedidoBobi();
        pedido.ClienteId = 0;

        Assert.Contains(nameof(CreateAnimalDto.ClienteId), CamposComErro(pedido));
    }

    [Fact(DisplayName = "UT-F002-004 — Um Cliente inexistente devolve 404 e nada é gravado")]
    public async Task ClienteInexistenteDevolve404()
    {
        using var db = CriarBase();
        var pedido = PedidoBobi();
        pedido.ClienteId = 999;

        await Assert.ThrowsAsync<NaoEncontradoException>(() => CriarServico(db).CriarAsync(pedido));

        Assert.Equal(3, await db.Pacientes.CountAsync());
    }

    [Fact(DisplayName = "UT-F002-005 — Sem nome, o pedido é recusado (400)")]
    public void NomeVazioEInvalido()
    {
        var pedido = PedidoBobi();
        pedido.Nome = string.Empty;

        Assert.Contains(nameof(CreateAnimalDto.Nome), CamposComErro(pedido));
    }

    [Fact(DisplayName = "UT-F002-006 — Um nome com 81 caracteres é recusado (máximo 80)")]
    public void NomeCom81CaracteresEInvalido()
    {
        var pedido = PedidoBobi();
        pedido.Nome = new string('B', 81);

        Assert.Contains(nameof(CreateAnimalDto.Nome), CamposComErro(pedido));
    }

    [Fact(DisplayName = "UT-F002-007 — Peso 0 devolve 400 «O peso deve ser superior a 0.»")]
    public async Task PesoZeroDevolve400()
    {
        using var db = CriarBase();
        var pedido = PedidoBobi();
        pedido.Peso = 0m;

        var erro = await Assert.ThrowsAsync<ValidacaoException>(() => CriarServico(db).CriarAsync(pedido));

        Assert.Equal("O peso deve ser superior a 0.", erro.Message);
    }

    [Fact(DisplayName = "UT-F002-008 — Peso 0,01 é aceite")]
    public async Task PesoMinimoEAceite()
    {
        using var db = CriarBase();
        var pedido = PedidoBobi();
        pedido.Peso = 0.01m;

        var criado = await CriarServico(db).CriarAsync(pedido);

        Assert.Equal(0.01m, criado.Peso);
    }

    [Fact(DisplayName = "UT-F002-009 — Peso negativo devolve 400")]
    public async Task PesoNegativoDevolve400()
    {
        using var db = CriarBase();
        var pedido = PedidoBobi();
        pedido.Peso = -3m;

        await Assert.ThrowsAsync<ValidacaoException>(() => CriarServico(db).CriarAsync(pedido));
    }

    [Fact(DisplayName = "UT-F002-010 — Data de nascimento de amanhã (2026-10-15) devolve 400")]
    public async Task DataDeNascimentoFuturaDevolve400()
    {
        using var db = CriarBase();
        var pedido = PedidoBobi();
        pedido.DataNascimento = new DateOnly(2026, 10, 15);

        await Assert.ThrowsAsync<ValidacaoException>(() => CriarServico(db).CriarAsync(pedido));
    }

    [Fact(DisplayName = "UT-F002-011 — Data de nascimento de hoje é aceite e a idade é 0")]
    public async Task NascidoHojeTemIdadeZero()
    {
        using var db = CriarBase();
        var pedido = PedidoBobi();
        pedido.DataNascimento = new DateOnly(2026, 10, 14);

        var criado = await CriarServico(db).CriarAsync(pedido);

        Assert.Equal(0, criado.IdadeAnos);
    }

    [Fact(DisplayName = "UT-F002-012 — Uma espécie que não existe devolve 400 com erro na espécie")]
    public async Task EspecieInexistenteDevolve400()
    {
        using var db = CriarBase();
        var pedido = PedidoBobi();
        pedido.Especie = "Dragão";
        pedido.Raca = null;

        var erro = await Assert.ThrowsAsync<ValidacaoException>(() => CriarServico(db).CriarAsync(pedido));

        Assert.Contains("espécie", erro.Message, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "UT-F002-013 — Uma raça de gato (Siamês) num cão devolve 400 com erro na raça")]
    public async Task RacaDeOutraEspecieDevolve400()
    {
        using var db = CriarBase();
        var pedido = PedidoBobi();
        pedido.Raca = "Siamês";

        var erro = await Assert.ThrowsAsync<ValidacaoException>(() => CriarServico(db).CriarAsync(pedido));

        Assert.Contains("raça", erro.Message, StringComparison.Ordinal);
    }

    // ----- EP-018 — Atualizar paciente -----

    [Fact(DisplayName = "UT-F002-014 — Atualizar o peso do ANIMAL_A1 grava o novo peso e DataUltimaAlteracao = RELOGIO")]
    public async Task AtualizarPesoGravaAlteracao()
    {
        using var db = CriarBase();
        var pedido = PedidoBobi();
        pedido.Peso = 29.1m;

        await CriarServico(db).AtualizarAsync(AnimalA1, pedido);

        var paciente = await db.Pacientes.AsNoTracking().SingleAsync(p => p.Id == AnimalA1);
        Assert.Equal(29.1m, paciente.Peso);
        Assert.Equal(Relogio.UtcDateTime, paciente.DataUltimaAlteracao);
    }

    [Fact(DisplayName = "UT-F002-015 — Atualizar com peso 0 devolve 400 e nada é alterado")]
    public async Task AtualizarComPesoZeroNaoAltera()
    {
        using var db = CriarBase();
        var pedido = PedidoBobi();
        pedido.Peso = 0m;

        await Assert.ThrowsAsync<ValidacaoException>(() => CriarServico(db).AtualizarAsync(AnimalA1, pedido));

        Assert.Equal(28.4m, (await db.Pacientes.AsNoTracking().SingleAsync(p => p.Id == AnimalA1)).Peso);
    }

    [Fact(DisplayName = "UT-F002-016 — Atualizar um animal inexistente devolve 404")]
    public async Task AtualizarAnimalInexistenteDevolve404()
    {
        using var db = CriarBase();

        await Assert.ThrowsAsync<NaoEncontradoException>(() => CriarServico(db).AtualizarAsync(999, PedidoBobi()));
    }

    [Fact(DisplayName = "UT-F002-017 — Atualizar para um Cliente inexistente devolve 404")]
    public async Task AtualizarParaClienteInexistenteDevolve404()
    {
        using var db = CriarBase();
        var pedido = PedidoBobi();
        pedido.ClienteId = 999;

        await Assert.ThrowsAsync<NaoEncontradoException>(() => CriarServico(db).AtualizarAsync(AnimalA1, pedido));
    }

    // ----- EP-015 e EP-016 — Pesquisar e consultar -----

    [Fact(DisplayName = "UT-F002-018 — A ficha do ANIMAL_A1 tem só dados administrativos e o telefone do Cliente")]
    public async Task FichaTemDadosAdministrativosETelefone()
    {
        using var db = CriarBase();

        var animal = await CriarServico(db).ObterAsync(AnimalA1);

        Assert.Equal("Bobi", animal.Nome);
        Assert.Equal("Labrador Retriever", animal.Raca);
        Assert.Equal("960000001", animal.Cliente.Telefone);
    }

    [Fact(DisplayName = "UT-F002-019 — Filtrar por clienteId do CLIENTE_A devolve só o ANIMAL_A1 e o ANIMAL_A2")]
    public async Task FiltroPorClienteDevolveSoOsSeusAnimais()
    {
        using var db = CriarBase();

        var resultado = await CriarServico(db).PesquisarAsync(new AnimalFiltroQuery { ClienteId = ClienteA });

        Assert.Equal([AnimalA1, AnimalA2], resultado.Itens.Select(a => a.Id));
    }

    [Theory(DisplayName = "UT-F002-031 — Depois do fim, ou numa página muito alta (int.MaxValue), a lista fica vazia, sem erro, e o total mantém-se")]
    [InlineData(2, 20)]
    [InlineData(int.MaxValue, 100)]
    public async Task PaginaDepoisDoFimDevolveListaVazia(int pagina, int tamanho)
    {
        using var db = CriarBase();
        var servico = CriarServico(db);
        var todos = await servico.PesquisarAsync(new AnimalFiltroQuery { Tamanho = 100 });

        var resultado = await servico.PesquisarAsync(new AnimalFiltroQuery { Pagina = pagina, Tamanho = tamanho });

        Assert.Empty(resultado.Itens);
        Assert.Equal(pagina, resultado.Pagina);
        Assert.NotEqual(0, todos.Total);
        Assert.Equal(todos.Total, resultado.Total);
    }

    // ----- EP-019 e EP-020 — Área do Cliente -----

    [Fact(DisplayName = "UT-F002-020 — Na área do Cliente, o CLIENTE_A vê só os seus animais")]
    public async Task ClienteVeSoOsSeusAnimais()
    {
        using var db = CriarBase();

        var animais = await CriarServico(db).ListarDoClienteAsync(ClienteA);

        Assert.Equal([AnimalA1, AnimalA2], animais.Select(a => a.Id));
    }

    [Fact(DisplayName = "UT-F002-021 — Um Cliente sem animais recebe uma lista vazia, sem erro")]
    public async Task ClienteSemAnimaisRecebeListaVazia()
    {
        using var db = CriarBase();

        var animais = await CriarServico(db).ListarDoClienteAsync(ClienteC);

        Assert.Empty(animais);
    }

    [Fact(DisplayName = "UT-F002-022 — O CLIENTE_A consulta o ANIMAL_A1 e recebe o MeuAnimalDto")]
    public async Task ClienteConsultaAnimalProprio()
    {
        using var db = CriarBase();

        var animal = await CriarServico(db).ObterDoClienteAsync(ClienteA, AnimalA1);

        Assert.Equal("Bobi", animal.Nome);
    }

    [Fact(DisplayName = "UT-F002-023 — O CLIENTE_A pede o ANIMAL_B1 e recebe 403 sem dados do animal")]
    public async Task AnimalDeOutroClienteDevolve403()
    {
        using var db = CriarBase();

        var erro = await Assert.ThrowsAsync<ForbiddenException>(
            () => CriarServico(db).ObterDoClienteAsync(ClienteA, AnimalB1));

        Assert.DoesNotContain("Kiko", erro.Message, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "UT-F002-024 — O CLIENTE_A pede um animal inexistente e recebe 404")]
    public async Task AnimalInexistenteNaAreaDoClienteDevolve404()
    {
        using var db = CriarBase();

        await Assert.ThrowsAsync<NaoEncontradoException>(() => CriarServico(db).ObterDoClienteAsync(ClienteA, 999));
    }

    // ----- Idade do animal -----

    [Fact(DisplayName = "UT-F002-025 — Nascido a 2022-05-10, em 2026-10-14 tem 4 anos")]
    public async Task IdadeQuatroAnos()
    {
        Assert.Equal(4, await IdadeAsync(new DateOnly(2022, 5, 10), new DateOnly(2026, 10, 14)));
    }

    [Fact(DisplayName = "UT-F002-026 — Nascido a 2022-10-14, faz 4 anos no próprio dia")]
    public async Task IdadeNoDiaDeAnos()
    {
        Assert.Equal(4, await IdadeAsync(new DateOnly(2022, 10, 14), new DateOnly(2026, 10, 14)));
    }

    [Fact(DisplayName = "UT-F002-027 — Nascido a 2022-10-15, ainda tem 3 anos na véspera do aniversário")]
    public async Task IdadeNaVesperaDosAnos()
    {
        Assert.Equal(3, await IdadeAsync(new DateOnly(2022, 10, 15), new DateOnly(2026, 10, 14)));
    }

    [Fact(DisplayName = "UT-F002-028 — Nascido a 2024-02-29, a 2025-02-28 ainda tem 0 anos")]
    public async Task NascidoA29DeFevereiroAinda0AnosA28()
    {
        Assert.Equal(0, await IdadeAsync(new DateOnly(2024, 2, 29), new DateOnly(2025, 2, 28)));
    }

    [Fact(DisplayName = "UT-F002-029 — Nascido a 2024-02-29, a 2025-03-01 tem 1 ano")]
    public async Task NascidoA29DeFevereiroTem1AnoA1DeMarco()
    {
        Assert.Equal(1, await IdadeAsync(new DateOnly(2024, 2, 29), new DateOnly(2025, 3, 1)));
    }

    [Fact(DisplayName = "UT-F002-030 — Sem data de nascimento, idadeAnos fica vazio (divergência registada; a I028 propunha 0)")]
    public async Task SemDataDeNascimentoIdadeVazia()
    {
        Assert.Null(await IdadeAsync(null, new DateOnly(2026, 10, 14)));
    }

    // ----- Apoio -----

    private static AnimaisService CriarServico(ClinicaDbContext db) => new(db, new RelogioFixo(Relogio));

    /// <summary>Idade do ANIMAL_A1 com a data de nascimento e o dia de hoje indicados.</summary>
    private static async Task<int?> IdadeAsync(DateOnly? nascimento, DateOnly hoje)
    {
        using var db = CriarBase();
        (await db.Pacientes.SingleAsync(p => p.Id == AnimalA1)).DataNascimento = nascimento;
        await db.SaveChangesAsync();
        var relogio = new RelogioFixo(new DateTimeOffset(hoje.ToDateTime(new TimeOnly(8, 0)), TimeSpan.Zero));

        return (await new AnimaisService(db, relogio).ObterAsync(AnimalA1)).IdadeAnos;
    }

    private static ClinicaDbContext CriarBase()
    {
        var db = AjudaBaseDados.CriarContexto();

        db.Clientes.AddRange(
            new Cliente { Id = ClienteA, UtilizadorId = 10, Nome = "João Costa", Telefone = "960000001" },
            new Cliente { Id = ClienteB, UtilizadorId = 11, Nome = "Maria Silva", Telefone = "960000002" },
            new Cliente { Id = ClienteC, UtilizadorId = 12, Nome = "Carla Sem Animais", Telefone = "960000003" });
        db.Especies.AddRange(new Especie { Id = EspecieCao, Nome = "Cão" }, new Especie { Id = EspecieGato, Nome = "Gato" });
        db.Racas.AddRange(
            new Raca { Id = 1, EspecieId = EspecieCao, Nome = "Labrador Retriever" },
            new Raca { Id = 2, EspecieId = EspecieGato, Nome = "Siamês" });
        db.Pacientes.AddRange(
            new Paciente
            {
                Id = AnimalA1,
                ClienteId = ClienteA,
                Nome = "Bobi",
                EspecieId = EspecieCao,
                RacaId = 1,
                Sexo = "Macho",
                DataNascimento = new DateOnly(2022, 5, 10),
                Peso = 28.4m,
                DataRegisto = Relogio.UtcDateTime.AddDays(-30)
            },
            new Paciente { Id = AnimalA2, ClienteId = ClienteA, Nome = "Mia", EspecieId = EspecieGato, RacaId = 2 },
            new Paciente { Id = AnimalB1, ClienteId = ClienteB, Nome = "Kiko", EspecieId = EspecieCao });
        db.SaveChanges();
        db.ChangeTracker.Clear();

        return db;
    }

    /// <summary>Pedido do exemplo de EP-017 da Documentação da API, para o CLIENTE_A.</summary>
    private static CreateAnimalDto PedidoBobi() => new()
    {
        Nome = "Bobi",
        Especie = "Cão",
        Raca = "Labrador Retriever",
        Sexo = "Macho",
        DataNascimento = new DateOnly(2022, 5, 10),
        Peso = 28.4m,
        ClienteId = ClienteA
    };

    private static List<string> CamposComErro(object pedido)
    {
        var erros = new List<ValidationResult>();
        Validator.TryValidateObject(pedido, new ValidationContext(pedido), erros, validateAllProperties: true);
        return erros.SelectMany(e => e.MemberNames).ToList();
    }
}
