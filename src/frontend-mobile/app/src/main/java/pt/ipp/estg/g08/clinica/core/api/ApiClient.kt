package pt.ipp.estg.g08.clinica.core.api

import java.io.IOException
import java.util.concurrent.atomic.AtomicReference
import kotlin.coroutines.resume
import kotlin.coroutines.resumeWithException
import kotlin.time.toJavaDuration
import kotlinx.coroutines.CoroutineDispatcher
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.suspendCancellableCoroutine
import kotlinx.coroutines.withContext
import kotlinx.serialization.DeserializationStrategy
import kotlinx.serialization.SerializationException
import kotlinx.serialization.SerializationStrategy
import kotlinx.serialization.json.Json
import kotlinx.serialization.serializer
import okhttp3.Call
import okhttp3.Callback
import okhttp3.HttpUrl
import okhttp3.HttpUrl.Companion.toHttpUrlOrNull
import okhttp3.MediaType.Companion.toMediaType
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody
import okhttp3.RequestBody.Companion.toRequestBody
import okhttp3.Response
import pt.ipp.estg.g08.clinica.core.config.AppConfig

private const val STATUS_NAO_AUTENTICADO = 401
private val ROTAS_PERMITIDAS = listOf("/auth/", "/me/")
private val TIPO_JSON = "application/json; charset=utf-8".toMediaType()

/**
 * Único ponto de comunicação com a API (DM-02).
 *
 * - acrescenta o cabeçalho `Authorization: Bearer` quando existe token;
 * - converte respostas de erro e falhas de rede em [ApiException];
 * - quando a API responde `401` a um pedido com token, descarta o token e invoca
 *   [onNaoAutenticado] uma única vez (a sessão terminou no servidor ou expirou).
 *
 * Só são aceites rotas sob `/auth/` e `/me/` (RNF003).
 */
class ApiClient(
    config: AppConfig,
    private val httpClient: OkHttpClient = criarHttpClient(config),
    private val json: Json = ApiJson.json,
    private val dispatcher: CoroutineDispatcher = Dispatchers.IO,
) {
    private val baseUrl: HttpUrl =
        config.apiBaseUrl.toHttpUrlOrNull()
            ?: throw IllegalArgumentException(
                "API_BASE_URL inválida: '${config.apiBaseUrl}'. Use, por exemplo, " +
                    "-PapiBaseUrl=http://10.0.2.2:5159",
            )

    private val token = AtomicReference<String?>(null)

    /** Chamada quando um pedido autenticado recebe `401`. Ver `SessaoLocal`. */
    var onNaoAutenticado: (suspend () -> Unit)? = null

    fun definirToken(token: String?) {
        this.token.set(token)
    }

    suspend fun <T> get(
        rota: String,
        desserializador: DeserializationStrategy<T>,
        query: Map<String, String> = emptyMap(),
    ): T {
        val pedido = Request.Builder().url(construirUrl(rota, query)).get().build()
        return decodificar(desserializador, executar(pedido))
    }

    suspend fun <T, B> post(
        rota: String,
        corpo: B,
        serializadorCorpo: SerializationStrategy<B>,
        desserializador: DeserializationStrategy<T>,
    ): T {
        val conteudo = json.encodeToString(serializadorCorpo, corpo).toRequestBody(TIPO_JSON)
        val pedido = Request.Builder().url(construirUrl(rota, emptyMap())).post(conteudo).build()
        return decodificar(desserializador, executar(pedido))
    }

    /** POST sem corpo nem resposta com conteúdo (por exemplo, `POST /auth/logout`). */
    suspend fun postSemResposta(rota: String) {
        val vazio: RequestBody = ByteArray(0).toRequestBody()
        val pedido = Request.Builder().url(construirUrl(rota, emptyMap())).post(vazio).build()
        executar(pedido)
    }

    private fun construirUrl(rota: String, query: Map<String, String>): HttpUrl {
        require(ROTAS_PERMITIDAS.any { rota.startsWith(it) }) {
            "Rota fora de /auth/ e /me/: $rota"
        }
        val construtor = baseUrl.newBuilder().addPathSegments(rota.trimStart('/'))
        query.forEach { (nome, valor) -> construtor.addQueryParameter(nome, valor) }
        return construtor.build()
    }

    private fun <T> decodificar(desserializador: DeserializationStrategy<T>, corpo: String): T =
        try {
            json.decodeFromString(desserializador, corpo)
        } catch (e: SerializationException) {
            throw ApiException(
                status = ApiException.STATUS_SEM_RESPOSTA,
                mensagem = "Resposta inesperada do servidor.",
                detalhe = e.message,
            )
        }

    private suspend fun executar(pedido: Request): String = withContext(dispatcher) {
        val tokenEnviado = token.get()
        val pedidoFinal =
            if (tokenEnviado == null) {
                pedido
            } else {
                pedido.newBuilder().header("Authorization", "Bearer $tokenEnviado").build()
            }

        val resposta =
            try {
                httpClient.newCall(pedidoFinal).aguardar()
            } catch (e: IOException) {
                // Inclui tempo esgotado (SocketTimeoutException e InterruptedIOException).
                throw ApiException.semLigacao().also { it.initCause(e) }
            }

        resposta.use {
            val corpo = it.body.string()
            if (it.isSuccessful) {
                corpo
            } else {
                val erro = ApiException.fromJson(it.code, corpo, json)
                val sessaoTerminou =
                    it.code == STATUS_NAO_AUTENTICADO &&
                        tokenEnviado != null &&
                        token.compareAndSet(tokenEnviado, null)
                if (sessaoTerminou) {
                    onNaoAutenticado?.invoke()
                }
                throw erro
            }
        }
    }
}

/** Cliente OkHttp com o tempo limite global de cada pedido definido em [AppConfig.timeout]. */
fun criarHttpClient(config: AppConfig): OkHttpClient = OkHttpClient
    .Builder()
    .callTimeout(config.timeout.toJavaDuration())
    .build()

private suspend fun Call.aguardar(): Response = suspendCancellableCoroutine { continuacao ->
    continuacao.invokeOnCancellation { cancel() }
    enqueue(
        object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                if (continuacao.isActive) continuacao.resumeWithException(e)
            }

            override fun onResponse(call: Call, response: Response) {
                continuacao.resume(response)
            }
        },
    )
}

/** `GET` com resposta tipada: `api.get<MeuDto>("/me/animais")`. */
suspend inline fun <reified T> ApiClient.get(
    rota: String,
    query: Map<String, String> = emptyMap(),
): T = get(rota, serializer<T>(), query)

/** `POST` com corpo e resposta tipados: `api.post<LoginRequestDto, LoginResponseDto>(...)`. */
suspend inline fun <reified B, reified T> ApiClient.post(rota: String, corpo: B): T =
    post(rota, corpo, serializer<B>(), serializer<T>())
