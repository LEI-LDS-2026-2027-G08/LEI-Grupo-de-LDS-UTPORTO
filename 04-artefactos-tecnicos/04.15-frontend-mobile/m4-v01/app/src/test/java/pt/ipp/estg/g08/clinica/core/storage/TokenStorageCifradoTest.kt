package pt.ipp.estg.g08.clinica.core.storage

import java.security.SecureRandom
import java.time.Instant
import java.util.Base64
import javax.crypto.Cipher
import javax.crypto.spec.GCMParameterSpec
import javax.crypto.spec.SecretKeySpec
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.test.runTest
import org.junit.Assert.assertEquals
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Test
import pt.ipp.estg.g08.clinica.sessao.Perfil
import pt.ipp.estg.g08.clinica.sessao.SessaoAutenticada
import pt.ipp.estg.g08.clinica.sessao.UtilizadorResumoDto

/** Cifra de testes: o mesmo AES-GCM do Keystore, mas com uma chave em memória. */
class CifraEmMemoria(chave: ByteArray = ByteArray(32).also { SecureRandom().nextBytes(it) }) :
    Cifra {
    private val chaveAes = SecretKeySpec(chave, "AES")

    override fun cifrar(claro: ByteArray): ByteArray {
        val iv = ByteArray(12).also { SecureRandom().nextBytes(it) }
        val cifra = Cipher.getInstance("AES/GCM/NoPadding")
        cifra.init(Cipher.ENCRYPT_MODE, chaveAes, GCMParameterSpec(128, iv))
        return iv + cifra.doFinal(claro)
    }

    override fun decifrar(cifrado: ByteArray): ByteArray {
        val cifra = Cipher.getInstance("AES/GCM/NoPadding")
        cifra.init(Cipher.DECRYPT_MODE, chaveAes, GCMParameterSpec(128, cifrado, 0, 12))
        return cifra.doFinal(cifrado, 12, cifrado.size - 12)
    }
}

class ArmazenamentoEmMemoria : ArmazenamentoChaveValor {
    val dados = mutableMapOf<String, String>()

    override fun ler(chave: String): String? = dados[chave]

    override fun escrever(chave: String, valor: String) {
        dados[chave] = valor
    }

    override fun apagar(chave: String) {
        dados.remove(chave)
    }
}

fun sessaoDeExemplo(token: String = "segredo-do-token-123") = SessaoAutenticada(
    token = token,
    expiraEm = Instant.parse("2026-11-10T22:00:00Z"),
    utilizador =
    UtilizadorResumoDto(
        id = 7,
        nome = "João Costa",
        email = "joao@exemplo.invalid",
        perfil = Perfil.Cliente,
    ),
)

class TokenStorageCifradoTest {
    private val armazenamento = ArmazenamentoEmMemoria()

    private fun storage(cifra: Cifra = CifraEmMemoria()) =
        TokenStorageCifrado(cifra, armazenamento, dispatcher = Dispatchers.Unconfined)

    @Test
    fun `guardar e obter devolve a mesma sessao`() = runTest {
        val storage = storage()

        storage.guardar(sessaoDeExemplo())

        assertEquals(sessaoDeExemplo(), storage.obter())
    }

    @Test
    fun `o token nunca fica em texto simples`() = runTest {
        storage().guardar(sessaoDeExemplo("segredo-do-token-123"))

        val guardado = armazenamento.dados.values.single()
        val descodificado = String(Base64.getDecoder().decode(guardado), Charsets.ISO_8859_1)
        assertFalse(guardado.contains("segredo-do-token-123"))
        assertFalse(descodificado.contains("segredo-do-token-123"))
        assertFalse(descodificado.contains("João"))
    }

    @Test
    fun `sem sessao guardada devolve null`() = runTest {
        assertNull(storage().obter())
    }

    @Test
    fun `limpar remove todos os dados de autenticacao`() = runTest {
        val storage = storage()
        storage.guardar(sessaoDeExemplo())

        storage.limpar()

        assertNull(storage.obter())
        assertTrue(armazenamento.dados.isEmpty())
    }

    @Test
    fun `guardar de novo substitui a sessao anterior`() = runTest {
        val storage = storage()
        storage.guardar(sessaoDeExemplo("primeiro"))

        storage.guardar(sessaoDeExemplo("segundo"))

        assertEquals("segundo", storage.obter()?.token)
    }

    @Test
    fun `dados corrompidos sao tratados como sem sessao e apagados`() = runTest {
        armazenamento.escrever("sessao", "%%%isto-nao-e-base64%%%")

        assertNull(storage().obter())
        assertTrue(armazenamento.dados.isEmpty())
    }

    @Test
    fun `dados cifrados com outra chave sao tratados como sem sessao e apagados`() = runTest {
        storage(CifraEmMemoria()).guardar(sessaoDeExemplo())

        assertNull(storage(CifraEmMemoria()).obter())
        assertTrue(armazenamento.dados.isEmpty())
    }
}
