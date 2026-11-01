using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.Animais;

/// <summary>
/// I069 e I092 — GET /animais (EP-015), GET /animais/{id} (EP-016), POST /animais (EP-017) e
/// PUT /animais/{id} (EP-018), casos IT-EP015-01 a 06, IT-EP016-01 a 03, IT-EP017-01 a 06 e
/// IT-EP018-01 a 04 da Especificação de Testes de Integração (I030), mais os 401 e 403
/// previstos no contrato da API v02 (pesquisa, registo e atualização: Rececionista e
/// Veterinário; ficha: também o Auxiliar Veterinário, só em leitura).
/// Animais da I061: Bobi (Cão) e Mia (Gato) do João, Kiko (Cão) da Maria e Luna (Gato) do Rui.
/// </summary>
public sealed class AnimaisEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailJoao = "cliente.joao@clinica-g08.test";

    public AnimaisEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    // ----- EP-015 -----

    [Fact(DisplayName = "IT-EP015-01 — existem animais: 200 com envelope paginado, por ordem do nome")]
    public async Task ListarDevolveEnvelopePaginado()
    {
        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var resposta = await cliente.GetAsync("/animais");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
        var raiz = documento.RootElement;
        var nomes = raiz.GetProperty("itens").EnumerateArray().Select(i => i.GetProperty("nome").GetString()).ToList();

        Assert.Equal(1, raiz.GetProperty("pagina").GetInt32());
        Assert.Equal(20, raiz.GetProperty("tamanho").GetInt32());
        Assert.Equal(await ContarAnimaisAsync(), raiz.GetProperty("total").GetInt32());
        Assert.Equal("Bobi|Kiko|Luna|Mia", string.Join('|', nomes));
    }

    [Fact(DisplayName = "IT-EP015-02 — clienteId válido: só os animais desse Cliente")]
    public async Task FiltrarPorCliente()
    {
        var clienteId = await ObterClienteIdAsync(EmailJoao);

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Veterinario");

        var itens = await PesquisarAsync(cliente, $"/animais?clienteId={clienteId}");

        Assert.Equal("Bobi|Mia", string.Join('|', itens.Select(i => i.Nome)));
        Assert.All(itens, i => Assert.Equal(clienteId, i.ClienteId));
    }

    [Theory(DisplayName = "IT-EP015-03 — pesquisa pelo nome do animal ou do proprietário devolve os resultados corretos")]
    [InlineData("Kiko", "Kiko")]
    [InlineData("kik", "Kiko")]
    [InlineData("Maria", "Kiko")]
    [InlineData("Costa", "Bobi|Mia")]
    public async Task PesquisarPorNome(string termo, string esperado)
    {
        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        var itens = await PesquisarAsync(cliente, $"/animais?pesquisa={Uri.EscapeDataString(termo)}");

        Assert.Equal(esperado, string.Join('|', itens.Select(i => i.Nome)));
    }

    [Fact(DisplayName = "IT-EP015-04 — filtro por espécie: só animais da espécie pedida")]
    public async Task FiltrarPorEspecie()
    {
        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        var itens = await PesquisarAsync(cliente, "/animais?especie=Gato");

        Assert.Equal("Luna|Mia", string.Join('|', itens.Select(i => i.Nome)));
        Assert.All(itens, i => Assert.Equal("Gato", i.Especie));
    }

    [Theory(DisplayName = "IT-EP015-05 — parâmetros inválidos devolvem 400")]
    [InlineData("/animais?pagina=0")]
    [InlineData("/animais?tamanho=0")]
    [InlineData("/animais?tamanho=101")]
    [InlineData("/animais?clienteId=0")]
    [InlineData("/animais?clienteId=abc")]
    public async Task ParametrosInvalidosDevolvem400(string url)
    {
        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var resposta = await cliente.GetAsync(url);

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "IT-EP015-06 — tamanho=100 com mais de 100 animais: 200 com no máximo 100 elementos e o resto na página seguinte")]
    public async Task PaginacaoComTamanhoMaximo()
    {
        await CriarAnimaisDiretamenteAsync(105);
        var total = await ContarAnimaisAsync();

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var primeira = await cliente.GetAsync("/animais?pagina=1&tamanho=100");
        using var segunda = await cliente.GetAsync("/animais?pagina=2&tamanho=100");

        Assert.Equal(HttpStatusCode.OK, primeira.StatusCode);
        Assert.Equal(HttpStatusCode.OK, segunda.StatusCode);

        using var documento1 = JsonDocument.Parse(await primeira.Content.ReadAsStringAsync());
        using var documento2 = JsonDocument.Parse(await segunda.Content.ReadAsStringAsync());

        Assert.Equal(100, documento1.RootElement.GetProperty("itens").GetArrayLength());
        Assert.Equal(total, documento1.RootElement.GetProperty("total").GetInt32());
        Assert.Equal(total - 100, documento2.RootElement.GetProperty("itens").GetArrayLength());
    }

    // ----- EP-016 -----

    [Fact(DisplayName = "IT-EP016-01 — animal existente: 200 só com dados administrativos")]
    public async Task ObterFichaAdministrativa()
    {
        var animalId = await ObterAnimalIdAsync("Bobi");
        var clienteId = await ObterClienteIdAsync(EmailJoao);

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var resposta = await cliente.GetAsync($"/animais/{animalId}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
        var raiz = documento.RootElement;

        Assert.Equal(animalId, raiz.GetProperty("id").GetInt32());
        Assert.Equal("Bobi", raiz.GetProperty("nome").GetString());
        Assert.Equal("Cão", raiz.GetProperty("especie").GetString());
        Assert.Equal("Labrador Retriever", raiz.GetProperty("raca").GetString());
        Assert.Equal("Macho", raiz.GetProperty("sexo").GetString());
        Assert.Equal(clienteId, raiz.GetProperty("cliente").GetProperty("id").GetInt32());
        AssertSemDadosClinicos(raiz);
    }

    [Fact(DisplayName = "IT-EP016-02 — identificador inexistente devolve 404")]
    public async Task ObterInexistenteDevolve404()
    {
        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var resposta = await cliente.GetAsync($"/animais/{await ObterAnimalIdInexistenteAsync()}");

        await AssertErroAsync(resposta, HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "IT-EP016-03 — Auxiliar Veterinário: 200 na ficha, sem dados clínicos, e 403 ao registar ou alterar (só leitura)")]
    public async Task AuxiliarSoTemLeitura()
    {
        var animalId = await ObterAnimalIdAsync("Bobi");
        var clienteId = await ObterClienteIdAsync(EmailJoao);
        var antes = await LerAnimalAsync(animalId);
        var totalAntes = await ContarAnimaisAsync();

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("AuxiliarVeterinario");

        using var ficha = await cliente.GetAsync($"/animais/{animalId}");
        using var criar = await cliente.PostAsJsonAsync("/animais", Pedido(clienteId, "Thor Auxiliar"));
        using var alterar = await cliente.PutAsJsonAsync($"/animais/{animalId}", Pedido(clienteId, "Bobi Auxiliar"));

        Assert.Equal(HttpStatusCode.OK, ficha.StatusCode);

        using var documento = JsonDocument.Parse(await ficha.Content.ReadAsStringAsync());
        Assert.Equal("Bobi", documento.RootElement.GetProperty("nome").GetString());
        AssertSemDadosClinicos(documento.RootElement);

        await AssertErroAsync(criar, HttpStatusCode.Forbidden);
        await AssertErroAsync(alterar, HttpStatusCode.Forbidden);
        Assert.Equal(totalAntes, await ContarAnimaisAsync());
        Assert.Equal(antes, await LerAnimalAsync(animalId));
    }

    // ----- EP-017 -----

    [Fact(DisplayName = "IT-EP017-01 — dados válidos e Cliente existente: 201 com Location e o animal fica persistido")]
    public async Task RegistarAnimalValido()
    {
        var clienteId = await ObterClienteIdAsync(EmailJoao);

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var resposta = await cliente.PostAsJsonAsync("/animais", Pedido(clienteId, "Thor I092"));

        Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);

        using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
        var id = documento.RootElement.GetProperty("id").GetInt32();

        Assert.NotNull(resposta.Headers.Location);
        Assert.EndsWith($"/animais/{id}", resposta.Headers.Location!.ToString(), StringComparison.Ordinal);
        Assert.Equal("Thor I092", documento.RootElement.GetProperty("nome").GetString());

        var guardado = await LerAnimalAsync(id);

        Assert.Equal("Thor I092", guardado.Nome);
        Assert.Equal(clienteId, guardado.ClienteId);
        Assert.Equal("Macho", guardado.Sexo);
        Assert.Equal(new DateOnly(2022, 5, 10), guardado.DataNascimento);
        Assert.Equal(25.40m, guardado.Peso);
    }

    [Theory(DisplayName = "IT-EP017-02 — campo obrigatório ou clienteId em falta devolve 400 e nada é criado")]
    [InlineData("nome")]
    [InlineData("especie")]
    [InlineData("clienteId")]
    public async Task CampoEmFaltaDevolve400(string campo)
    {
        var pedido = Pedido(await ObterClienteIdAsync(EmailJoao), "Campo Em Falta");
        pedido.Remove(campo);
        var antes = await ContarAnimaisAsync();

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var resposta = await cliente.PostAsJsonAsync("/animais", pedido);

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
        Assert.Equal(antes, await ContarAnimaisAsync());
    }

    [Theory(DisplayName = "IT-EP017-03 — peso igual ou inferior a 0 devolve 400 e nada é criado")]
    [InlineData("0")]
    [InlineData("-1")]
    public async Task PesoNaoPositivoDevolve400(string peso)
    {
        var pedido = Pedido(await ObterClienteIdAsync(EmailJoao), "Peso Inválido");
        pedido["peso"] = decimal.Parse(peso, CultureInfo.InvariantCulture);
        var antes = await ContarAnimaisAsync();

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Veterinario");

        using var resposta = await cliente.PostAsJsonAsync("/animais", pedido);

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
        Assert.Equal(antes, await ContarAnimaisAsync());
    }

    [Fact(DisplayName = "IT-EP017-04 — data de nascimento futura devolve 400 e nada é criado")]
    public async Task DataNascimentoFuturaDevolve400()
    {
        var pedido = Pedido(await ObterClienteIdAsync(EmailJoao), "Animal Futuro");
        pedido["dataNascimento"] = DataFutura();
        var antes = await ContarAnimaisAsync();

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var resposta = await cliente.PostAsJsonAsync("/animais", pedido);

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
        Assert.Equal(antes, await ContarAnimaisAsync());
    }

    [Fact(DisplayName = "IT-EP017-05 — Cliente inexistente devolve 404 e nada é criado")]
    public async Task ClienteInexistenteDevolve404()
    {
        var antes = await ContarAnimaisAsync();

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var resposta = await cliente.PostAsJsonAsync("/animais", Pedido(await ObterClienteIdInexistenteAsync(), "Sem Dono"));

        await AssertErroAsync(resposta, HttpStatusCode.NotFound);
        Assert.Equal(antes, await ContarAnimaisAsync());
    }

    [Fact(DisplayName = "IT-EP017-06 — pedidos rejeitados (400 e 404) não deixam nenhum animal criado, nem parcialmente")]
    public async Task PedidosRejeitadosNaoCriamAnimais()
    {
        var clienteId = await ObterClienteIdAsync(EmailJoao);
        var antes = await ContarAnimaisAsync();

        var especieInexistente = Pedido(clienteId, "Rejeitado Especie");
        especieInexistente["especie"] = "Dragão";

        var racaDeOutraEspecie = Pedido(clienteId, "Rejeitado Raca");
        racaDeOutraEspecie["raca"] = "Siamês";

        var pesoInvalido = Pedido(clienteId, "Rejeitado Peso");
        pesoInvalido["peso"] = -5m;

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var r1 = await cliente.PostAsJsonAsync("/animais", especieInexistente);
        using var r2 = await cliente.PostAsJsonAsync("/animais", racaDeOutraEspecie);
        using var r3 = await cliente.PostAsJsonAsync("/animais", pesoInvalido);
        using var r4 = await cliente.PostAsJsonAsync("/animais", Pedido(await ObterClienteIdInexistenteAsync(), "Rejeitado Cliente"));

        await AssertErroAsync(r1, HttpStatusCode.BadRequest);
        await AssertErroAsync(r2, HttpStatusCode.BadRequest);
        await AssertErroAsync(r3, HttpStatusCode.BadRequest);
        await AssertErroAsync(r4, HttpStatusCode.NotFound);

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        Assert.Equal(antes, await db.Pacientes.AsNoTracking().CountAsync());
        Assert.False(await db.Pacientes.AsNoTracking().AnyAsync(p => EF.Functions.Like(p.Nome, "Rejeitado%")));
    }

    // ----- EP-018 -----

    [Fact(DisplayName = "IT-EP018-01 — animal existente e dados válidos: 200 e as alterações ficam persistidas")]
    public async Task AtualizarAnimalValido()
    {
        var animalId = await ObterAnimalIdAsync("Bobi");
        var clienteMaria = await ObterClienteIdAsync("cliente.maria@clinica-g08.test");

        var pedido = Pedido(clienteMaria, "Bobi Atualizado");
        pedido["peso"] = 29.10m;
        pedido["dataNascimento"] = "2022-03-10";

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Veterinario");

        using var resposta = await cliente.PutAsJsonAsync($"/animais/{animalId}", pedido);

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        var guardado = await LerAnimalAsync(animalId);

        Assert.Equal("Bobi Atualizado", guardado.Nome);
        Assert.Equal(29.10m, guardado.Peso);
        Assert.Equal(new DateOnly(2022, 3, 10), guardado.DataNascimento);
        Assert.Equal(clienteMaria, guardado.ClienteId);
        Assert.NotNull(guardado.DataUltimaAlteracao);
    }

    [Theory(DisplayName = "IT-EP018-02 — peso igual ou inferior a 0 ou data de nascimento futura devolve 400 e o estado anterior mantém-se")]
    [InlineData("peso")]
    [InlineData("dataNascimento")]
    public async Task DadosInvalidosNaAtualizacaoDevolvem400(string campo)
    {
        var animalId = await ObterAnimalIdAsync("Bobi");
        var antes = await LerAnimalAsync(animalId);

        var pedido = Pedido(antes.ClienteId, "Bobi Inválido");
        pedido[campo] = campo == "peso" ? 0m : DataFutura();

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var resposta = await cliente.PutAsJsonAsync($"/animais/{animalId}", pedido);

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
        Assert.Equal(antes, await LerAnimalAsync(animalId));
    }

    [Fact(DisplayName = "IT-EP018-03 — animal inexistente devolve 404")]
    public async Task AtualizarInexistenteDevolve404()
    {
        var clienteId = await ObterClienteIdAsync(EmailJoao);
        var antes = await ContarAnimaisAsync();

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var resposta = await cliente.PutAsJsonAsync($"/animais/{await ObterAnimalIdInexistenteAsync()}", Pedido(clienteId, "Fantasma"));

        await AssertErroAsync(resposta, HttpStatusCode.NotFound);
        Assert.Equal(antes, await ContarAnimaisAsync());
    }

    [Fact(DisplayName = "IT-EP018-04 — novo Cliente inexistente devolve 404 e a associação anterior mantém-se")]
    public async Task AtualizarParaClienteInexistenteMantemAssociacao()
    {
        var animalId = await ObterAnimalIdAsync("Bobi");
        var antes = await LerAnimalAsync(animalId);

        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var resposta = await cliente.PutAsJsonAsync(
            $"/animais/{animalId}",
            Pedido(await ObterClienteIdInexistenteAsync(), "Nome Que Não Deve Ser Gravado"));

        await AssertErroAsync(resposta, HttpStatusCode.NotFound);
        Assert.Equal(antes, await LerAnimalAsync(animalId));
    }

    // ----- 401 e 403 (contrato da API v02) -----

    [Theory(DisplayName = "I092 — EP-015 a EP-018: Administrador e Cliente recebem 403 e nada é alterado")]
    [InlineData("Administrador")]
    [InlineData("Cliente")]
    public async Task PerfisSemAcessoRecebem403(string perfil)
    {
        var animalId = await ObterAnimalIdAsync("Bobi");
        var clienteId = await ObterClienteIdAsync(EmailJoao);
        var antes = await LerAnimalAsync(animalId);
        var totalAntes = await ContarAnimaisAsync();

        using var cliente = await Fixture.CriarClienteAutenticadoAsync(perfil);

        using var pesquisar = await cliente.GetAsync("/animais");
        using var obter = await cliente.GetAsync($"/animais/{animalId}");
        using var criar = await cliente.PostAsJsonAsync("/animais", Pedido(clienteId, "Sem Permissão"));
        using var alterar = await cliente.PutAsJsonAsync($"/animais/{animalId}", Pedido(clienteId, "Sem Permissão"));

        await AssertErroAsync(pesquisar, HttpStatusCode.Forbidden);
        await AssertErroAsync(obter, HttpStatusCode.Forbidden);
        await AssertErroAsync(criar, HttpStatusCode.Forbidden);
        await AssertErroAsync(alterar, HttpStatusCode.Forbidden);
        Assert.Equal(totalAntes, await ContarAnimaisAsync());
        Assert.Equal(antes, await LerAnimalAsync(animalId));
    }

    [Fact(DisplayName = "I092 — EP-015: o Auxiliar Veterinário não pesquisa pacientes (403)")]
    public async Task AuxiliarNaoPesquisa()
    {
        using var cliente = await Fixture.CriarClienteAutenticadoAsync("AuxiliarVeterinario");

        using var resposta = await cliente.GetAsync("/animais");

        await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
    }

    [Fact(DisplayName = "I092 — EP-015 a EP-018: sem token devolvem 401 e nada é alterado")]
    public async Task SemTokenDevolve401()
    {
        var animalId = await ObterAnimalIdAsync("Bobi");
        var clienteId = await ObterClienteIdAsync(EmailJoao);
        var antes = await LerAnimalAsync(animalId);
        var totalAntes = await ContarAnimaisAsync();

        using var cliente = Fixture.Factory.CreateClient();

        using var pesquisar = await cliente.GetAsync("/animais");
        using var obter = await cliente.GetAsync($"/animais/{animalId}");
        using var criar = await cliente.PostAsJsonAsync("/animais", Pedido(clienteId, "Sem Token"));
        using var alterar = await cliente.PutAsJsonAsync($"/animais/{animalId}", Pedido(clienteId, "Sem Token"));

        await AssertErroAsync(pesquisar, HttpStatusCode.Unauthorized);
        await AssertErroAsync(obter, HttpStatusCode.Unauthorized);
        await AssertErroAsync(criar, HttpStatusCode.Unauthorized);
        await AssertErroAsync(alterar, HttpStatusCode.Unauthorized);
        Assert.Equal(totalAntes, await ContarAnimaisAsync());
        Assert.Equal(antes, await LerAnimalAsync(animalId));
    }

    // ----- Auxiliares -----

    private static Dictionary<string, object?> Pedido(int clienteId, string nome) => new()
    {
        ["nome"] = nome,
        ["especie"] = "Cão",
        ["raca"] = "Labrador Retriever",
        ["sexo"] = "Macho",
        ["dataNascimento"] = "2022-05-10",
        ["peso"] = 25.40m,
        ["clienteId"] = clienteId
    };

    private static string DataFutura() =>
        DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    private static void AssertSemDadosClinicos(JsonElement raiz)
    {
        foreach (var campo in new[] { "historicoClinico", "registosClinicos", "diagnostico", "notasClinicas", "tratamentos", "prescricoes" })
        {
            Assert.False(raiz.TryGetProperty(campo, out _), $"A ficha administrativa não devia ter o campo «{campo}».");
        }
    }

    private static async Task<IReadOnlyList<ItemAnimal>> PesquisarAsync(HttpClient cliente, string url)
    {
        using var resposta = await cliente.GetAsync(url);

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());

        return documento.RootElement.GetProperty("itens").EnumerateArray()
            .Select(i => new ItemAnimal(
                i.GetProperty("nome").GetString(),
                i.GetProperty("especie").GetString(),
                i.GetProperty("cliente").GetProperty("id").GetInt32()))
            .ToList();
    }

    /// <summary>Formato único de erro da infraestrutura (I058): <c>code</c> igual ao código HTTP.</summary>
    private static async Task AssertErroAsync(HttpResponseMessage resposta, HttpStatusCode esperado)
    {
        Assert.Equal(esperado, resposta.StatusCode);

        var corpo = await resposta.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(corpo), $"A resposta {(int)esperado} não tem corpo.");

        using var documento = JsonDocument.Parse(corpo);
        Assert.Equal((int)esperado, documento.RootElement.GetProperty("code").GetInt32());
    }

    private async Task CriarAnimaisDiretamenteAsync(int quantidade)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        var clienteId = await ObterClienteIdAsync(EmailJoao);
        var especieId = await db.Especies.AsNoTracking().Where(e => e.Nome == "Cão").Select(e => e.Id).SingleAsync();

        for (var i = 0; i < quantidade; i++)
        {
            db.Pacientes.Add(new Paciente
            {
                ClienteId = clienteId,
                Nome = string.Create(CultureInfo.InvariantCulture, $"Paginacao {i:D3}"),
                EspecieId = especieId,
                Sexo = "Desconhecido",
                DataRegisto = DateTime.UtcNow
            });
        }

        await db.SaveChangesAsync();
    }

    private async Task<int> ContarAnimaisAsync()
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Pacientes.AsNoTracking().CountAsync();
    }

    private async Task<AnimalGuardado> LerAnimalAsync(int id)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Pacientes.AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new AnimalGuardado(p.Nome, p.ClienteId, p.EspecieId, p.RacaId, p.Sexo, p.DataNascimento, p.Peso, p.DataUltimaAlteracao))
            .SingleAsync();
    }

    private async Task<int> ObterAnimalIdAsync(string nome)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Pacientes.AsNoTracking().Where(p => p.Nome == nome).Select(p => p.Id).SingleAsync();
    }

    private async Task<int> ObterAnimalIdInexistenteAsync()
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return (await db.Pacientes.AsNoTracking().MaxAsync(p => (int?)p.Id) ?? 0) + 1000;
    }

    private async Task<int> ObterClienteIdAsync(string email)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await (
                from c in db.Clientes.AsNoTracking()
                join u in db.Utilizadores.AsNoTracking() on c.UtilizadorId equals u.Id
                where u.Email == email
                select c.Id)
            .SingleAsync();
    }

    private async Task<int> ObterClienteIdInexistenteAsync()
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return (await db.Clientes.AsNoTracking().MaxAsync(c => (int?)c.Id) ?? 0) + 1000;
    }

    private sealed record ItemAnimal(string? Nome, string? Especie, int ClienteId);

    private sealed record AnimalGuardado(
        string Nome,
        int ClienteId,
        int EspecieId,
        int? RacaId,
        string Sexo,
        DateOnly? DataNascimento,
        decimal? Peso,
        DateTime? DataUltimaAlteracao);
}
