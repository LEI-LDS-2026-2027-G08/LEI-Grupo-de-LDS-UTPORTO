package pt.ipp.estg.g08.clinica.navegacao

/**
 * Rotas da aplicação (secção 6 do Modelo de Classes Mobile).
 *
 * [LOGIN] e [TECNICO] só existem fora da sessão; as restantes só existem dentro da área
 * autenticada (`MainShell`), que o `AuthGate` só mostra com uma sessão de Cliente válida.
 */
object Rotas {
    const val ARG_ANIMAL_ID = "animalId"

    const val LOGIN = "login"
    const val TECNICO = "tecnico"

    const val ANIMAIS = "animais"
    const val ANIMAL_DETALHE = "animais/{$ARG_ANIMAL_ID}"
    const val BOLETIM = "boletim/{$ARG_ANIMAL_ID}"

    /** M04 de todos os animais do Cliente (separador Consultas). */
    const val CONSULTAS = "consultas"

    /**
     * M04 só de um animal, aberto a partir de M03 ("Ver consultas"). É uma rota própria do
     * percurso dos animais: o separador Consultas mostra sempre todos os animais e não fica
     * preso ao filtro de um animal aberto antes.
     */
    const val CONSULTAS_ANIMAL = "animais/{$ARG_ANIMAL_ID}/consultas"

    fun animalDetalhe(animalId: Int): String = "animais/$animalId"

    fun consultasDoAnimal(animalId: Int): String = "animais/$animalId/consultas"

    fun boletim(animalId: Int): String = "boletim/$animalId"
}
