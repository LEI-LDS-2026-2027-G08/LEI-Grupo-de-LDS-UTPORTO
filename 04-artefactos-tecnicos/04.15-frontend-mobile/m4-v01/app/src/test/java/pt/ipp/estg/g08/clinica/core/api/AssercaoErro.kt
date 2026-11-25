package pt.ipp.estg.g08.clinica.core.api

/** Executa [bloco] e devolve a exceção do tipo [E] que ele lançar; falha se não lançar nenhuma. */
suspend inline fun <reified E : Throwable> esperarErro(bloco: () -> Unit): E {
    try {
        bloco()
    } catch (e: Throwable) {
        if (e is E) return e
        throw e
    }
    throw AssertionError("Era esperada a exceção ${E::class.simpleName}.")
}
