package pt.ipp.estg.g08.clinica.sessao

import kotlinx.serialization.Serializable

/** Pedido de `POST /auth/login` (EP-001). */
@Serializable
data class LoginRequestDto(val email: String, val password: String) {
    /** Nunca expõe a palavra-passe (por exemplo, em registos ou mensagens de erro). */
    override fun toString(): String = "LoginRequestDto(email=$email, password=***)"
}
