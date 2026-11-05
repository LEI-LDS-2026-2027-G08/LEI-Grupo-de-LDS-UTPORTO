namespace ClinicaVeterinaria.Api.Seguranca;

/// <summary>
/// Configuração da limpeza periódica das revogações de tokens expirados (I064),
/// lida da secção <c>LimpezaTokensRevogados</c>.
/// </summary>
public sealed class LimpezaTokensRevogadosOptions
{
    public const string Seccao = "LimpezaTokensRevogados";

    public const int IntervaloMinimoMinutos = 1;

    public const int IntervaloMaximoMinutos = 1440;

    /// <summary>
    /// Minutos entre duas limpezas, de 1 a 1440 (24 horas). Por omissão, 60.
    /// A primeira limpeza é feita logo no arranque da API.
    /// </summary>
    public int IntervaloMinutos { get; set; } = 60;
}
