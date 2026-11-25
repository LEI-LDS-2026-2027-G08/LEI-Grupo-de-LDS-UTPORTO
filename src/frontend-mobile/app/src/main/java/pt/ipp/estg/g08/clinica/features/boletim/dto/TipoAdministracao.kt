package pt.ipp.estg.g08.clinica.features.boletim.dto

import kotlinx.serialization.Serializable

/**
 * Tipo de administração do boletim. Os valores coincidem com os da API (secção 2.2 da
 * Documentação da API); [rotulo] é o texto apresentado ao Cliente (secção 9.1 do Modelo de
 * Classes Mobile).
 */
@Serializable
enum class TipoAdministracao(val rotulo: String) {
    Vacina("Vacina"),
    Desparasitacao("Desparasitação"),
}
