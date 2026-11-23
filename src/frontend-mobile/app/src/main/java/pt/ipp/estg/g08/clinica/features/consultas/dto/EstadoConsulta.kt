package pt.ipp.estg.g08.clinica.features.consultas.dto

import kotlinx.serialization.Serializable

/**
 * Estado de uma marcação. Os valores coincidem com os da API (secção 2.2 da Documentação da API);
 * [rotulo] é o texto apresentado ao Cliente (secção 9.1 do Modelo de Classes Mobile).
 */
@Serializable
enum class EstadoConsulta(val rotulo: String) {
    Pendente("Pendente"),
    Confirmada("Confirmada"),
    Realizada("Realizada"),
    Cancelada("Cancelada"),
}
