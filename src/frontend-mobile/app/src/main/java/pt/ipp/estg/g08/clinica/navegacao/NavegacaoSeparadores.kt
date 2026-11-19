package pt.ipp.estg.g08.clinica.navegacao

import androidx.navigation.NavController

/**
 * Abre o ecrã inicial de um separador da barra inferior ([MainShell]).
 *
 * Retira da pilha tudo o que está acima da lista de animais (o destino inicial) e abre o
 * [destino] sem o duplicar. Assim, carregar em Animais volta sempre à lista "Os meus animais",
 * também a partir do detalhe (M03), do boletim (M06) ou das Consultas de um animal (M04), e
 * carregar no separador já aberto não faz nada.
 *
 * A pilha de cada separador não é guardada nem restaurada (`saveState`/`restoreState`): com a
 * lista de animais como destino inicial, carregar em Animais restaurava a pilha acabada de
 * guardar (por exemplo, detalhe → Consultas) e a aplicação ficava no mesmo ecrã.
 */
fun NavController.abrirSeparador(destino: String) {
    navigate(destino) {
        popUpTo(Rotas.ANIMAIS)
        launchSingleTop = true
    }
}
