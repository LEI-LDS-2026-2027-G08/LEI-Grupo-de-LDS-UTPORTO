package pt.ipp.estg.g08.clinica.sessao

import java.time.Instant
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.test.runTest
import okio.Buffer
import org.junit.Assert.assertEquals
import org.junit.Assert.assertNull
import org.junit.Test
import pt.ipp.estg.g08.clinica.core.api.ApiClient
import pt.ipp.estg.g08.clinica.core.api.ApiException
import pt.ipp.estg.g08.clinica.core.api.ServidorFalso
import pt.ipp.estg.g08.clinica.core.api.esperarErro
import pt.ipp.estg.g08.clinica.core.api.respostaJson
import pt.ipp.estg.g08.clinica.core.config.AppConfig

private const val RESPOSTA_LOGIN =
    """{"token":"token-123","expiraEm":"2026-11-10T22:00:00Z",""" +
        """"utilizador":{"id":12,"nome":"João Costa","email":"cliente.joao@clinica-g08.test",""" +
        """"perfil":"Cliente","clienteId":7}}"""

private const val ERRO_CREDENCIAIS =
    """{"code":401,"message":"Autenticação necessária.","detail":"Credenciais inválidas."}"""

class AuthServiceTest {
    private fun api(servidor: ServidorFalso) = ApiClient(
        AppConfig(apiBaseUrl = "http://localhost:5159"),
        httpClient = servidor.cliente,
        dispatcher = Dispatchers.Unconfined,
    )

    @Test
    fun `login envia email e palavra-passe para EP-001 sem token e le a resposta`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, RESPOSTA_LOGIN) }
        val api = api(servidor)

        val resposta =
            AuthService(api).login(LoginRequestDto("cliente.joao@clinica-g08.test", "segredo"))

        val pedido = servidor.pedidos.single()
        assertEquals("POST", pedido.method)
        assertEquals("http://localhost:5159/auth/login", pedido.url.toString())
        assertNull(pedido.header("Authorization"))
        val corpo = Buffer().also { pedido.body!!.writeTo(it) }.readUtf8()
        assertEquals("""{"email":"cliente.joao@clinica-g08.test","password":"segredo"}""", corpo)
        assertEquals("token-123", resposta.token)
        assertEquals(Instant.parse("2026-11-10T22:00:00Z"), resposta.expiraEm)
        assertEquals(Perfil.Cliente, resposta.utilizador.perfil)
        assertEquals("João Costa", resposta.utilizador.nome)
    }

    @Test
    fun `login reconhece os perfis internos devolvidos pela API`() = runTest {
        val corpo = RESPOSTA_LOGIN.replace("\"Cliente\"", "\"AuxiliarVeterinario\"")
        val servidor = ServidorFalso { respostaJson(it, 200, corpo) }

        val resposta = AuthService(api(servidor)).login(LoginRequestDto("a@b.test", "x"))

        assertEquals(Perfil.AuxiliarVeterinario, resposta.utilizador.perfil)
    }

    @Test
    fun `credenciais incorretas dao 401 sem terminar nenhuma sessao`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 401, ERRO_CREDENCIAIS) }
        val api = api(servidor)
        var avisos = 0
        api.onNaoAutenticado = { avisos++ }

        val erro =
            esperarErro<ApiException> {
                AuthService(api).login(LoginRequestDto("a@b.test", "errada"))
            }

        assertEquals(401, erro.status)
        assertEquals(0, avisos)
    }

    @Test
    fun `logout envia POST para EP-002 com o token`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 204) }
        val api = api(servidor)
        api.definirToken("token-123")

        AuthService(api).logout()

        val pedido = servidor.pedidos.single()
        assertEquals("POST", pedido.method)
        assertEquals("http://localhost:5159/auth/logout", pedido.url.toString())
        assertEquals("Bearer token-123", pedido.header("Authorization"))
    }

    @Test
    fun `me consulta EP-003 com o token e le o perfil`() = runTest {
        val servidor =
            ServidorFalso {
                respostaJson(
                    it,
                    200,
                    """{"id":12,"nome":"João Costa","email":"cliente.joao@clinica-g08.test",""" +
                        """"perfil":"Cliente","clienteId":7}""",
                )
            }
        val api = api(servidor)
        api.definirToken("token-123")

        val eu = AuthService(api).me()

        val pedido = servidor.pedidos.single()
        assertEquals("GET", pedido.method)
        assertEquals("http://localhost:5159/auth/me", pedido.url.toString())
        assertEquals("Bearer token-123", pedido.header("Authorization"))
        assertEquals(Perfil.Cliente, eu.perfil)
        assertEquals(7, eu.clienteId)
        assertEquals(UtilizadorResumoDto(12, "João Costa", eu.email, Perfil.Cliente), eu.resumo())
    }
}
