using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.Utilizadores;

/// <summary>
/// I066 — PUT /utilizadores/{id} (EP-007) e PATCH /utilizadores/{id}/estado (EP-008),
/// casos IT-EP007-01 a 06 e IT-EP008-01 a 07 da Especificação de Testes de Integração.
/// Dados de demonstração da I061: um Administrador, a Rececionista Rita, a Rececionista
/// bloqueada Rui, os Veterinários Ana e Bruno e as Auxiliares Carla (com apoio) e Diogo.
/// </summary>
public sealed class UtilizadoresEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailAdmin = "admin@clinica-g08.test";
    private const string EmailRececao = "rececao@clinica-g08.test";
    private const string EmailBloqueada = "rececao.bloqueada@clinica-g08.test";
    private const string EmailVetAna = "vet.ana@clinica-g08.test";
    private const string EmailAuxCarla = "aux.carla@clinica-g08.test";
    private const string EmailClienteJoao = "cliente.joao@clinica-g08.test";

    public UtilizadoresEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    // ----- EP-007 -----

    [Fact(DisplayName = "IT-EP007-01 — funcionário existente com dados válidos devolve 200 e persiste as alterações")]
    public async Task AtualizaFuncionarioExistente()
    {
        var id = await ObterIdAsync(EmailRececao);
        var novoEmail = DadosTeste.EmailUnico("rita.lopes");

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

        using var resposta = await cliente.PutAsJsonAsync($"/utilizadores/{id}", new
        {
            nome = "Rita Lopes",
            email = novoEmail,
            telefone = "934567890",
            perfil = "AuxiliarVeterinario",
            podeRegistarApoio = true
        });

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
        var raiz = documento.RootElement;

        Assert.Equal(id, raiz.GetProperty("id").GetInt32());
        Assert.Equal("Rita Lopes", raiz.GetProperty("nome").GetString());
        Assert.Equal(novoEmail, raiz.GetProperty("email").GetString());
        Assert.Equal("AuxiliarVeterinario", raiz.GetProperty("perfil").GetString());
        Assert.Equal("Ativo", raiz.GetProperty("estado").GetString());
        Assert.True(raiz.GetProperty("podeRegistarApoio").GetBoolean());
        Assert.False(raiz.TryGetProperty("passwordHash", out _));

        var conta = await LerContaAsync(id);

        Assert.Equal(novoEmail, conta.Email);
        Assert.Equal("AuxiliarVeterinario", conta.Perfil);
        Assert.Equal("Rita Lopes", conta.Nome);
        Assert.True(conta.PodeRegistarApoio);
    }

    [Theory(DisplayName = "IT-EP007-02 — dados inválidos ou perfil Cliente devolvem 400 e mantêm o estado anterior")]
    [InlineData("", "rita@clinica-g08.test", "Rececionista")]
    [InlineData("Rita Lopes", "rec", "Rececionista")]
    [InlineData("Rita Lopes", "rita@clinica-g08.test", "Gestor")]
    [InlineData("Rita Lopes", "rita@clinica-g08.test", "Cliente")]
    public async Task DadosInvalidosOuPerfilClienteDevolvem400(string nome, string email, string perfil)
    {
        var id = await ObterIdAsync(EmailRececao);
        var antes = await LerContaAsync(id);

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

        using var resposta = await cliente.PutAsJsonAsync($"/utilizadores/{id}", new
        {
            nome,
            email,
            telefone = "934567890",
            perfil,
            podeRegistarApoio = false
        });

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
        Assert.Equal(antes, await LerContaAsync(id));
    }

    [Fact(DisplayName = "IT-EP007-03 — identificador inexistente ou conta de Cliente devolvem 404")]
    public async Task ContaInexistenteOuClienteDevolve404()
    {
        var idInexistente = await ObterIdInexistenteAsync();
        var idCliente = await ObterIdAsync(EmailClienteJoao);

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

        using var inexistente = await cliente.PutAsJsonAsync($"/utilizadores/{idInexistente}", PedidoValido());
        using var contaCliente = await cliente.PutAsJsonAsync($"/utilizadores/{idCliente}", PedidoValido());

        await AssertErroAsync(inexistente, HttpStatusCode.NotFound);
        await AssertErroAsync(contaCliente, HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "IT-EP007-04 — e-mail usado por outra conta devolve 409 sem alterar dados")]
    public async Task EmailDeOutraContaDevolve409()
    {
        var id = await ObterIdAsync(EmailRececao);
        var antes = await LerContaAsync(id);

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

        using var resposta = await cliente.PutAsJsonAsync($"/utilizadores/{id}", new
        {
            nome = "Rita Lopes",
            email = EmailVetAna.ToUpperInvariant(),
            telefone = "934567890",
            perfil = "Rececionista",
            podeRegistarApoio = false
        });

        await AssertErroAsync(resposta, HttpStatusCode.Conflict);
        Assert.Equal(antes, await LerContaAsync(id));
    }

    [Fact(DisplayName = "IT-EP007-05 — o Administrador não pode remover o próprio perfil (422) e mantém-se Administrador")]
    public async Task AdministradorNaoRemoveOProprioPerfil()
    {
        var id = await ObterIdAsync(EmailAdmin);

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

        using var resposta = await cliente.PutAsJsonAsync($"/utilizadores/{id}", new
        {
            nome = "Administração",
            email = EmailAdmin,
            telefone = "910000001",
            perfil = "Rececionista",
            podeRegistarApoio = false
        });

        await AssertErroAsync(resposta, HttpStatusCode.UnprocessableEntity);
        Assert.Equal("Administrador", (await LerContaAsync(id)).Perfil);
    }

    [Fact(DisplayName = "IT-EP007-06 — podeRegistarApoio num perfil incompatível devolve 422, também ao mudar de perfil")]
    public async Task ApoioEmPerfilIncompativelDevolve422()
    {
        var idVet = await ObterIdAsync(EmailVetAna);
        var idCarla = await ObterIdAsync(EmailAuxCarla);
        var carlaAntes = await LerContaAsync(idCarla);

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

        using var veterinario = await cliente.PutAsJsonAsync($"/utilizadores/{idVet}", new
        {
            nome = "Dra. Ana Veterinária",
            email = EmailVetAna,
            perfil = "Veterinario",
            podeRegistarApoio = true
        });

        // A Carla é Auxiliar com apoio: ao mudar para Rececionista tem de perder a autorização.
        using var mudancaDePerfil = await cliente.PutAsJsonAsync($"/utilizadores/{idCarla}", new
        {
            nome = "Carla Auxiliar",
            email = EmailAuxCarla,
            perfil = "Rececionista",
            podeRegistarApoio = true
        });

        await AssertErroAsync(veterinario, HttpStatusCode.UnprocessableEntity);
        await AssertErroAsync(mudancaDePerfil, HttpStatusCode.UnprocessableEntity);
        Assert.Equal(carlaAntes, await LerContaAsync(idCarla));
    }

    [Fact(DisplayName = "I066 — depois da mudança de perfil, o token emitido com o perfil antigo devolve 401")]
    public async Task MudancaDePerfilInvalidaOTokenAntigo()
    {
        var id = await ObterIdAsync(EmailRececao);

        using var rececionista = await CriarClienteAutenticadoPorEmailAsync(EmailRececao);
        using var antes = await rececionista.GetAsync("/auth/me");
        Assert.Equal(HttpStatusCode.OK, antes.StatusCode);

        using var admin = await Fixture.CriarClienteAutenticadoAsync("Administrador");
        using var atualizacao = await admin.PutAsJsonAsync($"/utilizadores/{id}", new
        {
            nome = "Rita Rececionista (demonstração)",
            email = EmailRececao,
            perfil = "Veterinario",
            podeRegistarApoio = false
        });
        Assert.Equal(HttpStatusCode.OK, atualizacao.StatusCode);

        using var depois = await rececionista.GetAsync("/auth/me");

        Assert.Equal(HttpStatusCode.Unauthorized, depois.StatusCode);
    }

    // ----- EP-008 -----

    [Fact(DisplayName = "IT-EP008-01 — funcionário ativo é bloqueado: 204 e estado persistido como Bloqueado")]
    public async Task BloqueiaFuncionarioAtivo()
    {
        var id = await ObterIdAsync(EmailVetAna);

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

        using var resposta = await cliente.PatchAsJsonAsync($"/utilizadores/{id}/estado", PedidoBloqueio());

        Assert.Equal(HttpStatusCode.NoContent, resposta.StatusCode);
        Assert.Empty(await resposta.Content.ReadAsStringAsync());
        Assert.Equal("Bloqueado", (await LerContaAsync(id)).Estado);
    }

    [Theory(DisplayName = "IT-EP008-02 — estado diferente de Bloqueado, ou em falta, devolve 400")]
    [InlineData("Ativo")]
    [InlineData("Suspenso")]
    [InlineData("")]
    public async Task EstadoDiferenteDeBloqueadoDevolve400(string estado)
    {
        var id = await ObterIdAsync(EmailVetAna);

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

        using var resposta = await cliente.PatchAsJsonAsync($"/utilizadores/{id}/estado", new { estado });

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
        Assert.Equal("Ativo", (await LerContaAsync(id)).Estado);
    }

    [Fact(DisplayName = "IT-EP008-03 — identificador inexistente devolve 404")]
    public async Task ContaInexistenteNoBloqueioDevolve404()
    {
        var idInexistente = await ObterIdInexistenteAsync();

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

        using var resposta = await cliente.PatchAsJsonAsync($"/utilizadores/{idInexistente}/estado", PedidoBloqueio());

        await AssertErroAsync(resposta, HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "IT-EP008-04 — bloquear uma conta já bloqueada devolve 422")]
    public async Task ContaJaBloqueadaDevolve422()
    {
        var id = await ObterIdAsync(EmailBloqueada);

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

        using var resposta = await cliente.PatchAsJsonAsync($"/utilizadores/{id}/estado", PedidoBloqueio());

        await AssertErroAsync(resposta, HttpStatusCode.UnprocessableEntity);
    }

    [Fact(DisplayName = "IT-EP008-05 — o Administrador não pode bloquear a própria conta (422)")]
    public async Task AdministradorNaoSeBloqueia()
    {
        var id = await ObterIdAsync(EmailAdmin);

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

        using var resposta = await cliente.PatchAsJsonAsync($"/utilizadores/{id}/estado", PedidoBloqueio());

        await AssertErroAsync(resposta, HttpStatusCode.UnprocessableEntity);
        Assert.Equal("Ativo", (await LerContaAsync(id)).Estado);
    }

    [Fact(DisplayName = "IT-EP008-06 — bloquear uma conta de Cliente devolve 422")]
    public async Task ContaDeClienteDevolve422()
    {
        var id = await ObterIdAsync(EmailClienteJoao);

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

        using var resposta = await cliente.PatchAsJsonAsync($"/utilizadores/{id}/estado", PedidoBloqueio());

        await AssertErroAsync(resposta, HttpStatusCode.UnprocessableEntity);
        Assert.Equal("Ativo", (await LerContaAsync(id)).Estado);
    }

    [Fact(DisplayName = "IT-EP008-07 — o token emitido antes do bloqueio passa a ser rejeitado com 401")]
    public async Task TokenEmitidoAntesDoBloqueioERejeitado()
    {
        var id = await ObterIdAsync(EmailRececao);

        using var rececionista = await CriarClienteAutenticadoPorEmailAsync(EmailRececao);
        using var antes = await rececionista.GetAsync("/clientes");
        Assert.Equal(HttpStatusCode.OK, antes.StatusCode);

        using var admin = await Fixture.CriarClienteAutenticadoAsync("Administrador");
        using var bloqueio = await admin.PatchAsJsonAsync($"/utilizadores/{id}/estado", PedidoBloqueio());
        Assert.Equal(HttpStatusCode.NoContent, bloqueio.StatusCode);

        using var depois = await rececionista.GetAsync("/clientes");

        Assert.Equal(HttpStatusCode.Unauthorized, depois.StatusCode);
    }

    // ----- Autorização -----

    [Theory(DisplayName = "IT-EP007/EP-008 — perfis que não são Administrador recebem 403")]
    [InlineData("Rececionista")]
    [InlineData("Veterinario")]
    [InlineData("AuxiliarVeterinario")]
    [InlineData("Cliente")]
    public async Task OutrosPerfisRecebem403(string perfil)
    {
        var id = await ObterIdAsync(EmailVetAna);

        using var cliente = await Fixture.CriarClienteAutenticadoAsync(perfil);

        using var atualizacao = await cliente.PutAsJsonAsync($"/utilizadores/{id}", PedidoValido());
        using var bloqueio = await cliente.PatchAsJsonAsync($"/utilizadores/{id}/estado", PedidoBloqueio());

        await AssertErroAsync(atualizacao, HttpStatusCode.Forbidden);
        await AssertErroAsync(bloqueio, HttpStatusCode.Forbidden);
        Assert.Equal("Ativo", (await LerContaAsync(id)).Estado);
    }

    [Fact(DisplayName = "IT-EP007/EP-008 — sem token devolvem 401")]
    public async Task SemTokenDevolve401()
    {
        var id = await ObterIdAsync(EmailVetAna);

        using var cliente = Fixture.Factory.CreateClient();

        using var atualizacao = await cliente.PutAsJsonAsync($"/utilizadores/{id}", PedidoValido());
        using var bloqueio = await cliente.PatchAsJsonAsync($"/utilizadores/{id}/estado", PedidoBloqueio());

        await AssertErroAsync(atualizacao, HttpStatusCode.Unauthorized);
        await AssertErroAsync(bloqueio, HttpStatusCode.Unauthorized);
    }

    private static object PedidoValido() => new
    {
        nome = "Rui Lopes",
        email = DadosTeste.EmailUnico("rui.lopes"),
        telefone = "934567890",
        perfil = "Rececionista",
        podeRegistarApoio = false
    };

    private static object PedidoBloqueio() => new
    {
        estado = "Bloqueado",
        motivo = "Funcionário deixou a clínica."
    };

    /// <summary>Formato único de erro da infraestrutura (I058): <c>code</c> igual ao código HTTP.</summary>
    private static async Task AssertErroAsync(HttpResponseMessage resposta, HttpStatusCode esperado)
    {
        Assert.Equal(esperado, resposta.StatusCode);

        var corpo = await resposta.Content.ReadAsStringAsync();

        Assert.False(string.IsNullOrWhiteSpace(corpo), $"A resposta {(int)esperado} não tem corpo.");

        using var documento = JsonDocument.Parse(corpo);

        Assert.Equal((int)esperado, documento.RootElement.GetProperty("code").GetInt32());
    }

    private async Task<int> ObterIdAsync(string email)
    {
        using var scope = Fixture.Factory.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Utilizadores
            .AsNoTracking()
            .Where(u => u.Email == email)
            .Select(u => u.Id)
            .SingleAsync();
    }

    private async Task<int> ObterIdInexistenteAsync()
    {
        using var scope = Fixture.Factory.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        var maior = await db.Utilizadores
            .AsNoTracking()
            .MaxAsync(u => (int?)u.Id) ?? 0;

        return maior + 1000;
    }

    private async Task<Conta> LerContaAsync(int id)
    {
        using var scope = Fixture.Factory.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        var utilizador = await db.Utilizadores.AsNoTracking().SingleAsync(u => u.Id == id);
        var funcionario = await db.Funcionarios.AsNoTracking().SingleOrDefaultAsync(f => f.UtilizadorId == id);

        return new Conta(
            utilizador.Email,
            utilizador.Perfil,
            utilizador.Estado,
            funcionario?.Nome,
            funcionario?.PodeRegistarApoio ?? false);
    }

    /// <summary>Igual a <c>CriarClienteAutenticadoAsync</c>, mas para uma conta concreta.</summary>
    private async Task<HttpClient> CriarClienteAutenticadoPorEmailAsync(string email)
    {
        using var scope = Fixture.Factory.Services.CreateScope();

        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var jwtTokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();

        var token = jwtTokenService.GerarToken(
            await authService.ObterAutenticadoAsync(await ObterIdAsync(email)));

        var cliente = Fixture.Factory.CreateClient();

        cliente.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token.Token);

        return cliente;
    }

    private sealed record Conta(string Email, string Perfil, string Estado, string? Nome, bool PodeRegistarApoio);
}
