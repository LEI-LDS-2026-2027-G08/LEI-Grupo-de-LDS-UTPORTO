using System.Net.Http.Headers;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.Infraestrutura;

public static class AutenticacaoTeste
{
    public static async Task<HttpClient> CriarClienteAutenticadoAsync(
        this IntegracaoFixture fixture,
        string perfil)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        if (string.IsNullOrWhiteSpace(perfil))
        {
            throw new ArgumentException(
                "O perfil é obrigatório.",
                nameof(perfil));
        }

        using var scope = fixture.Factory.Services.CreateScope();

        var dbContext =
            scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        var utilizadorId = await dbContext.Utilizadores
            .AsNoTracking()
            .Where(utilizador =>
                utilizador.Perfil == perfil &&
                utilizador.Estado == "Ativo")
            .Select(utilizador => utilizador.Id)
            .FirstOrDefaultAsync();

        if (utilizadorId == 0)
        {
            throw new InvalidOperationException(
                $"Não existe uma conta ativa de demonstração para o perfil '{perfil}'.");
        }

        var authService =
            scope.ServiceProvider.GetRequiredService<IAuthService>();

        var jwtTokenService =
            scope.ServiceProvider.GetRequiredService<IJwtTokenService>();

        var utilizadorAutenticado =
            await authService.ObterAutenticadoAsync(utilizadorId);

        var token =
            jwtTokenService.GerarToken(utilizadorAutenticado);

        var cliente = fixture.Factory.CreateClient();

        cliente.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token.Token);

        return cliente;
    }
}
