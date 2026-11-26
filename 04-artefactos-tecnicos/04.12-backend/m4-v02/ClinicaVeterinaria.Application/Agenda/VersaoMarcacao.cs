namespace ClinicaVeterinaria.Application.Agenda;

/// <summary>
/// Versão de uma marcação exposta como ETag.
/// </summary>
public static class VersaoMarcacao
{
    public static string ParaETag(byte[] rowVersion)
    {
        ArgumentNullException.ThrowIfNull(rowVersion);

        return "\"" + Convert.ToBase64String(rowVersion) + "\"";
    }

    /// <summary>
    /// Verifica o cabeçalho If-Match usando comparação forte.
    /// Sem If-Match, a operação é aceite.
    /// "*" corresponde sempre quando o recurso existe.
    /// </summary>
    public static bool CorrespondeIfMatch(byte[] rowVersion, string? ifMatch)
    {
        ArgumentNullException.ThrowIfNull(rowVersion);

        if (string.IsNullOrWhiteSpace(ifMatch))
        {
            return true;
        }

        var valor = ifMatch.Trim();

        if (valor == "*")
        {
            return true;
        }

        // If-Match exige comparação forte: ETags fracos nunca correspondem.
        if (valor.StartsWith("W/", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var atual = ParaETag(rowVersion);

        return string.Equals(valor, atual, StringComparison.Ordinal);
    }
}
