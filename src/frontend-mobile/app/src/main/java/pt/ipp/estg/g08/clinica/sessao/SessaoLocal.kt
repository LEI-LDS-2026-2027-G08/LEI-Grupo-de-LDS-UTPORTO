package pt.ipp.estg.g08.clinica.sessao

import kotlinx.coroutines.channels.Channel
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.receiveAsFlow
import pt.ipp.estg.g08.clinica.core.api.ApiClient
import pt.ipp.estg.g08.clinica.core.storage.TokenStorage

/**
 * Fim da sessão no dispositivo: remove o token do `ApiClient` e do armazenamento e emite um
 * evento em [sessaoTerminada] (por exemplo, para a navegação regressar ao início de sessão).
 *
 * Não chama a API: a revogação no servidor (`POST /auth/logout`) é feita pelo
 * `SessaoViewModel` antes de terminar a sessão local.
 * Cada evento é entregue uma única vez; se ainda não houver quem o observe, fica em espera.
 */
class SessaoLocal(private val tokenStorage: TokenStorage, private val apiClient: ApiClient) {
    private val eventos = Channel<MotivoFimSessao>(Channel.BUFFERED)

    val sessaoTerminada: Flow<MotivoFimSessao> = eventos.receiveAsFlow()

    suspend fun terminar(motivo: MotivoFimSessao) {
        apiClient.definirToken(null)
        tokenStorage.limpar()
        eventos.send(motivo)
    }
}
