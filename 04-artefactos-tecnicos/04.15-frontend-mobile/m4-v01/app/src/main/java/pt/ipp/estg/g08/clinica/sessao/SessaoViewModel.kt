package pt.ipp.estg.g08.clinica.sessao

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import java.time.Duration
import java.time.Instant
import kotlinx.coroutines.Job
import kotlinx.coroutines.NonCancellable
import kotlinx.coroutines.delay
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import pt.ipp.estg.g08.clinica.core.api.ApiClient
import pt.ipp.estg.g08.clinica.core.api.ApiException
import pt.ipp.estg.g08.clinica.core.storage.TokenStorage

private const val STATUS_NAO_AUTENTICADO = 401
private const val STATUS_SEM_PERMISSAO = 403

const val MSG_CREDENCIAIS_INCORRETAS = "Email ou palavra-passe incorretos."
const val MSG_CONTA_BLOQUEADA = "A conta encontra-se bloqueada."
const val MSG_SO_CLIENTES = "Esta aplicação destina-se apenas a clientes da clínica."

/**
 * Ciclo de vida da sessão do Cliente (secções 6.2 e 6.3 do Modelo de Classes Mobile):
 * restauro no arranque, início de sessão com verificação do perfil, fim de sessão e fim local
 * da sessão (401, expiração).
 *
 * Regras:
 * - só o perfil Cliente fica autenticado; uma conta interna é recusada, o token que recebeu é
 *   revogado no servidor (EP-002) e nada fica guardado no dispositivo (RF005, RNF003);
 * - no arranque, a sessão guardada só é aceite se não estiver expirada e se EP-003 a confirmar;
 * - o fim de sessão (UC02) chama EP-002 e limpa sempre a sessão local, mesmo que o pedido falhe;
 * - quando o token expira com a aplicação aberta, a sessão termina localmente.
 *
 * Todo o fim de sessão passa por [SessaoLocal], que também recebe os `401` do `ApiClient`.
 */
class SessaoViewModel(
    private val authService: AuthService,
    private val tokenStorage: TokenStorage,
    private val apiClient: ApiClient,
    private val sessaoLocal: SessaoLocal,
    private val agora: () -> Instant = { Instant.now() },
) : ViewModel() {
    private val estadoInterno = MutableStateFlow(EstadoSessao.Desconhecida)
    val estado: StateFlow<EstadoSessao> = estadoInterno.asStateFlow()

    private val utilizadorInterno = MutableStateFlow<UtilizadorResumoDto?>(null)
    val utilizador: StateFlow<UtilizadorResumoDto?> = utilizadorInterno.asStateFlow()

    /** Erro a mostrar: no início de sessão (M01) ou no arranque (sem ligação ao servidor). */
    private val erroInterno = MutableStateFlow<String?>(null)
    val erro: StateFlow<String?> = erroInterno.asStateFlow()

    private val errosCamposInterno = MutableStateFlow(ErrosLogin())
    val errosCampos: StateFlow<ErrosLogin> = errosCamposInterno.asStateFlow()

    private val aProcessarInterno = MutableStateFlow(false)
    val aProcessar: StateFlow<Boolean> = aProcessarInterno.asStateFlow()

    /** Porque é que a última sessão terminou (aviso mostrado em M01). */
    private val avisoInterno = MutableStateFlow<MotivoFimSessao?>(null)
    val aviso: StateFlow<MotivoFimSessao?> = avisoInterno.asStateFlow()

    private var sessaoAtual: SessaoAutenticada? = null
    private var temporizadorExpiracao: Job? = null
    private var logoutEmCurso = false

    init {
        viewModelScope.launch {
            sessaoLocal.sessaoTerminada.collect { motivo -> aoTerminarSessao(motivo) }
        }
        restaurarSessao()
    }

    /** RF004 — retoma a sessão guardada se ainda for válida (EP-003). */
    fun restaurarSessao() {
        viewModelScope.launch {
            estadoInterno.value = EstadoSessao.Desconhecida
            erroInterno.value = null
            val guardada = tokenStorage.obter()
            when {
                guardada == null -> mostrarInicioDeSessao(aviso = null)
                guardada.expirada(agora()) -> {
                    apiClient.definirToken(null)
                    tokenStorage.limpar()
                    mostrarInicioDeSessao(aviso = MotivoFimSessao.NaoAutenticado)
                }
                else -> confirmarSessaoGuardada(guardada)
            }
        }
    }

    /** RF001, RF002 e RF005 — UC01. */
    fun login(email: String, password: String) {
        if (aProcessarInterno.value) return
        val erros = ValidacaoLogin.validar(email, password)
        errosCamposInterno.value = erros
        if (erros.temErros) {
            erroInterno.value = null
            return
        }
        viewModelScope.launch {
            aProcessarInterno.value = true
            erroInterno.value = null
            avisoInterno.value = null
            try {
                // Nenhum token anterior pode acompanhar o pedido de login.
                apiClient.definirToken(null)
                val resposta = authService.login(LoginRequestDto(email.trim(), password))
                if (resposta.utilizador.perfil == Perfil.Cliente) {
                    val sessao = SessaoAutenticada.deLogin(resposta)
                    tokenStorage.guardar(sessao)
                    apiClient.definirToken(sessao.token)
                    autenticar(sessao)
                } else {
                    recusarPerfilInterno(resposta.token)
                    erroInterno.value = MSG_SO_CLIENTES
                }
            } catch (e: ApiException) {
                erroInterno.value = mensagemErroLogin(e)
            } finally {
                aProcessarInterno.value = false
            }
        }
    }

    /** RF003 — UC02: revoga o token no servidor (EP-002) e limpa sempre a sessão local. */
    fun logout() {
        if (estadoInterno.value != EstadoSessao.Autenticada || aProcessarInterno.value) return
        viewModelScope.launch {
            aProcessarInterno.value = true
            logoutEmCurso = true
            try {
                authService.logout()
            } catch (e: ApiException) {
                // Sem rede, erro do servidor ou token já inválido: a sessão local termina na mesma.
            } finally {
                withContext(NonCancellable) { encerrarLocalmente(MotivoFimSessao.Utilizador) }
                logoutEmCurso = false
                aProcessarInterno.value = false
            }
        }
    }

    /** O utilizador alterou o formulário: os erros anteriores deixam de se aplicar. */
    fun limparErros() {
        errosCamposInterno.value = ErrosLogin()
        erroInterno.value = null
    }

    /** Fim da sessão só no dispositivo (token expirado ou invalidado). */
    fun terminarSessaoLocal() {
        viewModelScope.launch { encerrarLocalmente(MotivoFimSessao.NaoAutenticado) }
    }

    /** Chamado quando a aplicação volta a primeiro plano: termina a sessão se o token expirou. */
    fun verificarExpiracao() {
        val sessao = sessaoAtual ?: return
        if (estadoInterno.value == EstadoSessao.Autenticada && sessao.expirada(agora())) {
            terminarSessaoLocal()
        }
    }

    private suspend fun confirmarSessaoGuardada(guardada: SessaoAutenticada) {
        apiClient.definirToken(guardada.token)
        try {
            val atual = authService.me()
            if (atual.perfil == Perfil.Cliente) {
                val sessao = guardada.copy(utilizador = atual.resumo())
                if (sessao != guardada) tokenStorage.guardar(sessao)
                autenticar(sessao)
            } else {
                // O perfil da conta deixou de ser Cliente: a sessão não pode continuar.
                revogarSemErro()
                encerrarLocalmente(MotivoFimSessao.NaoAutenticado)
            }
        } catch (e: ApiException) {
            if (e.status == STATUS_NAO_AUTENTICADO) {
                // O ApiClient já avisou a SessaoLocal; garante o estado mesmo assim.
                encerrarLocalmente(MotivoFimSessao.NaoAutenticado)
            } else {
                // Sem ligação ou erro do servidor: a sessão guardada mantém-se e o ecrã de
                // arranque mostra o erro com "Tentar novamente" (RNF006).
                erroInterno.value = e.mensagemParaUtilizador()
            }
        }
    }

    private suspend fun recusarPerfilInterno(token: String) {
        apiClient.definirToken(token)
        try {
            revogarSemErro()
        } finally {
            apiClient.definirToken(null)
        }
    }

    private suspend fun revogarSemErro() {
        try {
            authService.logout()
        } catch (e: ApiException) {
            // Revogação de melhor esforço: o token não é guardado nem volta a ser usado.
        }
    }

    private fun autenticar(sessao: SessaoAutenticada) {
        sessaoAtual = sessao
        utilizadorInterno.value = sessao.utilizador
        erroInterno.value = null
        errosCamposInterno.value = ErrosLogin()
        avisoInterno.value = null
        estadoInterno.value = EstadoSessao.Autenticada
        agendarExpiracao(sessao)
    }

    private fun agendarExpiracao(sessao: SessaoAutenticada) {
        temporizadorExpiracao?.cancel()
        val restante = Duration.between(agora(), sessao.expiraEm).toMillis().coerceAtLeast(0)
        temporizadorExpiracao = viewModelScope.launch {
            delay(restante)
            encerrarLocalmente(MotivoFimSessao.NaoAutenticado)
        }
    }

    private suspend fun encerrarLocalmente(motivo: MotivoFimSessao) {
        sessaoLocal.terminar(motivo)
        aoTerminarSessao(motivo)
    }

    /** Passa a "não autenticada". Eventos repetidos do mesmo fim de sessão são ignorados. */
    private fun aoTerminarSessao(motivo: MotivoFimSessao) {
        if (estadoInterno.value == EstadoSessao.NaoAutenticada) return
        mostrarInicioDeSessao(aviso = if (logoutEmCurso) MotivoFimSessao.Utilizador else motivo)
    }

    private fun mostrarInicioDeSessao(aviso: MotivoFimSessao?) {
        temporizadorExpiracao?.cancel()
        temporizadorExpiracao = null
        sessaoAtual = null
        utilizadorInterno.value = null
        avisoInterno.value = aviso
        estadoInterno.value = EstadoSessao.NaoAutenticada
    }

    private fun mensagemErroLogin(erro: ApiException): String = when {
        erro.semLigacao -> erro.mensagemParaUtilizador()
        erro.status == STATUS_NAO_AUTENTICADO -> MSG_CREDENCIAIS_INCORRETAS
        erro.status == STATUS_SEM_PERMISSAO -> MSG_CONTA_BLOQUEADA
        else -> erro.mensagemParaUtilizador()
    }
}
