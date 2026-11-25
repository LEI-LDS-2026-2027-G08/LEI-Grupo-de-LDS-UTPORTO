package pt.ipp.estg.g08.clinica.features.animais.dto

import java.time.LocalDate
import kotlinx.serialization.Serializable
import pt.ipp.estg.g08.clinica.core.api.LocalDateSerializer

/**
 * Resposta de `GET /me/animais/{id}` (EP-020): dados básicos de um animal do Cliente
 * autenticado (UC08, passo 5). Não inclui o proprietário nem dados clínicos.
 */
@Serializable
data class MeuAnimalDto(
    val id: Int,
    val nome: String,
    val especie: String,
    val raca: String? = null,
    val sexo: Sexo,
    @Serializable(with = LocalDateSerializer::class)
    val dataNascimento: LocalDate? = null,
    val idadeAnos: Int? = null,
    val peso: Double? = null,
)
