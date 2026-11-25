package pt.ipp.estg.g08.clinica.sessao

import java.time.Instant
import kotlinx.serialization.Serializable
import pt.ipp.estg.g08.clinica.core.api.InstantSerializer

/** Resposta de `POST /auth/login` (EP-001): token JWT, expiração e utilizador. */
@Serializable
data class LoginResponseDto(
    val token: String,
    @Serializable(with = InstantSerializer::class)
    val expiraEm: Instant,
    val utilizador: UtilizadorResumoDto,
) {
    /** Nunca expõe o token. */
    override fun toString(): String =
        "LoginResponseDto(token=***, expiraEm=$expiraEm, utilizador=$utilizador)"
}
