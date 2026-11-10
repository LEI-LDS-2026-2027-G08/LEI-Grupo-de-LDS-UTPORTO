package pt.ipp.estg.g08.clinica.sessao

import io.mockk.coEvery
import io.mockk.coVerify
import io.mockk.mockk
import io.mockk.spyk
import io.mockk.verifyOrder
import java.time.Duration
import java.time.Instant
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.ExperimentalCoroutinesApi
import kotlinx.coroutines.test.advanceTimeBy
import kotlinx.coroutines.test.advanceUntilIdle
import kotlinx.coroutines.test.runCurrent
import kotlinx.coroutines.test.runTest
import kotlinx.serialization.json.JsonObject
import okhttp3.Request
import okhttp3.Response
import org.junit.Assert.assertEquals
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Rule
import org.junit.Test
import pt.ipp.estg.g08.clinica.core.api.ApiClient
import pt.ipp.estg.g08.clinica.core.api.ApiException
import pt.ipp.estg.g08.clinica.core.api.ERRO_401
import pt.ipp.estg.g08.clinica.core.api.ServidorFalso
import pt.ipp.estg.g08.clinica.core.api.esperarErro
import pt.ipp.estg.g08.clinica.core.api.get
import pt.ipp.estg.g08.clinica.core.api.respostaJson
import pt.ipp.estg.g08.clinica.core.config.AppConfig
import pt.ipp.estg.g08.clinica.core.storage.ArmazenamentoEmMemoria
import pt.ipp.estg.g08.clinica.core.storage.CifraEmMemoria
import pt.ipp.estg.g08.clinica.core.storage.TokenStorageCifrado
import pt.ipp.estg.g08.clinica.core.storage.sessaoDeExemplo

private const val MSG_SEM_LIGACAO = "Sem ligação ao servidor. Verifique a sua ligação à rede."

/**
 * Regras de sessão da secção 6.2 do Modelo de Classes Mobile (RF001 a RF005).
 *
 * O `AuthService` é simulado com MockK; o armazenamento cifrado, a `SessaoLocal` e o `ApiClient`
 * são os reais (o `ApiClient` responde através de um servidor falso, sem rede).
 */
@OptIn(ExperimentalCoroutinesApi::class)
class SessaoViewModelTest {
    @get:Rule
    val regra = RegraDispatcherPrincipal()

    private val inicio = Instant.parse("2026-11-10T14:00:00Z")
    private var agora = inicio

    private var responder: (Request) -> Response = { respostaJson(it, 200, "{}") }
    private val servidor = ServidorFalso { responder(it) }
    private val api =
        spyk(
            ApiClient(
                AppConfig(apiBaseUrl = "http://localhost:5159"),
                httpClient = servidor.cliente,
                dispatcher = Dispatchers.Unconfined,
            ),
        )
    private val armazenamento = ArmazenamentoEmMemoria()
    private val storage =
        TokenStorageCifrado(CifraEmMemoria(), armazenamento, dispatcher = Dispatchers.Unconfined)
    private val sessaoLocal = SessaoLocal(storage, api)
    private val auth = mockk<AuthService>()

    init {
        // Mesma ligação que o AppContainer faz no arranque da aplicação.
        api.onNaoAutenticado = { sessaoLocal.terminar(MotivoFimSessao.NaoAutenticado) }
    }

    private fun criarViewModel() = SessaoViewModel(auth, storage, api, sessaoLocal) { agora }

    private fun utilizador(perfil: Perfil) =
        UtilizadorResumoDto(12, "João Costa", "cliente.joao@clinica-g08.test", perfil)

    private fun respostaLogin(
        perfil: Perfil = Perfil.Cliente,
        token: String = "token-cliente",
        validade: Duration = Duration.ofHours(8),
    ) = LoginResponseDto(token, inicio.plus(validade), utilizador(perfil))

    private fun utilizadorAutenticado(perfil: Perfil) =
        UtilizadorAutenticadoDto(12, "João Costa", "cliente.joao@clinica-g08.test", perfil, 7)

    /** O cabeçalho `Authorization` que o `ApiClient` envia agora (pedido a `/auth/me`). */
    private suspend fun cabecalhoEnviado(): String? {
        api.get<JsonObject>("/auth/me")
        return servidor.pedidos.last().header("Authorization")
    }

    private suspend fun autenticarCliente(
        vm: SessaoViewModel,
        validade: Duration = Duration.ofHours(8),
    ) {
        coEvery { auth.login(any()) } returns respostaLogin(validade = validade)
        vm.login("cliente.joao@clinica-g08.test", "segredo")
        assertEquals(EstadoSessao.Autenticada, vm.estado.value)
    }

    // --- Restauro da sessão (RF004) -------------------------------------------------------

    @Test
    fun `sem sessao guardada mostra o inicio de sessao sem aviso`() = runTest(regra.dispatcher) {
        val vm = criarViewModel()

        assertEquals(EstadoSessao.NaoAutenticada, vm.estado.value)
        assertNull(vm.aviso.value)
        coVerify(exactly = 0) { auth.me() }
    }

    @Test
    fun `sessao guardada valida e confirmada pela API fica autenticada`() =
        runTest(regra.dispatcher) {
            storage.guardar(sessaoDeExemplo())
            coEvery { auth.me() } returns utilizadorAutenticado(Perfil.Cliente)

            val vm = criarViewModel()

            assertEquals(EstadoSessao.Autenticada, vm.estado.value)
            assertEquals("João Costa", vm.utilizador.value?.nome)
            assertEquals("Bearer segredo-do-token-123", cabecalhoEnviado())
        }

    @Test
    fun `sessao guardada expirada e apagada sem chamar a API`() = runTest(regra.dispatcher) {
        storage.guardar(sessaoDeExemplo())
        agora = Instant.parse("2026-11-10T22:00:00Z")

        val vm = criarViewModel()

        assertEquals(EstadoSessao.NaoAutenticada, vm.estado.value)
        assertEquals(MotivoFimSessao.NaoAutenticado, vm.aviso.value)
        assertNull(storage.obter())
        coVerify(exactly = 0) { auth.me() }
        assertNull(cabecalhoEnviado())
    }

    @Test
    fun `token revogado no servidor termina a sessao no arranque`() = runTest(regra.dispatcher) {
        storage.guardar(sessaoDeExemplo())
        coEvery { auth.me() } throws ApiException(401, "Autenticação necessária.")

        val vm = criarViewModel()

        assertEquals(EstadoSessao.NaoAutenticada, vm.estado.value)
        assertEquals(MotivoFimSessao.NaoAutenticado, vm.aviso.value)
        assertNull(storage.obter())
        assertNull(cabecalhoEnviado())
    }

    @Test
    fun `sem ligacao no arranque mantem a sessao e permite tentar novamente`() =
        runTest(regra.dispatcher) {
            storage.guardar(sessaoDeExemplo())
            coEvery { auth.me() } throws ApiException.semLigacao()

            val vm = criarViewModel()

            assertEquals(EstadoSessao.Desconhecida, vm.estado.value)
            assertEquals(MSG_SEM_LIGACAO, vm.erro.value)
            assertNotNull(storage.obter())

            coEvery { auth.me() } returns utilizadorAutenticado(Perfil.Cliente)
            vm.restaurarSessao()

            assertEquals(EstadoSessao.Autenticada, vm.estado.value)
            assertNull(vm.erro.value)
        }

    @Test
    fun `conta que deixou de ser Cliente termina a sessao no arranque`() =
        runTest(regra.dispatcher) {
            storage.guardar(sessaoDeExemplo())
            coEvery { auth.me() } returns utilizadorAutenticado(Perfil.Rececionista)
            coEvery { auth.logout() } returns Unit

            val vm = criarViewModel()

            assertEquals(EstadoSessao.NaoAutenticada, vm.estado.value)
            assertNull(storage.obter())
            coVerify(exactly = 1) { auth.logout() }
        }

    // --- Início de sessão (RF001, RF002 e RF005) ----------------------------------------------

    @Test
    fun `login de Cliente guarda a sessao cifrada e autentica`() = runTest(regra.dispatcher) {
        coEvery { auth.login(any()) } returns respostaLogin()
        val vm = criarViewModel()

        vm.login("  cliente.joao@clinica-g08.test ", "segredo")

        assertEquals(EstadoSessao.Autenticada, vm.estado.value)
        assertEquals(Perfil.Cliente, vm.utilizador.value?.perfil)
        assertNull(vm.erro.value)
        assertFalse(vm.aProcessar.value)
        coVerify { auth.login(LoginRequestDto("cliente.joao@clinica-g08.test", "segredo")) }
        assertEquals("token-cliente", storage.obter()?.token)
        // Guardada cifrada: o token não aparece em claro no armazenamento.
        assertTrue(armazenamento.dados.values.none { it.contains("token-cliente") })
        assertEquals("Bearer token-cliente", cabecalhoEnviado())
    }

    @Test
    fun `perfil interno e recusado, o token e revogado e nada fica guardado`() =
        runTest(regra.dispatcher) {
            coEvery { auth.login(any()) } returns
                respostaLogin(perfil = Perfil.Veterinario, token = "token-interno")
            coEvery { auth.logout() } returns Unit
            val vm = criarViewModel()

            vm.login("vet.ana@clinica-g08.test", "segredo")

            assertEquals(EstadoSessao.NaoAutenticada, vm.estado.value)
            assertEquals(MSG_SO_CLIENTES, vm.erro.value)
            assertNull(vm.utilizador.value)
            assertNull(storage.obter())
            assertTrue(armazenamento.dados.isEmpty())
            // O token recebido só é usado para o revogar (EP-002) e é logo descartado.
            verifyOrder {
                api.definirToken("token-interno")
                api.definirToken(null)
            }
            coVerify(exactly = 1) { auth.logout() }
            assertNull(cabecalhoEnviado())
        }

    @Test
    fun `os quatro perfis internos sao recusados`() = runTest(regra.dispatcher) {
        coEvery { auth.logout() } returns Unit
        val vm = criarViewModel()
        val internos =
            listOf(
                Perfil.Administrador,
                Perfil.Rececionista,
                Perfil.Veterinario,
                Perfil.AuxiliarVeterinario,
            )

        internos.forEach { perfil ->
            coEvery { auth.login(any()) } returns respostaLogin(perfil = perfil)
            vm.login("interno@clinica-g08.test", "segredo")

            assertEquals(perfil.name, EstadoSessao.NaoAutenticada, vm.estado.value)
            assertEquals(perfil.name, MSG_SO_CLIENTES, vm.erro.value)
        }
        assertNull(storage.obter())
    }

    @Test
    fun `credenciais incorretas mostram mensagem que nao revela o campo errado`() =
        runTest(regra.dispatcher) {
            coEvery { auth.login(any()) } throws
                ApiException(401, "Autenticação necessária.", "Credenciais inválidas.")
            val vm = criarViewModel()

            vm.login("cliente.joao@clinica-g08.test", "errada")

            assertEquals(EstadoSessao.NaoAutenticada, vm.estado.value)
            assertEquals(MSG_CREDENCIAIS_INCORRETAS, vm.erro.value)
            assertNull(vm.aviso.value)
            assertNull(storage.obter())
        }

    @Test
    fun `conta bloqueada mostra mensagem propria`() = runTest(regra.dispatcher) {
        coEvery { auth.login(any()) } throws
            ApiException(403, "Sem permissão.", "A conta está bloqueada. Contacte a clínica.")
        val vm = criarViewModel()

        vm.login("rececao.bloqueada@clinica-g08.test", "segredo")

        assertEquals(MSG_CONTA_BLOQUEADA, vm.erro.value)
        assertNull(storage.obter())
    }

    @Test
    fun `falta de rede, dados invalidos e erro do servidor mostram mensagens em portugues`() =
        runTest(regra.dispatcher) {
            val vm = criarViewModel()
            val casos =
                listOf(
                    ApiException.semLigacao() to MSG_SEM_LIGACAO,
                    ApiException(400, "Pedido inválido.", "O e-mail é obrigatório.") to
                        "O e-mail é obrigatório.",
                    ApiException(500, "Erro interno.") to
                        "Ocorreu um erro no servidor. Tente novamente mais tarde.",
                )

            casos.forEach { (erro, mensagem) ->
                coEvery { auth.login(any()) } throws erro
                vm.login("cliente.joao@clinica-g08.test", "segredo")

                assertEquals(mensagem, vm.erro.value)
                assertEquals(EstadoSessao.NaoAutenticada, vm.estado.value)
                assertFalse(vm.aProcessar.value)
            }
        }

    @Test
    fun `validacao local impede o pedido com campos em falta ou email invalido`() =
        runTest(regra.dispatcher) {
            val vm = criarViewModel()

            vm.login("", "")
            assertEquals(
                ErrosLogin(MSG_EMAIL_EM_FALTA, MSG_PASSWORD_EM_FALTA),
                vm.errosCampos.value,
            )

            vm.login("joao", "segredo")
            assertEquals(ErrosLogin(email = MSG_EMAIL_INVALIDO), vm.errosCampos.value)

            coVerify(exactly = 0) { auth.login(any()) }

            vm.limparErros()
            assertEquals(ErrosLogin(), vm.errosCampos.value)
        }

    // --- Fim de sessão (RF003) e sessão expirada ou revogada (RF004) -------------------------

    @Test
    fun `terminar sessao revoga o token na API e limpa a sessao local`() =
        runTest(regra.dispatcher) {
            coEvery { auth.logout() } returns Unit
            val vm = criarViewModel()
            autenticarCliente(vm)

            vm.logout()

            coVerify(exactly = 1) { auth.logout() }
            assertEquals(EstadoSessao.NaoAutenticada, vm.estado.value)
            assertEquals(MotivoFimSessao.Utilizador, vm.aviso.value)
            assertNull(vm.utilizador.value)
            assertNull(storage.obter())
            assertTrue(armazenamento.dados.isEmpty())
            assertNull(cabecalhoEnviado())
        }

    @Test
    fun `terminar sessao sem rede limpa a sessao local na mesma`() = runTest(regra.dispatcher) {
        coEvery { auth.logout() } throws ApiException.semLigacao()
        val vm = criarViewModel()
        autenticarCliente(vm)

        vm.logout()

        assertEquals(EstadoSessao.NaoAutenticada, vm.estado.value)
        assertEquals(MotivoFimSessao.Utilizador, vm.aviso.value)
        assertNull(storage.obter())
    }

    @Test
    fun `terminar sessao com token ja revogado mostra o aviso de fim pelo utilizador`() =
        runTest(regra.dispatcher) {
            val vm = criarViewModel()
            autenticarCliente(vm)
            coEvery { auth.logout() } coAnswers {
                // O que o ApiClient faz quando EP-002 responde 401.
                sessaoLocal.terminar(MotivoFimSessao.NaoAutenticado)
                throw ApiException(401, "Autenticação necessária.")
            }

            vm.logout()
            advanceUntilIdle()

            assertEquals(EstadoSessao.NaoAutenticada, vm.estado.value)
            assertEquals(MotivoFimSessao.Utilizador, vm.aviso.value)
        }

    @Test
    fun `resposta 401 a um pedido posterior termina a sessao e avisa`() =
        runTest(regra.dispatcher) {
            val vm = criarViewModel()
            autenticarCliente(vm)
            responder = { respostaJson(it, 401, ERRO_401) }

            esperarErro<ApiException> { api.get<JsonObject>("/me/animais") }
            advanceUntilIdle()

            assertEquals(EstadoSessao.NaoAutenticada, vm.estado.value)
            assertEquals(MotivoFimSessao.NaoAutenticado, vm.aviso.value)
            assertNull(storage.obter())
        }

    @Test
    fun `a sessao termina quando o token expira com a aplicacao aberta`() =
        runTest(regra.dispatcher) {
            val vm = criarViewModel()
            autenticarCliente(vm, validade = Duration.ofHours(1))

            advanceTimeBy(Duration.ofMinutes(59).toMillis())
            runCurrent()
            assertEquals(EstadoSessao.Autenticada, vm.estado.value)

            advanceTimeBy(Duration.ofMinutes(1).toMillis())
            runCurrent()
            assertEquals(EstadoSessao.NaoAutenticada, vm.estado.value)
            assertEquals(MotivoFimSessao.NaoAutenticado, vm.aviso.value)
            assertNull(storage.obter())
        }

    @Test
    fun `ao voltar a primeiro plano com o token expirado a sessao termina`() =
        runTest(regra.dispatcher) {
            val vm = criarViewModel()
            autenticarCliente(vm)

            vm.verificarExpiracao()
            assertEquals(EstadoSessao.Autenticada, vm.estado.value)

            agora = inicio.plus(Duration.ofHours(9))
            vm.verificarExpiracao()

            assertEquals(EstadoSessao.NaoAutenticada, vm.estado.value)
            assertEquals(MotivoFimSessao.NaoAutenticado, vm.aviso.value)
            assertNull(storage.obter())
        }

    @Test
    fun `novo login depois de terminar sessao limpa o aviso`() = runTest(regra.dispatcher) {
        coEvery { auth.logout() } returns Unit
        val vm = criarViewModel()
        autenticarCliente(vm)
        vm.logout()
        assertEquals(MotivoFimSessao.Utilizador, vm.aviso.value)

        autenticarCliente(vm)

        assertNull(vm.aviso.value)
        assertEquals("token-cliente", storage.obter()?.token)
    }
}
