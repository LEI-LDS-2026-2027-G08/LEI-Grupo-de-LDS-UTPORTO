package pt.ipp.estg.g08.clinica.core.api

import java.io.IOException
import java.net.SocketTimeoutException
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.test.runTest
import kotlinx.serialization.Serializable
import okio.Buffer
import org.junit.Assert.assertEquals
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Test
import pt.ipp.estg.g08.clinica.core.config.AppConfig

@Serializable
private data class Ola(val mensagem: String)

@Serializable
private data class Pedido(val nome: String)

class ApiClientTest {
    private val config = AppConfig(apiBaseUrl = "http://localhost:5159")

    private fun api(servidor: ServidorFalso) =
        ApiClient(config, httpClient = servidor.cliente, dispatcher = Dispatchers.Unconfined)

    @Test
    fun `get usa a URL base a rota e a query e descodifica a resposta`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, """{"mensagem":"ola"}""") }

        val resposta: Ola = api(servidor).get("/me/animais", mapOf("tipo" to "futuras"))

        assertEquals("ola", resposta.mensagem)
        assertEquals(
            "http://localhost:5159/me/animais?tipo=futuras",
            servidor.pedidos[0].url.toString(),
        )
        assertNull(servidor.pedidos[0].header("Authorization"))
    }

    @Test
    fun `acrescenta o cabecalho Authorization quando ha token`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, """{"mensagem":"ola"}""") }
        val api = api(servidor)
        api.definirToken("abc123")

        api.get<Ola>("/auth/me")

        assertEquals("Bearer abc123", servidor.pedidos[0].header("Authorization"))
    }

    @Test
    fun `post envia o corpo em JSON`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, """{"mensagem":"ok"}""") }

        val resposta: Ola = api(servidor).post("/auth/login", Pedido("Rui"))

        assertEquals("ok", resposta.mensagem)
        val enviado = Buffer().also { servidor.pedidos[0].body!!.writeTo(it) }.readUtf8()
        assertEquals("""{"nome":"Rui"}""", enviado)
        assertEquals("POST", servidor.pedidos[0].method)
    }

    @Test
    fun `postSemResposta aceita 204`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 204) }

        api(servidor).postSemResposta("/auth/logout")

        assertEquals("POST", servidor.pedidos[0].method)
    }

    @Test
    fun `401 com token descarta o token e avisa uma unica vez`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 401, ERRO_401) }
        val api = api(servidor)
        var avisos = 0
        api.onNaoAutenticado = { avisos++ }
        api.definirToken("abc123")

        val erro = esperarErro<ApiException> { api.get<Ola>("/auth/me") }
        assertEquals(401, erro.status)
        assertEquals("Token inválido ou expirado.", erro.detalhe)
        assertEquals(1, avisos)

        // O token foi descartado: o pedido seguinte já não o envia e não volta a avisar.
        esperarErro<ApiException> { api.get<Ola>("/auth/me") }
        assertNull(servidor.pedidos.last().header("Authorization"))
        assertEquals(1, avisos)
    }

    @Test
    fun `401 sem token (login) nao termina a sessao`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 401, ERRO_401) }
        val api = api(servidor)
        var avisos = 0
        api.onNaoAutenticado = { avisos++ }

        esperarErro<ApiException> { api.get<Ola>("/auth/me") }

        assertEquals(0, avisos)
    }

    @Test
    fun `403 e acesso recusado e nao termina a sessao`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 403, ERRO_403) }
        val api = api(servidor)
        var avisos = 0
        api.onNaoAutenticado = { avisos++ }
        api.definirToken("abc123")

        val erro = esperarErro<ApiException> { api.get<Ola>("/me/animais") }

        assertEquals(403, erro.status)
        assertEquals("Não tem acesso a esta informação.", erro.mensagemParaUtilizador())
        assertEquals(0, avisos)
        esperarErro<ApiException> { api.get<Ola>("/me/animais") }
        assertEquals("Bearer abc123", servidor.pedidos.last().header("Authorization"))
    }

    @Test
    fun `falha de rede e tempo esgotado viram erro sem ligacao`() = runTest {
        val semRede = ServidorFalso { throw IOException("sem rede") }
        val lento = ServidorFalso { throw SocketTimeoutException("timeout") }

        val erroRede = esperarErro<ApiException> { api(semRede).get<Ola>("/auth/me") }
        val erroTempo = esperarErro<ApiException> { api(lento).get<Ola>("/auth/me") }

        assertTrue(erroRede.semLigacao)
        assertTrue(erroTempo.semLigacao)
        assertEquals(ApiException.STATUS_SEM_RESPOSTA, erroRede.status)
    }

    @Test
    fun `500 com corpo que nao e JSON da mensagem de erro do servidor`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 500, "<html>erro</html>") }

        val erro = esperarErro<ApiException> { api(servidor).get<Ola>("/auth/me") }

        assertEquals(500, erro.status)
        assertFalse(erro.semLigacao)
        assertEquals(
            "Ocorreu um erro no servidor. Tente novamente mais tarde.",
            erro.mensagemParaUtilizador(),
        )
    }

    @Test
    fun `resposta de sucesso ilegivel vira erro e nao rebenta`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, "isto nao e json") }

        val erro = esperarErro<ApiException> { api(servidor).get<Ola>("/auth/me") }

        assertEquals(ApiException.STATUS_SEM_RESPOSTA, erro.status)
    }

    @Test
    fun `rotas fora de auth e me sao recusadas`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, """{"mensagem":"x"}""") }

        esperarErro<IllegalArgumentException> { api(servidor).get<Ola>("/funcionarios") }

        assertTrue(servidor.pedidos.isEmpty())
    }

    @Test
    fun `URL base invalida e recusada ao criar o cliente`() {
        val erro =
            try {
                ApiClient(AppConfig(apiBaseUrl = "isto nao e um url"))
                null
            } catch (e: IllegalArgumentException) {
                e
            }

        assertTrue(erro != null)
    }
}
