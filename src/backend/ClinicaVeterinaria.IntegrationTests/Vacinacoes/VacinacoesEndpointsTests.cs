using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.Vacinacoes;

/// <summary>
/// I082 — EP-042 (GET /animais/{id}/vacinacoes), EP-043 (POST /animais/{id}/vacinacoes), a
/// atualização do RF056 (PUT /animais/{id}/vacinacoes/{administracaoId}) e EP-044
/// (GET /me/animais/{id}/vacinacoes) com a API real e o SQL Server dos testes de integração (I091),
/// sobre os dados de demonstração da I061: casos IT-EP042-01 a 05, IT-EP043-01 a 07 e IT-EP044-01 a 04
/// da Especificação de Testes de Integração (I030, secção 13), IT-TRV-15 (isolamento entre Clientes)
/// e a matriz de perfis (401 e 403).
/// </summary>
/// <remarks>
/// Dados da I061 (todos registados pela vet.ana): Bobi (João) com a vacina antirrábica de
/// 2025-10-14 (365 dias), a vacina polivalente de 2026-10-14 (365 dias, próxima 2027-10-14) e o
/// desparasitante interno de 2026-10-14 (90 dias, próxima 2027-01-12); Kiko (Maria) com o
/// desparasitante externo de 2026-08-01, sem intervalo; Mia (João) sem registos. Estes testes correm
/// com o relógio real, por isso as datas «futuras» são calculadas a partir de hoje e as próximas
/// administrações verificadas são as de 2027.
/// </remarks>
public sealed class VacinacoesEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailVetAna = "vet.ana@clinica-g08.test";
    private const string EmailVetBruno = "vet.bruno@clinica-g08.test";
    private const string EmailJoao = "cliente.joao@clinica-g08.test";
    private const string EmailMaria = "cliente.maria@clinica-g08.test";

    public VacinacoesEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    // ----- EP-042 — Consultar boletim do paciente (web) -----
    [Fact(DisplayName = "IT-EP042-01 — animal com administrações: 200 com o histórico (mais recente primeiro) e as próximas")]
    public async Task BoletimComHistoricoEProximas()
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.GetAsync($"/animais/{bobi}/vacinacoes");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        var raiz = documento.RootElement;
        Assert.Equal("Bobi", raiz.GetProperty("animal").GetProperty("nome").GetString());

        var administracoes = raiz.GetProperty("administracoes").EnumerateArray().ToList();
        Assert.Equal(3, administracoes.Count);
        Assert.Equal("2026-10-14", administracoes[0].GetProperty("dataAdministracao").GetString());
        Assert.Equal("2025-10-14", administracoes[2].GetProperty("dataAdministracao").GetString());
        Assert.Equal("Dra. Ana Veterinária (demonstração)", administracoes[0].GetProperty("veterinario").GetString());

        var desparasitante = administracoes.Single(a => a.GetProperty("lote").GetString() == "DP-0042");
        Assert.Equal("Desparasitacao", desparasitante.GetProperty("tipo").GetString());
        Assert.Equal(90, desparasitante.GetProperty("intervaloDias").GetInt32());
        Assert.Equal("2027-01-12", desparasitante.GetProperty("dataProximaPrevista").GetString());

        // As próximas que ainda não passaram, da mais próxima para a mais distante.
        var proximas = raiz.GetProperty("proximas").EnumerateArray()
            .Select(p => p.GetProperty("dataProximaPrevista").GetString())
            .ToList();
        Assert.Contains("2027-01-12", proximas);
        Assert.Contains("2027-10-14", proximas);
        Assert.Equal(proximas.Order(StringComparer.Ordinal), proximas);
    }

    [Theory(DisplayName = "IT-EP042-02 e IT-EP042-03 — tipo=Vacina e tipo=Desparasitacao filtram o boletim")]
    [InlineData("Vacina", 2)]
    [InlineData("Desparasitacao", 1)]
    public async Task BoletimFiltradoPorTipo(string tipo, int esperadas)
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.GetAsync($"/animais/{bobi}/vacinacoes?tipo={tipo}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        var administracoes = documento.RootElement.GetProperty("administracoes").EnumerateArray().ToList();
        Assert.Equal(esperadas, administracoes.Count);
        Assert.All(administracoes, a => Assert.Equal(tipo, a.GetProperty("tipo").GetString()));
        Assert.All(
            documento.RootElement.GetProperty("proximas").EnumerateArray(),
            p => Assert.Equal(tipo, p.GetProperty("tipo").GetString()));
    }

    [Fact(DisplayName = "IT-EP042-04 — tipo inválido devolve 400")]
    public async Task TipoInvalidoDevolve400()
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.GetAsync($"/animais/{bobi}/vacinacoes?tipo=Vacinas");

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "IT-EP042-05 — animal inexistente devolve 404")]
    public async Task BoletimDeAnimalInexistenteDevolve404()
    {
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.GetAsync("/animais/999999/vacinacoes");

        await AssertErroAsync(resposta, HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "I082 — animal sem registos (Mia): 200 com as duas listas vazias")]
    public async Task BoletimSemRegistos()
    {
        var mia = await ObterAnimalIdAsync("Mia");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.GetAsync($"/animais/{mia}/vacinacoes");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        Assert.Empty(documento.RootElement.GetProperty("administracoes").EnumerateArray());
        Assert.Empty(documento.RootElement.GetProperty("proximas").EnumerateArray());
    }

    // ----- EP-043 — Registar vacinação ou desparasitação -----
    [Fact(DisplayName = "IT-EP043-01 e IT-EP043-02 — dados válidos com 90 dias: 201, persistido e dataProximaPrevista = data + 90 (também na coluna calculada)")]
    public async Task RegistaComIntervalo()
    {
        var mia = await ObterAnimalIdAsync("Mia");
        var vetAna = await ObterUtilizadorIdAsync(EmailVetAna);
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PostAsJsonAsync(
            $"/animais/{mia}/vacinacoes",
            new { tipo = "Desparasitacao", produto = "Desparasitante interno Y", lote = "DP-0042", dataAdministracao = "2026-10-14", intervaloDias = 90 });

        Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        var raiz = documento.RootElement;
        var id = raiz.GetProperty("id").GetInt32();
        Assert.Equal($"/animais/{mia}/vacinacoes/{id}", resposta.Headers.Location!.OriginalString);
        Assert.Equal("Desparasitacao", raiz.GetProperty("tipo").GetString());
        Assert.Equal("2026-10-14", raiz.GetProperty("dataAdministracao").GetString());
        Assert.Equal(90, raiz.GetProperty("intervaloDias").GetInt32());
        Assert.Equal("2027-01-12", raiz.GetProperty("dataProximaPrevista").GetString());
        Assert.Equal("Dra. Ana Veterinária (demonstração)", raiz.GetProperty("veterinario").GetString());

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
        var gravado = await db.RegistosProfilaxias.AsNoTracking().SingleAsync(r => r.Id == id);
        Assert.Equal(mia, gravado.PacienteId);
        Assert.Equal(vetAna, gravado.VeterinarioId);
        Assert.Equal("DP-0042", gravado.Lote);
        Assert.Equal(new DateOnly(2027, 1, 12), gravado.DataProximaAdministracao);
    }

    [Fact(DisplayName = "IT-EP043-03 — sem intervalo: 201 com dataProximaPrevista = null")]
    public async Task RegistaSemIntervalo()
    {
        var mia = await ObterAnimalIdAsync("Mia");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PostAsJsonAsync(
            $"/animais/{mia}/vacinacoes",
            new { tipo = "Vacina", produto = "Vacina felina (fictícia)", lote = "VF-0001", dataAdministracao = "2026-10-20" });

        Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        Assert.Equal(JsonValueKind.Null, documento.RootElement.GetProperty("intervaloDias").ValueKind);
        Assert.Equal(JsonValueKind.Null, documento.RootElement.GetProperty("dataProximaPrevista").ValueKind);

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
        var gravado = await db.RegistosProfilaxias.AsNoTracking().SingleAsync(r => r.Id == documento.RootElement.GetProperty("id").GetInt32());
        Assert.Null(gravado.DataProximaAdministracao);
    }

    [Theory(DisplayName = "IT-EP043-04 — tipo, produto, lote ou data em falta (ou tipo inválido) devolvem 400")]
    [InlineData("""{ "produto": "Vacina X", "lote": "L1", "dataAdministracao": "2026-10-14" }""")]
    [InlineData("""{ "tipo": "Vacina", "lote": "L1", "dataAdministracao": "2026-10-14" }""")]
    [InlineData("""{ "tipo": "Vacina", "produto": "Vacina X", "dataAdministracao": "2026-10-14" }""")]
    [InlineData("""{ "tipo": "Vacina", "produto": "Vacina X", "lote": "L1" }""")]
    [InlineData("""{ "tipo": "Vacinas", "produto": "Vacina X", "lote": "L1", "dataAdministracao": "2026-10-14" }""")]
    public async Task CampoEmFaltaDevolve400(string corpo)
    {
        var mia = await ObterAnimalIdAsync("Mia");
        var antes = await ContarAsync();
        using var cliente = await ClienteDeAsync(EmailVetAna);
        using var conteudo = new StringContent(corpo, System.Text.Encoding.UTF8, "application/json");

        using var resposta = await cliente.PostAsync($"/animais/{mia}/vacinacoes", conteudo);

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
        Assert.Equal(antes, await ContarAsync());
    }

    [Fact(DisplayName = "IT-EP043-05 — data de administração futura devolve 400")]
    public async Task DataFuturaDevolve400()
    {
        var mia = await ObterAnimalIdAsync("Mia");
        var antes = await ContarAsync();
        var depoisDeAmanha = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(2).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PostAsJsonAsync(
            $"/animais/{mia}/vacinacoes",
            new { tipo = "Vacina", produto = "Vacina X", lote = "L1", dataAdministracao = depoisDeAmanha, intervaloDias = 365 });

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
        Assert.Equal(antes, await ContarAsync());
    }

    [Theory(DisplayName = "IT-EP043-06 — intervaloDias ≤ 0 devolve 400")]
    [InlineData(0)]
    [InlineData(-30)]
    public async Task IntervaloNaoPositivoDevolve400(int intervaloDias)
    {
        var mia = await ObterAnimalIdAsync("Mia");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PostAsJsonAsync(
            $"/animais/{mia}/vacinacoes",
            new { tipo = "Vacina", produto = "Vacina X", lote = "L1", dataAdministracao = "2026-10-14", intervaloDias });

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "IT-EP043-07 — animal inexistente devolve 404")]
    public async Task RegistoEmAnimalInexistenteDevolve404()
    {
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PostAsJsonAsync(
            "/animais/999999/vacinacoes",
            new { tipo = "Vacina", produto = "Vacina X", lote = "L1", dataAdministracao = "2026-10-14", intervaloDias = 365 });

        await AssertErroAsync(resposta, HttpStatusCode.NotFound);
    }

    // ----- RF056 — Atualizar vacinação ou desparasitação (I082, critério 4) -----
    [Fact(DisplayName = "I082 — atualizar a data e o intervalo: 200 e a próxima é recalculada (resposta e coluna calculada)")]
    public async Task AtualizaERecalcula()
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        var id = await ObterAdministracaoIdAsync(bobi, "DP-0042");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PutAsJsonAsync(
            $"/animais/{bobi}/vacinacoes/{id}",
            new { tipo = "Desparasitacao", produto = "Desparasitante interno (fictício)", lote = "DP-0042", dataAdministracao = "2026-10-20", intervaloDias = 30 });

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        Assert.Equal(id, documento.RootElement.GetProperty("id").GetInt32());
        Assert.Equal("2026-10-20", documento.RootElement.GetProperty("dataAdministracao").GetString());
        Assert.Equal("2026-11-19", documento.RootElement.GetProperty("dataProximaPrevista").GetString());

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
        var gravado = await db.RegistosProfilaxias.AsNoTracking().SingleAsync(r => r.Id == id);
        Assert.Equal(new DateOnly(2026, 10, 20), gravado.DataAdministracao);
        Assert.Equal(new DateOnly(2026, 11, 19), gravado.DataProximaAdministracao);
        Assert.Equal(bobi, gravado.PacienteId);
    }

    [Fact(DisplayName = "I082 — retirar o intervalo na atualização deixa a próxima nula")]
    public async Task AtualizaSemIntervalo()
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        var id = await ObterAdministracaoIdAsync(bobi, "PV-1187");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PutAsJsonAsync(
            $"/animais/{bobi}/vacinacoes/{id}",
            new { tipo = "Vacina", produto = "Vacina polivalente (fictícia)", lote = "PV-1187", dataAdministracao = "2026-10-14" });

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        Assert.Equal(JsonValueKind.Null, documento.RootElement.GetProperty("dataProximaPrevista").ValueKind);

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
        Assert.Null((await db.RegistosProfilaxias.AsNoTracking().SingleAsync(r => r.Id == id)).DataProximaAdministracao);
    }

    [Fact(DisplayName = "I082 — atualização com dados inválidos devolve 400 e o registo fica como estava")]
    public async Task AtualizacaoInvalidaDevolve400()
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        var id = await ObterAdministracaoIdAsync(bobi, "DP-0042");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PutAsJsonAsync(
            $"/animais/{bobi}/vacinacoes/{id}",
            new { tipo = "Desparasitacao", produto = "Desparasitante interno (fictício)", lote = "DP-0042", dataAdministracao = "2026-10-14", intervaloDias = 0 });

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
        Assert.Equal((short)90, (await db.RegistosProfilaxias.AsNoTracking().SingleAsync(r => r.Id == id)).IntervaloDias);
    }

    [Fact(DisplayName = "I082 — atualizar um registo de outro animal (Kiko pelo Bobi), ou inexistente, devolve 404")]
    public async Task AtualizarRegistoDeOutroAnimalDevolve404()
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        var kiko = await ObterAnimalIdAsync("Kiko");
        var doKiko = await ObterAdministracaoIdAsync(kiko, "DE-0310");
        using var cliente = await ClienteDeAsync(EmailVetAna);
        var corpo = new { tipo = "Vacina", produto = "Vacina X", lote = "L1", dataAdministracao = "2026-10-14" };

        using var deOutroAnimal = await cliente.PutAsJsonAsync($"/animais/{bobi}/vacinacoes/{doKiko}", corpo);
        using var inexistente = await cliente.PutAsJsonAsync($"/animais/{bobi}/vacinacoes/999999", corpo);

        await AssertErroAsync(deOutroAnimal, HttpStatusCode.NotFound);
        await AssertErroAsync(inexistente, HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "I082 — atualizar um registo feito por outro Veterinário (pedido do vet.bruno) devolve 403")]
    public async Task AtualizarRegistoDeOutroVeterinarioDevolve403()
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        var id = await ObterAdministracaoIdAsync(bobi, "DP-0042");
        using var cliente = await ClienteDeAsync(EmailVetBruno);

        using var resposta = await cliente.PutAsJsonAsync(
            $"/animais/{bobi}/vacinacoes/{id}",
            new { tipo = "Desparasitacao", produto = "Outro", lote = "DP-0042", dataAdministracao = "2026-10-14", intervaloDias = 90 });

        await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
    }

    // ----- EP-044 — Consultar boletim do meu animal (Cliente) -----
    [Fact(DisplayName = "IT-EP044-01 — o João consulta o Bobi: 200 com o histórico e as próximas")]
    public async Task ClienteConsultaOSeuAnimal()
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        using var cliente = await ClienteDeAsync(EmailJoao);

        using var resposta = await cliente.GetAsync($"/me/animais/{bobi}/vacinacoes");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        var historico = documento.RootElement.GetProperty("historico").EnumerateArray().ToList();
        Assert.Equal(3, historico.Count);
        Assert.Equal("2026-10-14", historico[0].GetProperty("dataAdministracao").GetString());

        // O Cliente vê só tipo, produto e data (MeuBoletimDto): sem lote nem Veterinário.
        Assert.False(historico[0].TryGetProperty("lote", out _));
        Assert.False(historico[0].TryGetProperty("veterinario", out _));

        var proximas = documento.RootElement.GetProperty("proximas").EnumerateArray()
            .Select(p => p.GetProperty("dataProximaPrevista").GetString())
            .ToList();
        Assert.Contains("2027-01-12", proximas);
        Assert.Contains("2027-10-14", proximas);
    }

    [Fact(DisplayName = "IT-EP044-01 — a Maria consulta o Kiko: só o histórico do Kiko, sem próximas")]
    public async Task OutroClienteConsultaOSeuAnimal()
    {
        var kiko = await ObterAnimalIdAsync("Kiko");
        using var cliente = await ClienteDeAsync(EmailMaria);

        using var resposta = await cliente.GetAsync($"/me/animais/{kiko}/vacinacoes");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        var historico = Assert.Single(documento.RootElement.GetProperty("historico").EnumerateArray().ToList());
        Assert.Equal("2026-08-01", historico.GetProperty("dataAdministracao").GetString());
        Assert.Empty(documento.RootElement.GetProperty("proximas").EnumerateArray());
    }

    [Fact(DisplayName = "IT-EP044-02 — animal próprio sem registos (Mia, do João): 200 com as listas vazias")]
    public async Task ClienteConsultaAnimalSemRegistos()
    {
        var mia = await ObterAnimalIdAsync("Mia");
        using var cliente = await ClienteDeAsync(EmailJoao);

        using var resposta = await cliente.GetAsync($"/me/animais/{mia}/vacinacoes");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        Assert.Empty(documento.RootElement.GetProperty("historico").EnumerateArray());
        Assert.Empty(documento.RootElement.GetProperty("proximas").EnumerateArray());
    }

    [Fact(DisplayName = "IT-EP044-03 e IT-TRV-15 — o João pede o boletim do Kiko (da Maria): 403, sem nenhum dado do Kiko")]
    public async Task AnimalDeOutroClienteDevolve403()
    {
        var kiko = await ObterAnimalIdAsync("Kiko");
        using var cliente = await ClienteDeAsync(EmailJoao);

        using var resposta = await cliente.GetAsync($"/me/animais/{kiko}/vacinacoes");

        await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
        var corpo = await resposta.Content.ReadAsStringAsync();
        Assert.DoesNotContain("DE-0310", corpo, StringComparison.Ordinal);
        Assert.DoesNotContain("Desparasitante externo", corpo, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "IT-EP044-04 — animal inexistente devolve 404")]
    public async Task ClienteAnimalInexistenteDevolve404()
    {
        using var cliente = await ClienteDeAsync(EmailJoao);

        using var resposta = await cliente.GetAsync("/me/animais/999999/vacinacoes");

        await AssertErroAsync(resposta, HttpStatusCode.NotFound);
    }

    // ----- Perfis sem acesso e pedidos sem token (I082, critério 6) -----
    [Theory(DisplayName = "I082 — só o Veterinário: Rececionista, Auxiliar, Administrador e Cliente recebem 403 em EP-042, EP-043 e na atualização")]
    [InlineData("rececao@clinica-g08.test")]
    [InlineData("aux.carla@clinica-g08.test")]
    [InlineData("admin@clinica-g08.test")]
    [InlineData(EmailJoao)]
    public async Task OutrosPerfisRecebem403NoWeb(string email)
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        var id = await ObterAdministracaoIdAsync(bobi, "DP-0042");
        var antes = await ContarAsync();
        using var cliente = await ClienteDeAsync(email);
        var corpo = new { tipo = "Vacina", produto = "Vacina X", lote = "L1", dataAdministracao = "2026-10-14", intervaloDias = 365 };

        using var boletim = await cliente.GetAsync($"/animais/{bobi}/vacinacoes");
        using var registar = await cliente.PostAsJsonAsync($"/animais/{bobi}/vacinacoes", corpo);
        using var atualizar = await cliente.PutAsJsonAsync($"/animais/{bobi}/vacinacoes/{id}", corpo);

        await AssertErroAsync(boletim, HttpStatusCode.Forbidden);
        await AssertErroAsync(registar, HttpStatusCode.Forbidden);
        await AssertErroAsync(atualizar, HttpStatusCode.Forbidden);
        Assert.Equal(antes, await ContarAsync());
    }

    [Fact(DisplayName = "I082 — EP-044 é só do Cliente: o Veterinário recebe 403")]
    public async Task VeterinarioRecebe403NoMobile()
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.GetAsync($"/me/animais/{bobi}/vacinacoes");

        await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
    }

    [Fact(DisplayName = "I082 — EP-042, EP-043, atualização e EP-044 sem token devolvem 401")]
    public async Task SemTokenDevolve401()
    {
        using var cliente = Fixture.Factory.CreateClient();
        var corpo = new { tipo = "Vacina", produto = "Vacina X", lote = "L1", dataAdministracao = "2026-10-14" };

        using var boletim = await cliente.GetAsync("/animais/1/vacinacoes");
        using var registar = await cliente.PostAsJsonAsync("/animais/1/vacinacoes", corpo);
        using var atualizar = await cliente.PutAsJsonAsync("/animais/1/vacinacoes/1", corpo);
        using var meu = await cliente.GetAsync("/me/animais/1/vacinacoes");

        await AssertErroAsync(boletim, HttpStatusCode.Unauthorized);
        await AssertErroAsync(registar, HttpStatusCode.Unauthorized);
        await AssertErroAsync(atualizar, HttpStatusCode.Unauthorized);
        await AssertErroAsync(meu, HttpStatusCode.Unauthorized);
    }

    // ----- Auxiliares -----
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

        var utilizador = await authService.ObterAutenticadoAsync(await ObterUtilizadorIdAsync(email));
        var token = jwtTokenService.GerarToken(utilizador);

        var cliente = Fixture.Factory.CreateClient();
        cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        return cliente;
    }

    private async Task<int> ObterUtilizadorIdAsync(string email)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Utilizadores.AsNoTracking().Where(u => u.Email == email).Select(u => u.Id).SingleAsync();
    }

    private async Task<int> ObterAnimalIdAsync(string nome)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Pacientes.AsNoTracking().Where(p => p.Nome == nome).Select(p => p.Id).SingleAsync();
    }

    private async Task<int> ObterAdministracaoIdAsync(int animalId, string lote)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.RegistosProfilaxias.AsNoTracking()
            .Where(r => r.PacienteId == animalId && r.Lote == lote)
            .Select(r => r.Id)
            .SingleAsync();
    }

    private async Task<int> ContarAsync()
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.RegistosProfilaxias.CountAsync();
    }
}
