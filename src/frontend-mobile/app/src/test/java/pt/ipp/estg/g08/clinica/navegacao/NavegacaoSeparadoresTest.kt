package pt.ipp.estg.g08.clinica.navegacao

import androidx.compose.material3.Text
import androidx.compose.ui.test.junit4.createComposeRule
import androidx.compose.ui.test.onNodeWithText
import androidx.navigation.NavHostController
import androidx.navigation.NavType
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import androidx.navigation.navArgument
import androidx.test.ext.junit.runners.AndroidJUnit4
import org.junit.Assert.assertEquals
import org.junit.Assert.assertNull
import org.junit.Before
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.annotation.Config

/**
 * Regressão da barra inferior ([abrirSeparador]) com o Navigation Compose real e as rotas da
 * área autenticada; cada ecrã é substituído por um texto com o seu nome.
 */
@RunWith(AndroidJUnit4::class)
@Config(sdk = [35])
class NavegacaoSeparadoresTest {
    @get:Rule
    val compose = createComposeRule()

    private lateinit var nav: NavHostController

    @Before
    fun montar() {
        compose.setContent {
            nav = rememberNavController()
            val argumentoAnimal = navArgument(Rotas.ARG_ANIMAL_ID) { type = NavType.IntType }
            NavHost(navController = nav, startDestination = Rotas.ANIMAIS) {
                composable(Rotas.ANIMAIS) { Text("lista") }
                composable(Rotas.ANIMAL_DETALHE, arguments = listOf(argumentoAnimal)) {
                    Text("detalhe")
                }
                composable(Rotas.BOLETIM, arguments = listOf(argumentoAnimal)) { Text("boletim") }
                composable(
                    Rotas.CONSULTAS,
                    arguments =
                    listOf(
                        navArgument(Rotas.ARG_ANIMAL_ID) {
                            type = NavType.IntType
                            defaultValue = Rotas.SEM_ANIMAL
                        },
                    ),
                ) { Text("consultas") }
            }
        }
    }

    private fun ir(rota: String) {
        compose.runOnIdle { nav.navigate(rota) }
    }

    private fun separador(destino: String) {
        compose.runOnIdle { nav.abrirSeparador(destino) }
    }

    /** Está na lista "Os meus animais", sem nada por baixo na pilha. */
    private fun verificarListaNaRaiz() {
        compose.onNodeWithText("lista").assertExists()
        compose.runOnIdle {
            assertEquals(Rotas.ANIMAIS, nav.currentDestination?.route)
            assertNull(nav.previousBackStackEntry)
        }
    }

    @Test
    fun `Animais nas Consultas de um animal volta a lista (defeito da revisao)`() {
        ir(Rotas.animalDetalhe(5))
        ir(Rotas.consultas(5))
        compose.onNodeWithText("consultas").assertExists()

        separador(Rotas.ANIMAIS)

        verificarListaNaRaiz()
        compose.onNodeWithText("consultas").assertDoesNotExist()
    }

    @Test
    fun `Animais no detalhe volta a lista`() {
        ir(Rotas.animalDetalhe(5))

        separador(Rotas.ANIMAIS)

        verificarListaNaRaiz()
        compose.onNodeWithText("detalhe").assertDoesNotExist()
    }

    @Test
    fun `Animais no boletim volta a lista`() {
        ir(Rotas.animalDetalhe(5))
        ir(Rotas.boletim(5))

        separador(Rotas.ANIMAIS)

        verificarListaNaRaiz()
        compose.onNodeWithText("boletim").assertDoesNotExist()
    }

    @Test
    fun `depois de ir a Consultas, Animais volta a lista e nao ao detalhe anterior`() {
        ir(Rotas.animalDetalhe(5))
        separador(Rotas.consultas())
        compose.onNodeWithText("consultas").assertExists()

        separador(Rotas.ANIMAIS)

        verificarListaNaRaiz()
        compose.onNodeWithText("detalhe").assertDoesNotExist()
    }

    @Test
    fun `Consultas abre por cima da lista e recuar volta a lista`() {
        ir(Rotas.animalDetalhe(5))

        separador(Rotas.consultas())

        compose.onNodeWithText("consultas").assertExists()
        compose.runOnIdle {
            assertEquals(Rotas.ANIMAIS, nav.previousBackStackEntry?.destination?.route)
            nav.popBackStack()
        }
        verificarListaNaRaiz()
    }

    @Test
    fun `Animais na lista nao muda nada`() {
        separador(Rotas.ANIMAIS)

        verificarListaNaRaiz()
    }
}
