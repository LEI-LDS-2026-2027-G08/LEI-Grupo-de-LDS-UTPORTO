package pt.ipp.estg.g08.clinica.core.api

import kotlinx.serialization.json.Json

/** Configuração única de JSON usada na comunicação com a API e no armazenamento da sessão. */
object ApiJson {
    val json: Json =
        Json {
            ignoreUnknownKeys = true
            encodeDefaults = true
        }
}
