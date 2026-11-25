package pt.ipp.estg.g08.clinica.features.consultas.dto

import kotlinx.serialization.Serializable

/** Referência ao animal de uma marcação (`animal` de EP-030): só identificação, sem dados clínicos. */
@Serializable
data class AnimalRefDto(val id: Int, val nome: String, val especie: String? = null)
