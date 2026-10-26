using ClinicaVeterinaria.Application.DTOs.Auth;

namespace ClinicaVeterinaria.Application.Interfaces;

public interface IAuthService
{
    /// <summary>EP-001 — valida as credenciais e emite o JWT.</summary>
    Task<LoginResponseDto> LoginAsync(LoginRequestDto pedido);

    /// <summary>EP-003 — devolve o utilizador associado ao token, com o perfil lido da base de dados.</summary>
    Task<UtilizadorAutenticadoDto> ObterAutenticadoAsync(int utilizadorId);

    /// <summary>
    /// Confirma, em cada pedido autenticado, que a conta do token continua ativa,
    /// que o perfil do token é o perfil atual da conta e que o token não foi revogado.
    /// </summary>
    Task<bool> SessaoValidaAsync(int utilizadorId, string perfil, string jti);
}
