package pt.ipp.estg.g08.clinica.features.boletim.dto

import java.time.LocalDate
import kotlinx.serialization.Serializable
import pt.ipp.estg.g08.clinica.core.api.LocalDateSerializer

/** Vacinação ou desparasitação já administrada (lista `historico` de EP-044). */
@Serializable
data class HistoricoAdministracaoDto(
    val tipo: TipoAdministracao,
    val produto: String,
    @Serializable(with = LocalDateSerializer::class)
    val dataAdministracao: LocalDate,
)
