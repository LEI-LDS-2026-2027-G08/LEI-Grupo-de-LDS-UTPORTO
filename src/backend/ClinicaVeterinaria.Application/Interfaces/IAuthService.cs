using ClinicaVeterinaria.Application.DTOs.Auth;

namespace ClinicaVeterinaria.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto pedido);
    Task<UtilizadorAutenticadoDto> ObterAutenticadoAsync(int utilizadorId);
}