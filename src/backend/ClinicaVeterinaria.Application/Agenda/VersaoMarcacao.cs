namespace ClinicaVeterinaria.Application.Agenda;

/// <summary>
/// Versão de uma marcação exposta como <c>ETag</c> (Documentação de API v02, secção 5.1): o valor
/// de <c>Marcacao.RowVersion</c> em Base64, entre aspas. EP-024 devolve-a no cabeçalho
/// <c>ETag</c>; os DTOs não têm um campo de versão.
/// </summary>
/// <remarks>
/// A comparação com o cabeçalho <c>If-Match</c> (412 em EP-026 a EP-028, casos UT-F003-086 a
/// UT-F003-090) fica para a I074, que acrescenta essa operação a esta classe.
/// </remarks>
public static class VersaoMarcacao
{
    /// <summary>ETag forte da versão: por exemplo, <c>00 00 00 00 00 00 07 D1</c> dá <c>"AAAAAAAAB9E="</c>.</summary>
    public static string ParaETag(byte[] rowVersion)
    {
        ArgumentNullException.ThrowIfNull(rowVersion);

        return "\"" + Convert.ToBase64String(rowVersion) + "\"";
    }
}
