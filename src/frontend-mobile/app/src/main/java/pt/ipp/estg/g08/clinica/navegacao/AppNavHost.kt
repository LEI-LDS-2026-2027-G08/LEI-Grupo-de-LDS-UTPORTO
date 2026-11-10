package pt.ipp.estg.g08.clinica.navegacao

import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.navigation.NavGraph.Companion.findStartDestination
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import kotlinx.coroutines.launch
import pt.ipp.estg.g08.clinica.AppContainer
import pt.ipp.estg.g08.clinica.features.login.LoginScreen
import pt.ipp.estg.g08.clinica.features.tecnico.EcraTecnicoScreen
import pt.ipp.estg.g08.clinica.sessao.MotivoFimSessao

/**
 * Navegação de topo: início de sessão, área autenticada e (só em debug) o ecrã técnico.
 *
 * Quando a sessão termina (401 da API ou "Terminar sessão"), regressa ao início de sessão.
 */
@Composable
fun AppNavHost(container: AppContainer, modifier: Modifier = Modifier) {
    val navController = rememberNavController()
    val escopo = rememberCoroutineScope()
    var motivoFim by remember { mutableStateOf<MotivoFimSessao?>(null) }

    LaunchedEffect(container) {
        container.sessaoLocal.sessaoTerminada.collect { motivo ->
            motivoFim = motivo
            navController.navigate(Rotas.LOGIN) {
                popUpTo(navController.graph.findStartDestination().id) { inclusive = true }
            }
        }
    }

    NavHost(
        navController = navController,
        startDestination = Rotas.inicial(),
        modifier = modifier,
    ) {
        composable(Rotas.LOGIN) {
            LoginScreen(
                aviso = motivoFim,
                onAbrirTecnico = { navController.navigate(Rotas.TECNICO) },
            )
        }
        composable(Rotas.TECNICO) {
            EcraTecnicoScreen(
                container = container,
                onAbrirPrincipal = { navController.navigate(Rotas.PRINCIPAL) },
            )
        }
        composable(Rotas.PRINCIPAL) {
            MainShell(
                onTerminarSessao = {
                    escopo.launch { container.sessaoLocal.terminar(MotivoFimSessao.Utilizador) }
                },
            )
        }
    }
}
