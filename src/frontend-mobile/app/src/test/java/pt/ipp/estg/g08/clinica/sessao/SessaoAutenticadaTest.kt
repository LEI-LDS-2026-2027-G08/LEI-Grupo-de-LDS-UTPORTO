package pt.ipp.estg.g08.clinica.sessao

import java.time.Instant
import org.junit.Assert.assertFalse
import org.junit.Assert.assertTrue
import org.junit.Test
import pt.ipp.estg.g08.clinica.core.storage.sessaoDeExemplo

class SessaoAutenticadaTest {
    private val sessao = sessaoDeExemplo()

    @Test
    fun `nao esta expirada antes do momento de expiracao`() {
        assertFalse(sessao.expirada(Instant.parse("2026-11-10T21:59:59Z")))
    }

    @Test
    fun `esta expirada no momento de expiracao e depois`() {
        assertTrue(sessao.expirada(Instant.parse("2026-11-10T22:00:00Z")))
        assertTrue(sessao.expirada(Instant.parse("2026-11-11T08:00:00Z")))
    }
}
