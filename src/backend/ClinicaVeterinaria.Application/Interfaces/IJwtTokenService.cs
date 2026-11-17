using ClinicaVeterinaria.Application.Models.Auth;

namespace ClinicaVeterinaria.Application.Interfaces;

/// <summary>
/// Emissão dos JWT (ARC-SEC). A validação dos tokens é feita pelo
/// middleware JwtBearer do ASP.NET Core, configurado na API.
/// </summary>
public interface IJwtTokenService
{
    TokenEmitido GerarToken(UtilizadorAutenticado utilizador);
}
