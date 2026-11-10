package pt.ipp.estg.g08.clinica.core.api

import kotlinx.serialization.Serializable

/** Formato de erro da API v03: `{ code, message, detail }` (secção 5 da Documentação da API). */
@Serializable
internal data class ErroApiDto(
    val code: Int = 0,
    val message: String = "",
    val detail: String? = null,
)
