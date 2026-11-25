package pt.ipp.estg.g08.clinica.features.boletim

import pt.ipp.estg.g08.clinica.core.api.ApiClient
import pt.ipp.estg.g08.clinica.core.api.get
import pt.ipp.estg.g08.clinica.features.boletim.dto.MeuBoletimDto

/**
 * Boletim de vacinação e desparasitação de um animal do Cliente autenticado (secção 8 do
 * Modelo de Classes Mobile): EP-044.
 *
 * O servidor confirma que o animal pertence ao Cliente do token (RNF002) e calcula as próximas
 * administrações (RF045). Os erros chegam como `ApiException`.
 */
class BoletimService(private val api: ApiClient) {
    /** EP-044 — `GET /me/animais/{id}/vacinacoes`. */
    suspend fun obter(animalId: Int): MeuBoletimDto =
        api.get<MeuBoletimDto>("/me/animais/$animalId/vacinacoes")
}
