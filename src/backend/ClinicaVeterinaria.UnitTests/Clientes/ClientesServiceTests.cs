using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ClinicaVeterinaria.Application.DTOs.Clientes;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.UnitTests.Seguranca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ClinicaVeterinaria.UnitTests.Clientes;

/// <summary>
/// I086 — contas de Cliente (EP-011 a EP-014) no <see cref="ClientesService"/>: casos UT-F001-112 a
/// UT-F001-125 e UT-TRV-005 da Especificação de Testes Unitários (I028, secções 7.1.10 e 7.6.1).
/// As regras de formato (NIF, e-mail e telefone) estão nos DTO e dão 400 na validação automática
/// do [ApiController]; por isso são verificadas no pedido.
/// </summary>
public class ClientesServiceTests
{
    // RELOGIO da Especificação de Testes Unitários (I028, secção 5).
    private static readonly DateTimeOffset Relogio = new(2026, 10, 14, 8, 0, 0, TimeSpan.Zero);

    // CLIENTE_A (João Costa, com ANIMAL_A1 e ANIMAL_A2) e CLIENTE_B (Maria Silva, com ANIMAL_B1).
    private const int ClienteA = 1;
    private const int ClienteB = 2;
    private const int ContaClienteA = 10;
    private const int ContaClienteB = 11;
    private const string NifClienteA = "210000015";
    private const string NifClienteB = "234567899";
    private const string EmailClienteA = "cliente.joao@clinica-g08.test";
    private const string EmailClienteB = "cliente.maria@clinica-g08.test";

    private const string NifLivre = "123456789";
    private const string EmailLivre = "novo.cliente@clinica-g08.test";

    // ----- EP-013 — Criar Cliente -----

    [Fact(DisplayName = "UT-F001-112 — Criar um Cliente cria a conta Utilizador (Cliente, Ativo) e o Cliente ligado a ela; o e-mail fica só na conta")]
    public async Task CriarClienteCriaContaEClienteLigados()
    {
        using var db = CriarBase();

        var criado = await CriarServico(db).CriarAsync(PedidoCriacao(NifLivre, EmailLivre));

        var conta = await db.Utilizadores.AsNoTracking().SingleAsync(u => u.Email == EmailLivre);
        var cliente = await db.Clientes.AsNoTracking().SingleAsync(c => c.Id == criado.Id);
        Assert.Equal(Perfis.Cliente, conta.Perfil);
        Assert.Equal(EstadosUtilizador.Ativo, conta.Estado);
        Assert.Equal(HasherDuplo.NovoHash, conta.PasswordHash);
        Assert.Equal(conta.Id, cliente.UtilizadorId);
        Assert.Equal(EmailLivre, criado.Email);
    }

    [Fact(DisplayName = "UT-F001-113 — Sem NIF, o Cliente é criado (NIF opcional)")]
    public async Task CriarClienteSemNif()
    {
        using var db = CriarBase();

        var criado = await CriarServico(db).CriarAsync(PedidoCriacao(nif: null, EmailLivre));

        Assert.Null((await db.Clientes.AsNoTracking().SingleAsync(c => c.Id == criado.Id)).NIF);
    }

    [Fact(DisplayName = "UT-F001-114 — Um NIF com o dígito de controlo errado é recusado no pedido (400)")]
    public void NifInvalidoNaCriacaoEInvalido()
    {
        Assert.Contains(nameof(CreateClienteDto.Nif), CamposComErro(PedidoCriacao("501234567", EmailLivre)));
    }

    [Fact(DisplayName = "UT-F001-115 — Um e-mail sem @ é recusado no pedido (400)")]
    public void EmailInvalidoNaCriacaoEInvalido()
    {
        Assert.Contains(nameof(CreateClienteDto.Email), CamposComErro(PedidoCriacao(NifLivre, "joao")));
    }

    [Fact(DisplayName = "UT-F001-116 — Sem telefone, o pedido é recusado (400)")]
    public void TelefoneEmFaltaEInvalido()
    {
        var pedido = PedidoCriacao(NifLivre, EmailLivre);
        pedido.Telefone = string.Empty;

        Assert.Contains(nameof(CreateClienteDto.Telefone), CamposComErro(pedido));
    }

    [Theory(DisplayName = "UT-F001-127 — Uma palavra-passe inicial fraca é recusada no pedido (400), com a regra dos funcionários")]
    [InlineData("aaaaaaaa")]
    [InlineData("12345678")]
    [InlineData("Temp2026a")]
    public void PalavraPasseFracaNaCriacaoEInvalida(string palavraPasse)
    {
        var pedido = PedidoCriacao(NifLivre, EmailLivre);
        pedido.PasswordInicial = palavraPasse;

        Assert.Contains(nameof(CreateClienteDto.PasswordInicial), CamposComErro(pedido));
    }

    [Fact(DisplayName = "UT-F001-128 — Uma palavra-passe inicial forte é aceite no pedido")]
    public void PalavraPasseForteNaCriacaoEValida()
    {
        Assert.DoesNotContain(nameof(CreateClienteDto.PasswordInicial), CamposComErro(PedidoCriacao(NifLivre, EmailLivre)));
    }

    [Fact(DisplayName = "UT-F001-117 — O NIF de outro Cliente devolve 409 e nada é gravado")]
    public async Task NifRepetidoDevolve409()
    {
        using var db = CriarBase();

        await Assert.ThrowsAsync<ConflitoException>(
            () => CriarServico(db).CriarAsync(PedidoCriacao(NifClienteA, EmailLivre)));

        Assert.Equal(2, await db.Clientes.CountAsync());
        Assert.Equal(2, await db.Utilizadores.CountAsync());
    }

    [Fact(DisplayName = "UT-F001-118 — Um e-mail já registado devolve 409 e nada é gravado")]
    public async Task EmailRepetidoDevolve409()
    {
        using var db = CriarBase();

        await Assert.ThrowsAsync<ConflitoException>(
            () => CriarServico(db).CriarAsync(PedidoCriacao(NifLivre, EmailClienteA)));

        Assert.Equal(2, await db.Clientes.CountAsync());
        Assert.Equal(2, await db.Utilizadores.CountAsync());
    }

    // ----- EP-014 — Atualizar Cliente -----

    [Fact(DisplayName = "UT-F001-119 — Um novo e-mail atualiza o Cliente e o e-mail da conta Utilizador")]
    public async Task NovoEmailAtualizaClienteEConta()
    {
        using var db = CriarBase();
        var pedido = PedidoAtualizacao(NifClienteA, "joao.costa.novo@clinica-g08.test");
        pedido.Telefone = "960000999";

        await CriarServico(db).AtualizarAsync(ClienteA, pedido);

        Assert.Equal("960000999", (await db.Clientes.AsNoTracking().SingleAsync(c => c.Id == ClienteA)).Telefone);
        Assert.Equal("joao.costa.novo@clinica-g08.test", (await db.Utilizadores.AsNoTracking().SingleAsync(u => u.Id == ContaClienteA)).Email);
    }

    [Fact(DisplayName = "UT-F001-120 — Atualizar um Cliente com o NIF de outro Cliente devolve 409")]
    public async Task NifDeOutroClienteDevolve409()
    {
        using var db = CriarBase();

        await Assert.ThrowsAsync<ConflitoException>(
            () => CriarServico(db).AtualizarAsync(ClienteA, PedidoAtualizacao(NifClienteB, EmailClienteA)));
    }

    [Fact(DisplayName = "UT-F001-121 — Manter o próprio NIF não conta como conflito")]
    public async Task ProprioNifEAceite()
    {
        using var db = CriarBase();

        var atualizado = await CriarServico(db).AtualizarAsync(ClienteA, PedidoAtualizacao(NifClienteA, EmailClienteA));

        Assert.Equal(NifClienteA, atualizado.Nif);
    }

    [Fact(DisplayName = "UT-F001-122 — Um NIF com 5 algarismos é recusado no pedido de atualização (400)")]
    public void NifCurtoNaAtualizacaoEInvalido()
    {
        Assert.Contains(nameof(UpdateClienteDto.Nif), CamposComErro(PedidoAtualizacao("12345", EmailClienteA)));
    }

    [Fact(DisplayName = "UT-F001-123 — Atualizar um Cliente inexistente devolve 404")]
    public async Task ClienteInexistenteDevolve404()
    {
        using var db = CriarBase();

        await Assert.ThrowsAsync<NaoEncontradoException>(
            () => CriarServico(db).AtualizarAsync(999, PedidoAtualizacao(NifLivre, EmailLivre)));
    }

    // ----- EP-011 e EP-012 — Pesquisar e consultar -----

    [Fact(DisplayName = "UT-F001-124 — A pesquisa \"Costa\" devolve só o CLIENTE_A, com numeroAnimais = 2")]
    public async Task PesquisaPorNomeDevolveClienteComNumeroDeAnimais()
    {
        using var db = CriarBase();

        var resultado = await CriarServico(db).PesquisarAsync(new ClienteFiltroQuery { Pesquisa = "Costa" });

        var cliente = Assert.Single(resultado.Itens);
        Assert.Equal(ClienteA, cliente.Id);
        Assert.Equal(2, cliente.NumeroAnimais);
    }

    [Fact(DisplayName = "UT-F001-125 — O ClienteDto tem o e-mail da conta, o estado da conta e os dois animais")]
    public async Task ObterClienteTemContaEAnimais()
    {
        using var db = CriarBase();

        var cliente = await CriarServico(db).ObterAsync(ClienteA);

        Assert.Equal(EmailClienteA, cliente.Email);
        Assert.Equal(EstadosUtilizador.Ativo, cliente.EstadoConta);
        Assert.Equal(["Bobi", "Mia"], cliente.Animais.Select(a => a.Nome));
    }

    [Fact(DisplayName = "UT-TRV-005 — Com 57 Clientes, a página 3 de 20 tem 17 elementos e total = 57")]
    public async Task TerceiraPaginaTemOsUltimosDezassete()
    {
        using var db = CriarContexto();
        for (var i = 1; i <= 57; i++)
        {
            db.Utilizadores.Add(new Utilizador { Id = i, Email = $"cliente{i:D2}@clinica-g08.test", PasswordHash = "h", Perfil = Perfis.Cliente });
            db.Clientes.Add(new Cliente { Id = i, UtilizadorId = i, Nome = $"Cliente {i:D2}", Telefone = "960000000" });
        }

        await db.SaveChangesAsync();

        var pagina = await CriarServico(db).PesquisarAsync(new ClienteFiltroQuery { Pagina = 3, Tamanho = 20 });

        Assert.Equal(17, pagina.Itens.Count);
        Assert.Equal(3, pagina.Pagina);
        Assert.Equal(20, pagina.Tamanho);
        Assert.Equal(57, pagina.Total);
    }

    [Theory(DisplayName = "UT-F001-126 — Depois do fim, ou numa página muito alta (int.MaxValue), a lista fica vazia, sem erro, e o total mantém-se")]
    [InlineData(2, 20)]
    [InlineData(int.MaxValue, 100)]
    public async Task PaginaDepoisDoFimDevolveListaVazia(int pagina, int tamanho)
    {
        using var db = CriarBase();
        var servico = CriarServico(db);
        var todos = await servico.PesquisarAsync(new ClienteFiltroQuery { Tamanho = 100 });

        var resultado = await servico.PesquisarAsync(new ClienteFiltroQuery { Pagina = pagina, Tamanho = tamanho });

        Assert.Empty(resultado.Itens);
        Assert.Equal(pagina, resultado.Pagina);
        Assert.NotEqual(0, todos.Total);
        Assert.Equal(todos.Total, resultado.Total);
    }

    // ----- Apoio -----

    private static ClientesService CriarServico(ClinicaDbContext db) =>
        new(db, new HasherDuplo(), new RelogioFixo(Relogio));

    private static ClinicaDbContext CriarContexto() =>
        new(new DbContextOptionsBuilder<ClinicaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture))
            // O InMemory não tem transações (I028, secção 4.2): a transação do serviço é ignorada.
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options);

    private static ClinicaDbContext CriarBase()
    {
        var db = CriarContexto();

        db.Utilizadores.AddRange(
            new Utilizador { Id = ContaClienteA, Email = EmailClienteA, PasswordHash = "h", Perfil = Perfis.Cliente },
            new Utilizador { Id = ContaClienteB, Email = EmailClienteB, PasswordHash = "h", Perfil = Perfis.Cliente });
        db.Clientes.AddRange(
            new Cliente { Id = ClienteA, UtilizadorId = ContaClienteA, Nome = "João Costa", NIF = NifClienteA, Telefone = "960000001" },
            new Cliente { Id = ClienteB, UtilizadorId = ContaClienteB, Nome = "Maria Silva", NIF = NifClienteB, Telefone = "960000002" });
        db.Especies.AddRange(new Especie { Id = 1, Nome = "Cão" }, new Especie { Id = 2, Nome = "Gato" });
        db.Pacientes.AddRange(
            new Paciente { Id = 1, ClienteId = ClienteA, Nome = "Bobi", EspecieId = 1 },
            new Paciente { Id = 2, ClienteId = ClienteA, Nome = "Mia", EspecieId = 2 },
            new Paciente { Id = 3, ClienteId = ClienteB, Nome = "Kiko", EspecieId = 1 });
        db.SaveChanges();
        db.ChangeTracker.Clear();

        return db;
    }

    private static CreateClienteDto PedidoCriacao(string? nif, string email) => new()
    {
        Nome = "Novo Cliente",
        Nif = nif,
        Telefone = "960000003",
        Email = email,
        Morada = "Rua Fictícia, 3, 4470-003 Maia",
        PasswordInicial = "Temp#2026"
    };

    private static UpdateClienteDto PedidoAtualizacao(string? nif, string email) => new()
    {
        Nome = "João Costa",
        Nif = nif,
        Telefone = "960000001",
        Email = email
    };

    private static List<string> CamposComErro(object pedido)
    {
        var erros = new List<ValidationResult>();
        Validator.TryValidateObject(pedido, new ValidationContext(pedido), erros, validateAllProperties: true);
        return erros.SelectMany(e => e.MemberNames).ToList();
    }
}
