package pt.ipp.estg.g08.clinica.core.storage

import java.security.GeneralSecurityException
import java.security.ProviderException
import java.time.DateTimeException
import java.util.Base64
import kotlinx.coroutines.CoroutineDispatcher
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import kotlinx.serialization.json.Json
import pt.ipp.estg.g08.clinica.core.api.ApiJson
import pt.ipp.estg.g08.clinica.sessao.SessaoAutenticada

private const val CHAVE_SESSAO = "sessao"

/**
 * A sessão é convertida em JSON, cifrada com [Cifra] e guardada em Base64.
 * Nenhum dado de sessão fica em texto simples no dispositivo.
 */
class TokenStorageCifrado(
    private val cifra: Cifra,
    private val armazenamento: ArmazenamentoChaveValor,
    private val json: Json = ApiJson.json,
    private val dispatcher: CoroutineDispatcher = Dispatchers.IO,
) : TokenStorage {
    override suspend fun guardar(sessao: SessaoAutenticada) {
        withContext(dispatcher) {
            val claro = json.encodeToString(SessaoAutenticada.serializer(), sessao)
            val cifrado = cifra.cifrar(claro.toByteArray(Charsets.UTF_8))
            armazenamento.escrever(CHAVE_SESSAO, Base64.getEncoder().encodeToString(cifrado))
        }
    }

    override suspend fun obter(): SessaoAutenticada? = withContext(dispatcher) {
        val guardado = armazenamento.ler(CHAVE_SESSAO) ?: return@withContext null
        try {
            val claro = cifra.decifrar(Base64.getDecoder().decode(guardado))
            json.decodeFromString(SessaoAutenticada.serializer(), String(claro, Charsets.UTF_8))
        } catch (e: GeneralSecurityException) {
            descartar()
        } catch (e: ProviderException) {
            descartar()
        } catch (e: IllegalArgumentException) {
            // Inclui Base64 inválido e JSON inválido (SerializationException).
            descartar()
        } catch (e: DateTimeException) {
            descartar()
        }
    }

    override suspend fun limpar() {
        withContext(dispatcher) { armazenamento.apagar(CHAVE_SESSAO) }
    }

    /** Dados ilegíveis (chave perdida, corrompidos): trata como "sem sessão" e apaga-os. */
    private fun descartar(): SessaoAutenticada? {
        armazenamento.apagar(CHAVE_SESSAO)
        return null
    }
}
