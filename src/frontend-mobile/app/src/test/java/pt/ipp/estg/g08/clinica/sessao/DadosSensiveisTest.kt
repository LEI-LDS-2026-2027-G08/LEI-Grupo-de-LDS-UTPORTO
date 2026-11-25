package pt.ipp.estg.g08.clinica.sessao

import java.time.Instant
import org.junit.Assert.assertFalse
import org.junit.Test
import pt.ipp.estg.g08.clinica.core.storage.sessaoDeExemplo

/** O token e a palavra-passe nunca aparecem no texto dos objetos (por exemplo, em registos). */
class DadosSensiveisTest {
    @Test
    fun `pedido de login nao mostra a palavra-passe`() {
        assertFalse(LoginRequestDto("a@b.test", "segredo-123").toString().contains("segredo-123"))
    }

    @Test
    fun `resposta de login e sessao nao mostram o token`() {
        val sessao = sessaoDeExemplo(token = "token-secreto-456")
        val resposta =
            LoginResponseDto("token-secreto-456", Instant.EPOCH, sessao.utilizador)

        assertFalse(sessao.toString().contains("token-secreto-456"))
        assertFalse(resposta.toString().contains("token-secreto-456"))
        assertFalse(SessaoAutenticada.deLogin(resposta).toString().contains("token-secreto-456"))
    }
}
