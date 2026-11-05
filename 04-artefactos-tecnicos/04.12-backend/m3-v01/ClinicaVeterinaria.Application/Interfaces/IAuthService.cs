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

    /// <summary>
    /// EP-002 — termina a sessão: regista o <paramref name="jti"/> do token como revogado
    /// até <paramref name="dataExpiracao"/> (expiração original do token). A partir daí,
    /// <see cref="SessaoValidaAsync"/> devolve <see langword="false"/> para esse token,
    /// também depois de a API reiniciar. Repetir o pedido com o mesmo <paramref name="jti"/>
    /// não tem efeito (operação idempotente).
    /// </summary>
    Task TerminarSessaoAsync(int utilizadorId, string jti, DateTime dataExpiracao);

    /// <summary>
    /// Apaga as revogações de tokens cuja expiração já passou: esses tokens já são
    /// rejeitados pela validação da data de expiração. Devolve o número de registos apagados.
    /// </summary>
    Task<int> RemoverRevogacoesExpiradasAsync(CancellationToken cancelamento = default);
}
