package pt.ipp.estg.g08.clinica.navegacao

/**
 * Rotas da aplicação (secção 6 do Modelo de Classes Mobile).
 *
 * [LOGIN] e [TECNICO] só existem fora da sessão; as restantes só existem dentro da área
 * autenticada (`MainShell`), que o `AuthGate` só mostra com uma sessão de Cliente válida.
 */
object Rotas {
    const val ARG_ANIMAL_ID = "animalId"
    const val SEM_ANIMAL = -1

    const val LOGIN = "login"
    const val TECNICO = "tecnico"

    const val ANIMAIS = "animais"
    const val ANIMAL_DETALHE = "animais/{$ARG_ANIMAL_ID}"
    const val CONSULTAS = "consultas?$ARG_ANIMAL_ID={$ARG_ANIMAL_ID}"
    const val BOLETIM = "boletim/{$ARG_ANIMAL_ID}"

    fun animalDetalhe(animalId: Int): String = "animais/$animalId"

    fun consultas(animalId: Int? = null): String =
        if (animalId == null) "consultas" else "consultas?$ARG_ANIMAL_ID=$animalId"

    fun boletim(animalId: Int): String = "boletim/$animalId"
}
