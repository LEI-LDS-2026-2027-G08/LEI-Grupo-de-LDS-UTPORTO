package pt.ipp.estg.g08.clinica.sessao

import kotlinx.serialization.Serializable

/** Utilizador devolvido por `POST /auth/login` (EP-001). */
@Serializable
data class UtilizadorResumoDto(
    val id: Int,
    val nome: String,
    val email: String,
    val perfil: Perfil,
)
