package pt.ipp.estg.g08.clinica.sessao

import java.time.Instant
import kotlinx.serialization.Serializable
import pt.ipp.estg.g08.clinica.core.api.InstantSerializer

/**
 * Sessão do Cliente: token, momento de expiração e utilizador (devolvidos por EP-001).
 *
 * É a única informação que a aplicação guarda no dispositivo, sempre cifrada (ver `TokenStorage`).
 */
@Serializable
data class SessaoAutenticada(
    val token: String,
    @Serializable(with = InstantSerializer::class)
    val expiraEm: Instant,
    val utilizador: UtilizadorResumoDto,
) {
    fun expirada(agora: Instant = Instant.now()): Boolean = !agora.isBefore(expiraEm)

    /** Nunca expõe o token (por exemplo, em registos). */
    override fun toString(): String =
        "SessaoAutenticada(token=***, expiraEm=$expiraEm, utilizador=$utilizador)"

    companion object {
        fun deLogin(resposta: LoginResponseDto): SessaoAutenticada = SessaoAutenticada(
            token = resposta.token,
            expiraEm = resposta.expiraEm,
            utilizador = resposta.utilizador,
        )
    }
}
