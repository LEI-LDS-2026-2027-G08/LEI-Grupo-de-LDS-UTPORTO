package pt.ipp.estg.g08.clinica.core.storage

/** Cifra simétrica com autenticação. A implementação real usa o Android Keystore. */
interface Cifra {
    fun cifrar(claro: ByteArray): ByteArray

    fun decifrar(cifrado: ByteArray): ByteArray
}
