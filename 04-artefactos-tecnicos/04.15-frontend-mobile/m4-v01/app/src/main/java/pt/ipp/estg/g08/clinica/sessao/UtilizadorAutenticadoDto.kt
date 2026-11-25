package pt.ipp.estg.g08.clinica.sessao

import kotlinx.serialization.Serializable

/**
 * Resposta de `GET /auth/me` (EP-003). `clienteId` só vem preenchido para o perfil Cliente e a
 * aplicação nunca o envia nos pedidos (o servidor identifica o Cliente pelo token, RNF002).
 */
@Serializable
data class UtilizadorAutenticadoDto(
    val id: Int,
    val nome: String,
    val email: String,
    val perfil: Perfil,
    val clienteId: Int? = null,
) {
    fun resumo(): UtilizadorResumoDto =
        UtilizadorResumoDto(id = id, nome = nome, email = email, perfil = perfil)
}
