package pt.ipp.estg.g08.clinica.core.api

import kotlinx.serialization.json.Json

private const val STATUS_PEDIDO_INVALIDO = 400
private const val STATUS_NAO_AUTENTICADO = 401
private const val STATUS_SEM_PERMISSAO = 403
private const val STATUS_NAO_ENCONTRADO = 404
private const val STATUS_CONFLITO = 409
private const val STATUS_PRECONDICAO = 412
private const val STATUS_REGRA_VIOLADA = 422
private const val STATUS_ERRO_SERVIDOR = 500

private const val MSG_SEM_LIGACAO = "Sem ligação ao servidor. Verifique a sua ligação à rede."
private const val MSG_DADOS_INVALIDOS = "Os dados indicados não são válidos."
private const val MSG_SESSAO_TERMINADA = "A sua sessão terminou. Inicie sessão novamente."
private const val MSG_ACESSO_RECUSADO = "Não tem acesso a esta informação."
private const val MSG_NAO_ENCONTRADO = "A informação pedida já não existe."
private const val MSG_ERRO_SERVIDOR = "Ocorreu um erro no servidor. Tente novamente mais tarde."
private const val MSG_GENERICA = "Não foi possível concluir a operação. Tente novamente."

/**
 * Erro devolvido pela API (`status`, `mensagem` e `detalhe`) ou falta de ligação à rede.
 *
 * [mensagemParaUtilizador] devolve sempre texto em português europeu, sem informação técnica.
 */
class ApiException(
    val status: Int,
    val mensagem: String,
    val detalhe: String? = null,
    val semLigacao: Boolean = false,
) : Exception(detalhe ?: mensagem) {
    fun mensagemParaUtilizador(): String = when {
        semLigacao -> MSG_SEM_LIGACAO
        status == STATUS_PEDIDO_INVALIDO -> detalhe ?: MSG_DADOS_INVALIDOS
        status == STATUS_NAO_AUTENTICADO -> MSG_SESSAO_TERMINADA
        status == STATUS_SEM_PERMISSAO -> MSG_ACESSO_RECUSADO
        status == STATUS_NAO_ENCONTRADO -> MSG_NAO_ENCONTRADO
        status in REGRAS_DE_NEGOCIO -> detalhe ?: mensagem.ifBlank { MSG_GENERICA }
        status >= STATUS_ERRO_SERVIDOR -> MSG_ERRO_SERVIDOR
        else -> MSG_GENERICA
    }

    companion object {
        /** Estado usado quando não houve resposta HTTP (sem rede ou tempo esgotado). */
        const val STATUS_SEM_RESPOSTA = 0

        private val REGRAS_DE_NEGOCIO =
            setOf(STATUS_CONFLITO, STATUS_PRECONDICAO, STATUS_REGRA_VIOLADA)

        /**
         * Converte o corpo de uma resposta de erro. Se o corpo não tiver o formato esperado,
         * mantém o estado HTTP e usa uma mensagem genérica.
         */
        fun fromJson(status: Int, corpo: String, json: Json = ApiJson.json): ApiException {
            val erro =
                try {
                    json.decodeFromString<ErroApiDto>(corpo)
                } catch (e: IllegalArgumentException) {
                    // SerializationException é uma IllegalArgumentException.
                    null
                }
            return ApiException(
                status = status,
                mensagem = erro?.message.orEmpty(),
                detalhe = erro?.detail?.takeIf { it.isNotBlank() },
            )
        }

        fun semLigacao(): ApiException = ApiException(
            status = STATUS_SEM_RESPOSTA,
            mensagem = MSG_SEM_LIGACAO,
            semLigacao = true,
        )
    }
}
