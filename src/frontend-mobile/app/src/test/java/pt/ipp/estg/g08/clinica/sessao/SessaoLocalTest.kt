package pt.ipp.estg.g08.clinica.sessao

import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.first
import kotlinx.coroutines.test.runTest
import kotlinx.serialization.json.JsonObject
import org.junit.Assert.assertEquals
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
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

class SessaoLocalTest {
    private val armazenamento = ArmazenamentoEmMemoria()
    private val storage =
        TokenStorageCifrado(CifraEmMemoria(), armazenamento, dispatcher = Dispatchers.Unconfined)
    private val servidor = ServidorFalso { respostaJson(it, 401, ERRO_401) }
    private val api =
        ApiClient(
            AppConfig(apiBaseUrl = "http://localhost:5159"),
            httpClient = servidor.cliente,
            dispatcher = Dispatchers.Unconfined,
        )
    private val sessaoLocal = SessaoLocal(storage, api)

    @Test
    fun `terminar remove o token da memoria e do armazenamento e emite o evento`() = runTest {
        storage.guardar(sessaoDeExemplo())
        api.definirToken("segredo-do-token-123")

        sessaoLocal.terminar(MotivoFimSessao.Utilizador)

        assertNull(storage.obter())
        assertTrue(armazenamento.dados.isEmpty())
        assertEquals(MotivoFimSessao.Utilizador, sessaoLocal.sessaoTerminada.first())
        // O ApiClient já não envia o token.
        esperarErro<ApiException> { api.get<JsonObject>("/auth/me") }
        assertNull(servidor.pedidos.last().header("Authorization"))
    }

    @Test
    fun `resposta 401 apaga a sessao guardada e emite NaoAutenticado`() = runTest {
        // Mesma ligação que o AppContainer faz no arranque da aplicação.
        api.onNaoAutenticado = { sessaoLocal.terminar(MotivoFimSessao.NaoAutenticado) }
        storage.guardar(sessaoDeExemplo())
        api.definirToken("segredo-do-token-123")

        val erro =
            esperarErro<ApiException> { api.get<JsonObject>("/auth/me") }

        assertEquals(401, erro.status)
        assertNull(storage.obter())
        assertTrue(armazenamento.dados.isEmpty())
        assertEquals(MotivoFimSessao.NaoAutenticado, sessaoLocal.sessaoTerminada.first())
    }
}
