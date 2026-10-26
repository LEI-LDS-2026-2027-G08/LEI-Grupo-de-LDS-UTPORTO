using Microsoft.Data.SqlClient;

namespace ClinicaVeterinaria.IntegrationTests.Infraestrutura;

public sealed class InfraestruturaTests : TesteIntegracaoBase, IClassFixture<IntegracaoFixture>
{
    public InfraestruturaTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    [Fact(DisplayName = "I091-INF-001 — SQL Server isolado é criado, migrado e populado")]
    public async Task BaseDeDadosIsoladaECriadaComDadosDemonstracao()
    {
        await using var connection =
            new SqlConnection(Fixture.ConnectionString);

        await connection.OpenAsync();

        await using var command = connection.CreateCommand();

        command.CommandText = """
            SELECT
                DB_NAME(),
                (SELECT COUNT(*) FROM dbo.Clinica),
                (SELECT COUNT(*) FROM dbo.Utilizador);
            """;

        await using var reader = await command.ExecuteReaderAsync();

        Assert.True(await reader.ReadAsync());

        var nomeBaseDados = reader.GetString(0);
        var totalClinicas = reader.GetInt32(1);
        var totalUtilizadores = reader.GetInt32(2);

        Assert.StartsWith(
            "ClinicaVeterinaria_I091_",
            nomeBaseDados);

        Assert.Equal(1, totalClinicas);
        Assert.True(totalUtilizadores >= 10);
    }

    [Fact(DisplayName = "I091-INF-002 — alterações de um teste são removidas pelo reset")]
    public async Task ResetRemoveDadosCriadosDuranteTeste()
    {
        const string email = "isolamento.i091@clinica-g08.test";

        await using (var connection =
            new SqlConnection(Fixture.ConnectionString))
        {
            await connection.OpenAsync();

            await using var inserir = connection.CreateCommand();

            inserir.CommandText = """
                INSERT INTO dbo.Utilizador
                    (Email, PasswordHash, Perfil, Estado)
                VALUES
                    (@email, N'hash-teste', N'Administrador', N'Ativo');
                """;

            inserir.Parameters.AddWithValue("@email", email);

            await inserir.ExecuteNonQueryAsync();
        }

        await Fixture.ResetarBaseDadosAsync();

        await using var verificacao =
            new SqlConnection(Fixture.ConnectionString);

        await verificacao.OpenAsync();

        await using var command = verificacao.CreateCommand();

        command.CommandText = """
            SELECT COUNT(*)
            FROM dbo.Utilizador
            WHERE Email = @email;
            """;

        command.Parameters.AddWithValue("@email", email);

        var total = Convert.ToInt32(await command.ExecuteScalarAsync(), System.Globalization.CultureInfo.InvariantCulture);

        Assert.Equal(0, total);
    }
}
