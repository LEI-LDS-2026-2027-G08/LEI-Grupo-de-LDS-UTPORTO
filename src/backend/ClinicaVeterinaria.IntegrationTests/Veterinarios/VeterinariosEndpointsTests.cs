using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.Veterinarios;

/// <summary>
/// I071 — critério 8 automatizado com a API real e o SQL Server dos testes de integração (I091):
/// um pedido válido e um pedido por cada código de erro de EP-021 e EP-022, com os dados de
/// demonstração da I061. A I093 acrescentou os casos IT-EP022-05 e IT-EP022-06 da I030
/// (horários ocupados e marcação cancelada).
/// </summary>
public sealed class VeterinariosEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailVetAna = "vet.ana@clinica-g08.test";
    private const string EmailVetBruno = "vet.bruno@clinica-g08.test";

    public VeterinariosEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    [Fact(DisplayName = "IT-EP021-01 — a Rececionista lista os Veterinários ativos")]
    public async Task RececionistaListaVeterinariosAtivos()
    {
        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var resposta = await cliente.GetAsync("/veterinarios");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
        var nomes = documento.RootElement
            .EnumerateArray()
            .Select(veterinario => veterinario.GetProperty("nome").GetString())
            .ToList();

        Assert.Equal(2, nomes.Count);
        Assert.Contains("Dra. Ana Veterinária (demonstração)", nomes);
        Assert.Contains("Dr. Bruno Veterinário (demonstração)", nomes);
    }

    [Fact(DisplayName = "IT-EP022-01 — Veterinário existente e período válido devolve os horários livres")]
    public async Task DisponibilidadeDeUmDiaSemMarcacoes()
    {
        var vetAna = await ObterUtilizadorIdAsync(EmailVetAna);
        var dia = QuartaFeiraFutura();
        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var resposta = await cliente.GetAsync(Url(vetAna, Texto(dia)));

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
        var raiz = documento.RootElement;
        Assert.Equal(vetAna, raiz.GetProperty("veterinarioId").GetInt32());

        var unicoDia = Assert.Single(raiz.GetProperty("dias").EnumerateArray().ToList());
        Assert.Equal(Texto(dia), unicoDia.GetProperty("data").GetString());
        Assert.Equal(18, unicoDia.GetProperty("horariosLivres").GetArrayLength());
    }

    [Theory(DisplayName = "IT-EP022-02 e IT-EP022-03 — datas em falta, inválidas, invertidas ou mais de 31 dias devolvem 400")]
    [InlineData("")]
    [InlineData("?dataInicio=2026-13-40")]
    [InlineData("?dataInicio=2026-11-05&dataFim=2026-11-04")]
    [InlineData("?dataInicio=2026-11-01&dataFim=2026-12-02")]
    public async Task PeriodoInvalidoDevolve400(string parametros)
    {
        var vetAna = await ObterUtilizadorIdAsync(EmailVetAna);
        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var resposta = await cliente.GetAsync($"/veterinarios/{vetAna}/disponibilidade{parametros}");

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "IT-EP022-04 — Veterinário inexistente, ou conta que não é de Veterinário, devolve 404")]
    public async Task VeterinarioInexistenteDevolve404()
    {
        var rececao = await ObterUtilizadorIdAsync("rececao@clinica-g08.test");
        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var inexistente = await cliente.GetAsync(Url(999999, "2026-11-04"));
        using var naoVeterinario = await cliente.GetAsync(Url(rececao, "2026-11-04"));

        await AssertErroAsync(inexistente, HttpStatusCode.NotFound);
        await AssertErroAsync(naoVeterinario, HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "IT-EP022-05 — os horários ocupados não aparecem como livres")]
    public async Task HorariosOcupadosNaoSaoLivres()
    {
        var vetAna = await ObterUtilizadorIdAsync(EmailVetAna);
        var vetBruno = await ObterUtilizadorIdAsync(EmailVetBruno);
        var dia = QuartaFeiraFutura();
        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var criada = await CriarMarcacaoAsync(cliente, vetAna, dia, 10, 0);
        Assert.Equal(HttpStatusCode.Created, criada.StatusCode);

        var livresAna = await HorariosLivresAsync(cliente, vetAna, dia);
        var livresBruno = await HorariosLivresAsync(cliente, vetBruno, dia);

        // Só o horário das 10:00 da Dra. Ana fica ocupado: os vizinhos e a agenda do Dr. Bruno não mudam.
        Assert.Equal(17, livresAna.Count);
        Assert.DoesNotContain("10:00", livresAna);
        Assert.Contains("09:30", livresAna);
        Assert.Contains("10:30", livresAna);
        Assert.Equal(18, livresBruno.Count);
        Assert.Contains("10:00", livresBruno);
    }

    [Fact(DisplayName = "IT-EP022-06 — o horário de uma marcação cancelada volta a estar livre")]
    public async Task MarcacaoCanceladaLibertaOHorario()
    {
        var vetAna = await ObterUtilizadorIdAsync(EmailVetAna);
        var dia = QuartaFeiraFutura();
        using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var criada = await CriarMarcacaoAsync(cliente, vetAna, dia, 10, 0);
        Assert.Equal(HttpStatusCode.Created, criada.StatusCode);
        using var documento = JsonDocument.Parse(await criada.Content.ReadAsStringAsync());
        var marcacaoId = documento.RootElement.GetProperty("id").GetInt32();

        var antes = await HorariosLivresAsync(cliente, vetAna, dia);

        using var cancelamento = await cliente.PatchAsJsonAsync(
            $"/consultas/{marcacaoId}/estado",
            new { estado = "Cancelada", observacao = (string?)null });
        Assert.Equal(HttpStatusCode.NoContent, cancelamento.StatusCode);

        var depois = await HorariosLivresAsync(cliente, vetAna, dia);

        Assert.DoesNotContain("10:00", antes);
        Assert.Contains("10:00", depois);
        Assert.Equal(18, depois.Count);
    }

    [Theory(DisplayName = "IT-EP021 e IT-EP022 — perfis que não são Rececionista recebem 403")]
    [InlineData("Administrador")]
    [InlineData("Veterinario")]
    [InlineData("AuxiliarVeterinario")]
    [InlineData("Cliente")]
    public async Task OutrosPerfisRecebem403(string perfil)
    {
        var vetAna = await ObterUtilizadorIdAsync(EmailVetAna);
        using var cliente = await Fixture.CriarClienteAutenticadoAsync(perfil);

        using var lista = await cliente.GetAsync("/veterinarios");
        using var disponibilidade = await cliente.GetAsync(Url(vetAna, "2026-11-04"));

        await AssertErroAsync(lista, HttpStatusCode.Forbidden);
        await AssertErroAsync(disponibilidade, HttpStatusCode.Forbidden);
    }

    [Fact(DisplayName = "IT-EP021 e IT-EP022 — sem token devolvem 401")]
    public async Task SemTokenDevolve401()
    {
        var vetAna = await ObterUtilizadorIdAsync(EmailVetAna);
        using var cliente = Fixture.Factory.CreateClient();

        using var lista = await cliente.GetAsync("/veterinarios");
        using var disponibilidade = await cliente.GetAsync(Url(vetAna, "2026-11-04"));

        await AssertErroAsync(lista, HttpStatusCode.Unauthorized);
        await AssertErroAsync(disponibilidade, HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Quarta-feira pelo menos 28 dias depois de hoje (hora de Lisboa): a API usa o relógio real e
    /// não devolve horários passados; os dados de demonstração não têm marcações nessas datas.
    /// </summary>
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

    /// <summary>Cria uma marcação do Bobi em EP-025, à hora de Lisboa indicada (regra R-FUSO).</summary>
    private async Task<HttpResponseMessage> CriarMarcacaoAsync(
        HttpClient cliente,
        int veterinarioId,
        DateOnly dia,
        int hora,
        int minuto)
    {
        var inicioUtc = FusoHorarioClinica.ParaUtc(dia, new TimeOnly(hora, minuto), FusoHorarioClinica.Obter())!.Value;
        var corpo = new Dictionary<string, object?>
        {
            ["animalId"] = await ObterAnimalIdAsync("Bobi"),
            ["veterinarioId"] = veterinarioId,
            ["dataHora"] = inicioUtc.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture),
            ["motivo"] = "Teste de integração I093",
        };

        return await cliente.PostAsJsonAsync("/consultas", corpo);
    }

    /// <summary>Horários livres (hora de Lisboa) de um único dia, lidos em EP-022.</summary>
    private static async Task<List<string?>> HorariosLivresAsync(HttpClient cliente, int veterinarioId, DateOnly dia)
    {
        using var resposta = await cliente.GetAsync(Url(veterinarioId, Texto(dia)));
        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
        var unicoDia = Assert.Single(documento.RootElement.GetProperty("dias").EnumerateArray().ToList());

        return unicoDia.GetProperty("horariosLivres")
            .EnumerateArray()
            .Select(horario => horario.GetString())
            .ToList();
    }

    private static string Texto(DateOnly data) =>
        data.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    private static string Url(int veterinarioId, string dataInicio) =>
        $"/veterinarios/{veterinarioId}/disponibilidade?dataInicio={dataInicio}";

    /// <summary>O código HTTP esperado, no formato de erro da I058 (<c>code</c>, <c>message</c>, <c>detail</c>).</summary>
    private static async Task AssertErroAsync(HttpResponseMessage resposta, HttpStatusCode esperado)
    {
        Assert.Equal(esperado, resposta.StatusCode);

        using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
        Assert.Equal((int)esperado, documento.RootElement.GetProperty("code").GetInt32());
        Assert.False(string.IsNullOrWhiteSpace(documento.RootElement.GetProperty("message").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(documento.RootElement.GetProperty("detail").GetString()));
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
}
