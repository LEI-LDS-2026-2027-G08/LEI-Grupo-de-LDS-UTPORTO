package pt.ipp.estg.g08.clinica.shared

import pt.ipp.estg.g08.clinica.core.api.ApiException

/**
 * Executa um pedido à API e converte o resultado no [EstadoCarregamento] do ecrã (secção 7.2 do
 * Modelo de Classes Mobile):
 * - [EstadoCarregamento.Vazio] quando [semDados] indica que a resposta não tem nada a mostrar;
 * - [EstadoCarregamento.Sucesso] com os dados devolvidos, nos restantes casos;
 * - [EstadoCarregamento.Erro] com a mensagem de [ApiException.mensagemParaUtilizador] quando a
 *   API responde com erro ou não há ligação (por exemplo, `403` dá "Não tem acesso a esta
 *   informação." e nenhum dado é apresentado).
 *
 * Um `401` também chega aqui como erro, mas o `ApiClient` já terminou a sessão e a aplicação
 * regressa ao início de sessão.
 */
suspend fun <T> carregarEstado(
    semDados: (T) -> Boolean = { false },
    pedido: suspend () -> T,
): EstadoCarregamento<T> = try {
    val dados = pedido()
    if (semDados(dados)) EstadoCarregamento.Vazio else EstadoCarregamento.Sucesso(dados)
} catch (e: ApiException) {
    EstadoCarregamento.Erro(e.mensagemParaUtilizador())
}
