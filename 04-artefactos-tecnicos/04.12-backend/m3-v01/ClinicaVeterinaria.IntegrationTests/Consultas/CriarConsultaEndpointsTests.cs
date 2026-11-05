using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.Consultas;

/// <summary>
/// I073 — EP-025 (POST /consultas) com a API real e o SQL Server dos testes de integração (I091),
/// sobre os dados de demonstração da I061: casos IT-EP025-01 a IT-EP025-08 da Especificação de
/// Testes de Integração (I030, secção 11.5), a matriz de perfis (401 e 403) e a criação simultânea
/// para a mesma vaga (critério 2 da I073).
/// </summary>
/// <remarks>
/// A API usa o relógio real: as marcações são feitas numa quarta-feira pelo menos 28 dias à
/// frente, em hora de Lisboa, onde os dados de demonstração não têm marcações.
/// </remarks>
public sealed class CriarConsultaEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailRececao = "rececao@clinica-g08.test";
    private const string EmailVetAna = "vet.ana@clinica-g08.test";
    private const string EmailVetBruno = "vet.bruno@clinica-g08.test";

    public CriarConsultaEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    [Fact(DisplayName = "IT-EP025-01 — cria a marcação Pendente, com Location e ETag")]
    public async Task CriaMarcacaoPendente()
    {
        var vetAna = await ObterUtilizadorIdAsync(EmailVetAna);
        var bobi = await ObterAnimalIdAsync("Bobi");
        var dataHora = InicioUtc(QuartaFeiraFutura(), 10, 0);
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var resposta = await cliente.PostAsJsonAsync("/consultas", Corpo(bobi, vetAna, dataHora, "Consulta de rotina"));

        Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);
        Assert.NotNull(resposta.Headers.ETag);
        using var documento = await LerAsync(resposta);
        var raiz = documento.RootElement;
        var id = raiz.GetProperty("id").GetInt32();
        Assert.EndsWith($"/consultas/{id}", resposta.Headers.Location!.ToString(), StringComparison.Ordinal);
        Assert.Equal("Pendente", raiz.GetProperty("estado").GetString());
        Assert.Equal(Texto(dataHora), raiz.GetProperty("dataHora").GetString());
        Assert.Equal(30, raiz.GetProperty("duracaoMinutos").GetInt32());
        Assert.Equal("Consulta de rotina", raiz.GetProperty("motivo").GetString());
        Assert.Equal("Bobi", raiz.GetProperty("animal").GetProperty("nome").GetString());
        Assert.Equal(vetAna, raiz.GetProperty("veterinario").GetProperty("id").GetInt32());
        Assert.Equal(JsonValueKind.Null, raiz.GetProperty("chegadaEm").ValueKind);
    }

    [Theory(DisplayName = "IT-EP025-02 — campo obrigatório em falta devolve 400")]
    [InlineData("animalId")]
    [InlineData("veterinarioId")]
    [InlineData("dataHora")]
    public async Task CampoEmFaltaDevolve400(string campoEmFalta)
    {
        var corpo = new Dictionary<string, object>
        {
            ["animalId"] = await ObterAnimalIdAsync("Bobi"),
            ["veterinarioId"] = await ObterUtilizadorIdAsync(EmailVetAna),
            ["dataHora"] = Texto(InicioUtc(QuartaFeiraFutura(), 10, 0))
        };
        corpo.Remove(campoEmFalta);
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var resposta = await cliente.PostAsJsonAsync("/consultas", corpo);

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
        Assert.Equal(0, await ContarMarcacoesFuturasAsync());
    }

    [Fact(DisplayName = "IT-EP025-03 — data no passado devolve 400")]
    public async Task DataNoPassadoDevolve400()
    {
        var corpo = Corpo(
            await ObterAnimalIdAsync("Bobi"),
            await ObterUtilizadorIdAsync(EmailVetAna),
            DateTime.UtcNow.AddDays(-1));
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var resposta = await cliente.PostAsJsonAsync("/consultas", corpo);

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "IT-EP025-04 — animal inexistente devolve 404")]
    public async Task AnimalInexistenteDevolve404()
    {
        var corpo = Corpo(999999, await ObterUtilizadorIdAsync(EmailVetAna), InicioUtc(QuartaFeiraFutura(), 10, 0));
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var resposta = await cliente.PostAsJsonAsync("/consultas", corpo);

        await AssertErroAsync(resposta, HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "IT-EP025-05 — Veterinário inexistente, ou conta que não é de Veterinário, devolve 404")]
    public async Task VeterinarioInexistenteDevolve404()
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        var rececionista = await ObterUtilizadorIdAsync(EmailRececao);
        var dataHora = InicioUtc(QuartaFeiraFutura(), 10, 0);
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var inexistente = await cliente.PostAsJsonAsync("/consultas", Corpo(bobi, 999999, dataHora));
        using var naoVeterinario = await cliente.PostAsJsonAsync("/consultas", Corpo(bobi, rececionista, dataHora));

        await AssertErroAsync(inexistente, HttpStatusCode.NotFound);
        await AssertErroAsync(naoVeterinario, HttpStatusCode.NotFound);
    }

    [Theory(DisplayName = "IT-EP025-06 — fora do horário da clínica devolve 422")]
    [InlineData(0, 12, 45)] // quarta-feira, acaba às 13:15
    [InlineData(0, 13, 0)]  // quarta-feira, hora de almoço
    [InlineData(4, 10, 0)]  // domingo
    public async Task ForaDoHorarioDevolve422(int diasDepoisDeQuarta, int hora, int minuto)
    {
        var dia = QuartaFeiraFutura().AddDays(diasDepoisDeQuarta);
        var corpo = Corpo(await ObterAnimalIdAsync("Bobi"), await ObterUtilizadorIdAsync(EmailVetAna), InicioUtc(dia, hora, minuto));
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var resposta = await cliente.PostAsJsonAsync("/consultas", corpo);

        await AssertErroAsync(resposta, HttpStatusCode.UnprocessableEntity);
        Assert.Equal(0, await ContarMarcacoesFuturasAsync());
    }

    [Fact(DisplayName = "IT-EP025-07 — Veterinário já ocupado devolve 422 e não cria a segunda marcação")]
    public async Task VeterinarioOcupadoDevolve422()
    {
        var vetAna = await ObterUtilizadorIdAsync(EmailVetAna);
        var dia = QuartaFeiraFutura();
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var primeira = await cliente.PostAsJsonAsync(
            "/consultas", Corpo(await ObterAnimalIdAsync("Bobi"), vetAna, InicioUtc(dia, 10, 0)));
        using var sobreposta = await cliente.PostAsJsonAsync(
            "/consultas", Corpo(await ObterAnimalIdAsync("Kiko"), vetAna, InicioUtc(dia, 10, 15)));

        Assert.Equal(HttpStatusCode.Created, primeira.StatusCode);
        await AssertErroAsync(sobreposta, HttpStatusCode.UnprocessableEntity);
        Assert.Equal(1, await ContarMarcacoesFuturasAsync());
    }

    [Fact(DisplayName = "IT-EP025-08 — a marcação criada fica persistida e é lida em EP-024 e na agenda (EP-023)")]
    public async Task MarcacaoPersistida()
    {
        var vetBruno = await ObterUtilizadorIdAsync(EmailVetBruno);
        var dia = QuartaFeiraFutura();
        using var cliente = await ClienteDeAsync(EmailRececao);

        using var criada = await cliente.PostAsJsonAsync(
            "/consultas", Corpo(await ObterAnimalIdAsync("Luna"), vetBruno, InicioUtc(dia, 15, 30), "Primeira consulta"));
        Assert.Equal(HttpStatusCode.Created, criada.StatusCode);
        using var documentoCriada = await LerAsync(criada);
        var id = documentoCriada.RootElement.GetProperty("id").GetInt32();

        using var lida = await cliente.GetAsync(criada.Headers.Location);
        Assert.Equal(HttpStatusCode.OK, lida.StatusCode);
        Assert.Equal(criada.Headers.ETag!.Tag, lida.Headers.ETag!.Tag);
        using var documentoLida = await LerAsync(lida);
        Assert.Equal(id, documentoLida.RootElement.GetProperty("id").GetInt32());
        Assert.Equal("Primeira consulta", documentoLida.RootElement.GetProperty("motivo").GetString());

        var data = dia.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        using var agenda = await cliente.GetAsync($"/consultas?dataInicio={data}&dataFim={data}&veterinarioId={vetBruno}");
        using var documentoAgenda = await LerAsync(agenda);
        var item = Assert.Single(documentoAgenda.RootElement.GetProperty("itens").EnumerateArray().ToList());
        Assert.Equal(id, item.GetProperty("id").GetInt32());
        Assert.Equal("Pendente", item.GetProperty("estado").GetString());

        // A vaga deixa de aparecer em EP-022 (15:30 em hora de Lisboa).
        using var disponibilidade = await cliente.GetAsync($"/veterinarios/{vetBruno}/disponibilidade?dataInicio={data}");
        using var documentoDisponibilidade = await LerAsync(disponibilidade);
        var horarios = documentoDisponibilidade.RootElement.GetProperty("dias")[0].GetProperty("horariosLivres")
            .EnumerateArray().Select(h => h.GetString()).ToList();
        Assert.DoesNotContain("15:30", horarios);
        Assert.Contains("15:00", horarios);
        Assert.Contains("16:00", horarios);
    }

    [Fact(DisplayName = "I073 — critério 2: pedidos simultâneos para a mesma vaga, só um é aceite (201) e os outros recebem 422")]
    public async Task PedidosSimultaneosParaAMesmaVaga()
    {
        const int Pedidos = 8;
        var corpo = Corpo(await ObterAnimalIdAsync("Bobi"), await ObterUtilizadorIdAsync(EmailVetAna), InicioUtc(QuartaFeiraFutura(), 11, 0));
        var clientes = new List<HttpClient>();

        for (var i = 0; i < Pedidos; i++)
        {
            clientes.Add(await ClienteDeAsync(EmailRececao));
        }

        try
        {
            var respostas = await Task.WhenAll(clientes.Select(c => c.PostAsJsonAsync("/consultas", corpo)));

            try
            {
                Assert.Single(respostas, r => r.StatusCode == HttpStatusCode.Created);
                Assert.Equal(Pedidos - 1, respostas.Count(r => r.StatusCode == HttpStatusCode.UnprocessableEntity));
                Assert.Equal(1, await ContarMarcacoesFuturasAsync());
            }
            finally
            {
                foreach (var resposta in respostas)
                {
                    resposta.Dispose();
                }
            }
        }
        finally
        {
            foreach (var cliente in clientes)
            {
                cliente.Dispose();
            }
        }
    }

    [Theory(DisplayName = "I073 — EP-025 só para a Rececionista: os outros quatro perfis recebem 403")]
    [InlineData("admin@clinica-g08.test")]
    [InlineData(EmailVetAna)]
    [InlineData("aux.carla@clinica-g08.test")]
    [InlineData("cliente.joao@clinica-g08.test")]
    public async Task OutrosPerfisRecebem403(string email)
    {
        var corpo = Corpo(await ObterAnimalIdAsync("Bobi"), await ObterUtilizadorIdAsync(EmailVetAna), InicioUtc(QuartaFeiraFutura(), 10, 0));
        using var cliente = await ClienteDeAsync(email);

        using var resposta = await cliente.PostAsJsonAsync("/consultas", corpo);

        await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
        Assert.Equal(0, await ContarMarcacoesFuturasAsync());
    }

    [Fact(DisplayName = "I073 — EP-025 sem token devolve 401")]
    public async Task SemTokenDevolve401()
    {
        var corpo = Corpo(await ObterAnimalIdAsync("Bobi"), await ObterUtilizadorIdAsync(EmailVetAna), InicioUtc(QuartaFeiraFutura(), 10, 0));
        using var cliente = Fixture.Factory.CreateClient();

        using var resposta = await cliente.PostAsJsonAsync("/consultas", corpo);

        await AssertErroAsync(resposta, HttpStatusCode.Unauthorized);
    }

    // ----- Auxiliares -----

    private static Dictionary<string, object?> Corpo(int animalId, int veterinarioId, DateTime dataHoraUtc, string? motivo = null) =>
        new()
        {
            ["animalId"] = animalId,
            ["veterinarioId"] = veterinarioId,
            ["dataHora"] = Texto(dataHoraUtc),
            ["motivo"] = motivo
        };

    /// <summary>Quarta-feira pelo menos 28 dias depois de hoje (hora de Lisboa), como nos testes da I071.</summary>
    private static DateOnly QuartaFeiraFutura()
    {
        var hoje = DateOnly.FromDateTime(
            FusoHorarioClinica.ParaLocal(DateTime.UtcNow, FusoHorarioClinica.Obter()));
        var dia = hoje.AddDays(28);

        while (dia.DayOfWeek != DayOfWeek.Wednesday)
        {
            dia = dia.AddDays(1);
        }

        return dia;
    }

    /// <summary>Hora de Lisboa nesse dia, convertida para UTC (regra R-FUSO).</summary>
    private static DateTime InicioUtc(DateOnly dia, int hora, int minuto) =>
        FusoHorarioClinica.ParaUtc(dia, new TimeOnly(hora, minuto), FusoHorarioClinica.Obter())!.Value;

    private static string Texto(DateTime utc) =>
        utc.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture);

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

        return await db.Utilizadores
            .AsNoTracking()
            .Where(u => u.Email == email)
            .Select(u => u.Id)
            .SingleAsync();
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

    /// <summary>Marcações a partir da quarta-feira de teste (os dados de demonstração não têm nenhuma).</summary>
    private async Task<int> ContarMarcacoesFuturasAsync()
    {
        var desde = InicioUtc(QuartaFeiraFutura(), 0, 0);

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Marcacoes.AsNoTracking().CountAsync(m => m.DataHoraInicio >= desde);
    }
}
