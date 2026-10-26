using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.Animais;

/// <summary>
/// I070 — /me/animais (EP-019, EP-020). Dados de demonstração da I061:
/// João tem Bobi e Mia, Maria tem Kiko e Rui tem Luna (sem data de nascimento nem peso).
/// </summary>
public sealed class MeusAnimaisEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailJoao = "cliente.joao@clinica-g08.test";
    private const string EmailMaria = "cliente.maria@clinica-g08.test";
    private const string EmailRui = "cliente.rui@clinica-g08.test";

    public MeusAnimaisEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    [Fact(DisplayName = "IT-EP019 — lista só os animais do Cliente autenticado, por nome")]
    public async Task ListaSoOsAnimaisDoClienteAutenticado()
    {
        using var cliente = await CriarClienteAutenticadoPorEmailAsync(EmailJoao);

        using var resposta = await cliente.GetAsync("/me/animais");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        var nomes = await LerNomesAsync(resposta);

        Assert.Collection(
            nomes,
            nome => Assert.Equal("Bobi", nome),
            nome => Assert.Equal("Mia", nome));
    }

    [Fact(DisplayName = "IT-EP019 — outro Cliente vê apenas o seu animal, com campos vazios quando não há dados")]
    public async Task OutroClienteVeApenasOSeuAnimal()
    {
        using var cliente = await CriarClienteAutenticadoPorEmailAsync(EmailRui);

        using var resposta = await cliente.GetAsync("/me/animais");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());

        var animal = Assert.Single(documento.RootElement.EnumerateArray());

        Assert.Equal("Luna", animal.GetProperty("nome").GetString());
        Assert.Equal(JsonValueKind.Null, animal.GetProperty("idadeAnos").ValueKind);
        Assert.Equal(JsonValueKind.Null, animal.GetProperty("peso").ValueKind);
    }

    [Fact(DisplayName = "IT-EP019 — o clienteId no pedido é ignorado: vale o Cliente do token")]
    public async Task ClienteIdNoPedidoEIgnorado()
    {
        var clienteIdMaria = await ObterClienteIdAsync(EmailMaria);

        using var cliente = await CriarClienteAutenticadoPorEmailAsync(EmailJoao);

        using var resposta = await cliente.GetAsync($"/me/animais?clienteId={clienteIdMaria}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        var nomes = await LerNomesAsync(resposta);

        Assert.Collection(
            nomes,
            nome => Assert.Equal("Bobi", nome),
            nome => Assert.Equal("Mia", nome));
    }

    [Fact(DisplayName = "IT-EP019 — Cliente sem animais recebe uma lista vazia")]
    public async Task ClienteSemAnimaisRecebeListaVazia()
    {
        var email = await CriarClienteSemAnimaisAsync();

        using var cliente = await CriarClienteAutenticadoPorEmailAsync(email);

        using var resposta = await cliente.GetAsync("/me/animais");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());

        Assert.Equal(JsonValueKind.Array, documento.RootElement.ValueKind);
        Assert.Empty(documento.RootElement.EnumerateArray());
    }

    [Fact(DisplayName = "IT-EP020 — consulta um animal próprio só com dados administrativos")]
    public async Task ConsultaAnimalProprio()
    {
        var animalId = await ObterAnimalIdAsync("Bobi");

        using var cliente = await CriarClienteAutenticadoPorEmailAsync(EmailJoao);

        using var resposta = await cliente.GetAsync($"/me/animais/{animalId}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
        var raiz = documento.RootElement;

        Assert.Equal(animalId, raiz.GetProperty("id").GetInt32());
        Assert.Equal("Bobi", raiz.GetProperty("nome").GetString());
        Assert.Equal("Cão", raiz.GetProperty("especie").GetString());
        Assert.Equal("Labrador Retriever", raiz.GetProperty("raca").GetString());
        Assert.Equal("Macho", raiz.GetProperty("sexo").GetString());
        Assert.Equal("2022-03-10", raiz.GetProperty("dataNascimento").GetString());

        Assert.False(raiz.TryGetProperty("cliente", out _));
        Assert.False(raiz.TryGetProperty("diagnostico", out _));
        Assert.False(raiz.TryGetProperty("notasClinicas", out _));
    }

    [Fact(DisplayName = "IT-EP020 — animal de outro Cliente devolve 403 sem dados do animal")]
    public async Task AnimalDeOutroClienteDevolve403()
    {
        var animalIdMaria = await ObterAnimalIdAsync("Kiko");

        using var cliente = await CriarClienteAutenticadoPorEmailAsync(EmailJoao);

        using var resposta = await cliente.GetAsync($"/me/animais/{animalIdMaria}");

        Assert.Equal(HttpStatusCode.Forbidden, resposta.StatusCode);

        var corpo = await resposta.Content.ReadAsStringAsync();

        Assert.DoesNotContain("Kiko", corpo, StringComparison.Ordinal);
        Assert.DoesNotContain("Sem raça definida", corpo, StringComparison.Ordinal);

        using var documento = JsonDocument.Parse(corpo);

        Assert.Equal(403, documento.RootElement.GetProperty("code").GetInt32());
    }

    [Fact(DisplayName = "IT-EP020 — animal inexistente devolve 404")]
    public async Task AnimalInexistenteDevolve404()
    {
        var idInexistente = await ObterIdInexistenteAsync();

        using var cliente = await CriarClienteAutenticadoPorEmailAsync(EmailJoao);

        using var resposta = await cliente.GetAsync($"/me/animais/{idInexistente}");

        Assert.Equal(HttpStatusCode.NotFound, resposta.StatusCode);
    }

    [Theory(DisplayName = "IT-EP019/EP-020 — perfis internos recebem 403")]
    [InlineData("Administrador")]
    [InlineData("Rececionista")]
    [InlineData("Veterinario")]
    [InlineData("AuxiliarVeterinario")]
    public async Task PerfisInternosRecebem403(string perfil)
    {
        var animalId = await ObterAnimalIdAsync("Bobi");

        using var cliente = await Fixture.CriarClienteAutenticadoAsync(perfil);

        using var lista = await cliente.GetAsync("/me/animais");
        using var animal = await cliente.GetAsync($"/me/animais/{animalId}");

        Assert.Equal(HttpStatusCode.Forbidden, lista.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, animal.StatusCode);
    }

    [Fact(DisplayName = "IT-EP019/EP-020 — sem token devolvem 401")]
    public async Task SemTokenDevolve401()
    {
        var animalId = await ObterAnimalIdAsync("Bobi");

        using var cliente = Fixture.Factory.CreateClient();

        using var lista = await cliente.GetAsync("/me/animais");
        using var animal = await cliente.GetAsync($"/me/animais/{animalId}");

        Assert.Equal(HttpStatusCode.Unauthorized, lista.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, animal.StatusCode);
    }

    private static async Task<List<string?>> LerNomesAsync(HttpResponseMessage resposta)
    {
        using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());

        return documento.RootElement
            .EnumerateArray()
            .Select(animal => animal.GetProperty("nome").GetString())
            .ToList();
    }

    /// <summary>
    /// Igual a <c>CriarClienteAutenticadoAsync</c>, mas para uma conta concreta,
    /// para poder autenticar dois Clientes diferentes no mesmo teste.
    /// </summary>
    private async Task<HttpClient> CriarClienteAutenticadoPorEmailAsync(string email)
    {
        using var scope = Fixture.Factory.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        var utilizadorId = await db.Utilizadores
            .AsNoTracking()
            .Where(u => u.Email == email)
            .Select(u => u.Id)
            .SingleAsync();

        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var jwtTokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();

        var token = jwtTokenService.GerarToken(
            await authService.ObterAutenticadoAsync(utilizadorId));

        var cliente = Fixture.Factory.CreateClient();

        cliente.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token.Token);

        return cliente;
    }

    private async Task<string> CriarClienteSemAnimaisAsync()
    {
        using var scope = Fixture.Factory.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        var email = DadosTeste.EmailUnico("cliente.sem.animais");

        var utilizador = new Utilizador
        {
            Email = email,
            PasswordHash = "nao-usado-neste-teste",
            Perfil = "Cliente",
            Estado = "Ativo"
        };

        db.Utilizadores.Add(utilizador);
        await db.SaveChangesAsync();

        db.Clientes.Add(new Cliente
        {
            UtilizadorId = utilizador.Id,
            Nome = DadosTeste.NomeUnico("Cliente"),
            Telefone = "960000099"
        });

        await db.SaveChangesAsync();

        return email;
    }

    private async Task<int> ObterAnimalIdAsync(string nome)
    {
        using var scope = Fixture.Factory.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Pacientes
            .AsNoTracking()
            .Where(p => p.Nome == nome)
            .Select(p => p.Id)
            .SingleAsync();
    }

    private async Task<int> ObterIdInexistenteAsync()
    {
        using var scope = Fixture.Factory.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        var maior = await db.Pacientes
            .AsNoTracking()
            .MaxAsync(p => (int?)p.Id) ?? 0;

        return maior + 1000;
    }

    private async Task<int> ObterClienteIdAsync(string email)
    {
        using var scope = Fixture.Factory.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await (
            from c in db.Clientes.AsNoTracking()
            join u in db.Utilizadores.AsNoTracking()
                on c.UtilizadorId equals u.Id
            where u.Email == email
            select c.Id)
            .SingleAsync();
    }
}
