package pt.ipp.estg.g08.clinica.features.animais

import pt.ipp.estg.g08.clinica.core.api.ApiClient
import pt.ipp.estg.g08.clinica.core.api.get
import pt.ipp.estg.g08.clinica.features.animais.dto.MeuAnimalDto
import pt.ipp.estg.g08.clinica.features.animais.dto.MeuAnimalListItemDto

private const val ROTA_MEUS_ANIMAIS = "/me/animais"

/**
 * Animais do Cliente autenticado (secção 8 do Modelo de Classes Mobile): EP-019 e EP-020.
 *
 * Nenhum pedido envia o identificador do Cliente: o servidor obtém-no do token (RNF002) e
 * recusa com `403` um animal que não pertença ao Cliente. Os erros chegam como `ApiException`.
 */
class AnimaisService(private val api: ApiClient) {
    /** EP-019 — `GET /me/animais`: só os animais do Cliente; lista vazia se não tiver nenhum. */
    suspend fun listar(): List<MeuAnimalListItemDto> =
        api.get<List<MeuAnimalListItemDto>>(ROTA_MEUS_ANIMAIS)

    /** EP-020 — `GET /me/animais/{id}`: dados básicos de um animal do Cliente. */
    suspend fun obter(animalId: Int): MeuAnimalDto =
        api.get<MeuAnimalDto>("$ROTA_MEUS_ANIMAIS/$animalId")
}
