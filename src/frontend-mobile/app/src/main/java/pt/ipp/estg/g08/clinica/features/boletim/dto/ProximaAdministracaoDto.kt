package pt.ipp.estg.g08.clinica.features.boletim.dto

import java.time.LocalDate
import kotlinx.serialization.Serializable
import pt.ipp.estg.g08.clinica.core.api.LocalDateSerializer

/**
 * Próxima administração prevista (lista `proximas` de EP-044). A data é calculada pelo servidor
 * (RF045); a aplicação só a apresenta.
 */
@Serializable
data class ProximaAdministracaoDto(
    val tipo: TipoAdministracao,
    val produto: String,
    @Serializable(with = LocalDateSerializer::class)
    val dataProximaPrevista: LocalDate,
)
