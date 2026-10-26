namespace ClinicaVeterinaria.IntegrationTests.Infraestrutura;

/// <summary>
/// Utilitários para criar dados independentes entre testes.
/// </summary>
public static class DadosTeste
{
    public static string EmailUnico(string prefixo = "teste")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prefixo);

        return $"{prefixo}.{Guid.NewGuid():N}@clinica-g08.test";
    }

    public static string NomeUnico(string prefixo = "Teste")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prefixo);

        var sufixo = Guid.NewGuid()
            .ToString("N")[..8];

        return $"{prefixo} {sufixo}";
    }

    public static string TextoUnico(string prefixo = "Teste")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prefixo);

        return $"{prefixo}-{Guid.NewGuid():N}";
    }
}
