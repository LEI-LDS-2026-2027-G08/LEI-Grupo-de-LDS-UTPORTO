using ClinicaVeterinaria.Application.DTOs.Auth;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.UnitTests.Seguranca;

/// <summary>
/// I086 — início de sessão (EP-001), validação da sessão e utilizador autenticado (EP-003) no
/// <see cref="AuthService"/>: casos UT-F001-001 a UT-F001-015 e UT-F001-017 a UT-F001-020 da
/// Especificação de Testes Unitários (I028, secção 7.1.1). Os casos UT-F001-016, 021 e 022
/// (fim de sessão, I064) estão em <see cref="AuthServiceFimSessaoTests"/>.
/// </summary>
public class AuthServiceTests
{
    // RELOGIO da Especificação de Testes Unitários (I028, secção 5).
    private static readonly DateTimeOffset Relogio = new(2026, 10, 14, 8, 0, 0, TimeSpan.Zero);

    private const string PalavraPasse = "Teste#2026";

    // Contas da secção 5 da I028 (ADMIN_A, REC_A, VET_A, CLIENTE_A e UTILIZADOR_BLOQUEADO).
    private const int AdminA = 1;
    private const int RecA = 2;
    private const int VetA = 3;
    private const int ContaClienteA = 10;
    private const int ClienteA = 1;
    private const int Bloqueado = 20;

    private const string EmailAdmin = "admin@clinica-g08.test";
    private const string EmailClienteA = "cliente.a@clinica-g08.test";
    private const string EmailBloqueado = "bloqueado@clinica-g08.test";
    private const string EmailInexistente = "nao.existe@clinica-g08.test";

    private const string MensagemCredenciaisInvalidas = "Credenciais inválidas.";

    // ----- EP-001 — Iniciar sessão -----

    [Fact(DisplayName = "UT-F001-001 — Login do Administrador devolve o token do IJwtTokenService e regista o último acesso")]
    public async Task LoginDoAdministradorDevolveTokenERegistaUltimoAcesso()
    {
        using var db = CriarBase();
        var tokens = new TokenServiceDuplo();

        var resposta = await CriarServico(db, tokens: tokens).LoginAsync(Pedido(EmailAdmin, PalavraPasse));

        Assert.Equal(TokenServiceDuplo.Token, resposta.Token);
        Assert.Equal(TokenServiceDuplo.ExpiraEm, resposta.ExpiraEm);
        Assert.Equal(Perfis.Administrador, resposta.Utilizador.Perfil);
        Assert.Null(resposta.Utilizador.ClienteId);
        Assert.Equal(Relogio.UtcDateTime, (await LerUtilizadorAsync(db, AdminA)).DataUltimoAcesso);
    }

    [Fact(DisplayName = "UT-F001-002 — Login de um Cliente devolve o clienteId e passa-o ao IJwtTokenService")]
    public async Task LoginDeClienteDevolveClienteId()
    {
        using var db = CriarBase();
        var tokens = new TokenServiceDuplo();

        var resposta = await CriarServico(db, tokens: tokens).LoginAsync(Pedido(EmailClienteA, PalavraPasse));

        Assert.Equal(Perfis.Cliente, resposta.Utilizador.Perfil);
        Assert.Equal(ClienteA, resposta.Utilizador.ClienteId);
        Assert.Equal(ClienteA, Assert.Single(tokens.Pedidos).ClienteId);
    }

    [Fact(DisplayName = "UT-F001-003 — E-mail vazio devolve 400 «O e-mail é obrigatório.» sem consultar o hasher nem a base de dados")]
    public async Task EmailVazioDevolve400SemConsultas()
    {
        // Um contexto já libertado falha em qualquer consulta: o teste só passa se a validação vier antes.
        var db = CriarBase();
        await db.DisposeAsync();
        var hasher = new HasherDuplo();

        var erro = await Assert.ThrowsAsync<ValidacaoException>(
            () => CriarServico(db, hasher).LoginAsync(Pedido(string.Empty, PalavraPasse)));

        Assert.Equal("O e-mail é obrigatório.", erro.Message);
        Assert.Equal(0, hasher.Verificacoes);
    }

    [Fact(DisplayName = "UT-F001-004 — Palavra-passe vazia devolve 400 «A palavra-passe é obrigatória.»")]
    public async Task PalavraPasseVaziaDevolve400()
    {
        using var db = CriarBase();

        var erro = await Assert.ThrowsAsync<ValidacaoException>(
            () => CriarServico(db).LoginAsync(Pedido(EmailAdmin, string.Empty)));

        Assert.Equal("A palavra-passe é obrigatória.", erro.Message);
    }

    [Fact(DisplayName = "UT-F001-005 — Um e-mail só com espaços conta como em falta (400)")]
    public async Task EmailSoComEspacosContaComoEmFalta()
    {
        using var db = CriarBase();

        var erro = await Assert.ThrowsAsync<ValidacaoException>(
            () => CriarServico(db).LoginAsync(Pedido("   ", PalavraPasse)));

        Assert.Equal("O e-mail é obrigatório.", erro.Message);
    }

    [Fact(DisplayName = "UT-F001-006 — O e-mail é comparado sem os espaços iniciais e finais")]
    public async Task EmailComEspacosAVoltaAutentica()
    {
        using var db = CriarBase();

        var resposta = await CriarServico(db).LoginAsync(Pedido(" " + EmailAdmin + " ", PalavraPasse));

        Assert.Equal(AdminA, resposta.Utilizador.Id);
    }

    [Fact(DisplayName = "UT-F001-007 — E-mail inexistente devolve 401 «Credenciais inválidas.», verifica um hash de referência e regista o evento 3001")]
    public async Task EmailInexistenteDevolve401ComHashDeReferencia()
    {
        using var db = CriarBase();
        var hasher = new HasherDuplo();
        var logger = new LoggerDeTeste<AuthService>();

        var erro = await Assert.ThrowsAsync<NaoAutenticadoException>(
            () => CriarServico(db, hasher, logger: logger).LoginAsync(Pedido(EmailInexistente, "Qualquer#1")));

        Assert.Equal(MensagemCredenciaisInvalidas, erro.Message);
        Assert.Equal(1, hasher.Verificacoes);
        Assert.Contains(logger.Registos, r => r.Evento.Id == 3001 && r.Mensagem.Contains("não registado", StringComparison.Ordinal));
    }

    [Fact(DisplayName = "UT-F001-008 — Palavra-passe errada devolve 401 com a mesma mensagem, sem token e sem alterar o último acesso")]
    public async Task PalavraPasseErradaDevolve401SemToken()
    {
        using var db = CriarBase();
        var tokens = new TokenServiceDuplo();
        var servico = CriarServico(db, new HasherDuplo(PasswordVerificationResult.Failed), tokens);

        var erro = await Assert.ThrowsAsync<NaoAutenticadoException>(
            () => servico.LoginAsync(Pedido(EmailAdmin, "Errada#0000")));

        Assert.Equal(MensagemCredenciaisInvalidas, erro.Message);
        Assert.Empty(tokens.Pedidos);
        Assert.Null((await LerUtilizadorAsync(db, AdminA)).DataUltimoAcesso);
    }

    [Fact(DisplayName = "UT-F001-009 — Conta bloqueada com a palavra-passe certa devolve 403 sem emitir token")]
    public async Task ContaBloqueadaComPalavraPasseCertaDevolve403()
    {
        using var db = CriarBase();
        var tokens = new TokenServiceDuplo();

        await Assert.ThrowsAsync<ForbiddenException>(
            () => CriarServico(db, tokens: tokens).LoginAsync(Pedido(EmailBloqueado, PalavraPasse)));

        Assert.Empty(tokens.Pedidos);
    }

    [Fact(DisplayName = "UT-F001-010 — Conta bloqueada com a palavra-passe errada devolve 401 e não 403")]
    public async Task ContaBloqueadaComPalavraPasseErradaDevolve401()
    {
        using var db = CriarBase();
        var servico = CriarServico(db, new HasherDuplo(PasswordVerificationResult.Failed));

        var erro = await Assert.ThrowsAsync<NaoAutenticadoException>(
            () => servico.LoginAsync(Pedido(EmailBloqueado, "Errada#0000")));

        Assert.Equal(MensagemCredenciaisInvalidas, erro.Message);
    }

    [Fact(DisplayName = "UT-F001-011 — Com SuccessRehashNeeded, o login aceita e grava o novo hash")]
    public async Task RehashNecessarioGravaNovoHash()
    {
        using var db = CriarBase();
        var servico = CriarServico(db, new HasherDuplo(PasswordVerificationResult.SuccessRehashNeeded));

        await servico.LoginAsync(Pedido(EmailAdmin, PalavraPasse));

        Assert.Equal(HasherDuplo.NovoHash, (await LerUtilizadorAsync(db, AdminA)).PasswordHash);
    }

    [Fact(DisplayName = "UT-F001-012 — O registo da tentativa falhada tem o e-mail e o motivo, mas nunca a palavra-passe")]
    public async Task RegistoDaTentativaNaoTemAPalavraPasse()
    {
        using var db = CriarBase();
        var logger = new LoggerDeTeste<AuthService>();

        await Assert.ThrowsAsync<NaoAutenticadoException>(
            () => CriarServico(db, logger: logger).LoginAsync(Pedido(EmailInexistente, "Segredo#123")));

        var mensagem = Assert.Single(logger.Registos).Mensagem;
        Assert.Contains(EmailInexistente, mensagem, StringComparison.Ordinal);
        Assert.Contains("e-mail não registado", mensagem, StringComparison.Ordinal);
        Assert.DoesNotContain("Segredo#123", mensagem, StringComparison.Ordinal);
    }

    // ----- Validação da sessão em cada pedido autenticado -----

    [Fact(DisplayName = "UT-F001-013 — Conta ativa, com o perfil do token e jti não revogado: a sessão é válida")]
    public async Task SessaoDeContaAtivaEValida()
    {
        using var db = CriarBase();

        var valida = await CriarServico(db).SessaoValidaAsync(RecA, Perfis.Rececionista, "j1");

        Assert.True(valida);
    }

    [Fact(DisplayName = "UT-F001-014 — Conta bloqueada depois da emissão do token: a sessão deixa de ser válida")]
    public async Task ContaBloqueadaDepoisDoTokenInvalidaASessao()
    {
        using var db = CriarBase();
        (await db.Utilizadores.SingleAsync(u => u.Id == RecA)).Estado = EstadosUtilizador.Bloqueado;
        await db.SaveChangesAsync();

        var valida = await CriarServico(db).SessaoValidaAsync(RecA, Perfis.Rececionista, "j1");

        Assert.False(valida);
    }

    [Fact(DisplayName = "UT-F001-015 — Perfil alterado depois da emissão do token: a sessão deixa de ser válida")]
    public async Task PerfilAlteradoInvalidaASessao()
    {
        using var db = CriarBase();
        (await db.Utilizadores.SingleAsync(u => u.Id == RecA)).Perfil = Perfis.Veterinario;
        await db.SaveChangesAsync();

        var valida = await CriarServico(db).SessaoValidaAsync(RecA, Perfis.Rececionista, "j1");

        Assert.False(valida);
    }

    [Fact(DisplayName = "UT-F001-017 — Utilizador inexistente: a sessão não é válida")]
    public async Task UtilizadorInexistenteNaoTemSessao()
    {
        using var db = CriarBase();

        var valida = await CriarServico(db).SessaoValidaAsync(999, Perfis.Rececionista, "j1");

        Assert.False(valida);
    }

    // ----- EP-003 — Consultar utilizador autenticado -----

    [Fact(DisplayName = "UT-F001-018 — Veterinário autenticado: nome do Funcionario, perfil Veterinario e sem clienteId")]
    public async Task VeterinarioAutenticadoTemNomeDoFuncionario()
    {
        using var db = CriarBase();

        var autenticado = await CriarServico(db).ObterAutenticadoAsync(VetA);

        Assert.Equal("Dra. Ana Veterinária", autenticado.Nome);
        Assert.Equal(Perfis.Veterinario, autenticado.Perfil);
        Assert.Null(autenticado.ClienteId);
    }

    [Fact(DisplayName = "UT-F001-019 — Cliente autenticado: clienteId e nome do registo Cliente")]
    public async Task ClienteAutenticadoTemClienteIdENome()
    {
        using var db = CriarBase();

        var autenticado = await CriarServico(db).ObterAutenticadoAsync(ContaClienteA);

        Assert.Equal(ClienteA, autenticado.ClienteId);
        Assert.Equal("João Costa", autenticado.Nome);
    }

    [Fact(DisplayName = "UT-F001-020 — Conta bloqueada em /auth/me devolve 401")]
    public async Task ContaBloqueadaNoAuthMeDevolve401()
    {
        using var db = CriarBase();

        await Assert.ThrowsAsync<NaoAutenticadoException>(
            () => CriarServico(db).ObterAutenticadoAsync(Bloqueado));
    }

    // ----- Apoio -----

    private static LoginRequestDto Pedido(string email, string password) => new() { Email = email, Password = password };

    private static AuthService CriarServico(
        ClinicaDbContext db,
        HasherDuplo? hasher = null,
        TokenServiceDuplo? tokens = null,
        LoggerDeTeste<AuthService>? logger = null) =>
        new(
            db,
            hasher ?? new HasherDuplo(),
            tokens ?? new TokenServiceDuplo(),
            logger ?? new LoggerDeTeste<AuthService>(),
            new RelogioFixo(Relogio));

    private static Task<Utilizador> LerUtilizadorAsync(ClinicaDbContext db, int id) =>
        db.Utilizadores.AsNoTracking().SingleAsync(u => u.Id == id);

    private static ClinicaDbContext CriarBase()
    {
        var db = AjudaBaseDados.CriarContexto();

        db.Utilizadores.AddRange(
            Conta(AdminA, EmailAdmin, Perfis.Administrador),
            Conta(RecA, "rececao@clinica-g08.test", Perfis.Rececionista),
            Conta(VetA, "vet.ana@clinica-g08.test", Perfis.Veterinario),
            Conta(ContaClienteA, EmailClienteA, Perfis.Cliente),
            Conta(Bloqueado, EmailBloqueado, Perfis.Rececionista, EstadosUtilizador.Bloqueado));
        db.Funcionarios.AddRange(
            new Funcionario { UtilizadorId = AdminA, Nome = "Administração" },
            new Funcionario { UtilizadorId = RecA, Nome = "Rita Rececionista" },
            new Funcionario { UtilizadorId = VetA, Nome = "Dra. Ana Veterinária" },
            new Funcionario { UtilizadorId = Bloqueado, Nome = "Conta Bloqueada" });
        db.Clientes.Add(new Cliente { Id = ClienteA, UtilizadorId = ContaClienteA, Nome = "João Costa", Telefone = "960000001" });
        db.SaveChanges();
        db.ChangeTracker.Clear();

        return db;
    }

    private static Utilizador Conta(int id, string email, string perfil, string estado = EstadosUtilizador.Ativo) => new()
    {
        Id = id,
        Email = email,
        PasswordHash = "hash-guardado",
        Perfil = perfil,
        Estado = estado,
        DataCriacao = Relogio.UtcDateTime.AddDays(-30)
    };
}
