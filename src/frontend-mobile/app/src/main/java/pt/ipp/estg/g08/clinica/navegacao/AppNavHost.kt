package pt.ipp.estg.g08.clinica.navegacao

import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.lifecycle.compose.LifecycleResumeEffect
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import pt.ipp.estg.g08.clinica.AppContainer
import pt.ipp.estg.g08.clinica.BuildConfig
import pt.ipp.estg.g08.clinica.features.login.LoginScreen
import pt.ipp.estg.g08.clinica.features.tecnico.EcraTecnicoScreen
import pt.ipp.estg.g08.clinica.sessao.SessaoViewModel

/**
 * Navegação de topo. O [SessaoViewModel] decide o que se mostra (através do [AuthGate]):
 * - sem sessão: início de sessão (M01) e, só em debug, o ecrã técnico;
 * - com sessão de Cliente: a área autenticada ([MainShell]).
 *
 * Quando a sessão termina (Terminar sessão, 401 da API ou token expirado), o [AuthGate] volta
 * ao início de sessão e a pilha de navegação da área autenticada é descartada.
 */
@Composable
fun AppNavHost(container: AppContainer, modifier: Modifier = Modifier) {
    val sessao: SessaoViewModel =
        viewModel(
            factory =
            viewModelFactory {
                initializer {
                    SessaoViewModel(
                        authService = container.authService,
                        tokenStorage = container.tokenStorage,
                        apiClient = container.apiClient,
                        sessaoLocal = container.sessaoLocal,
                    )
                }
            },
        )

    // Ao voltar a primeiro plano, uma sessão cujo token já expirou termina de imediato.
    LifecycleResumeEffect(sessao) {
        sessao.verificarExpiracao()
        onPauseOrDispose { }
    }

    AuthGate(
        sessao = sessao,
        modifier = modifier,
        areaNaoAutenticada = { AreaNaoAutenticada(container = container, sessao = sessao) },
        areaAutenticada = { MainShell(onTerminarSessao = sessao::logout) },
    )
}

@Composable
private fun AreaNaoAutenticada(
    container: AppContainer,
    sessao: SessaoViewModel,
    modifier: Modifier = Modifier,
) {
    val navController = rememberNavController()
    NavHost(
        navController = navController,
        startDestination = Rotas.LOGIN,
        modifier = modifier,
    ) {
        composable(Rotas.LOGIN) {
            val abrirTecnico = { navController.navigate(Rotas.TECNICO) }
            LoginScreen(sessao = sessao, onAbrirTecnico = abrirTecnico.takeIf { BuildConfig.DEBUG })
        }
        // O ecrã técnico só existe em debug; em release a rota nem é registada.
        if (BuildConfig.DEBUG) {
            composable(Rotas.TECNICO) { EcraTecnicoScreen(container = container) }
        }
    }
}
