using System.Net;
using System.Text.Json;

namespace ClinicaVeterinaria.IntegrationTests.Infraestrutura;

public sealed class AutenticacaoInfraestruturaTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    public AutenticacaoInfraestruturaTests(
        IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    [Theory(
        DisplayName =
            "IT-EP003-01 / I091 — cliente autenticado por perfil acede a /auth/me")]
    [InlineData("Administrador")]
    [InlineData("Rececionista")]
    [InlineData("Veterinario")]
    [InlineData("AuxiliarVeterinario")]
    [InlineData("Cliente")]
    public async Task ClienteAutenticadoPorPerfilAcedeAAuthMe(
        string perfil)
    {
        using var cliente =
            await Fixture.CriarClienteAutenticadoAsync(perfil);

        using var resposta =
            await cliente.GetAsync("/auth/me");

        Assert.Equal(
            HttpStatusCode.OK,
            resposta.StatusCode);

        var json =
            await resposta.Content.ReadAsStringAsync();

        using var documento =
            JsonDocument.Parse(json);

        Assert.Equal(
            perfil,
            documento.RootElement
                .GetProperty("perfil")
                .GetString());
    }
}
