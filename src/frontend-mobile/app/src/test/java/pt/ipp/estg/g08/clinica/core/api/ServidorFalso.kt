package pt.ipp.estg.g08.clinica.core.api

import okhttp3.MediaType.Companion.toMediaType
import okhttp3.OkHttpClient
import okhttp3.Protocol
import okhttp3.Request
import okhttp3.Response
import okhttp3.ResponseBody.Companion.toResponseBody

/**
 * Servidor de testes: um interceptor que responde sem rede. Guarda os pedidos recebidos.
 * [responder] pode lançar `IOException` para simular falhas de ligação.
 */
class ServidorFalso(private val responder: (Request) -> Response) {
    val pedidos = mutableListOf<Request>()

    val cliente: OkHttpClient =
        OkHttpClient
            .Builder()
            .addInterceptor { cadeia ->
                val pedido = cadeia.request()
                pedidos.add(pedido)
                responder(pedido)
            }.build()
}

fun respostaJson(pedido: Request, codigo: Int, corpo: String = ""): Response = Response
    .Builder()
    .request(pedido)
    .protocol(Protocol.HTTP_1_1)
    .code(codigo)
    .message("teste")
    .body(corpo.toResponseBody("application/json".toMediaType()))
    .build()

const val ERRO_401 =
    """{"code":401,"message":"Autenticação necessária.","detail":"Token inválido ou expirado."}"""
const val ERRO_403 =
    """{"code":403,"message":"Não tem permissão para executar esta operação.","detail":"Acesso recusado."}"""
