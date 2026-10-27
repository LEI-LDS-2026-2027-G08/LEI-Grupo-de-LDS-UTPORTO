using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.Consultas;

/// <summary>
/// I072 — EP-023 e EP-024 com a API real e o SQL Server dos testes de integração (I091), sobre as
/// marcações de demonstração da I061 (horas em UTC):
/// M1 Bobi/vet.ana 2026-10-14 09:30 Realizada (com uma nota de preparação e o registo clínico),
/// M2 Mia/vet.bruno 2026-10-20 10:00 Cancelada, M3 Kiko/vet.ana 2026-11-03 09:00 Confirmada,
/// M4 Luna/vet.bruno 2026-11-04 14:30 Pendente e M5 Bobi/vet.ana 2026-11-05 11:00 Pendente.
/// Casos IT-EP023-01 a 05 e IT-EP024-01 a 05 da Especificação dos Testes de Integração (I030).
/// </summary>
public sealed class ConsultasEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailRececao = "rececao@clinica-g08.test";
    private const string EmailVetAna = "vet.ana@clinica-g08.test";
    private const string EmailVetBruno = "vet.bruno@clinica-g08.test";
    private const string EmailAuxSemApoio = "aux.diogo@clinica-g08.test";
    private const string Periodo = "dataInicio=2026-10-14&dataFim=2026-11-05";

    public ConsultasEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    // ----- EP-023 — Consultar agenda -----

    [Fact(DisplayName = "IT-EP023-01 — a Rececionista consulta a agenda do período, por data e hora")]
    public async Task RececionistaConsultaAgenda()
    {
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var resposta = await cliente.GetAsync($"/consultas?{Periodo}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        var raiz = documento.RootElement;
        Assert.Equal(5, raiz.GetProperty("total").GetInt32());
        Assert.Equal(1, raiz.GetProperty("pagina").GetInt32());
        Assert.Equal(20, raiz.GetProperty("tamanho").GetInt32());
        Assert.Equal(
            "2026-10-14T09:30:00Z|2026-10-20T10:00:00Z|2026-11-03T09:00:00Z|2026-11-04T14:30:00Z|2026-11-05T11:00:00Z",
            string.Join('|', Itens(raiz).Select(i => i.GetProperty("dataHora").GetString())));

        var primeira = Itens(raiz)[0];
        Assert.Equal("Realizada", primeira.GetProperty("estado").GetString());
        Assert.Equal("Bobi", primeira.GetProperty("animal").GetProperty("nome").GetString());
        Assert.Equal("João Costa (demonstração)", primeira.GetProperty("cliente").GetProperty("nome").GetString());
        Assert.Equal("Dra. Ana Veterinária (demonstração)", primeira.GetProperty("veterinario").GetProperty("nome").GetString());
        Assert.Equal("2026-10-14T09:22:00Z", primeira.GetProperty("chegadaEm").GetString());
    }

    [Theory(DisplayName = "IT-EP023-02 — intervalo de datas inválido, ou estado inválido, devolve 400")]
    [InlineData("dataInicio=2026-10-15&dataFim=2026-10-14")]
    [InlineData("dataInicio=2026-13-40")]
    [InlineData("estado=Faturada")]
    [InlineData("tamanho=101")]
    public async Task FiltroInvalidoDevolve400(string parametros)
    {
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var resposta = await cliente.GetAsync($"/consultas?{parametros}");

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "IT-EP023-03 — o Veterinário só vê as consultas que lhe estão atribuídas")]
    public async Task VeterinarioVeSoAsSuas()
    {
        var vetAna = await ObterUtilizadorIdAsync(EmailVetAna);
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.GetAsync($"/consultas?{Periodo}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        Assert.Equal(3, documento.RootElement.GetProperty("total").GetInt32());
        Assert.All(Itens(documento.RootElement), i =>
            Assert.Equal(vetAna, i.GetProperty("veterinario").GetProperty("id").GetInt32()));
    }

    [Fact(DisplayName = "IT-EP023-04 — o veterinarioId de outro Veterinário é ignorado")]
    public async Task VeterinarioIdDeOutroEIgnorado()
    {
        var vetAna = await ObterUtilizadorIdAsync(EmailVetAna);
        var vetBruno = await ObterUtilizadorIdAsync(EmailVetBruno);
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.GetAsync($"/consultas?{Periodo}&veterinarioId={vetBruno}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        Assert.Equal(3, documento.RootElement.GetProperty("total").GetInt32());
        Assert.All(Itens(documento.RootElement), i =>
            Assert.Equal(vetAna, i.GetProperty("veterinario").GetProperty("id").GetInt32()));
    }

    [Fact(DisplayName = "IT-EP023-05 — o Auxiliar Veterinário consulta a agenda (só leitura)")]
    public async Task AuxiliarConsultaAgenda()
    {
        using var cliente = await ClienteDeAsync(EmailAuxSemApoio);

        using var resposta = await cliente.GetAsync($"/consultas?{Periodo}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        Assert.Equal(5, documento.RootElement.GetProperty("total").GetInt32());
    }

    [Fact(DisplayName = "I072 — a Rececionista filtra a agenda por Veterinário e por estado")]
    public async Task RececionistaFiltraPorVeterinarioEEstado()
    {
        var vetBruno = await ObterUtilizadorIdAsync(EmailVetBruno);
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var resposta = await cliente.GetAsync($"/consultas?{Periodo}&veterinarioId={vetBruno}&estado=pendente");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        var unica = Assert.Single(Itens(documento.RootElement));
        Assert.Equal("Luna", unica.GetProperty("animal").GetProperty("nome").GetString());
        Assert.Equal("Pendente", unica.GetProperty("estado").GetString());
    }

    // ----- EP-024 — Consultar marcação -----

    [Fact(DisplayName = "IT-EP024-01 — a Rececionista lê a marcação com as notas de preparação")]
    public async Task RececionistaLeMarcacaoComNotas()
    {
        var m1 = await ObterMarcacaoAsync(EmailVetAna, new DateTime(2026, 10, 14, 9, 30, 0));
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var resposta = await cliente.GetAsync($"/consultas/{m1.Id}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        var raiz = documento.RootElement;
        Assert.Equal(m1.Id, raiz.GetProperty("id").GetInt32());
        Assert.Equal("Comichão no ouvido esquerdo", raiz.GetProperty("motivo").GetString());

        var nota = Assert.Single(raiz.GetProperty("notasPreparacao").EnumerateArray().ToList());
        Assert.Equal(
            "Sala 2 preparada; tutor refere que o animal coça o ouvido há uma semana.",
            nota.GetProperty("texto").GetString());
        Assert.Equal("Carla Auxiliar (com apoio)", nota.GetProperty("autor").GetString());
        Assert.Equal("2026-10-14T09:25:00Z", nota.GetProperty("criadaEm").GetString());
    }

    [Fact(DisplayName = "IT-EP024-02 — o Veterinário atribuído lê a marcação")]
    public async Task VeterinarioAtribuidoLeMarcacao()
    {
        var m1 = await ObterMarcacaoAsync(EmailVetAna, new DateTime(2026, 10, 14, 9, 30, 0));
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.GetAsync($"/consultas/{m1.Id}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
    }

    [Fact(DisplayName = "IT-EP024-03 — a marcação de outro Veterinário devolve 403")]
    public async Task MarcacaoDeOutroVeterinarioDevolve403()
    {
        var m1 = await ObterMarcacaoAsync(EmailVetAna, new DateTime(2026, 10, 14, 9, 30, 0));
        using var cliente = await ClienteDeAsync(EmailVetBruno);

        using var resposta = await cliente.GetAsync($"/consultas/{m1.Id}");

        await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
    }

    [Fact(DisplayName = "IT-EP024-04 — marcação inexistente devolve 404")]
    public async Task MarcacaoInexistenteDevolve404()
    {
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var resposta = await cliente.GetAsync("/consultas/999999");

        await AssertErroAsync(resposta, HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "IT-EP024-05 — a resposta expõe a versão atual da marcação no ETag")]
    public async Task RespostaComETag()
    {
        var m3 = await ObterMarcacaoAsync(EmailVetAna, new DateTime(2026, 11, 3, 9, 0, 0));
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var resposta = await cliente.GetAsync($"/consultas/{m3.Id}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        var etag = Assert.IsType<EntityTagHeaderValue>(resposta.Headers.ETag);
        Assert.False(etag.IsWeak);
        Assert.Equal("\"" + Convert.ToBase64String(m3.RowVersion) + "\"", etag.Tag);
    }

    [Fact(DisplayName = "I072 — critério 5: o Auxiliar recebe as notas de preparação e nenhum dado clínico")]
    public async Task AuxiliarSemDadosClinicos()
    {
        var m1 = await ObterMarcacaoAsync(EmailVetAna, new DateTime(2026, 10, 14, 9, 30, 0));
        using var cliente = await ClienteDeAsync(EmailAuxSemApoio);

        using var resposta = await cliente.GetAsync($"/consultas/{m1.Id}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        var corpo = await resposta.Content.ReadAsStringAsync();
        using var documento = JsonDocument.Parse(corpo);
        Assert.Single(documento.RootElement.GetProperty("notasPreparacao").EnumerateArray().ToList());

        // A M1 tem registo clínico, tratamento e prescrição na base (dados da I061): nada disso
        // pode aparecer na resposta.
        Assert.Equal(
            "animal,chegadaEm,cliente,dataHora,duracaoMinutos,estado,id,motivo,notasPreparacao,veterinario",
            string.Join(',', documento.RootElement.EnumerateObject().Select(p => p.Name).Order(StringComparer.Ordinal)));
        Assert.DoesNotContain("Otite", corpo, StringComparison.Ordinal);
        Assert.DoesNotContain("eritematoso", corpo, StringComparison.Ordinal);
        Assert.DoesNotContain("Gotas auriculares", corpo, StringComparison.Ordinal);
        Assert.DoesNotContain("Limpeza auricular", corpo, StringComparison.Ordinal);
    }

    // ----- 401 e 403 nos dois endpoints -----

    [Theory(DisplayName = "I072 — perfis sem acesso (Administrador e Cliente) recebem 403 nos dois endpoints")]
    [InlineData("Administrador")]
    [InlineData("Cliente")]
    public async Task PerfisSemAcessoRecebem403(string perfil)
    {
        var m1 = await ObterMarcacaoAsync(EmailVetAna, new DateTime(2026, 10, 14, 9, 30, 0));
        using var cliente = await Fixture.CriarClienteAutenticadoAsync(perfil);

        using var agenda = await cliente.GetAsync("/consultas");
        using var marcacao = await cliente.GetAsync($"/consultas/{m1.Id}");

        await AssertErroAsync(agenda, HttpStatusCode.Forbidden);
        await AssertErroAsync(marcacao, HttpStatusCode.Forbidden);
    }

    [Fact(DisplayName = "I072 — sem token, os dois endpoints devolvem 401")]
    public async Task SemTokenDevolve401()
    {
        var m1 = await ObterMarcacaoAsync(EmailVetAna, new DateTime(2026, 10, 14, 9, 30, 0));
        using var cliente = Fixture.Factory.CreateClient();

        using var agenda = await cliente.GetAsync("/consultas");
        using var marcacao = await cliente.GetAsync($"/consultas/{m1.Id}");

        await AssertErroAsync(agenda, HttpStatusCode.Unauthorized);
        await AssertErroAsync(marcacao, HttpStatusCode.Unauthorized);
    }

    // ----- Auxiliares -----

    private sealed record MarcacaoDemo(int Id, byte[] RowVersion);

    private static List<JsonElement> Itens(JsonElement raiz) =>
        raiz.GetProperty("itens").EnumerateArray().ToList();

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

    /// <summary>
    /// Cliente HTTP autenticado como a conta indicada. O <c>CriarClienteAutenticadoAsync</c> da I091
    /// escolhe uma conta qualquer do perfil; estes casos precisam de um Veterinário em concreto.
    /// </summary>
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

        return await db.Utilizadores
            .AsNoTracking()
            .Where(u => u.Email == email)
            .Select(u => u.Id)
            .SingleAsync();
    }

    /// <summary>Marcação de demonstração do Veterinário que começa no instante UTC indicado.</summary>
    private async Task<MarcacaoDemo> ObterMarcacaoAsync(string emailVeterinario, DateTime inicioUtc)
    {
        var veterinarioId = await ObterUtilizadorIdAsync(emailVeterinario);

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Marcacoes
            .AsNoTracking()
            .Where(m => m.VeterinarioId == veterinarioId && m.DataHoraInicio == inicioUtc)
            .Select(m => new MarcacaoDemo(m.Id, m.RowVersion))
            .SingleAsync();
    }
}
