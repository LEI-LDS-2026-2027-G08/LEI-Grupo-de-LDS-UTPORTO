using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.Consultas;

/// <summary>
/// I075 — EP-028 (PATCH /consultas/{id}/chegada) e EP-029 (POST /consultas/{id}/notas-preparacao)
/// com API real e SQL Server. Casos IT-EP028-01 a 05 e IT-EP029-01 a 06.
/// Dados de demonstração: M3 Confirmada (vet.ana), M4 Pendente (vet.bruno), M2 Cancelada (vet.bruno),
/// M1 Realizada com registo clínico (vet.ana).
/// </summary>
public sealed class ChegadaENotasPreparacaoEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailRececao = "rececao@clinica-g08.test";
    private const string EmailVetAna = "vet.ana@clinica-g08.test";
    private const string EmailVetBruno = "vet.bruno@clinica-g08.test";
    private const string EmailAuxComApoio = "aux.carla@clinica-g08.test";
    private const string EmailAuxSemApoio = "aux.diogo@clinica-g08.test";

    public ChegadaENotasPreparacaoEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    // ----- EP-028 -----

    [Fact(DisplayName = "IT-EP028-01 — marcação Confirmada sem chegada devolve 200 com chegadaEm")]
    public async Task ChegadaValidaDevolve200()
    {
        var id = await ObterMarcacaoAsync(EmailVetAna, "Confirmada");
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var resposta = await EnviarChegadaAsync(cliente, id);

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        Assert.NotNull(resposta.Headers.ETag);

        using var documento = await LerAsync(resposta);
        Assert.Equal(id, documento.RootElement.GetProperty("id").GetInt32());
        Assert.Equal(JsonValueKind.String, documento.RootElement.GetProperty("chegadaEm").ValueKind);
    }

    [Fact(DisplayName = "IT-EP028-02 — marcação inexistente devolve 404")]
    public async Task ChegadaMarcacaoInexistenteDevolve404()
    {
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var resposta = await EnviarChegadaAsync(cliente, 999999);

        await AssertErroAsync(resposta, HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "IT-EP028-03 — If-Match desatualizado devolve 412")]
    public async Task ChegadaIfMatchDesatualizadoDevolve412()
    {
        var id = await ObterMarcacaoAsync(EmailVetAna, "Confirmada");
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var leitura = await cliente.GetAsync($"/consultas/{id}");
        var etagAntigo = leitura.Headers.ETag!.Tag;

        using var primeira = await EnviarChegadaAsync(cliente, id);
        Assert.Equal(HttpStatusCode.OK, primeira.StatusCode);

        using var segunda = await EnviarChegadaAsync(cliente, id, etagAntigo);

        await AssertErroAsync(segunda, HttpStatusCode.PreconditionFailed);
    }

    [Fact(DisplayName = "IT-EP028-04 — marcação não Confirmada devolve 422")]
    public async Task ChegadaMarcacaoPendenteDevolve422()
    {
        var id = await ObterMarcacaoAsync(EmailVetBruno, "Pendente");
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var resposta = await EnviarChegadaAsync(cliente, id);

        await AssertErroAsync(resposta, HttpStatusCode.UnprocessableEntity);
    }

    [Fact(DisplayName = "IT-EP028-05 — chegada repetida devolve 422 e mantém a hora original")]
    public async Task ChegadaRepetidaDevolve422()
    {
        var id = await ObterMarcacaoAsync(EmailVetAna, "Confirmada");
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var primeira = await EnviarChegadaAsync(cliente, id);
        using var documento = await LerAsync(primeira);
        var horaOriginal = documento.RootElement.GetProperty("chegadaEm").GetDateTime();

        using var segunda = await EnviarChegadaAsync(cliente, id);

        await AssertErroAsync(segunda, HttpStatusCode.UnprocessableEntity);
        Assert.Equal(horaOriginal, await ObterChegadaAsync(id));
    }

    [Fact(DisplayName = "I075 — EP-028 sem token devolve 401")]
    public async Task ChegadaSemTokenDevolve401()
    {
        using var cliente = Fixture.Factory.CreateClient();

        using var resposta = await EnviarChegadaAsync(cliente, 999999);

        await AssertErroAsync(resposta, HttpStatusCode.Unauthorized);
    }

    [Theory(DisplayName = "I075 — EP-028 só pode ser usado pela Rececionista")]
    [InlineData("admin@clinica-g08.test")]
    [InlineData(EmailVetAna)]
    [InlineData(EmailAuxComApoio)]
    [InlineData("cliente.joao@clinica-g08.test")]
    public async Task ChegadaOutrosPerfisRecebem403(string email)
    {
        var id = await ObterMarcacaoAsync(EmailVetAna, "Confirmada");
        using var cliente = await ClienteDeAsync(email);

        using var resposta = await EnviarChegadaAsync(cliente, id);

        await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
    }

    // ----- EP-029 -----

    [Theory(DisplayName = "IT-EP029-01 — Auxiliar autorizado cria a nota (texto normal e com 1000 caracteres)")]
    [InlineData(17)]
    [InlineData(1000)]
    public async Task NotaValidaDevolve201(int comprimento)
    {
        var id = await ObterMarcacaoAsync(EmailVetAna, "Confirmada");
        var texto = new string('a', comprimento);
        using var cliente = await ClienteDeAsync(EmailAuxComApoio);

        using var resposta = await EnviarNotaAsync(cliente, id, texto);

        Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);

        using var documento = await LerAsync(resposta);
        Assert.Equal(id, documento.RootElement.GetProperty("consultaId").GetInt32());
        Assert.Equal(texto, documento.RootElement.GetProperty("texto").GetString());

        // A nota fica persistida e aparece em EP-024.
        using var leitura = await cliente.GetAsync($"/consultas/{id}");
        using var consulta = await LerAsync(leitura);
        Assert.Contains(
            consulta.RootElement.GetProperty("notasPreparacao").EnumerateArray(),
            n => n.GetProperty("texto").GetString() == texto);
    }

    [Theory(DisplayName = "IT-EP029-02 e 03 — texto vazio ou com mais de 1000 caracteres devolve 400")]
    [InlineData(0)]
    [InlineData(1001)]
    public async Task TextoInvalidoDevolve400(int comprimento)
    {
        var id = await ObterMarcacaoAsync(EmailVetAna, "Confirmada");
        using var cliente = await ClienteDeAsync(EmailAuxComApoio);

        using var resposta = await EnviarNotaAsync(cliente, id, new string('a', comprimento));

        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);
    }

    [Fact(DisplayName = "IT-EP029-04 — Auxiliar com podeRegistarApoio=false devolve 403")]
    public async Task AuxiliarSemApoioDevolve403()
    {
        var id = await ObterMarcacaoAsync(EmailVetAna, "Confirmada");
        using var cliente = await ClienteDeAsync(EmailAuxSemApoio);

        using var resposta = await EnviarNotaAsync(cliente, id, "Sala 2 preparada.");

        await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
    }

    [Fact(DisplayName = "IT-EP029-05 — marcação inexistente devolve 404")]
    public async Task NotaMarcacaoInexistenteDevolve404()
    {
        using var cliente = await ClienteDeAsync(EmailAuxComApoio);

        using var resposta = await EnviarNotaAsync(cliente, 999999, "Sala 2 preparada.");

        await AssertErroAsync(resposta, HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "IT-EP029-06 — marcação Cancelada devolve 422")]
    public async Task NotaMarcacaoCanceladaDevolve422()
    {
        var id = await ObterMarcacaoAsync(EmailVetBruno, "Cancelada");
        using var cliente = await ClienteDeAsync(EmailAuxComApoio);

        using var resposta = await EnviarNotaAsync(cliente, id, "Sala 2 preparada.");

        await AssertErroAsync(resposta, HttpStatusCode.UnprocessableEntity);
    }

    [Fact(DisplayName = "I075 — EP-029 não altera a marcação nem o registo clínico")]
    public async Task NotaNaoAlteraMarcacaoNemRegistoClinico()
    {
        var id = await ObterMarcacaoAsync(EmailVetAna, "Realizada");
        using var cliente = await ClienteDeAsync(EmailAuxComApoio);

        using var antes = await cliente.GetAsync($"/consultas/{id}");
        var etagAntes = antes.Headers.ETag!.Tag;
        var registoAntes = await ObterRegistoClinicoAsync(id);

        using var resposta = await EnviarNotaAsync(cliente, id, "Tutor pediu corte de unhas.");
        Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);

        using var depois = await cliente.GetAsync($"/consultas/{id}");
        Assert.Equal(etagAntes, depois.Headers.ETag!.Tag);
        Assert.Equal(registoAntes, await ObterRegistoClinicoAsync(id));
    }

    [Fact(DisplayName = "I075 — EP-029 sem token devolve 401")]
    public async Task NotaSemTokenDevolve401()
    {
        using var cliente = Fixture.Factory.CreateClient();

        using var resposta = await EnviarNotaAsync(cliente, 999999, "Sala 2 preparada.");

        await AssertErroAsync(resposta, HttpStatusCode.Unauthorized);
    }

    [Theory(DisplayName = "I075 — EP-029 só pode ser usado pelo Auxiliar Veterinário")]
    [InlineData("admin@clinica-g08.test")]
    [InlineData(EmailRececao)]
    [InlineData(EmailVetAna)]
    [InlineData("cliente.joao@clinica-g08.test")]
    public async Task NotaOutrosPerfisRecebem403(string email)
    {
        var id = await ObterMarcacaoAsync(EmailVetAna, "Confirmada");
        using var cliente = await ClienteDeAsync(email);

        using var resposta = await EnviarNotaAsync(cliente, id, "Sala 2 preparada.");

        await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
    }

    // ----- Auxiliares -----

    private sealed record RegistoClinicoResumo(string? NotasClinicas, string? Diagnostico, DateTime? AtualizadoEm);

    private static async Task<HttpResponseMessage> EnviarChegadaAsync(HttpClient cliente, int id, string? ifMatch = null)
    {
        using var pedido = new HttpRequestMessage(HttpMethod.Patch, $"/consultas/{id}/chegada");

        if (ifMatch is not null)
        {
            pedido.Headers.TryAddWithoutValidation("If-Match", ifMatch);
        }

        return await cliente.SendAsync(pedido);
    }

    private static Task<HttpResponseMessage> EnviarNotaAsync(HttpClient cliente, int id, string texto) =>
        cliente.PostAsJsonAsync($"/consultas/{id}/notas-preparacao", new { texto });

    private static async Task<JsonDocument> LerAsync(HttpResponseMessage resposta) =>
        JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());

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

        return await db.Utilizadores.AsNoTracking()
            .Where(u => u.Email == email)
            .Select(u => u.Id)
            .SingleAsync();
    }

    private async Task<int> ObterMarcacaoAsync(string emailVeterinario, string estado)
    {
        var veterinarioId = await ObterUtilizadorIdAsync(emailVeterinario);

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Marcacoes.AsNoTracking()
            .Where(m => m.VeterinarioId == veterinarioId && m.Estado == estado)
            .Select(m => m.Id)
            .SingleAsync();
    }

    private async Task<DateTime?> ObterChegadaAsync(int id)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        var chegada = await db.Marcacoes.AsNoTracking()
            .Where(m => m.Id == id)
            .Select(m => m.DataHoraChegada)
            .SingleAsync();

        return chegada is DateTime valor ? DateTime.SpecifyKind(valor, DateTimeKind.Utc) : null;
    }

    private async Task<RegistoClinicoResumo> ObterRegistoClinicoAsync(int marcacaoId)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.RegistosClinicos.AsNoTracking()
            .Where(r => r.MarcacaoId == marcacaoId)
            .Select(r => new RegistoClinicoResumo(r.NotasClinicas, r.Diagnostico, r.AtualizadoEm))
            .SingleAsync();
    }
}
