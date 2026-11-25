package pt.ipp.estg.g08.clinica.sessao

const val MSG_EMAIL_EM_FALTA = "Indique o email."
const val MSG_EMAIL_INVALIDO = "Indique um email válido."
const val MSG_PASSWORD_EM_FALTA = "Indique a palavra-passe."

private val FORMATO_EMAIL = Regex("^[^\\s@]+@[^\\s@]+\\.[^\\s@]+$")

/** Erros de preenchimento do formulário de início de sessão (por campo). */
data class ErrosLogin(val email: String? = null, val password: String? = null) {
    val temErros: Boolean get() = email != null || password != null
}

/**
 * Validação local do formulário M01 (secção 12.1 do Modelo de Classes Mobile): email preenchido
 * e com formato válido e palavra-passe preenchida. Não substitui a validação da API.
 */
object ValidacaoLogin {
    fun validar(email: String, password: String): ErrosLogin = ErrosLogin(
        email = validarEmail(email),
        password = validarPassword(password),
    )

    fun validarEmail(valor: String): String? {
        val email = valor.trim()
        return when {
            email.isEmpty() -> MSG_EMAIL_EM_FALTA
            !FORMATO_EMAIL.matches(email) -> MSG_EMAIL_INVALIDO
            else -> null
        }
    }

    fun validarPassword(valor: String): String? =
        if (valor.isBlank()) MSG_PASSWORD_EM_FALTA else null
}
