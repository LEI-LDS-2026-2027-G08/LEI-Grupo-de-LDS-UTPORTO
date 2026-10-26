using System.Security.Cryptography;
using System.Text.RegularExpressions;
using ClinicaVeterinaria.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Graph;
using Testcontainers.MsSql;

namespace ClinicaVeterinaria.IntegrationTests.Infraestrutura;

public sealed class IntegracaoFixture : IAsyncLifetime
{
    private const string ImagemSqlServer =
        "mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04";

    private readonly MsSqlContainer _sqlServer =
        new MsSqlBuilder(ImagemSqlServer).Build();

    private Respawner? _respawner;

    public ClinicaWebApplicationFactory Factory { get; private set; } = null!;

    public string ConnectionString { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        await _sqlServer.StartAsync();

        var nomeBaseDados =
            $"ClinicaVeterinaria_I091_{Guid.NewGuid():N}";

        await CriarBaseDadosAsync(
            _sqlServer.GetConnectionString(),
            nomeBaseDados);

        var connectionStringBuilder =
            new SqlConnectionStringBuilder(_sqlServer.GetConnectionString())
            {
                InitialCatalog = nomeBaseDados,
                TrustServerCertificate = true
            };

        ConnectionString = connectionStringBuilder.ConnectionString;

        var jwtKey =
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        Factory = new ClinicaWebApplicationFactory(
            ConnectionString,
            jwtKey);

        await AplicarMigracoesAsync();
        await CarregarDadosDemonstracaoAsync();
        await PrepararRespawnAsync();
    }

    public async Task ResetarBaseDadosAsync()
    {
        if (_respawner is null)
        {
            throw new InvalidOperationException(
                "A infraestrutura de testes ainda não foi inicializada.");
        }

        await using var connection =
            new SqlConnection(ConnectionString);

        await connection.OpenAsync();

        await _respawner.ResetAsync(connection);

        await CarregarDadosDemonstracaoAsync();
    }

    private async Task AplicarMigracoesAsync()
    {
        using var scope = Factory.Services.CreateScope();

        var dbContext =
            scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        await dbContext.Database.MigrateAsync();
    }

    private async Task PrepararRespawnAsync()
    {
        await using var connection =
            new SqlConnection(ConnectionString);

        await connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(
            connection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.SqlServer,
                SchemasToInclude = ["dbo"],
                TablesToIgnore =
                [
                    new Table("__EFMigrationsHistory"),
                    new Table("Clinica"),
                    new Table("ConfiguracaoSistema"),
                    new Table("Especie"),
                    new Table("Raca")
                ]
            });
    }

    private async Task CarregarDadosDemonstracaoAsync()
    {
        var caminho =
            Path.Combine(
                LocalizarRaizRepositorio(),
                "04-artefactos-tecnicos",
                "04.13-scripts-de-base-de-dados",
                "m3-v02",
                "dados-demonstracao",
                "02-dados-demonstracao.sql");

        var script = await File.ReadAllTextAsync(caminho);

        var batches = Regex.Split(
            script,
            @"^\s*GO\s*;?\s*$",
            RegexOptions.Multiline | RegexOptions.IgnoreCase);

        await using var connection =
            new SqlConnection(ConnectionString);

        await connection.OpenAsync();

        foreach (var batch in batches)
        {
            if (string.IsNullOrWhiteSpace(batch))
            {
                continue;
            }

            await using var command = connection.CreateCommand();

            command.CommandText = batch;
            command.CommandTimeout = 120;

            await command.ExecuteNonQueryAsync();
        }
    }

    private static async Task CriarBaseDadosAsync(
        string connectionString,
        string nomeBaseDados)
    {
        var builder =
            new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = "master",
                TrustServerCertificate = true
            };

        await using var connection =
            new SqlConnection(builder.ConnectionString);

        await connection.OpenAsync();

        await using var command = connection.CreateCommand();

        command.CommandText =
            $"CREATE DATABASE [{nomeBaseDados}] " +
            "COLLATE Latin1_General_100_CI_AS;";

        await command.ExecuteNonQueryAsync();
    }

    private static string LocalizarRaizRepositorio()
    {
        var pasta = new DirectoryInfo(AppContext.BaseDirectory);

        while (pasta is not null)
        {
            if (Directory.Exists(
                    Path.Combine(
                        pasta.FullName,
                        "04-artefactos-tecnicos")))
            {
                return pasta.FullName;
            }

            pasta = pasta.Parent;
        }

        throw new DirectoryNotFoundException(
            "Não foi possível localizar a raiz do repositório.");
    }

    public async Task DisposeAsync()
    {
        Factory?.Dispose();
        await _sqlServer.DisposeAsync();
    }
}
