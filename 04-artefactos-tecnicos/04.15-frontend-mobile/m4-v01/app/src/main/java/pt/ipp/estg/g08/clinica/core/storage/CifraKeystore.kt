package pt.ipp.estg.g08.clinica.core.storage

import android.security.keystore.KeyGenParameterSpec
import android.security.keystore.KeyProperties
import java.security.KeyStore
import javax.crypto.Cipher
import javax.crypto.KeyGenerator
import javax.crypto.SecretKey
import javax.crypto.spec.GCMParameterSpec

private const val FORNECEDOR = "AndroidKeyStore"
private const val TRANSFORMACAO = "AES/GCM/NoPadding"
private const val TAMANHO_CHAVE_BITS = 256
private const val TAMANHO_IV_BYTES = 12
private const val TAMANHO_ETIQUETA_BITS = 128

/**
 * AES-256-GCM com a chave guardada no Android Keystore (DM-03): a chave nunca sai do
 * dispositivo e não é acessível à aplicação em claro. O IV (12 bytes) segue à frente do texto cifrado.
 */
class CifraKeystore(private val alias: String = "clinica_sessao") : Cifra {
    override fun cifrar(claro: ByteArray): ByteArray {
        val cifra = Cipher.getInstance(TRANSFORMACAO)
        cifra.init(Cipher.ENCRYPT_MODE, chave())
        return cifra.iv + cifra.doFinal(claro)
    }

    override fun decifrar(cifrado: ByteArray): ByteArray {
        require(cifrado.size > TAMANHO_IV_BYTES) { "Dados cifrados demasiado curtos." }
        val cifra = Cipher.getInstance(TRANSFORMACAO)
        val parametros = GCMParameterSpec(TAMANHO_ETIQUETA_BITS, cifrado, 0, TAMANHO_IV_BYTES)
        cifra.init(Cipher.DECRYPT_MODE, chave(), parametros)
        return cifra.doFinal(cifrado, TAMANHO_IV_BYTES, cifrado.size - TAMANHO_IV_BYTES)
    }

    private fun chave(): SecretKey {
        val keyStore = KeyStore.getInstance(FORNECEDOR).apply { load(null) }
        val existente = keyStore.getKey(alias, null) as? SecretKey
        if (existente != null) return existente

        val gerador = KeyGenerator.getInstance(KeyProperties.KEY_ALGORITHM_AES, FORNECEDOR)
        gerador.init(
            KeyGenParameterSpec
                .Builder(alias, KeyProperties.PURPOSE_ENCRYPT or KeyProperties.PURPOSE_DECRYPT)
                .setBlockModes(KeyProperties.BLOCK_MODE_GCM)
                .setEncryptionPaddings(KeyProperties.ENCRYPTION_PADDING_NONE)
                .setKeySize(TAMANHO_CHAVE_BITS)
                .build(),
        )
        return gerador.generateKey()
    }
}
