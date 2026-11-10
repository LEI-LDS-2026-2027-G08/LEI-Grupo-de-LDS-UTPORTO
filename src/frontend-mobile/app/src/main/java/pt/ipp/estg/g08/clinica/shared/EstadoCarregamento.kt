package pt.ipp.estg.g08.clinica.shared

/** Estados de carregamento comuns a todos os ecrãs de consulta (secção 7.2 do modelo). */
sealed interface EstadoCarregamento<out T> {
    data object Inicial : EstadoCarregamento<Nothing>

    data object ACarregar : EstadoCarregamento<Nothing>

    data class Sucesso<out T>(val dados: T) : EstadoCarregamento<T>

    data object Vazio : EstadoCarregamento<Nothing>

    data class Erro(val mensagem: String) : EstadoCarregamento<Nothing>
}
