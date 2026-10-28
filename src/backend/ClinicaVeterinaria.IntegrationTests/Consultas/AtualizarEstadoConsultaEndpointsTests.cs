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
/// I074 — EP-027 (PATCH /consultas/{id}/estado) com API real e SQL Server.
/// Casos IT-EP027-01 a IT-EP027-10 da especificação de integração.
/// </summary>
public sealed class AtualizarEstadoConsultaEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailRececao =
        "rececao@clinica-g08.test";

    private const string EmailVetAna =
        "vet.ana@clinica-g08.test";

    private const string EmailVetBruno =
        "vet.bruno@clinica-g08.test";

    public AtualizarEstadoConsultaEndpointsTests(
        IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    [Theory(DisplayName = "IT-EP027-01 a 04 — as quatro transições válidas devolvem 204 e ficam persistidas")]
    [InlineData(EmailVetBruno, "Pendente", "Confirmada")]
    [InlineData(EmailVetBruno, "Pendente", "Cancelada")]
    [InlineData(EmailVetAna, "Confirmada", "Realizada")]
    [InlineData(EmailVetAna, "Confirmada", "Cancelada")]
    public async Task TransicoesValidas(
        string emailVeterinario,
        string estadoInicial,
        string novoEstado)
    {
        var marcacao =
            await ObterMarcacaoAsync(
                emailVeterinario,
                estadoInicial);

        using var cliente =
            await ClienteDeAsync(EmailRececao);

        using var resposta =
            await EnviarPatchAsync(
                cliente,
                marcacao.Id,
                novoEstado);

        Assert.Equal(
            HttpStatusCode.NoContent,
            resposta.StatusCode);

        using var leitura =
            await cliente.GetAsync(
                $"/consultas/{marcacao.Id}");

        Assert.Equal(
            HttpStatusCode.OK,
            leitura.StatusCode);

        using var documento =
            await LerAsync(leitura);

        Assert.Equal(
            novoEstado,
            documento.RootElement
                .GetProperty("estado")
                .GetString());
    }

    [Theory(DisplayName = "IT-EP027-05 — estado inexistente ou Pendente devolve 400")]
    [InlineData("Faltou")]
    [InlineData("Pendente")]
    public async Task EstadoInvalidoDevolve400(
        string estado)
    {
        var marcacao =
            await ObterMarcacaoAsync(
                EmailVetBruno,
                "Pendente");

        using var cliente =
            await ClienteDeAsync(EmailRececao);

        using var resposta =
            await EnviarPatchAsync(
                cliente,
                marcacao.Id,
                estado);

        await AssertErroAsync(
            resposta,
            HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "IT-EP027-06 — consulta inexistente devolve 404")]
    public async Task ConsultaInexistenteDevolve404()
    {
        using var cliente =
            await ClienteDeAsync(EmailRececao);

        using var resposta =
            await EnviarPatchAsync(
                cliente,
                999999,
                "Confirmada");

        await AssertErroAsync(
            resposta,
            HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "IT-EP027-07 — transição inválida devolve 422")]
    public async Task TransicaoInvalidaDevolve422()
    {
        var marcacao =
            await ObterMarcacaoAsync(
                EmailVetBruno,
                "Pendente");

        using var cliente =
            await ClienteDeAsync(EmailRececao);

        using var resposta =
            await EnviarPatchAsync(
                cliente,
                marcacao.Id,
                "Realizada");

        await AssertErroAsync(
            resposta,
            HttpStatusCode.UnprocessableEntity);

        Assert.Equal(
            "Pendente",
            await ObterEstadoAsync(
                cliente,
                marcacao.Id));
    }

    [Theory(DisplayName = "IT-EP027-08 — estados finais não permitem nova transição")]
    [InlineData(EmailVetAna, "Realizada", "Cancelada")]
    [InlineData(EmailVetBruno, "Cancelada", "Confirmada")]
    public async Task EstadoFinalDevolve422(
        string emailVeterinario,
        string estadoAtual,
        string novoEstado)
    {
        var marcacao =
            await ObterMarcacaoAsync(
                emailVeterinario,
                estadoAtual);

        using var cliente =
            await ClienteDeAsync(EmailRececao);

        using var resposta =
            await EnviarPatchAsync(
                cliente,
                marcacao.Id,
                novoEstado);

        await AssertErroAsync(
            resposta,
            HttpStatusCode.UnprocessableEntity);

        Assert.Equal(
            estadoAtual,
            await ObterEstadoAsync(
                cliente,
                marcacao.Id));
    }

    [Fact(DisplayName = "IT-EP027-09 — If-Match antigo devolve 412 e mantém o estado atual")]
    public async Task IfMatchAntigoDevolve412()
    {
        var marcacao =
            await ObterMarcacaoAsync(
                EmailVetBruno,
                "Pendente");

        using var cliente =
            await ClienteDeAsync(EmailRececao);

        using var leituraInicial =
            await cliente.GetAsync(
                $"/consultas/{marcacao.Id}");

        var etagAntigo =
            leituraInicial.Headers.ETag!.Tag;

        using var primeira =
            await EnviarPatchAsync(
                cliente,
                marcacao.Id,
                "Confirmada");

        Assert.Equal(
            HttpStatusCode.NoContent,
            primeira.StatusCode);

        using var segunda =
            await EnviarPatchAsync(
                cliente,
                marcacao.Id,
                "Cancelada",
                etagAntigo);

        await AssertErroAsync(
            segunda,
            HttpStatusCode.PreconditionFailed);

        Assert.Equal(
            "Confirmada",
            await ObterEstadoAsync(
                cliente,
                marcacao.Id));
    }

    [Fact(DisplayName = "IT-EP027-10 — If-Match atual devolve 204 e novo ETag")]
    public async Task IfMatchAtualDevolveNovoETag()
    {
        var marcacao =
            await ObterMarcacaoAsync(
                EmailVetBruno,
                "Pendente");

        using var cliente =
            await ClienteDeAsync(EmailRececao);

        using var leitura =
            await cliente.GetAsync(
                $"/consultas/{marcacao.Id}");

        var etagAtual =
            Assert.IsType<EntityTagHeaderValue>(
                leitura.Headers.ETag).Tag;

        using var resposta =
            await EnviarPatchAsync(
                cliente,
                marcacao.Id,
                "Confirmada",
                etagAtual);

        Assert.Equal(
            HttpStatusCode.NoContent,
            resposta.StatusCode);

        var novoETag =
            Assert.IsType<EntityTagHeaderValue>(
                resposta.Headers.ETag).Tag;

        Assert.NotEqual(
            etagAtual,
            novoETag);

        using var leituraPosterior =
            await cliente.GetAsync(
                $"/consultas/{marcacao.Id}");

        Assert.Equal(
            novoETag,
            leituraPosterior.Headers.ETag!.Tag);

        Assert.Equal(
            "Confirmada",
            await ObterEstadoAsync(
                cliente,
                marcacao.Id));
    }

    [Fact(DisplayName = "I074 — EP-027 sem token devolve 401")]
    public async Task SemTokenDevolve401()
    {
        using var cliente =
            Fixture.Factory.CreateClient();

        using var resposta =
            await EnviarPatchAsync(
                cliente,
                999999,
                "Confirmada");

        await AssertErroAsync(
            resposta,
            HttpStatusCode.Unauthorized);
    }

    [Theory(DisplayName = "I074 — EP-027 só pode ser usado pela Rececionista")]
    [InlineData("admin@clinica-g08.test")]
    [InlineData(EmailVetAna)]
    [InlineData("aux.carla@clinica-g08.test")]
    [InlineData("cliente.joao@clinica-g08.test")]
    public async Task OutrosPerfisRecebem403(
        string email)
    {
        var marcacao =
            await ObterMarcacaoAsync(
                EmailVetBruno,
                "Pendente");

        using var cliente =
            await ClienteDeAsync(email);

        using var resposta =
            await EnviarPatchAsync(
                cliente,
                marcacao.Id,
                "Confirmada");

        await AssertErroAsync(
            resposta,
            HttpStatusCode.Forbidden);
    }

    // ----- Auxiliares -----

    private sealed record MarcacaoDemo(int Id);

    private static async Task<HttpResponseMessage> EnviarPatchAsync(
        HttpClient cliente,
        int id,
        string estado,
        string? ifMatch = null)
    {
        using var pedido =
            new HttpRequestMessage(
                HttpMethod.Patch,
                $"/consultas/{id}/estado")
            {
                Content = JsonContent.Create(
                    new
                    {
                        estado,
                        observacao = (string?)null
                    })
            };

        if (ifMatch is not null)
        {
            pedido.Headers.TryAddWithoutValidation(
                "If-Match",
                ifMatch);
        }

        return await cliente.SendAsync(pedido);
    }

    private static async Task<JsonDocument> LerAsync(
        HttpResponseMessage resposta) =>
        JsonDocument.Parse(
            await resposta.Content.ReadAsStringAsync());

    private static async Task AssertErroAsync(
        HttpResponseMessage resposta,
        HttpStatusCode esperado)
    {
        Assert.Equal(
            esperado,
            resposta.StatusCode);

        using var documento =
            await LerAsync(resposta);

        Assert.Equal(
            (int)esperado,
            documento.RootElement
                .GetProperty("code")
                .GetInt32());

        Assert.False(
            string.IsNullOrWhiteSpace(
                documento.RootElement
                    .GetProperty("message")
                    .GetString()));

        Assert.False(
            string.IsNullOrWhiteSpace(
                documento.RootElement
                    .GetProperty("detail")
                    .GetString()));
    }

    private static async Task<string?> ObterEstadoAsync(
        HttpClient cliente,
        int id)
    {
        using var resposta =
            await cliente.GetAsync(
                $"/consultas/{id}");

        Assert.Equal(
            HttpStatusCode.OK,
            resposta.StatusCode);

        using var documento =
            await LerAsync(resposta);

        return documento.RootElement
            .GetProperty("estado")
            .GetString();
    }

    private async Task<HttpClient> ClienteDeAsync(
        string email)
    {
        using var scope =
            Fixture.Factory.Services.CreateScope();

        var authService =
            scope.ServiceProvider
                .GetRequiredService<IAuthService>();

        var jwtTokenService =
            scope.ServiceProvider
                .GetRequiredService<IJwtTokenService>();

        var utilizador =
            await authService.ObterAutenticadoAsync(
                await ObterUtilizadorIdAsync(email));

        var token =
            jwtTokenService.GerarToken(utilizador);

        var cliente =
            Fixture.Factory.CreateClient();

        cliente.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token.Token);

        return cliente;
    }

    private async Task<int> ObterUtilizadorIdAsync(
        string email)
    {
        using var scope =
            Fixture.Factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ClinicaDbContext>();

        return await db.Utilizadores
            .AsNoTracking()
            .Where(u => u.Email == email)
            .Select(u => u.Id)
            .SingleAsync();
    }

    private async Task<MarcacaoDemo> ObterMarcacaoAsync(
        string emailVeterinario,
        string estado)
    {
        var veterinarioId =
            await ObterUtilizadorIdAsync(
                emailVeterinario);

        using var scope =
            Fixture.Factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ClinicaDbContext>();

        return await db.Marcacoes
            .AsNoTracking()
            .Where(
                m =>
                    m.VeterinarioId == veterinarioId
                    && m.Estado == estado)
            .Select(
                m => new MarcacaoDemo(m.Id))
            .SingleAsync();
    }
}
