namespace ClinicaVeterinaria.Application.DTOs.Auth;

/// <summary>Resultado da emissão de um JWT.</summary>
/// <param name="Token">Token assinado (HS256).</param>
/// <param name="ExpiraEm">Instante de expiração, em UTC e sem frações de segundo (igual à claim <c>exp</c>).</param>
/// <param name="Jti">Identificador único do token, usado na revogação (I064).</param>
public sealed record TokenEmitido(string Token, DateTime ExpiraEm, string Jti);
