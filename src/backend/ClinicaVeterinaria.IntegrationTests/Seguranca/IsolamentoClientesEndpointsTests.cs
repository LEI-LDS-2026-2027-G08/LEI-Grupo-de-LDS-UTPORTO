using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.Seguranca;

/// <summary>
/// I096 — isolamento entre Clientes nas rotas <c>/me/*</c> (I030, secção 14.2, IT-TRV-11 a 16;
/// critério 4): cada caso corre com os dois Clientes de demonstração, nos dois sentidos, e nenhuma
/// resposta inclui dados do outro Cliente.
/// </summary>
/// <remarks>
/// Dados da I061: o João tem o Bobi e a Mia; a Maria tem o Kiko. Os animais e os Clientes são
/// lidos da base pelo e-mail, para os testes não dependerem dos identificadores. A base é reposta
/// antes de cada teste.
/// </remarks>
public sealed class IsolamentoClientesEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailJoao = "cliente.joao@clinica-g08.test";
    private const string EmailMaria = "cliente.maria@clinica-g08.test";

    public IsolamentoClientesEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    public static TheoryData<string, string> ClienteEOutro => new()
    {
        { EmailJoao, EmailMaria },
        { EmailMaria, EmailJoao },
    };

    [Theory(DisplayName = "IT-TRV-11 — /me/animais devolve exatamente os animais do Cliente autenticado e nenhum do outro")]
    [MemberData(nameof(ClienteEOutro))]
    public async Task MeusAnimaisSoDoCliente(string email, string emailOutro)
    {
        var meus = await AnimaisDeAsync(email);
        var doOutro = await AnimaisDeAsync(emailOutro);
        using var cliente = await ClienteDeAsync(email);

        using var resposta = await cliente.GetAsync("/me/animais");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        var ids = documento.RootElement.EnumerateArray().Select(a => a.GetProperty("id").GetInt32()).Order().ToList();
        Assert.Equal(meus.Keys.Order().ToList(), ids);
        Assert.Empty(ids.Intersect(doOutro.Keys));
    }

    [Theory(DisplayName = "IT-TRV-12 — o Cliente pede cada animal do outro Cliente em /me/animais/{id}: 403, sem dados do animal")]
    [MemberData(nameof(ClienteEOutro))]
    public async Task AnimalDoOutroDevolve403(string email, string emailOutro)
    {
        var doOutro = await AnimaisDeAsync(emailOutro);
        Assert.NotEmpty(doOutro);
        using var cliente = await ClienteDeAsync(email);

        foreach (var (id, nome) in doOutro)
        {
            using var resposta = await cliente.GetAsync($"/me/animais/{id}");

            await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
            Assert.DoesNotContain(nome, await resposta.Content.ReadAsStringAsync(), StringComparison.Ordinal);
        }
    }

    [Theory(DisplayName = "IT-TRV-13 — /me/consultas (futuras e histórico) só tem marcações dos animais do Cliente")]
    [MemberData(nameof(ClienteEOutro))]
    public async Task MinhasConsultasSoDosMeusAnimais(string email, string emailOutro)
    {
        var meus = await AnimaisDeAsync(email);
        var doOutro = await AnimaisDeAsync(emailOutro);
        using var cliente = await ClienteDeAsync(email);
        var animaisVistos = new List<int>();

        foreach (var tipo in new[] { "futuras", "historico" })
        {
            using var resposta = await cliente.GetAsync($"/me/consultas?tipo={tipo}");

            Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
            using var documento = await LerAsync(resposta);
            animaisVistos.AddRange(documento.RootElement.EnumerateArray().Select(c => c.GetProperty("animal").GetProperty("id").GetInt32()));
        }

        // Os dois Clientes têm marcações nos dados da I061: a verificação não é feita sobre listas vazias.
        Assert.NotEmpty(animaisVistos);
        Assert.All(animaisVistos, id => Assert.Contains(id, meus.Keys));
        Assert.Empty(animaisVistos.Intersect(doOutro.Keys));
    }

    [Theory(DisplayName = "IT-TRV-14 — o Cliente filtra /me/consultas por cada animal do outro Cliente: 403, sem marcações")]
    [MemberData(nameof(ClienteEOutro))]
    public async Task FiltroPorAnimalDoOutroDevolve403(string email, string emailOutro)
    {
        var doOutro = await AnimaisDeAsync(emailOutro);
        using var cliente = await ClienteDeAsync(email);

        foreach (var (id, nome) in doOutro)
        {
            using var resposta = await cliente.GetAsync($"/me/consultas?animalId={id}");

            await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
            Assert.DoesNotContain(nome, await resposta.Content.ReadAsStringAsync(), StringComparison.Ordinal);
        }
    }

    [Theory(DisplayName = "IT-TRV-15 — o Cliente pede o boletim de cada animal do outro Cliente: 403, sem dados do boletim (nos dois sentidos)")]
    [MemberData(nameof(ClienteEOutro))]
    public async Task BoletimDoOutroDevolve403(string email, string emailOutro)
    {
        var doOutro = await AnimaisDeAsync(emailOutro);
        using var cliente = await ClienteDeAsync(email);

        foreach (var (id, nome) in doOutro)
        {
            using var resposta = await cliente.GetAsync($"/me/animais/{id}/vacinacoes");

            await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
            var corpo = await resposta.Content.ReadAsStringAsync();
            Assert.DoesNotContain(nome, corpo, StringComparison.Ordinal);
            Assert.DoesNotContain("historico", corpo, StringComparison.Ordinal);
        }
    }

    [Theory(DisplayName = "IT-TRV-16 — identificadores mudados à mão no URL ou na query (animal, clienteId, 0, negativos, máximo) nunca devolvem dados do outro Cliente")]
    [MemberData(nameof(ClienteEOutro))]
    public async Task IdentificadoresManipuladosNuncaDevolvemDadosDoOutro(string email, string emailOutro)
    {
        var meus = await AnimaisDeAsync(email);
        var doOutro = await AnimaisDeAsync(emailOutro);
        var outroClienteId = await ClienteIdDeAsync(emailOutro);
        using var cliente = await ClienteDeAsync(email);

        var ids = doOutro.Keys.Select(id => id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .Concat(["0", "-1", int.MaxValue.ToString(System.Globalization.CultureInfo.InvariantCulture)])
            .ToList();

        var urls = new List<string>
        {
            $"/me/animais?clienteId={outroClienteId}",
            $"/me/consultas?clienteId={outroClienteId}",
            $"/me/consultas?tipo=historico&clienteId={outroClienteId}",
        };
        foreach (var id in ids)
        {
            urls.Add($"/me/animais/{id}");
            urls.Add($"/me/animais/{id}/vacinacoes");
            urls.Add($"/me/consultas?animalId={id}");
            urls.Add($"/me/animais/{id}?clienteId={outroClienteId}");
        }

        var falhas = new List<string>();
        foreach (var url in urls)
        {
            using var resposta = await cliente.GetAsync(url);
            var corpo = await resposta.Content.ReadAsStringAsync();

            if (resposta.IsSuccessStatusCode)
            {
                // Uma resposta 200 só pode conter animais do próprio Cliente.
                var animais = AnimaisNaResposta(corpo);
                if (animais.Any(a => !meus.ContainsKey(a)))
                {
                    falhas.Add($"{url}: 200 com animais que não são do Cliente ({string.Join(", ", animais)})");
                }
            }
            else if (resposta.StatusCode is not (HttpStatusCode.Forbidden or HttpStatusCode.NotFound or HttpStatusCode.BadRequest))
            {
                falhas.Add($"{url}: código inesperado {(int)resposta.StatusCode}");
            }

            foreach (var nome in doOutro.Values)
            {
                if (corpo.Contains($"\"{nome}\"", StringComparison.Ordinal))
                {
                    falhas.Add($"{url}: a resposta contém o animal «{nome}» do outro Cliente");
                }
            }
        }

        Assert.True(falhas.Count == 0, string.Join("; ", falhas));
    }

    // ----- Auxiliares -----

    /// <summary>Identificadores de animais numa resposta de /me/*: listas e objetos de animal ou de marcação.</summary>
    private static List<int> AnimaisNaResposta(string corpo)
    {
        using var documento = JsonDocument.Parse(corpo);
        var raiz = documento.RootElement;
        var itens = raiz.ValueKind == JsonValueKind.Array ? raiz.EnumerateArray().ToList() : [raiz];

        return itens
            .Select(i => i.TryGetProperty("animal", out var animal) ? animal : i)
            .Where(a => a.ValueKind == JsonValueKind.Object && a.TryGetProperty("id", out _) && a.TryGetProperty("nome", out _))
            .Select(a => a.GetProperty("id").GetInt32())
            .ToList();
    }

    private static async Task<JsonDocument> LerAsync(HttpResponseMessage resposta) =>
        JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());

    /// <summary>O código HTTP esperado, no formato de erro da I058 (<c>code</c>, <c>message</c>, <c>detail</c>).</summary>
    private static async Task AssertErroAsync(HttpResponseMessage resposta, HttpStatusCode esperado)
    {
        Assert.Equal(esperado, resposta.StatusCode);

        using var documento = await LerAsync(resposta);
        Assert.Equal((int)esperado, documento.RootElement.GetProperty("code").GetInt32());
        Assert.False(string.IsNullOrWhiteSpace(documento.RootElement.GetProperty("message").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(documento.RootElement.GetProperty("detail").GetString()));
    }

    private async Task<HttpClient> ClienteDeAsync(string email)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var jwtTokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        var utilizadorId = await db.Utilizadores.AsNoTracking().Where(u => u.Email == email).Select(u => u.Id).SingleAsync();
        var token = jwtTokenService.GerarToken(await authService.ObterAutenticadoAsync(utilizadorId));

        var cliente = Fixture.Factory.CreateClient();
        cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        return cliente;
    }

    private async Task<int> ClienteIdDeAsync(string email)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await (
            from c in db.Clientes.AsNoTracking()
            join u in db.Utilizadores.AsNoTracking() on c.UtilizadorId equals u.Id
            where u.Email == email
            select c.Id).SingleAsync();
    }

    /// <summary>Animais do Cliente com o e-mail dado: identificador → nome.</summary>
    private async Task<Dictionary<int, string>> AnimaisDeAsync(string email)
    {
        var clienteId = await ClienteIdDeAsync(email);

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Pacientes.AsNoTracking()
            .Where(p => p.ClienteId == clienteId)
            .ToDictionaryAsync(p => p.Id, p => p.Nome);
    }
}
