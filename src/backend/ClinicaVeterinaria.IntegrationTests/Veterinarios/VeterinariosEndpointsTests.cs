using System.Globalization;
using System.Net;
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
/// demonstração da I061. Os restantes casos da I030 (IT-EP022-05 e 06) ficam para a I093.
/// </summary>
public sealed class VeterinariosEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailVetAna = "vet.ana@clinica-g08.test";

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
