package pt.ipp.estg.g08.clinica.features.tecnico

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import java.time.Duration
import java.time.Instant
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import kotlinx.serialization.json.JsonObject
import pt.ipp.estg.g08.clinica.core.api.ApiClient
import pt.ipp.estg.g08.clinica.core.api.ApiException
import pt.ipp.estg.g08.clinica.core.api.get
import pt.ipp.estg.g08.clinica.core.config.AppConfig
import pt.ipp.estg.g08.clinica.core.storage.TokenStorage
import pt.ipp.estg.g08.clinica.sessao.Perfil
import pt.ipp.estg.g08.clinica.sessao.SessaoAutenticada
import pt.ipp.estg.g08.clinica.sessao.UtilizadorResumoDto
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento

private const val HORAS_VALIDADE_TESTE = 8L

/** O que o ecrã técnico mostra. Nunca contém o token. */
data class EcraTecnicoEstado(
    val urlApi: String,
    val temSessao: Boolean = false,
    val perfil: Perfil? = null,
    val expiraEm: Instant? = null,
    val teste: EstadoCarregamento<Unit> = EstadoCarregamento.Inicial,
)

/**
 * Ecrã de diagnóstico da base (apenas em desenvolvimento): mostra a URL da API configurada e se
 * há sessão guardada, e permite exercitar o armazenamento cifrado, o tratamento de `401` e de
 * falhas de rede. Não faz login: o início de sessão é feito no ecrã M01.
 */
class EcraTecnicoViewModel(
    config: AppConfig,
    private val tokenStorage: TokenStorage,
    private val apiClient: ApiClient,
    private val agora: () -> Instant = { Instant.now() },
) : ViewModel() {
    private val estadoInterno = MutableStateFlow(EcraTecnicoEstado(urlApi = config.apiBaseUrl))
    val estado: StateFlow<EcraTecnicoEstado> = estadoInterno.asStateFlow()

    init {
        atualizar()
    }

    fun atualizar() {
        viewModelScope.launch { carregarSessao() }
    }

    /** Guarda uma sessão fictícia (token inválido) para exercitar o armazenamento cifrado. */
    fun guardarSessaoDeTeste() {
        viewModelScope.launch {
            tokenStorage.guardar(sessaoDeTeste(agora()))
            carregarSessao()
        }
    }

    /** Apaga a sessão guardada e o token em memória. */
    fun limparSessao() {
        viewModelScope.launch {
            apiClient.definirToken(null)
            tokenStorage.limpar()
            carregarSessao()
        }
    }

    /** `GET /auth/me`: com a API desligada mostra erro de rede; com token inválido, a API dá 401. */
    fun testarLigacao() {
        viewModelScope.launch {
            estadoInterno.update { it.copy(teste = EstadoCarregamento.ACarregar) }
            apiClient.definirToken(tokenStorage.obter()?.token)
            val resultado: EstadoCarregamento<Unit> =
                try {
                    apiClient.get<JsonObject>("/auth/me")
                    EstadoCarregamento.Sucesso(Unit)
                } catch (e: ApiException) {
                    EstadoCarregamento.Erro(textoDoErro(e))
                }
            estadoInterno.update { it.copy(teste = resultado) }
            carregarSessao()
        }
    }

    private suspend fun carregarSessao() {
        val sessao = tokenStorage.obter()
        estadoInterno.update {
            it.copy(
                temSessao = sessao != null,
                perfil = sessao?.utilizador?.perfil,
                expiraEm = sessao?.expiraEm,
            )
        }
    }

    private fun textoDoErro(erro: ApiException): String = if (erro.semLigacao) {
        // Ecrã só de depuração: mostra também a causa técnica (timeout, recusada, DNS...).
        val causa = erro.cause?.let { " [${it.javaClass.simpleName}: ${it.message}]" }.orEmpty()
        erro.mensagemParaUtilizador() + causa
    } else {
        "${erro.status}: ${erro.mensagemParaUtilizador()}"
    }

    private fun sessaoDeTeste(agora: Instant): SessaoAutenticada = SessaoAutenticada(
        token = "token-de-teste-sem-validade",
        expiraEm = agora.plus(Duration.ofHours(HORAS_VALIDADE_TESTE)),
        utilizador =
        UtilizadorResumoDto(
            id = 0,
            nome = "Utilizador de teste",
            email = "teste@exemplo.invalid",
            perfil = Perfil.Cliente,
        ),
    )
}
