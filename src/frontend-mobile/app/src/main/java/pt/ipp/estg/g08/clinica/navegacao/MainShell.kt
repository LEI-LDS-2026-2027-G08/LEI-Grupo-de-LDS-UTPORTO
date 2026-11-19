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
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import androidx.navigation.NavBackStackEntry
import androidx.navigation.NavDestination.Companion.hierarchy
import androidx.navigation.NavType
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.currentBackStackEntryAsState
import androidx.navigation.compose.rememberNavController
import androidx.navigation.navArgument
import pt.ipp.estg.g08.clinica.AppContainer
import pt.ipp.estg.g08.clinica.R
import pt.ipp.estg.g08.clinica.features.animais.AnimaisScreen
import pt.ipp.estg.g08.clinica.features.animais.AnimaisViewModel
import pt.ipp.estg.g08.clinica.features.animais.AnimalDetalheScreen
import pt.ipp.estg.g08.clinica.features.animais.AnimalDetalheViewModel
import pt.ipp.estg.g08.clinica.features.boletim.BoletimScreen
import pt.ipp.estg.g08.clinica.features.boletim.BoletimViewModel
import pt.ipp.estg.g08.clinica.features.consultas.ConsultasScreen
import pt.ipp.estg.g08.clinica.features.consultas.ConsultasViewModel

/** Separador da barra inferior; [rotas] são os ecrãs em que fica assinalado. */
private data class Separador(
    val rotas: Set<String>,
    val destino: String,
    @param:StringRes val rotulo: Int,
    val icone: ImageVector,
    val etiqueta: String,
)

private val SEPARADORES =
    listOf(
        Separador(
            rotas =
            setOf(Rotas.ANIMAIS, Rotas.ANIMAL_DETALHE, Rotas.BOLETIM, Rotas.CONSULTAS_ANIMAL),
            destino = Rotas.ANIMAIS,
            rotulo = R.string.nav_animais,
            icone = Icons.Filled.Home,
            etiqueta = "nav-animais",
        ),
        Separador(
            rotas = setOf(Rotas.CONSULTAS),
            destino = Rotas.CONSULTAS,
            rotulo = R.string.nav_consultas,
            icone = Icons.Filled.DateRange,
            etiqueta = "nav-consultas",
        ),
    )

/**
 * Área autenticada: barra de navegação inferior (Animais e Consultas) e a ação
 * "Terminar sessão", sempre acessível.
 *
 * Os animais (M02, M03) e o boletim (M06) são da Issue I122; as Consultas (M04) são da I123: o
 * separador Consultas mostra todos os animais e "Ver consultas" em M03 abre o mesmo ecrã só com
 * esse animal, no percurso dos animais. Cada ecrã recebe o seu `ViewModel`, criado com os
 * serviços do [container] e guardado na respetiva entrada da navegação.
 */
@Composable
fun MainShell(
    container: AppContainer,
    onTerminarSessao: () -> Unit,
    modifier: Modifier = Modifier,
) {
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
                        destinoAtual?.hierarchy?.any { it.route in separador.rotas } == true,
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
                val viewModel: AnimaisViewModel =
                    viewModel(
                        factory =
                        viewModelFactory {
                            initializer { AnimaisViewModel(container.animaisService) }
                        },
                    )
                AnimaisScreen(
                    viewModel = viewModel,
                    onAbrirAnimal = { animalId ->
                        navController.navigate(Rotas.animalDetalhe(animalId))
                    },
                )
            }
            composable(Rotas.ANIMAL_DETALHE, arguments = listOf(argumentoAnimal)) { entrada ->
                val animalId = entrada.animalId()
                val viewModel: AnimalDetalheViewModel =
                    viewModel(
                        factory =
                        viewModelFactory {
                            initializer {
                                AnimalDetalheViewModel(container.animaisService, animalId)
                            }
                        },
                    )
                AnimalDetalheScreen(
                    viewModel = viewModel,
                    onVerBoletim = { navController.navigate(Rotas.boletim(animalId)) },
                    onVerConsultas = {
                        navController.navigate(Rotas.consultasDoAnimal(animalId))
                    },
                    onVoltar = { navController.popBackStack() },
                )
            }
            composable(Rotas.CONSULTAS) {
                val viewModel: ConsultasViewModel =
                    viewModel(
                        factory =
                        viewModelFactory {
                            initializer { ConsultasViewModel(container.consultasService) }
                        },
                    )
                ConsultasScreen(viewModel = viewModel, onVoltar = null)
            }
            composable(Rotas.CONSULTAS_ANIMAL, arguments = listOf(argumentoAnimal)) { entrada ->
                val animalId = entrada.animalId()
                val viewModel: ConsultasViewModel =
                    viewModel(
                        factory =
                        viewModelFactory {
                            initializer {
                                ConsultasViewModel(container.consultasService, animalId)
                            }
                        },
                    )
                ConsultasScreen(viewModel = viewModel, onVoltar = { navController.popBackStack() })
            }
            composable(Rotas.BOLETIM, arguments = listOf(argumentoAnimal)) { entrada ->
                val animalId = entrada.animalId()
                val viewModel: BoletimViewModel =
                    viewModel(
                        factory =
                        viewModelFactory {
                            initializer { BoletimViewModel(container.boletimService, animalId) }
                        },
                    )
                BoletimScreen(viewModel = viewModel, onVoltar = { navController.popBackStack() })
            }
        }
    }
}

/** Identificador do animal da rota (`animais/{animalId}`, `.../consultas` ou `boletim/...`). */
private fun NavBackStackEntry.animalId(): Int =
    requireNotNull(arguments) { "Rota sem argumentos: ${destination.route}" }
        .getInt(Rotas.ARG_ANIMAL_ID)
