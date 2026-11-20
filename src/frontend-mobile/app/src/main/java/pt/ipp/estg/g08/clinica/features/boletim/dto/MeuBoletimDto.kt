package pt.ipp.estg.g08.clinica.features.boletim.dto

import kotlinx.serialization.Serializable

/**
 * Resposta de `GET /me/animais/{id}/vacinacoes` (EP-044): histórico de vacinação e
 * desparasitação do animal e próximas administrações previstas. A ordem das listas é a do
 * servidor (histórico da mais recente para a mais antiga; próximas da mais próxima para a mais
 * distante). Sem registos, as duas listas vêm vazias; essa situação é tratada pelo
 * `BoletimViewModel` (secção 9.5 do Modelo de Classes Mobile).
 */
@Serializable
data class MeuBoletimDto(
    val historico: List<HistoricoAdministracaoDto> = emptyList(),
    val proximas: List<ProximaAdministracaoDto> = emptyList(),
)
