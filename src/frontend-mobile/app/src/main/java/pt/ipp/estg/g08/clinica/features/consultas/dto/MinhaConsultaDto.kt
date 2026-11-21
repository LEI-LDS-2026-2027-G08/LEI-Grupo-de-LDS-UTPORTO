package pt.ipp.estg.g08.clinica.features.consultas.dto

import java.time.Instant
import kotlinx.serialization.Serializable
import pt.ipp.estg.g08.clinica.core.api.InstantSerializer

/**
 * Marcação de um animal do Cliente (resposta de EP-030; secção 9.4 do Modelo de Classes
 * Mobile). Só tem informação administrativa — data e hora, estado, animal e Veterinário — e
 * nenhum registo clínico (RF029).
 */
@Serializable
data class MinhaConsultaDto(
    val id: Int,
    @Serializable(with = InstantSerializer::class)
    val dataHora: Instant,
    val estado: EstadoConsulta,
    val animal: AnimalRefDto,
    val veterinario: String,
)
