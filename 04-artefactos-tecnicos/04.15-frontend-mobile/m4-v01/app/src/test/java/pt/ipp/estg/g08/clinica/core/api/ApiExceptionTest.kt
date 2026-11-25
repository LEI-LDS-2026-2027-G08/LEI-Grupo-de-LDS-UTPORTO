package pt.ipp.estg.g08.clinica.core.api

import org.junit.Assert.assertEquals
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Test

class ApiExceptionTest {
    @Test
    fun `fromJson le code message e detail da API v03`() {
        val erro =
            ApiException.fromJson(
                422,
                """{"code":422,"message":"Não foi possível processar os dados fornecidos.","detail":"Marcação sobreposta."}""",
            )

        assertEquals(422, erro.status)
        assertEquals("Não foi possível processar os dados fornecidos.", erro.mensagem)
        assertEquals("Marcação sobreposta.", erro.detalhe)
        assertFalse(erro.semLigacao)
    }

    @Test
    fun `fromJson com corpo que nao e JSON mantem o estado HTTP`() {
        val erro = ApiException.fromJson(502, "<html>Bad Gateway</html>")

        assertEquals(502, erro.status)
        assertEquals("", erro.mensagem)
        assertNull(erro.detalhe)
    }

    @Test
    fun `400 mostra o detalhe da API`() {
        val erro = ApiException.fromJson(
            400,
            """{"code":400,"message":"Pedido inválido.","detail":"O e-mail é obrigatório."}""",
        )

        assertEquals("O e-mail é obrigatório.", erro.mensagemParaUtilizador())
    }

    @Test
    fun `401 403 404 e 500 tem mensagens fixas em portugues`() {
        assertEquals(
            "A sua sessão terminou. Inicie sessão novamente.",
            ApiException.fromJson(401, "").mensagemParaUtilizador(),
        )
        assertEquals(
            "Não tem acesso a esta informação.",
            ApiException.fromJson(403, "").mensagemParaUtilizador(),
        )
        assertEquals(
            "A informação pedida já não existe.",
            ApiException.fromJson(404, "").mensagemParaUtilizador(),
        )
        assertEquals(
            "Ocorreu um erro no servidor. Tente novamente mais tarde.",
            ApiException.fromJson(500, "").mensagemParaUtilizador(),
        )
    }

    @Test
    fun `422 mostra a regra violada`() {
        val erro = ApiException.fromJson(
            422,
            """{"code":422,"message":"x","detail":"Regra violada."}""",
        )

        assertEquals("Regra violada.", erro.mensagemParaUtilizador())
    }

    @Test
    fun `sem ligacao tem estado 0 e mensagem de rede`() {
        val erro = ApiException.semLigacao()

        assertTrue(erro.semLigacao)
        assertEquals(ApiException.STATUS_SEM_RESPOSTA, erro.status)
        assertEquals(
            "Sem ligação ao servidor. Verifique a sua ligação à rede.",
            erro.mensagemParaUtilizador(),
        )
    }
}
