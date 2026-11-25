package pt.ipp.estg.g08.clinica.features.animais.dto

import kotlinx.serialization.Serializable

/**
 * Item da lista de `GET /me/animais` (EP-019): dados básicos de um animal do Cliente
 * autenticado. Não inclui o proprietário nem dados clínicos.
 *
 * `idadeAnos` é opcional porque a API só a calcula quando a data de nascimento é conhecida; o
 * peso, em quilogramas, também é opcional.
 */
@Serializable
data class MeuAnimalListItemDto(
    val id: Int,
    val nome: String,
    val especie: String,
    val raca: String? = null,
    val idadeAnos: Int? = null,
    val peso: Double? = null,
)
