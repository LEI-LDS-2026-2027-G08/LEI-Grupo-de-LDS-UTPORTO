package pt.ipp.estg.g08.clinica

import android.content.Context
import pt.ipp.estg.g08.clinica.core.api.ApiClient
import pt.ipp.estg.g08.clinica.core.config.AppConfig
import pt.ipp.estg.g08.clinica.core.storage.CifraKeystore
import pt.ipp.estg.g08.clinica.core.storage.SharedPreferencesArmazenamento
import pt.ipp.estg.g08.clinica.core.storage.TokenStorage
import pt.ipp.estg.g08.clinica.core.storage.TokenStorageCifrado
import pt.ipp.estg.g08.clinica.sessao.AuthService
import pt.ipp.estg.g08.clinica.sessao.MotivoFimSessao
import pt.ipp.estg.g08.clinica.sessao.SessaoLocal

private const val FICHEIRO_SESSAO = "sessao"

/**
 * Contentor de dependências (DM-05): injeção manual pelo construtor, sem framework.
 * Criado uma vez em [ClinicaApplication]; os testes constroem os seus próprios objetos.
 */
class AppContainer(contexto: Context) {
    val config: AppConfig = AppConfig.deBuildConfig()

    val apiClient: ApiClient = ApiClient(config)

    val tokenStorage: TokenStorage =
        TokenStorageCifrado(
            cifra = CifraKeystore(),
            armazenamento = SharedPreferencesArmazenamento(contexto, FICHEIRO_SESSAO),
        )

    val sessaoLocal: SessaoLocal = SessaoLocal(tokenStorage, apiClient)

    val authService: AuthService = AuthService(apiClient)

    init {
        // Resposta 401 a um pedido autenticado: a sessão terminou no servidor ou expirou.
        apiClient.onNaoAutenticado = { sessaoLocal.terminar(MotivoFimSessao.NaoAutenticado) }
    }
}
