namespace ClinicaVeterinaria.IntegrationTests.Infraestrutura;

/// <summary>
/// Base comum dos testes de integração.
/// Garante que cada teste começa com o mesmo estado conhecido da base de dados.
/// </summary>
public abstract class TesteIntegracaoBase : IAsyncLifetime
{
    protected TesteIntegracaoBase(IntegracaoFixture fixture)
    {
        Fixture = fixture;
    }

    protected IntegracaoFixture Fixture { get; }

    public Task InitializeAsync()
    {
        return Fixture.ResetarBaseDadosAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
