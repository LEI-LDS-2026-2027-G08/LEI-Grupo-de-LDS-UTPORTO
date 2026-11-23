package pt.ipp.estg.g08.clinica.features.consultas

import pt.ipp.estg.g08.clinica.core.api.ApiClient
import pt.ipp.estg.g08.clinica.core.api.get
import pt.ipp.estg.g08.clinica.features.consultas.dto.MinhaConsultaDto
import pt.ipp.estg.g08.clinica.features.consultas.dto.TipoConsultaFiltro

/**
 * Marcações dos animais do Cliente autenticado (secção 8 do Modelo de Classes Mobile): EP-030.
 *
 * O servidor identifica o Cliente pelo token (RNF002), decide o que é futuro e o que é
 * histórico e devolve as listas já ordenadas. Um `animalId` de outro Cliente dá `403`. Os erros
 * chegam como `ApiException`.
 */
class ConsultasService(private val api: ApiClient) {
    /** EP-030 — `GET /me/consultas?tipo=&animalId=`; sem [animalId], todos os animais. */
    suspend fun listar(tipo: TipoConsultaFiltro, animalId: Int? = null): List<MinhaConsultaDto> =
        api.get<List<MinhaConsultaDto>>(
            "/me/consultas",
            buildMap {
                put("tipo", tipo.valorApi)
                if (animalId != null) put("animalId", animalId.toString())
            },
        )
}
