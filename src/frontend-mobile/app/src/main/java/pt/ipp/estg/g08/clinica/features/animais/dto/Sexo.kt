package pt.ipp.estg.g08.clinica.features.animais.dto

import kotlinx.serialization.Serializable

/**
 * Sexo do animal. Os valores coincidem com os da API (secção 2.2 da Documentação da API);
 * [rotulo] é o texto apresentado ao Cliente (secção 9.1 do Modelo de Classes Mobile).
 */
@Serializable
enum class Sexo(val rotulo: String) {
    Macho("Macho"),
    Femea("Fêmea"),
    Desconhecido("Desconhecido"),
}
