using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ClinicaVeterinaria.Application.DTOs.Auth;
using ClinicaVeterinaria.Application.DTOs.Utilizadores;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.UnitTests.Seguranca;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;

namespace ClinicaVeterinaria.UnitTests.Utilizadores;

/// <summary>
/// I065 e I066 — contas de funcionário (EP-004 a EP-008). Inclui os casos UT-F001-073 a
/// UT-F001-099 e UT-TRV-001 da Especificação de Testes Unitários (I028, secções 7.1.8 e 7.6.1). As regras de formato (casos 078 a 082) estão no <see cref="CreateUtilizadorDto"/>
/// e dão 400 na validação automática do [ApiController]; por isso são verificadas no DTO.
/// </summary>
public class UtilizadoresServiceTests
{
    private const string PasswordInicial = "Temp#2026";
    private const string EmailRui = "rui.lopes@clinica-g08.test";
    private const string Dominio = "@clinica-g08.test";

    // RELOGIO da Especificação de Testes Unitários (I028, secção 5).
    private static readonly DateTimeOffset Relogio = new(2026, 10, 14, 8, 0, 0, TimeSpan.Zero);

    // Lida do SQL Server, a data não traz a indicação de UTC.
    private static readonly DateTime DataCriacaoExistente = new(2026, 10, 1, 9, 0, 0, DateTimeKind.Unspecified);

    // ----- EP-006 — Criar conta de funcionário -----

    [Fact(DisplayName = "UT-F001-073 — Criar um Auxiliar com podeRegistarApoio cria a conta ativa e o Funcionario")]
    public async Task CriarAuxiliarComApoioCriaContaAtivaEFuncionario()
    {
        using var db = CriarContexto();

        var criado = await CriarServico(db).CriarAsync(PedidoRui());

        var utilizador = Assert.Single(await db.Utilizadores.AsNoTracking().ToListAsync());
        var funcionario = Assert.Single(await db.Funcionarios.AsNoTracking().ToListAsync());
        Assert.Equal(EmailRui, utilizador.Email);
        Assert.Equal(Perfis.AuxiliarVeterinario, utilizador.Perfil);
        Assert.Equal(EstadosUtilizador.Ativo, utilizador.Estado);
        Assert.Equal(Relogio.UtcDateTime, utilizador.DataCriacao);
        Assert.Equal(utilizador.Id, funcionario.UtilizadorId);
        Assert.Equal("Rui Lopes", funcionario.Nome);
        Assert.Equal("934567890", funcionario.Telefone);
        Assert.True(funcionario.PodeRegistarApoio);

        // O hash vem do IPasswordHasher: é diferente da palavra-passe e confirma-a.
        Assert.NotEqual(PasswordInicial, utilizador.PasswordHash);
        Assert.Equal(
            PasswordVerificationResult.Success,
            new PasswordHasher<Utilizador>().VerifyHashedPassword(utilizador, utilizador.PasswordHash, PasswordInicial));

        Assert.Equal(utilizador.Id, criado.Id);
        Assert.Equal("Rui Lopes", criado.Nome);
        Assert.Equal(EmailRui, criado.Email);
        Assert.Equal(Perfis.AuxiliarVeterinario, criado.Perfil);
        Assert.Equal(EstadosUtilizador.Ativo, criado.Estado);
        Assert.True(criado.PodeRegistarApoio);
    }

    [Fact(DisplayName = "UT-F001-074 — Criar um Veterinário sem podeRegistarApoio guarda a autorização a false")]
    public async Task CriarVeterinarioSemApoio()
    {
        using var db = CriarContexto();

        var criado = await CriarServico(db).CriarAsync(PedidoRui(Perfis.Veterinario, podeRegistarApoio: false));

        Assert.False(Assert.Single(await db.Funcionarios.AsNoTracking().ToListAsync()).PodeRegistarApoio);
        Assert.Equal(Perfis.Veterinario, criado.Perfil);
        Assert.False(criado.PodeRegistarApoio);
    }

    [Theory(DisplayName = "I065 — Pode ser criada uma conta com cada um dos quatro perfis internos")]
    [InlineData(Perfis.Administrador)]
    [InlineData(Perfis.Rececionista)]
    [InlineData(Perfis.Veterinario)]
    [InlineData(Perfis.AuxiliarVeterinario)]
    public async Task CriaContaComCadaPerfilInterno(string perfil)
    {
        using var db = CriarContexto();

        var criado = await CriarServico(db).CriarAsync(PedidoRui(perfil, podeRegistarApoio: false));

        Assert.Equal(perfil, Assert.Single(await db.Utilizadores.AsNoTracking().ToListAsync()).Perfil);
        Assert.Single(await db.Funcionarios.AsNoTracking().ToListAsync());
        Assert.Equal(perfil, criado.Perfil);
    }

    [Fact(DisplayName = "UT-F001-075 — O perfil Cliente é recusado com 422 e nada é gravado")]
    public async Task PerfilClienteERecusado()
    {
        using var db = CriarContexto();
        var servico = CriarServico(db);

        await Assert.ThrowsAsync<UnprocessableEntityException>(
            () => servico.CriarAsync(PedidoRui(Perfis.Cliente, podeRegistarApoio: false)));

        await NadaFoiGravadoAsync(db);
    }

    [Fact(DisplayName = "UT-F001-076 — Um Rececionista com podeRegistarApoio é recusado com 422 e nada é gravado")]
    public async Task RececionistaComApoioERecusado()
    {
        using var db = CriarContexto();
        var servico = CriarServico(db);

        await Assert.ThrowsAsync<UnprocessableEntityException>(
            () => servico.CriarAsync(PedidoRui(Perfis.Rececionista, podeRegistarApoio: true)));

        await NadaFoiGravadoAsync(db);
    }

    [Theory(DisplayName = "I065 — podeRegistarApoio em qualquer perfil que não seja AuxiliarVeterinario dá 422")]
    [InlineData(Perfis.Administrador)]
    [InlineData(Perfis.Veterinario)]
    public async Task ApoioForaDoAuxiliarERecusado(string perfil)
    {
        using var db = CriarContexto();
        var servico = CriarServico(db);

        await Assert.ThrowsAsync<UnprocessableEntityException>(
            () => servico.CriarAsync(PedidoRui(perfil, podeRegistarApoio: true)));

        await NadaFoiGravadoAsync(db);
    }

    [Fact(DisplayName = "UT-F001-077 — Um e-mail já registado dá 409 e nada é gravado")]
    public async Task EmailJaRegistadoDaConflito()
    {
        using var db = CriarContexto();
        await AdicionarFuncionarioAsync(db, "Rui Lopes", Perfis.Veterinario, email: EmailRui);
        var servico = CriarServico(db);

        var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() => servico.CriarAsync(PedidoRui()));

        Assert.Equal("Já existe uma conta registada com este e-mail.", excecao.Message);
        Assert.Single(await db.Utilizadores.AsNoTracking().ToListAsync());
        Assert.Single(await db.Funcionarios.AsNoTracking().ToListAsync());
    }

    [Fact(DisplayName = "I065 — O e-mail é guardado em minúsculas e um repetido com outras maiúsculas dá 409")]
    public async Task EmailENormalizadoEmMinusculas()
    {
        using var db = CriarContexto();
        var servico = CriarServico(db);
        var pedido = PedidoRui();
        pedido.Email = "Rui.Lopes@Clinica-G08.test";
        var repetido = PedidoRui();
        repetido.Email = "RUI.LOPES@CLINICA-G08.TEST";

        var criado = await servico.CriarAsync(pedido);

        Assert.Equal(EmailRui, criado.Email);
        await Assert.ThrowsAsync<InvalidOperationException>(() => servico.CriarAsync(repetido));
        Assert.Equal(EmailRui, Assert.Single(await db.Utilizadores.AsNoTracking().ToListAsync()).Email);
    }

    [Fact(DisplayName = "UT-F001-078 — Um nome vazio dá 400 com o erro no campo nome")]
    public void NomeVazioERecusado()
    {
        var pedido = PedidoRui();
        pedido.Nome = "";

        Assert.Equal("O nome é obrigatório.", ErroNoCampo(pedido, nameof(CreateUtilizadorDto.Nome)));
    }

    [Fact(DisplayName = "UT-F001-079 — Um e-mail sem formato válido dá 400 com o erro no campo email")]
    public void EmailInvalidoERecusado()
    {
        var pedido = PedidoRui();
        pedido.Email = "rui.lopes";

        Assert.NotNull(ErroNoCampo(pedido, nameof(CreateUtilizadorDto.Email)));
    }

    [Fact(DisplayName = "UT-F001-080 — Uma palavra-passe fraca dá 400 com o erro no campo passwordInicial")]
    public void PalavraPasseFracaERecusada()
    {
        var pedido = PedidoRui();
        pedido.PasswordInicial = "12345678";

        Assert.NotNull(ErroNoCampo(pedido, nameof(CreateUtilizadorDto.PasswordInicial)));
    }

    [Fact(DisplayName = "UT-F001-081 — Um nome com 150 caracteres é aceite (limite de Funcionario.Nome)")]
    public async Task NomeCom150CaracteresEAceite()
    {
        using var db = CriarContexto();
        var pedido = PedidoRui();
        pedido.Nome = new string('N', 150);

        Assert.Empty(ErrosDeValidacao(pedido));
        await CriarServico(db).CriarAsync(pedido);

        Assert.Equal(150, Assert.Single(await db.Funcionarios.AsNoTracking().ToListAsync()).Nome.Length);
    }

    [Fact(DisplayName = "UT-F001-082 — Um nome com 151 caracteres dá 400")]
    public void NomeCom151CaracteresERecusado()
    {
        var pedido = PedidoRui();
        pedido.Nome = new string('N', 151);

        Assert.Equal(
            "O nome não pode ter mais de 150 caracteres.",
            ErroNoCampo(pedido, nameof(CreateUtilizadorDto.Nome)));
    }

    [Theory(DisplayName = "I065 — Um perfil fora da enumeração dá 400 (e não 422)")]
    [InlineData("Gestor")]
    [InlineData("Auxiliar")]
    public async Task PerfilInvalidoNaCriacaoERecusado(string perfil)
    {
        using var db = CriarContexto();
        var servico = CriarServico(db);

        await Assert.ThrowsAsync<ArgumentException>(
            () => servico.CriarAsync(PedidoRui(perfil, podeRegistarApoio: false)));

        await NadaFoiGravadoAsync(db);
    }

    [Fact(DisplayName = "I065 — A conta criada inicia sessão em EP-001 com a palavra-passe inicial (critério 6)")]
    public async Task ContaCriadaIniciaSessao()
    {
        using var db = CriarContexto();
        await CriarServico(db).CriarAsync(PedidoRui(Perfis.Veterinario, podeRegistarApoio: false));
        var authService = new AuthService(
            db,
            new PasswordHasher<Utilizador>(),
            AjudaSeguranca.CriarTokenService(AjudaSeguranca.NovaChave(), new RelogioFixo(Relogio)),
            NullLogger<AuthService>.Instance,
            new RelogioFixo(Relogio));

        var sessao = await authService.LoginAsync(new LoginRequestDto { Email = EmailRui, Password = PasswordInicial });

        Assert.False(string.IsNullOrEmpty(sessao.Token));
        Assert.Equal(Perfis.Veterinario, sessao.Utilizador.Perfil);
        Assert.Equal("Rui Lopes", sessao.Utilizador.Nome);
    }

    [Fact(DisplayName = "I065 — Os DTOs de resposta não têm campos de palavra-passe nem de hash (critério 4)")]
    public void DtosDeRespostaNaoTemCamposDePalavraPasse()
    {
        foreach (var tipo in new[] { typeof(UtilizadorListItemDto), typeof(UtilizadorDto) })
        {
            Assert.DoesNotContain(tipo.GetProperties(), propriedade =>
                propriedade.Name.Contains("Password", StringComparison.OrdinalIgnoreCase)
                || propriedade.Name.Contains("Hash", StringComparison.OrdinalIgnoreCase));
        }
    }

    // ----- EP-004 — Listar contas de funcionários -----

    [Fact(DisplayName = "UT-F001-098 — Filtrar por perfil Veterinario e estado Ativo devolve só os Veterinários ativos")]
    public async Task FiltroPorPerfilEEstadoDevolveSoVeterinariosAtivos()
    {
        using var db = CriarContexto();
        await CriarBaseComTodosOsPerfisAsync(db);

        var resultado = await CriarServico(db).ListarAsync(new UtilizadorFiltroQuery
        {
            Perfil = Perfis.Veterinario,
            Estado = EstadosUtilizador.Ativo
        });

        Assert.Equal(2, resultado.Total);
        Assert.Equal("Vera Vet|Vitor Vet", Nomes(resultado.Itens));
        Assert.All(resultado.Itens, item =>
        {
            Assert.Equal(Perfis.Veterinario, item.Perfil);
            Assert.Equal(EstadosUtilizador.Ativo, item.Estado);
        });
    }

    [Fact(DisplayName = "UT-F001-099 — Um perfil fora da enumeração (\"Gestor\") dá 400")]
    public async Task PerfilInvalidoNoFiltroERecusado()
    {
        using var db = CriarContexto();

        await Assert.ThrowsAsync<ArgumentException>(
            () => CriarServico(db).ListarAsync(new UtilizadorFiltroQuery { Perfil = "Gestor" }));
    }

    [Fact(DisplayName = "I065 — Um estado fora da enumeração dá 400")]
    public async Task EstadoInvalidoNoFiltroERecusado()
    {
        using var db = CriarContexto();

        await Assert.ThrowsAsync<ArgumentException>(
            () => CriarServico(db).ListarAsync(new UtilizadorFiltroQuery { Estado = "Suspenso" }));
    }

    [Fact(DisplayName = "I065 — Sem filtros, lista todas as contas de funcionário por nome e nunca as de Cliente")]
    public async Task SemFiltrosListaTodosOsFuncionariosPorNome()
    {
        using var db = CriarContexto();
        await CriarBaseComTodosOsPerfisAsync(db);

        var resultado = await CriarServico(db).ListarAsync(new UtilizadorFiltroQuery());

        Assert.Equal(8, resultado.Total);
        Assert.Equal(
            "Ana Admin|Carla Aux|Diogo Aux|Rita Rececao|Rui Bloqueado|Vasco Vet|Vera Vet|Vitor Vet",
            Nomes(resultado.Itens));
        Assert.DoesNotContain(resultado.Itens, item => item.Perfil == Perfis.Cliente);
    }

    [Fact(DisplayName = "I065 — Com perfil=Cliente a lista fica vazia (EP-004 só lista funcionários)")]
    public async Task FiltroPerfilClienteDevolveListaVazia()
    {
        using var db = CriarContexto();
        await CriarBaseComTodosOsPerfisAsync(db);

        var resultado = await CriarServico(db).ListarAsync(new UtilizadorFiltroQuery { Perfil = Perfis.Cliente });

        Assert.Empty(resultado.Itens);
        Assert.Equal(0, resultado.Total);
    }

    [Theory(DisplayName = "I065 — A pesquisa procura no nome e no e-mail e nunca devolve Clientes")]
    [InlineData("Vet", "Vasco Vet|Vera Vet|Vitor Vet")]
    [InlineData("carla.aux@", "Carla Aux")]
    [InlineData("costa", "")]
    public async Task PesquisaNoNomeEEmail(string pesquisa, string esperados)
    {
        using var db = CriarContexto();
        await CriarBaseComTodosOsPerfisAsync(db);

        var resultado = await CriarServico(db).ListarAsync(new UtilizadorFiltroQuery { Pesquisa = pesquisa });

        Assert.Equal(esperados, Nomes(resultado.Itens));
    }

    [Fact(DisplayName = "I065 — Contas com o mesmo nome ficam ordenadas pelo identificador (ordem estável)")]
    public async Task MesmoNomeOrdenadoPorIdentificador()
    {
        using var db = CriarContexto();
        var primeira = await AdicionarFuncionarioAsync(db, "Joana Silva", Perfis.Veterinario, email: "joana.1" + Dominio);
        var segunda = await AdicionarFuncionarioAsync(db, "Joana Silva", Perfis.Rececionista, email: "joana.2" + Dominio);
        await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var servico = CriarServico(db);

        var pagina1 = await servico.ListarAsync(new UtilizadorFiltroQuery { Pagina = 1, Tamanho = 2 });
        var pagina2 = await servico.ListarAsync(new UtilizadorFiltroQuery { Pagina = 2, Tamanho = 2 });

        Assert.Equal("Ana Admin", pagina1.Itens[0].Nome);
        Assert.Equal(primeira.Id, pagina1.Itens[1].Id);
        Assert.Equal(segunda.Id, Assert.Single(pagina2.Itens).Id);
    }

    [Fact(DisplayName = "UT-TRV-001 — Sem pagina nem tamanho, a lista usa a página 1 com 20 elementos")]
    public async Task PaginacaoPorOmissao()
    {
        using var db = CriarContexto();
        await CriarBaseComTodosOsPerfisAsync(db);

        var resultado = await CriarServico(db).ListarAsync(new UtilizadorFiltroQuery());

        Assert.Equal(1, resultado.Pagina);
        Assert.Equal(20, resultado.Tamanho);
    }

    [Theory(DisplayName = "I065 — A paginação devolve a página pedida; depois do fim, ou numa página muito alta, a lista fica vazia")]
    [InlineData(2, 3, "Rita Rececao|Rui Bloqueado|Vasco Vet")]
    [InlineData(3, 3, "Vera Vet|Vitor Vet")]
    [InlineData(4, 3, "")]
    [InlineData(int.MaxValue, 100, "")]
    public async Task PaginacaoDevolveAPaginaPedida(int pagina, int tamanho, string esperados)
    {
        using var db = CriarContexto();
        await CriarBaseComTodosOsPerfisAsync(db);

        var resultado = await CriarServico(db).ListarAsync(new UtilizadorFiltroQuery { Pagina = pagina, Tamanho = tamanho });

        Assert.Equal(esperados, Nomes(resultado.Itens));
        Assert.Equal(8, resultado.Total);
    }

    // ----- EP-005 — Consultar conta de funcionário -----

    [Fact(DisplayName = "I065 — A consulta devolve a conta de funcionário com a data de criação em UTC")]
    public async Task ConsultaDevolveTodosOsCampos()
    {
        using var db = CriarContexto();
        var conta = await AdicionarFuncionarioAsync(
            db, "Carla Aux", Perfis.AuxiliarVeterinario, podeRegistarApoio: true, telefone: "910000006");

        var detalhe = await CriarServico(db).ObterAsync(conta.Id);

        Assert.Equal(conta.Id, detalhe.Id);
        Assert.Equal("Carla Aux", detalhe.Nome);
        Assert.Equal("carla.aux" + Dominio, detalhe.Email);
        Assert.Equal("910000006", detalhe.Telefone);
        Assert.Equal(Perfis.AuxiliarVeterinario, detalhe.Perfil);
        Assert.Equal(EstadosUtilizador.Ativo, detalhe.Estado);
        Assert.True(detalhe.PodeRegistarApoio);
        Assert.Equal(DataCriacaoExistente, detalhe.CriadoEm);
        Assert.Equal(DateTimeKind.Utc, detalhe.CriadoEm.Kind);
    }

    [Fact(DisplayName = "I065 — Um identificador inexistente dá 404")]
    public async Task ContaInexistenteDaNaoEncontrado()
    {
        using var db = CriarContexto();

        await Assert.ThrowsAsync<KeyNotFoundException>(() => CriarServico(db).ObterAsync(999));
    }

    [Fact(DisplayName = "I065 — Uma conta de Cliente dá 404 (EP-005 só consulta contas de funcionário)")]
    public async Task ContaDeClienteDaNaoEncontrado()
    {
        using var db = CriarContexto();
        var cliente = await AdicionarClienteAsync(db, "Joao Costa", "joao.costa" + Dominio);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => CriarServico(db).ObterAsync(cliente.Id));
    }

    // ----- EP-007 — Atualizar conta de funcionário (I066) -----

    [Fact(DisplayName = "UT-F001-083 — Atualizar REC_A para Veterinario muda o perfil e os dados e devolve a conta")]
    public async Task AtualizarRececionistaParaVeterinario()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var rececionista = await AdicionarFuncionarioAsync(db, "Rita Rececao", Perfis.Rececionista, telefone: "910000002");
        var pedido = PedidoAtualizacao("Rita Lopes", "rita.lopes" + Dominio, Perfis.Veterinario);
        pedido.Telefone = " 910 000 099 ";

        var atualizado = await CriarServico(db).AtualizarAsync(rececionista.Id, admin.Id, pedido);

        var utilizador = await db.Utilizadores.AsNoTracking().SingleAsync(u => u.Id == rececionista.Id);
        var funcionario = await db.Funcionarios.AsNoTracking().SingleAsync(f => f.UtilizadorId == rececionista.Id);
        Assert.Equal(Perfis.Veterinario, utilizador.Perfil);
        Assert.Equal("rita.lopes" + Dominio, utilizador.Email);
        Assert.Equal(EstadosUtilizador.Ativo, utilizador.Estado);
        Assert.Equal("Rita Lopes", funcionario.Nome);
        Assert.Equal("910 000 099", funcionario.Telefone);
        Assert.False(funcionario.PodeRegistarApoio);

        Assert.Equal(rececionista.Id, atualizado.Id);
        Assert.Equal("Rita Lopes", atualizado.Nome);
        Assert.Equal("rita.lopes" + Dominio, atualizado.Email);
        Assert.Equal("910 000 099", atualizado.Telefone);
        Assert.Equal(Perfis.Veterinario, atualizado.Perfil);
        Assert.Equal(EstadosUtilizador.Ativo, atualizado.Estado);
        Assert.Equal(DataCriacaoExistente, atualizado.CriadoEm);
        Assert.Equal(DateTimeKind.Utc, atualizado.CriadoEm.Kind);
    }

    [Fact(DisplayName = "UT-F001-084 — O perfil Cliente dá 400 (e não 422) e nada muda")]
    public async Task AtualizarParaPerfilClienteERecusado()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var rececionista = await AdicionarFuncionarioAsync(db, "Rita Rececao", Perfis.Rececionista);

        await Assert.ThrowsAsync<ArgumentException>(() => CriarServico(db).AtualizarAsync(
            rececionista.Id, admin.Id, PedidoAtualizacao("Rita Rececao", rececionista.Email, Perfis.Cliente)));

        await ContaFicouInalteradaAsync(db, rececionista, "Rita Rececao");
    }

    [Fact(DisplayName = "UT-F001-085 — O Administrador não pode remover o próprio perfil de Administrador (422)")]
    public async Task AdministradorNaoRemoveOProprioPerfil()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);

        await Assert.ThrowsAsync<UnprocessableEntityException>(() => CriarServico(db).AtualizarAsync(
            admin.Id, admin.Id, PedidoAtualizacao("Ana Admin", admin.Email, Perfis.Rececionista)));

        await ContaFicouInalteradaAsync(db, admin, "Ana Admin");
    }

    [Fact(DisplayName = "UT-F001-086 — O Administrador altera os próprios dados mantendo o perfil")]
    public async Task AdministradorAlteraOsPropriosDados()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);

        var atualizado = await CriarServico(db).AtualizarAsync(
            admin.Id, admin.Id, PedidoAtualizacao("Ana Maria Admin", admin.Email, Perfis.Administrador));

        Assert.Equal("Ana Maria Admin", atualizado.Nome);
        Assert.Equal(Perfis.Administrador, atualizado.Perfil);
        Assert.Equal(Perfis.Administrador, (await db.Utilizadores.AsNoTracking().SingleAsync()).Perfil);
    }

    [Fact(DisplayName = "I066 — O Administrador pode mudar o perfil de outro Administrador")]
    public async Task AdministradorMudaPerfilDeOutroAdministrador()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var outro = await AdicionarFuncionarioAsync(db, "Bruno Admin", Perfis.Administrador);

        var atualizado = await CriarServico(db).AtualizarAsync(
            outro.Id, admin.Id, PedidoAtualizacao("Bruno Admin", outro.Email, Perfis.Rececionista));

        Assert.Equal(Perfis.Rececionista, atualizado.Perfil);
    }

    [Fact(DisplayName = "UT-F001-087 — podeRegistarApoio num Veterinário dá 422 e nada muda")]
    public async Task ApoioNumVeterinarioERecusado()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var vet = await AdicionarFuncionarioAsync(db, "Vera Vet", Perfis.Veterinario);

        await Assert.ThrowsAsync<UnprocessableEntityException>(() => CriarServico(db).AtualizarAsync(
            vet.Id, admin.Id, PedidoAtualizacao("Vera Vet", vet.Email, Perfis.Veterinario, podeRegistarApoio: true)));

        await ContaFicouInalteradaAsync(db, vet, "Vera Vet");
    }

    [Fact(DisplayName = "I066 — Um Auxiliar com apoio que muda de perfil sem retirar a autorização dá 422 (critério 3)")]
    public async Task MudancaDePerfilComApoioERecusada()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var aux = await AdicionarFuncionarioAsync(db, "Carla Aux", Perfis.AuxiliarVeterinario, podeRegistarApoio: true);

        await Assert.ThrowsAsync<UnprocessableEntityException>(() => CriarServico(db).AtualizarAsync(
            aux.Id, admin.Id, PedidoAtualizacao("Carla Aux", aux.Email, Perfis.Veterinario, podeRegistarApoio: true)));

        await ContaFicouInalteradaAsync(db, aux, "Carla Aux");
        Assert.True((await db.Funcionarios.AsNoTracking().SingleAsync(f => f.UtilizadorId == aux.Id)).PodeRegistarApoio);
    }

    [Theory(DisplayName = "I066 — A autorização de apoio de um Auxiliar pode ser concedida ou retirada")]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public async Task AuxiliarAlteraAutorizacaoDeApoio(bool antes, bool depois)
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var aux = await AdicionarFuncionarioAsync(db, "Carla Aux", Perfis.AuxiliarVeterinario, podeRegistarApoio: antes);

        var atualizado = await CriarServico(db).AtualizarAsync(
            aux.Id, admin.Id, PedidoAtualizacao("Carla Aux", aux.Email, Perfis.AuxiliarVeterinario, podeRegistarApoio: depois));

        Assert.Equal(depois, atualizado.PodeRegistarApoio);
        Assert.Equal(depois, (await db.Funcionarios.AsNoTracking().SingleAsync(f => f.UtilizadorId == aux.Id)).PodeRegistarApoio);
    }

    [Fact(DisplayName = "UT-F001-088 — O e-mail de outra conta dá 409 e nada muda")]
    public async Task EmailDeOutraContaDaConflito()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var rececionista = await AdicionarFuncionarioAsync(db, "Rita Rececao", Perfis.Rececionista);
        var vet = await AdicionarFuncionarioAsync(db, "Vera Vet", Perfis.Veterinario);

        var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() => CriarServico(db).AtualizarAsync(
            rececionista.Id, admin.Id, PedidoAtualizacao("Rita Lopes", vet.Email.ToUpperInvariant(), Perfis.Rececionista)));

        Assert.Equal("O e-mail indicado já é usado por outra conta.", excecao.Message);
        await ContaFicouInalteradaAsync(db, rececionista, "Rita Rececao");
    }

    [Fact(DisplayName = "I066 — O e-mail de uma conta de Cliente também dá 409")]
    public async Task EmailDeClienteDaConflito()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var rececionista = await AdicionarFuncionarioAsync(db, "Rita Rececao", Perfis.Rececionista);
        var cliente = await AdicionarClienteAsync(db, "Joao Costa", "joao.costa" + Dominio);

        await Assert.ThrowsAsync<InvalidOperationException>(() => CriarServico(db).AtualizarAsync(
            rececionista.Id, admin.Id, PedidoAtualizacao("Rita Rececao", cliente.Email, Perfis.Rececionista)));

        await ContaFicouInalteradaAsync(db, rececionista, "Rita Rececao");
    }

    [Fact(DisplayName = "UT-F001-089 — Manter o próprio e-mail (com outras maiúsculas) não é conflito")]
    public async Task ProprioEmailNaoEConflito()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var rececionista = await AdicionarFuncionarioAsync(db, "Rita Rececao", Perfis.Rececionista);

        var atualizado = await CriarServico(db).AtualizarAsync(
            rececionista.Id, admin.Id, PedidoAtualizacao("Rita Lopes", rececionista.Email.ToUpperInvariant(), Perfis.Rececionista));

        Assert.Equal(rececionista.Email, atualizado.Email);
        Assert.Equal("Rita Lopes", atualizado.Nome);
    }

    [Fact(DisplayName = "UT-F001-090 — Um identificador inexistente dá 404")]
    public async Task AtualizarContaInexistenteDaNaoEncontrado()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => CriarServico(db).AtualizarAsync(
            999, admin.Id, PedidoAtualizacao("Rita Lopes", "rita.lopes" + Dominio, Perfis.Rececionista)));
    }

    [Fact(DisplayName = "I066 — Uma conta de Cliente dá 404 em EP-007 (só contas de funcionário)")]
    public async Task AtualizarContaDeClienteDaNaoEncontrado()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var cliente = await AdicionarClienteAsync(db, "Joao Costa", "joao.costa" + Dominio);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => CriarServico(db).AtualizarAsync(
            cliente.Id, admin.Id, PedidoAtualizacao("Joao Costa", cliente.Email, Perfis.Rececionista)));

        Assert.Equal(Perfis.Cliente, (await db.Utilizadores.AsNoTracking().SingleAsync(u => u.Id == cliente.Id)).Perfil);
    }

    [Fact(DisplayName = "UT-F001-091 — Um e-mail sem formato válido dá 400 com o erro no campo email")]
    public void EmailInvalidoNaAtualizacaoERecusado()
    {
        var pedido = PedidoAtualizacao("Rita Lopes", "rec", Perfis.Rececionista);

        var erro = Assert.Single(ErrosDeValidacao(pedido));
        Assert.Contains(nameof(UpdateUtilizadorDto.Email), erro.MemberNames);
    }

    [Theory(DisplayName = "I066 — Um perfil fora da enumeração dá 400 na atualização")]
    [InlineData("Gestor")]
    [InlineData("Auxiliar")]
    public async Task PerfilInvalidoNaAtualizacaoERecusado(string perfil)
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var rececionista = await AdicionarFuncionarioAsync(db, "Rita Rececao", Perfis.Rececionista);

        await Assert.ThrowsAsync<ArgumentException>(() => CriarServico(db).AtualizarAsync(
            rececionista.Id, admin.Id, PedidoAtualizacao("Rita Rececao", rececionista.Email, perfil)));

        await ContaFicouInalteradaAsync(db, rececionista, "Rita Rececao");
    }

    [Fact(DisplayName = "I066 — Depois da mudança de perfil, o token antigo deixa de ser aceite (EP-003)")]
    public async Task MudancaDePerfilInvalidaOTokenAntigo()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var rececionista = await AdicionarFuncionarioAsync(db, "Rita Rececao", Perfis.Rececionista);
        var authService = CriarAuthService(db);
        Assert.True(await authService.SessaoValidaAsync(rececionista.Id, Perfis.Rececionista, "jti-antigo"));

        await CriarServico(db).AtualizarAsync(
            rececionista.Id, admin.Id, PedidoAtualizacao("Rita Rececao", rececionista.Email, Perfis.Veterinario));

        Assert.False(await authService.SessaoValidaAsync(rececionista.Id, Perfis.Rececionista, "jti-antigo"));
    }

    // ----- EP-008 — Bloquear conta de funcionário (I066) -----

    [Fact(DisplayName = "UT-F001-092 — Bloquear REC_A ativo passa o estado a Bloqueado")]
    public async Task BloquearContaAtiva()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var rececionista = await AdicionarFuncionarioAsync(db, "Rita Rececao", Perfis.Rececionista);

        await CriarServico(db).BloquearAsync(rececionista.Id, admin.Id, PedidoBloqueio());

        Assert.Equal(
            EstadosUtilizador.Bloqueado,
            (await db.Utilizadores.AsNoTracking().SingleAsync(u => u.Id == rececionista.Id)).Estado);
        Assert.Equal(
            EstadosUtilizador.Ativo,
            (await db.Utilizadores.AsNoTracking().SingleAsync(u => u.Id == admin.Id)).Estado);
    }

    [Theory(DisplayName = "I066 — O estado Bloqueado é aceite sem distinguir maiúsculas e sem motivo")]
    [InlineData("bloqueado")]
    [InlineData(" BLOQUEADO ")]
    public async Task EstadoBloqueadoSemDistinguirMaiusculas(string estado)
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var rececionista = await AdicionarFuncionarioAsync(db, "Rita Rececao", Perfis.Rececionista);

        await CriarServico(db).BloquearAsync(rececionista.Id, admin.Id, new AlterarEstadoUtilizadorDto { Estado = estado });

        Assert.Equal(
            EstadosUtilizador.Bloqueado,
            (await db.Utilizadores.AsNoTracking().SingleAsync(u => u.Id == rececionista.Id)).Estado);
    }

    [Theory(DisplayName = "UT-F001-093 — Um estado diferente de Bloqueado dá 400 e a conta mantém-se ativa")]
    [InlineData(EstadosUtilizador.Ativo)]
    [InlineData("Suspenso")]
    public async Task EstadoDiferenteDeBloqueadoERecusado(string estado)
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var rececionista = await AdicionarFuncionarioAsync(db, "Rita Rececao", Perfis.Rececionista);

        await Assert.ThrowsAsync<ArgumentException>(() => CriarServico(db).BloquearAsync(
            rececionista.Id, admin.Id, new AlterarEstadoUtilizadorDto { Estado = estado }));

        Assert.Equal(
            EstadosUtilizador.Ativo,
            (await db.Utilizadores.AsNoTracking().SingleAsync(u => u.Id == rececionista.Id)).Estado);
    }

    [Fact(DisplayName = "I066 — Um pedido sem estado dá 400 na validação do DTO")]
    public void PedidoSemEstadoERecusado()
    {
        var pedido = new AlterarEstadoUtilizadorDto();

        var resultados = new List<ValidationResult>();
        Assert.False(Validator.TryValidateObject(pedido, new ValidationContext(pedido), resultados, validateAllProperties: true));
        Assert.Contains(nameof(AlterarEstadoUtilizadorDto.Estado), Assert.Single(resultados).MemberNames);
    }

    [Fact(DisplayName = "UT-F001-094 — Bloquear uma conta já bloqueada dá 422")]
    public async Task BloquearContaJaBloqueadaERecusado()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var bloqueado = await AdicionarFuncionarioAsync(db, "Rui Bloqueado", Perfis.Rececionista, EstadosUtilizador.Bloqueado);

        var excecao = await Assert.ThrowsAsync<UnprocessableEntityException>(
            () => CriarServico(db).BloquearAsync(bloqueado.Id, admin.Id, PedidoBloqueio()));

        Assert.Equal("A conta já está bloqueada.", excecao.Message);
    }

    [Fact(DisplayName = "UT-F001-095 — O Administrador não pode bloquear a própria conta (422) e mantém-se ativo")]
    public async Task AdministradorNaoSeBloqueia()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);

        await Assert.ThrowsAsync<UnprocessableEntityException>(
            () => CriarServico(db).BloquearAsync(admin.Id, admin.Id, PedidoBloqueio()));

        Assert.Equal(EstadosUtilizador.Ativo, (await db.Utilizadores.AsNoTracking().SingleAsync()).Estado);
    }

    [Fact(DisplayName = "UT-F001-096 — Bloquear uma conta de Cliente dá 422 e a conta mantém-se ativa")]
    public async Task BloquearClienteERecusado()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        var cliente = await AdicionarClienteAsync(db, "Joao Costa", "joao.costa" + Dominio);

        await Assert.ThrowsAsync<UnprocessableEntityException>(
            () => CriarServico(db).BloquearAsync(cliente.Id, admin.Id, PedidoBloqueio()));

        Assert.Equal(
            EstadosUtilizador.Ativo,
            (await db.Utilizadores.AsNoTracking().SingleAsync(u => u.Id == cliente.Id)).Estado);
    }

    [Fact(DisplayName = "UT-F001-097 — Bloquear um identificador inexistente dá 404")]
    public async Task BloquearContaInexistenteDaNaoEncontrado()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => CriarServico(db).BloquearAsync(999, admin.Id, PedidoBloqueio()));
    }

    [Fact(DisplayName = "I066 — Depois do bloqueio, o login dá 403 e o token já emitido deixa de ser aceite (critério 4)")]
    public async Task ContaBloqueadaNaoIniciaSessaoEPerdeOToken()
    {
        using var db = CriarContexto();
        var admin = await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        await CriarServico(db).CriarAsync(PedidoRui(Perfis.Rececionista, podeRegistarApoio: false));
        var rui = await db.Utilizadores.AsNoTracking().SingleAsync(u => u.Email == EmailRui);
        var authService = CriarAuthService(db);
        Assert.True(await authService.SessaoValidaAsync(rui.Id, Perfis.Rececionista, "jti-emitido"));

        await CriarServico(db).BloquearAsync(rui.Id, admin.Id, PedidoBloqueio());

        await Assert.ThrowsAsync<ForbiddenException>(() =>
            authService.LoginAsync(new LoginRequestDto { Email = EmailRui, Password = PasswordInicial }));
        Assert.False(await authService.SessaoValidaAsync(rui.Id, Perfis.Rececionista, "jti-emitido"));
    }

    // ----- Preparação dos dados -----

    private static ClinicaDbContext CriarContexto() =>
        new(new DbContextOptionsBuilder<ClinicaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture))
            // O InMemory não tem transações (I028, secção 4.2): a transação do serviço é ignorada.
            .ConfigureWarnings(avisos => avisos.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options);

    private static UtilizadoresService CriarServico(ClinicaDbContext db) =>
        new(db, new PasswordHasher<Utilizador>(), new RelogioFixo(Relogio));

    /// <summary>Pedido do exemplo de EP-006 da Documentação da API, com um e-mail do domínio de teste.</summary>
    private static CreateUtilizadorDto PedidoRui(string perfil = Perfis.AuxiliarVeterinario, bool podeRegistarApoio = true) => new()
    {
        Nome = "Rui Lopes",
        Email = EmailRui,
        Telefone = "934567890",
        Perfil = perfil,
        PodeRegistarApoio = podeRegistarApoio,
        PasswordInicial = PasswordInicial
    };

    /// <summary>Pedido de EP-007 com o telefone por omissão.</summary>
    private static UpdateUtilizadorDto PedidoAtualizacao(
        string nome, string email, string perfil, bool podeRegistarApoio = false) => new()
    {
        Nome = nome,
        Email = email,
        Telefone = "934567890",
        Perfil = perfil,
        PodeRegistarApoio = podeRegistarApoio
    };

    /// <summary>Pedido do exemplo de EP-008 da Documentação da API.</summary>
    private static AlterarEstadoUtilizadorDto PedidoBloqueio() => new()
    {
        Estado = EstadosUtilizador.Bloqueado,
        Motivo = "Funcionário deixou a clínica."
    };

    private static AuthService CriarAuthService(ClinicaDbContext db) => new(
        db,
        new PasswordHasher<Utilizador>(),
        AjudaSeguranca.CriarTokenService(AjudaSeguranca.NovaChave(), new RelogioFixo(Relogio)),
        NullLogger<AuthService>.Instance,
        new RelogioFixo(Relogio));

    /// <summary>A conta mantém o e-mail, o perfil, o estado e o nome que tinha antes do pedido recusado.</summary>
    private static async Task ContaFicouInalteradaAsync(ClinicaDbContext db, Utilizador antes, string nome)
    {
        var utilizador = await db.Utilizadores.AsNoTracking().SingleAsync(u => u.Id == antes.Id);
        Assert.Equal(antes.Email, utilizador.Email);
        Assert.Equal(antes.Perfil, utilizador.Perfil);
        Assert.Equal(antes.Estado, utilizador.Estado);
        Assert.Equal(nome, (await db.Funcionarios.AsNoTracking().SingleAsync(f => f.UtilizadorId == antes.Id)).Nome);
    }

    /// <summary>Oito contas de funcionário (quatro perfis, duas bloqueadas) e duas de Cliente.</summary>
    private static async Task CriarBaseComTodosOsPerfisAsync(ClinicaDbContext db)
    {
        await AdicionarFuncionarioAsync(db, "Ana Admin", Perfis.Administrador);
        await AdicionarFuncionarioAsync(db, "Rita Rececao", Perfis.Rececionista);
        await AdicionarFuncionarioAsync(db, "Rui Bloqueado", Perfis.Rececionista, EstadosUtilizador.Bloqueado);
        await AdicionarFuncionarioAsync(db, "Vera Vet", Perfis.Veterinario);
        await AdicionarFuncionarioAsync(db, "Vitor Vet", Perfis.Veterinario);
        await AdicionarFuncionarioAsync(db, "Vasco Vet", Perfis.Veterinario, EstadosUtilizador.Bloqueado);
        await AdicionarFuncionarioAsync(db, "Carla Aux", Perfis.AuxiliarVeterinario, podeRegistarApoio: true);
        await AdicionarFuncionarioAsync(db, "Diogo Aux", Perfis.AuxiliarVeterinario);
        await AdicionarClienteAsync(db, "Joao Costa", "joao.costa" + Dominio);
        await AdicionarClienteAsync(db, "Maria Silva", "maria.silva" + Dominio);
    }

    private static async Task<Utilizador> AdicionarFuncionarioAsync(
        ClinicaDbContext db,
        string nome,
        string perfil,
        string estado = EstadosUtilizador.Ativo,
        bool podeRegistarApoio = false,
        string? telefone = null,
        string? email = null)
    {
        var utilizador = new Utilizador
        {
            // "Carla Aux" → carla.aux@clinica-g08.test
            Email = email ?? nome.Replace(' ', '.').ToLowerInvariant() + Dominio,
            PasswordHash = "hash-nao-usado-nestes-testes",
            Perfil = perfil,
            Estado = estado,
            DataCriacao = DataCriacaoExistente
        };
        db.Utilizadores.Add(utilizador);
        await db.SaveChangesAsync();

        db.Funcionarios.Add(new Funcionario
        {
            UtilizadorId = utilizador.Id,
            Nome = nome,
            Telefone = telefone,
            PodeRegistarApoio = podeRegistarApoio
        });
        await db.SaveChangesAsync();

        return utilizador;
    }

    private static async Task<Utilizador> AdicionarClienteAsync(ClinicaDbContext db, string nome, string email)
    {
        var utilizador = new Utilizador
        {
            Email = email,
            PasswordHash = "hash-nao-usado-nestes-testes",
            Perfil = Perfis.Cliente,
            Estado = EstadosUtilizador.Ativo,
            DataCriacao = DataCriacaoExistente
        };
        db.Utilizadores.Add(utilizador);
        await db.SaveChangesAsync();

        db.Clientes.Add(new Cliente
        {
            UtilizadorId = utilizador.Id,
            Nome = nome,
            Telefone = "960000001",
            DataRegisto = DataCriacaoExistente
        });
        await db.SaveChangesAsync();

        return utilizador;
    }

    private static async Task NadaFoiGravadoAsync(ClinicaDbContext db)
    {
        Assert.Empty(await db.Utilizadores.AsNoTracking().ToListAsync());
        Assert.Empty(await db.Funcionarios.AsNoTracking().ToListAsync());
    }

    private static List<ValidationResult> ErrosDeValidacao(UpdateUtilizadorDto pedido)
    {
        var resultados = new List<ValidationResult>();
        _ = Validator.TryValidateObject(pedido, new ValidationContext(pedido), resultados, validateAllProperties: true);
        return resultados;
    }

    /// <summary>A mensagem do único erro de validação do pedido, que tem de ser no campo indicado.</summary>
    private static string? ErroNoCampo(CreateUtilizadorDto pedido, string campo)
    {
        var erro = Assert.Single(ErrosDeValidacao(pedido));
        Assert.Contains(campo, erro.MemberNames);
        return erro.ErrorMessage;
    }

    private static string Nomes(IEnumerable<UtilizadorListItemDto> itens) =>
        string.Join('|', itens.Select(item => item.Nome));
}
