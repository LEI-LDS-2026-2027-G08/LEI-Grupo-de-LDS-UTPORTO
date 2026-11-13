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
/// I095 — persistência, recálculo e isolamento do boletim de vacinas e desparasitações através da
/// API real e do SQL Server dos testes de integração (I091): as alterações persistem e não afetam
/// outros pacientes nem outros Clientes; a próxima administração é recalculada na atualização,
/// também nos casos de calendário, e o boletim do Cliente (EP-044) mostra-a; a atualização recusada
/// deixa o registo como estava; EP-044 é recusado aos perfis da clínica.
/// </summary>
/// <remarks>
/// Dados da I061 (todos registados pela vet.ana): Bobi (João) com a vacina antirrábica de
/// 2025-10-14, a vacina polivalente PV-1187 e o desparasitante interno DP-0042 (90 dias, próxima
/// 2027-01-12); Kiko (Maria) com o desparasitante externo DE-0310 de 2026-08-01, sem intervalo;
/// Mia (João) sem registos. A base é reposta antes de cada teste. Os testes correm com o relógio
/// real, por isso as datas de administração usadas são passadas e as próximas que devem aparecer no
/// boletim são de 2027.
/// </remarks>
public sealed class VacinacoesPersistenciaEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailVetAna = "vet.ana@clinica-g08.test";
    private const string EmailVetBruno = "vet.bruno@clinica-g08.test";
    private const string EmailJoao = "cliente.joao@clinica-g08.test";
    private const string EmailMaria = "cliente.maria@clinica-g08.test";

    public VacinacoesPersistenciaEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    [Fact(DisplayName = "I095 — registar e atualizar no boletim do Bobi não altera os registos dos outros animais nem o boletim da Maria (EP-044)")]
    public async Task AlteracoesNoBobiNaoAfetamOutrosPacientesNemClientes()
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        var kiko = await ObterAnimalIdAsync("Kiko");
        var dp0042 = await ObterAdministracaoIdAsync(bobi, "DP-0042");
        var outrosAntes = await RegistosAsync(r => r.PacienteId != bobi);
        var doBobiAntes = await RegistosAsync(r => r.PacienteId == bobi && r.Id != dp0042);
        var boletimMariaAntes = await LerTextoAsync(EmailMaria, $"/me/animais/{kiko}/vacinacoes");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var registar = await cliente.PostAsJsonAsync(
            $"/animais/{bobi}/vacinacoes",
            new { tipo = "Vacina", produto = "Vacina da tosse do canil (fictícia)", lote = "TC-0007", dataAdministracao = "2026-10-20", intervaloDias = 365 });
        using var atualizar = await cliente.PutAsJsonAsync(
            $"/animais/{bobi}/vacinacoes/{dp0042}",
            new { tipo = "Desparasitacao", produto = "Desparasitante interno (fictício)", lote = "DP-0042", dataAdministracao = "2026-10-20", intervaloDias = 30 });

        Assert.Equal(HttpStatusCode.Created, registar.StatusCode);
        Assert.Equal(HttpStatusCode.OK, atualizar.StatusCode);

        // Os registos do Kiko e da Mia e os outros registos do Bobi ficam exatamente como estavam.
        Assert.Equal(outrosAntes, await RegistosAsync(r => r.PacienteId != bobi));
        Assert.Equal(doBobiAntes, await RegistosAsync(r => r.PacienteId == bobi && r.Id != dp0042 && r.Lote != "TC-0007"));
        Assert.Equal(boletimMariaAntes, await LerTextoAsync(EmailMaria, $"/me/animais/{kiko}/vacinacoes"));

        // O novo registo e a atualização ficam no Bobi.
        var novo = Assert.Single(await RegistosAsync(r => r.Lote == "TC-0007"));
        Assert.Equal(bobi, novo.PacienteId);
        Assert.Equal(new DateOnly(2027, 10, 20), novo.DataProximaAdministracao);
        var atualizado = Assert.Single(await RegistosAsync(r => r.Id == dp0042));
        Assert.Equal(bobi, atualizado.PacienteId);
        Assert.Equal(new DateOnly(2026, 11, 19), atualizado.DataProximaAdministracao);
    }

    [Fact(DisplayName = "I095 — dar intervalo à desparasitação do Kiko recalcula a próxima, que aparece no boletim web (EP-042) e no boletim da Maria (EP-044), e não no do João")]
    public async Task AtualizacaoApareceNoBoletimDoCliente()
    {
        var kiko = await ObterAnimalIdAsync("Kiko");
        var bobi = await ObterAnimalIdAsync("Bobi");
        var de0310 = await ObterAdministracaoIdAsync(kiko, "DE-0310");
        var produto = (await RegistosAsync(r => r.Id == de0310)).Single().Produto;
        var boletimJoaoAntes = await LerTextoAsync(EmailJoao, $"/me/animais/{bobi}/vacinacoes");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PutAsJsonAsync(
            $"/animais/{kiko}/vacinacoes/{de0310}",
            new { tipo = "Desparasitacao", produto, lote = "DE-0310", dataAdministracao = "2026-08-01", intervaloDias = 365 });

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using (var documento = await LerAsync(resposta))
        {
            Assert.Equal("2027-08-01", documento.RootElement.GetProperty("dataProximaPrevista").GetString());
        }

        Assert.Equal(new DateOnly(2027, 8, 1), (await RegistosAsync(r => r.Id == de0310)).Single().DataProximaAdministracao);

        using var web = await cliente.GetAsync($"/animais/{kiko}/vacinacoes");
        Assert.Equal(HttpStatusCode.OK, web.StatusCode);
        using (var documento = await LerAsync(web))
        {
            var proxima = Assert.Single(documento.RootElement.GetProperty("proximas").EnumerateArray().ToList());
            Assert.Equal("2027-08-01", proxima.GetProperty("dataProximaPrevista").GetString());
        }

        using var maria = await ClienteDeAsync(EmailMaria);
        using var mobile = await maria.GetAsync($"/me/animais/{kiko}/vacinacoes");
        Assert.Equal(HttpStatusCode.OK, mobile.StatusCode);
        using (var documento = await LerAsync(mobile))
        {
            var proxima = Assert.Single(documento.RootElement.GetProperty("proximas").EnumerateArray().ToList());
            Assert.Equal("Desparasitacao", proxima.GetProperty("tipo").GetString());
            Assert.Equal(produto, proxima.GetProperty("produto").GetString());
            Assert.Equal("2027-08-01", proxima.GetProperty("dataProximaPrevista").GetString());
        }

        Assert.Equal(boletimJoaoAntes, await LerTextoAsync(EmailJoao, $"/me/animais/{bobi}/vacinacoes"));
    }

    [Theory(DisplayName = "I095 — a atualização recalcula a próxima nos casos de calendário (fim de ano, fim de mês, 29 de fevereiro), na resposta e na coluna calculada")]
    [InlineData("2025-12-31", 1, "2026-01-01")]
    [InlineData("2026-01-31", 30, "2026-03-02")]
    [InlineData("2024-02-28", 1, "2024-02-29")]
    [InlineData("2023-03-01", 365, "2024-02-29")]
    public async Task AtualizacaoRecalculaNosCasosDeCalendario(string dataAdministracao, int intervaloDias, string esperada)
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        var id = await ObterAdministracaoIdAsync(bobi, "DP-0042");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PutAsJsonAsync(
            $"/animais/{bobi}/vacinacoes/{id}",
            new { tipo = "Desparasitacao", produto = "Desparasitante interno (fictício)", lote = "DP-0042", dataAdministracao, intervaloDias });

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        Assert.Equal(esperada, documento.RootElement.GetProperty("dataProximaPrevista").GetString());

        // A coluna DataProximaAdministracao é calculada pelo SQL Server e tem de dar o mesmo dia.
        var gravado = (await RegistosAsync(r => r.Id == id)).Single();
        Assert.Equal(DateOnly.Parse(esperada, System.Globalization.CultureInfo.InvariantCulture), gravado.DataProximaAdministracao);
        Assert.Equal((short)intervaloDias, gravado.IntervaloDias);
    }

    [Fact(DisplayName = "I095 — a atualização pedida por outro Veterinário (vet.bruno) devolve 403 e o registo fica como estava")]
    public async Task AtualizacaoDeOutroVeterinarioNaoAlteraORegisto()
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        var id = await ObterAdministracaoIdAsync(bobi, "DP-0042");
        var antes = await RegistosAsync(r => r.PacienteId == bobi);
        using var cliente = await ClienteDeAsync(EmailVetBruno);

        using var resposta = await cliente.PutAsJsonAsync(
            $"/animais/{bobi}/vacinacoes/{id}",
            new { tipo = "Vacina", produto = "Outro produto", lote = "XX-9999", dataAdministracao = "2026-10-01", intervaloDias = 7 });

        await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
        Assert.Equal(antes, await RegistosAsync(r => r.PacienteId == bobi));
    }

    [Theory(DisplayName = "I095 — EP-044 é só do Cliente: Rececionista, Auxiliar e Administrador recebem 403, sem nenhum dado do boletim")]
    [InlineData("rececao@clinica-g08.test")]
    [InlineData("aux.carla@clinica-g08.test")]
    [InlineData("admin@clinica-g08.test")]
    public async Task PerfisDaClinicaRecebem403NoMobile(string email)
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        using var cliente = await ClienteDeAsync(email);

        using var resposta = await cliente.GetAsync($"/me/animais/{bobi}/vacinacoes");

        await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
        var corpo = await resposta.Content.ReadAsStringAsync();
        Assert.DoesNotContain("DP-0042", corpo, StringComparison.Ordinal);
        Assert.DoesNotContain("2027-01-12", corpo, StringComparison.Ordinal);
    }

    // ----- Auxiliares -----
    private sealed record Registo(
        int Id,
        int PacienteId,
        int VeterinarioId,
        string Tipo,
        string Produto,
        string Lote,
        DateOnly DataAdministracao,
        short? IntervaloDias,
        DateOnly? DataProximaAdministracao);

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

    /// <summary>O corpo de um GET com 200, como texto, para comparar o boletim antes e depois.</summary>
    private async Task<string> LerTextoAsync(string email, string url)
    {
        using var cliente = await ClienteDeAsync(email);
        using var resposta = await cliente.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        return await resposta.Content.ReadAsStringAsync();
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

    /// <summary>Os registos que cumprem o filtro, por ordem de <c>Id</c>, com todos os campos clínicos.</summary>
    private async Task<List<Registo>> RegistosAsync(System.Linq.Expressions.Expression<Func<Domain.Entities.RegistoProfilaxia, bool>> filtro)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.RegistosProfilaxias.AsNoTracking()
            .Where(filtro)
            .OrderBy(r => r.Id)
            .Select(r => new Registo(r.Id, r.PacienteId, r.VeterinarioId, r.Tipo, r.Produto, r.Lote, r.DataAdministracao, r.IntervaloDias, r.DataProximaAdministracao))
            .ToListAsync();
    }
}
