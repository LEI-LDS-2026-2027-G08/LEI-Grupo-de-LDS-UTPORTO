package pt.ipp.estg.g08.clinica.sessao

import pt.ipp.estg.g08.clinica.core.api.ApiClient
import pt.ipp.estg.g08.clinica.core.api.get
import pt.ipp.estg.g08.clinica.core.api.post

private const val ROTA_LOGIN = "/auth/login"
private const val ROTA_LOGOUT = "/auth/logout"
private const val ROTA_ME = "/auth/me"

/**
 * Operações de sessão da API (secção 8 do Modelo de Classes Mobile): EP-001, EP-002 e EP-003.
 * Os erros chegam como `ApiException`; a decisão sobre o que fazer com eles é do
 * [SessaoViewModel].
 */
class AuthService(private val api: ApiClient) {
    /** EP-001 — `POST /auth/login`. */
    suspend fun login(pedido: LoginRequestDto): LoginResponseDto =
        api.post<LoginRequestDto, LoginResponseDto>(ROTA_LOGIN, pedido)

    /** EP-002 — `POST /auth/logout`: revoga no servidor o token enviado no pedido. */
    suspend fun logout() {
        api.postSemResposta(ROTA_LOGOUT)
    }

    /** EP-003 — `GET /auth/me`: confirma o token e devolve o utilizador e o perfil atuais. */
    suspend fun me(): UtilizadorAutenticadoDto = api.get<UtilizadorAutenticadoDto>(ROTA_ME)
}
