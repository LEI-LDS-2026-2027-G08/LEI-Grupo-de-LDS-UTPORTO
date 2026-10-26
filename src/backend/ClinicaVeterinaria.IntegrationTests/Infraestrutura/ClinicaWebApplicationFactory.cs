using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ClinicaVeterinaria.IntegrationTests.Infraestrutura;

/// <summary>
/// Arranca a API real em memória para os testes de integração.
/// A ligação à base de dados e a configuração JWT são fornecidas pela fixture
/// de testes e nunca dependem da configuração local do programador.
/// </summary>
public sealed class ClinicaWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly KeyValuePair<string, string?>[] _configuracao;

    public ClinicaWebApplicationFactory(string connectionString, string jwtKey)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException(
                "A cadeia de ligação da base de dados de testes é obrigatória.",
                nameof(connectionString));
        }

        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new ArgumentException(
                "A chave JWT de testes é obrigatória.",
                nameof(jwtKey));
        }

        _configuracao =
        [
            new("ConnectionStrings:ClinicaDb", connectionString),
            new("Jwt:Key", jwtKey),
            new("Jwt:Issuer", "ClinicaVeterinaria.Api"),
            new("Jwt:Audience", "ClinicaVeterinaria.Clients"),
            new("Jwt:ExpirationHours", "8")
        ];
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(configuracao =>
            configuracao.AddInMemoryCollection(_configuracao));

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }
}
