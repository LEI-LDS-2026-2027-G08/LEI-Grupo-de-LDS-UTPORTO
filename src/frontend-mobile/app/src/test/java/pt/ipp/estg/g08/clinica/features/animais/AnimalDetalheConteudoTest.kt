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
import androidx.compose.ui.test.performScrollTo
import androidx.test.ext.junit.runners.AndroidJUnit4
import java.time.LocalDate
import org.junit.Assert.assertEquals
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.annotation.Config
import pt.ipp.estg.g08.clinica.features.animais.dto.MeuAnimalDto
import pt.ipp.estg.g08.clinica.features.animais.dto.Sexo
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento

/** M03 com Compose UI Test e Robolectric (QG-07), sem backend. */
@RunWith(AndroidJUnit4::class)
@Config(sdk = [35], qualifiers = "w411dp-h891dp")
class AnimalDetalheConteudoTest {
    @get:Rule
    val compose = createComposeRule()

    private var boletins = 0
    private var consultas = 0
    private var voltas = 0
    private var tentativas = 0

    private val bobi =
        MeuAnimalDto(3, "Bobi", "Cão", "Labrador", Sexo.Macho, LocalDate.of(2022, 5, 10), 4, 29.1)

    private fun mostrar(estado: EstadoCarregamento<MeuAnimalDto>) {
        compose.setContent {
            AnimalDetalheConteudo(
                estado = estado,
                onTentarNovamente = { tentativas++ },
                onVerBoletim = { boletins++ },
                onVerConsultas = { consultas++ },
                onVoltar = { voltas++ },
            )
        }
    }

    @Test
    fun `mostra os dados basicos do animal (UC08 passo 5)`() {
        mostrar(EstadoCarregamento.Sucesso(bobi))

        compose.onNodeWithTag("m03-nome").assertTextEquals("Bobi")
        compose.onNodeWithText("Cão").assertIsDisplayed()
        compose.onNodeWithText("Labrador").assertIsDisplayed()
        compose.onNodeWithText("Macho").assertIsDisplayed()
        compose.onNodeWithText("10/05/2022").assertIsDisplayed()
        compose.onNodeWithText("4 anos").assertIsDisplayed()
        compose.onNodeWithText("29,1 kg").assertIsDisplayed()
    }

    @Test
    fun `dados opcionais em falta aparecem como Sem informacao`() {
        mostrar(
            EstadoCarregamento.Sucesso(
                MeuAnimalDto(9, "Tico", "Ave", raca = null, sexo = Sexo.Femea),
            ),
        )

        compose.onNodeWithText("Fêmea").assertIsDisplayed()
        compose.onAllNodesWithText("Sem informação").assertCountEquals(4)
    }

    @Test
    fun `as ligacoes abrem o boletim e as Consultas do animal`() {
        mostrar(EstadoCarregamento.Sucesso(bobi))

        compose.onNodeWithTag("m03-ver-boletim").performScrollTo().performClick()
        compose.onNodeWithTag("m03-ver-consultas").performScrollTo().performClick()
        compose.onNodeWithTag("m03-voltar").performClick()

        assertEquals(1, boletins)
        assertEquals(1, consultas)
        assertEquals(1, voltas)
    }

    @Test
    fun `nao ha acoes para criar, alterar ou eliminar o animal`() {
        mostrar(EstadoCarregamento.Sucesso(bobi))

        // Voltar, Ver boletim e Ver consultas.
        compose.onAllNodes(hasClickAction()).assertCountEquals(3)
    }

    @Test
    fun `animal de outro Cliente mostra so a mensagem, sem dados (UC08 E1)`() {
        mostrar(EstadoCarregamento.Erro("Não tem acesso a esta informação."))

        compose.onNodeWithTag("estado-erro").assertTextEquals("Não tem acesso a esta informação.")
        compose.onNodeWithTag("m03-nome").assertDoesNotExist()
        compose.onNodeWithTag("m03-ver-boletim").assertDoesNotExist()
        compose.onNodeWithTag("m03-ver-consultas").assertDoesNotExist()
        compose.onNodeWithTag("m03-voltar").assertIsDisplayed()
    }

    @Test
    fun `Tentar novamente volta a pedir o animal`() {
        mostrar(EstadoCarregamento.Erro("Sem ligação ao servidor. Verifique a sua ligação à rede."))

        compose.onNodeWithTag("estado-tentar-novamente").performClick()

        assertEquals(1, tentativas)
    }
}
