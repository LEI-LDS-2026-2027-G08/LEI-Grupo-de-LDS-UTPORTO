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
/// I093 — critério 4: visibilidade da agenda (EP-023) e da marcação (EP-024) por perfil, segundo a
/// matriz endpoint × perfil da Especificação de Testes de Integração (I030, secção 14.1), com a API
/// real e o SQL Server dos testes de integração (I091).
/// </summary>
/// <remarks>
/// Dados de demonstração da I061 entre 2026-10-14 e 2026-11-05: cinco marcações, três da Dra. Ana
/// (M1, M3 e M5) e duas do Dr. Bruno (M2 e M4). A marcação lida em EP-024 é a M3 (Dra. Ana,
/// Confirmada). Os dois Auxiliares (com e sem autorização de apoio) leem a agenda; o Veterinário só
/// vê as suas marcações; Administrador e Cliente recebem 403; sem token, 401.
/// </remarks>
public sealed class VisibilidadeAgendaEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string Periodo = "dataInicio=2026-10-14&dataFim=2026-11-05";
    private const string EmailVetAna = "vet.ana@clinica-g08.test";

    public VisibilidadeAgendaEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    [Theory(DisplayName = "I093 (critério 4) — visibilidade da agenda (EP-023) e da marcação (EP-024) por perfil")]
    [InlineData("rececao@clinica-g08.test", 200, 5, false, 200)]
    [InlineData("vet.ana@clinica-g08.test", 200, 3, true, 200)]
    [InlineData("vet.bruno@clinica-g08.test", 200, 2, true, 403)]
    [InlineData("aux.carla@clinica-g08.test", 200, 5, false, 200)]
    [InlineData("aux.diogo@clinica-g08.test", 200, 5, false, 200)]
    [InlineData("admin@clinica-g08.test", 403, 0, false, 403)]
    [InlineData("cliente.joao@clinica-g08.test", 403, 0, false, 403)]
    [InlineData(null, 401, 0, false, 401)]
    public async Task VisibilidadePorPerfil(
        string? email,
        int codigoAgenda,
        int totalEsperado,
        bool soAsProprias,
        int codigoMarcacao)
    {
        var m3 = await ObterMarcacaoAsync(EmailVetAna, "Confirmada");
        using var cliente = email is null
            ? Fixture.Factory.CreateClient()
            : await ClienteDeAsync(email);

        using var agenda = await cliente.GetAsync($"/consultas?{Periodo}");
        using var marcacao = await cliente.GetAsync($"/consultas/{m3}");

        if (codigoAgenda == (int)HttpStatusCode.OK)
        {
            Assert.Equal(HttpStatusCode.OK, agenda.StatusCode);
            using var documento = await LerAsync(agenda);
            var itens = documento.RootElement.GetProperty("itens").EnumerateArray().ToList();
            var veterinarios = itens
                .Select(i => i.GetProperty("veterinario").GetProperty("id").GetInt32())
                .Distinct()
                .ToList();

            Assert.Equal(totalEsperado, documento.RootElement.GetProperty("total").GetInt32());
            Assert.Equal(totalEsperado, itens.Count);

            if (soAsProprias)
            {
                Assert.Equal(await ObterUtilizadorIdAsync(email!), Assert.Single(veterinarios));
            }
            else
            {
                Assert.Equal(2, veterinarios.Count);
            }
        }
        else
        {
            await AssertErroAsync(agenda, (HttpStatusCode)codigoAgenda);
        }

        if (codigoMarcacao == (int)HttpStatusCode.OK)
        {
            Assert.Equal(HttpStatusCode.OK, marcacao.StatusCode);
            Assert.NotNull(marcacao.Headers.ETag);
            using var documento = await LerAsync(marcacao);
            Assert.Equal(m3, documento.RootElement.GetProperty("id").GetInt32());
        }
        else
        {
            await AssertErroAsync(marcacao, (HttpStatusCode)codigoMarcacao);
        }
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

        return await db.Utilizadores
            .AsNoTracking()
            .Where(u => u.Email == email)
            .Select(u => u.Id)
            .SingleAsync();
    }

    private async Task<int> ObterMarcacaoAsync(string emailVeterinario, string estado)
    {
        var veterinarioId = await ObterUtilizadorIdAsync(emailVeterinario);

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Marcacoes
            .AsNoTracking()
            .Where(m => m.VeterinarioId == veterinarioId && m.Estado == estado)
            .Select(m => m.Id)
            .SingleAsync();
    }
}
