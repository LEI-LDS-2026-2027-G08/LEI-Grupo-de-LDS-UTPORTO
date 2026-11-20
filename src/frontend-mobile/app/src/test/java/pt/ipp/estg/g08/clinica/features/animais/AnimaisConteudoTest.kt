package pt.ipp.estg.g08.clinica.features.animais

import androidx.compose.ui.test.assertCountEquals
import androidx.compose.ui.test.assertIsDisplayed
import androidx.compose.ui.test.assertTextEquals
import androidx.compose.ui.test.hasClickAction
import androidx.compose.ui.test.junit4.createComposeRule
import androidx.compose.ui.test.onAllNodesWithText
import androidx.compose.ui.test.onNodeWithTag
import androidx.compose.ui.test.onNodeWithText
import androidx.compose.ui.test.performClick
import androidx.test.ext.junit.runners.AndroidJUnit4
import org.junit.Assert.assertEquals
import org.junit.Assert.assertNull
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.annotation.Config
import pt.ipp.estg.g08.clinica.features.animais.dto.MeuAnimalListItemDto
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento

/** M02 com Compose UI Test e Robolectric (QG-07), sem backend. */
@RunWith(AndroidJUnit4::class)
@Config(sdk = [35], qualifiers = "w411dp-h891dp")
class AnimaisConteudoTest {
    @get:Rule
    val compose = createComposeRule()

    private var animalAberto: Int? = null
    private var tentativas = 0

    private val bobi = MeuAnimalListItemDto(3, "Bobi", "Cão", "Labrador", 4, 29.1)
    private val mia = MeuAnimalListItemDto(8, "Mia", "Gato", "Europeu Comum", 1, 4.2)

    private fun mostrar(estado: EstadoCarregamento<List<MeuAnimalListItemDto>>) {
        compose.setContent {
            AnimaisConteudo(
                estado = estado,
                onTentarNovamente = { tentativas++ },
                onAbrirAnimal = { animalAberto = it },
            )
        }
    }

    @Test
    fun `mostra os animais do Cliente com as etiquetas do modelo`() {
        mostrar(EstadoCarregamento.Sucesso(listOf(bobi, mia)))

        compose.onNodeWithTag("m02-lista-animais").assertIsDisplayed()
        compose.onNodeWithTag("m02-animal-3").assertIsDisplayed()
        compose.onNodeWithTag("m02-animal-8").assertIsDisplayed()
        compose.onNodeWithText("Bobi").assertIsDisplayed()
        compose.onNodeWithText("Cão • Labrador").assertIsDisplayed()
        compose.onNodeWithText("4 anos • 29,1 kg").assertIsDisplayed()
        compose.onNodeWithText("1 ano • 4,2 kg").assertIsDisplayed()
        compose.onNodeWithTag("m02-vazio").assertDoesNotExist()
    }

    @Test
    fun `tocar num animal abre o respetivo detalhe`() {
        mostrar(EstadoCarregamento.Sucesso(listOf(bobi, mia)))

        compose.onNodeWithTag("m02-animal-8").performClick()

        assertEquals(8, animalAberto)
    }

    @Test
    fun `so ha acoes para abrir os animais, sem criar, alterar ou eliminar`() {
        mostrar(EstadoCarregamento.Sucesso(listOf(bobi, mia)))

        compose.onAllNodes(hasClickAction()).assertCountEquals(2)
        compose.onAllNodesWithText("Ver detalhe").assertCountEquals(2)
    }

    @Test
    fun `dados opcionais em falta nao aparecem na lista`() {
        mostrar(
            EstadoCarregamento.Sucesso(
                listOf(MeuAnimalListItemDto(9, "Tico", "Ave", raca = null, idadeAnos = 0)),
            ),
        )

        compose.onNodeWithText("Ave").assertIsDisplayed()
        compose.onNodeWithText("Menos de 1 ano").assertIsDisplayed()
    }

    @Test
    fun `sem animais mostra o estado vazio (UC08 A1)`() {
        mostrar(EstadoCarregamento.Vazio)

        compose
            .onNodeWithTag("m02-vazio")
            .assertTextEquals("Ainda não tem animais associados à sua conta.")
        compose.onNodeWithTag("m02-lista-animais").assertDoesNotExist()
    }

    @Test
    fun `a carregar mostra o indicador de progresso`() {
        mostrar(EstadoCarregamento.ACarregar)

        compose.onNodeWithTag("estado-a-carregar").assertExists()
        compose.onNodeWithTag("m02-lista-animais").assertDoesNotExist()
    }

    @Test
    fun `erro mostra a mensagem e Tentar novamente volta a pedir`() {
        mostrar(EstadoCarregamento.Erro("Sem ligação ao servidor. Verifique a sua ligação à rede."))

        compose
            .onNodeWithTag("estado-erro")
            .assertTextEquals("Sem ligação ao servidor. Verifique a sua ligação à rede.")
        compose.onNodeWithTag("estado-tentar-novamente").performClick()

        assertEquals(1, tentativas)
        assertNull(animalAberto)
    }
}
