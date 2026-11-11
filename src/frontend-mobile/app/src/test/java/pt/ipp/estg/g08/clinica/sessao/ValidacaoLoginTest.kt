package pt.ipp.estg.g08.clinica.sessao

import org.junit.Assert.assertEquals
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Test

class ValidacaoLoginTest {
    @Test
    fun `email e palavra-passe preenchidos e validos nao dao erros`() {
        val erros = ValidacaoLogin.validar("cliente.joao@clinica-g08.test", "uma-palavra-passe")

        assertFalse(erros.temErros)
    }

    @Test
    fun `campos vazios dao erro em ambos`() {
        val erros = ValidacaoLogin.validar("", "")

        assertTrue(erros.temErros)
        assertEquals(MSG_EMAIL_EM_FALTA, erros.email)
        assertEquals(MSG_PASSWORD_EM_FALTA, erros.password)
    }

    @Test
    fun `email so com espacos conta como vazio e espacos a volta sao ignorados`() {
        assertEquals(MSG_EMAIL_EM_FALTA, ValidacaoLogin.validarEmail("   "))
        assertNull(ValidacaoLogin.validarEmail("  cliente.joao@clinica-g08.test  "))
    }

    @Test
    fun `email sem formato valido`() {
        listOf("joao", "joao@", "@clinica.test", "joao@clinica", "jo ao@clinica.test").forEach {
            assertEquals(it, MSG_EMAIL_INVALIDO, ValidacaoLogin.validarEmail(it))
        }
    }

    @Test
    fun `palavra-passe so com espacos conta como vazia`() {
        assertEquals(MSG_PASSWORD_EM_FALTA, ValidacaoLogin.validarPassword("   "))
        assertNull(ValidacaoLogin.validarPassword(" a "))
    }
}
