package pt.ipp.estg.g08.clinica.navegacao

import androidx.annotation.StringRes
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.statusBarsPadding
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.DateRange
import androidx.compose.material.icons.filled.Home
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.NavigationBar
import androidx.compose.material3.NavigationBarItem
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.navigation.NavDestination.Companion.hierarchy
import androidx.navigation.NavType
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.currentBackStackEntryAsState
import androidx.navigation.compose.rememberNavController
import androidx.navigation.navArgument
import pt.ipp.estg.g08.clinica.R
import pt.ipp.estg.g08.clinica.shared.components.EcraPorImplementar

private data class Separador(
    val rotaPadrao: String,
    val destino: String,
    @param:StringRes val rotulo: Int,
    val icone: ImageVector,
    val etiqueta: String,
)

private val SEPARADORES =
    listOf(
        Separador(
            rotaPadrao = Rotas.ANIMAIS,
            destino = Rotas.ANIMAIS,
            rotulo = R.string.nav_animais,
            icone = Icons.Filled.Home,
            etiqueta = "nav-animais",
        ),
        Separador(
            rotaPadrao = Rotas.CONSULTAS,
            destino = Rotas.consultas(),
            rotulo = R.string.nav_consultas,
            icone = Icons.Filled.DateRange,
            etiqueta = "nav-consultas",
        ),
    )

/**
 * Área autenticada: barra de navegação inferior (Animais e Consultas) e a ação
 * "Terminar sessão", sempre acessível. Os ecrãs são provisórios até às Issues I122 e I123.
 */
@Composable
fun MainShell(onTerminarSessao: () -> Unit, modifier: Modifier = Modifier) {
    val navController = rememberNavController()
    val entradaAtual by navController.currentBackStackEntryAsState()
    val destinoAtual = entradaAtual?.destination

    Scaffold(
        modifier = modifier,
        topBar = {
            Row(
                modifier =
                Modifier
                    .fillMaxWidth()
                    .statusBarsPadding()
                    .padding(horizontal = 16.dp),
                horizontalArrangement = Arrangement.SpaceBetween,
                verticalAlignment = Alignment.CenterVertically,
            ) {
                Text(
                    text = stringResource(R.string.app_name),
                    style = MaterialTheme.typography.titleMedium,
                )
                TextButton(
                    onClick = onTerminarSessao,
                    modifier = Modifier.testTag("nav-terminar-sessao"),
                ) {
                    Text(text = stringResource(R.string.nav_terminar_sessao))
                }
            }
        },
        bottomBar = {
            NavigationBar {
                SEPARADORES.forEach { separador ->
                    NavigationBarItem(
                        selected =
                        destinoAtual?.hierarchy?.any { it.route == separador.rotaPadrao } ==
                            true,
                        onClick = {
                            navController.navigate(separador.destino) {
                                popUpTo(Rotas.ANIMAIS) { saveState = true }
                                launchSingleTop = true
                                restoreState = true
                            }
                        },
                        icon = { Icon(imageVector = separador.icone, contentDescription = null) },
                        label = { Text(text = stringResource(separador.rotulo)) },
                        modifier = Modifier.testTag(separador.etiqueta),
                    )
                }
            }
        },
    ) { margens ->
        val argumentoAnimal = navArgument(Rotas.ARG_ANIMAL_ID) { type = NavType.IntType }
        NavHost(
            navController = navController,
            startDestination = Rotas.ANIMAIS,
            modifier = Modifier.padding(margens),
        ) {
            composable(Rotas.ANIMAIS) {
                EcraPorImplementar(titulo = stringResource(R.string.ecra_m02), issue = "I122")
            }
            composable(Rotas.ANIMAL_DETALHE, arguments = listOf(argumentoAnimal)) {
                EcraPorImplementar(titulo = stringResource(R.string.ecra_m03), issue = "I122")
            }
            composable(
                Rotas.CONSULTAS,
                arguments =
                listOf(
                    navArgument(Rotas.ARG_ANIMAL_ID) {
                        type = NavType.IntType
                        defaultValue = Rotas.SEM_ANIMAL
                    },
                ),
            ) {
                EcraPorImplementar(titulo = stringResource(R.string.ecra_m04), issue = "I123")
            }
            composable(Rotas.BOLETIM, arguments = listOf(argumentoAnimal)) {
                EcraPorImplementar(titulo = stringResource(R.string.ecra_m06), issue = "I122")
            }
        }
    }
}
