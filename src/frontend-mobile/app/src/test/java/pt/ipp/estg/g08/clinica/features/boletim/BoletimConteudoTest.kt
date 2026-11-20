package pt.ipp.estg.g08.clinica.features.boletim

import androidx.compose.ui.test.assertCountEquals
import androidx.compose.ui.test.assertIsDisplayed
import androidx.compose.ui.test.assertTextEquals
import androidx.compose.ui.test.hasAnyAncestor
import androidx.compose.ui.test.hasClickAction
import androidx.compose.ui.test.hasTestTag
import androidx.compose.ui.test.hasText
import androidx.compose.ui.test.junit4.createComposeRule
import androidx.compose.ui.test.onNodeWithTag
import androidx.compose.ui.test.onNodeWithText
import androidx.compose.ui.test.performClick
import androidx.test.ext.junit.runners.AndroidJUnit4
import java.time.LocalDate
import org.junit.Assert.assertEquals
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.annotation.Config
import pt.ipp.estg.g08.clinica.features.boletim.dto.HistoricoAdministracaoDto
import pt.ipp.estg.g08.clinica.features.boletim.dto.MeuBoletimDto
import pt.ipp.estg.g08.clinica.features.boletim.dto.ProximaAdministracaoDto
import pt.ipp.estg.g08.clinica.features.boletim.dto.TipoAdministracao
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento

/** M06 com Compose UI Test e Robolectric (QG-07), sem backend. */
@RunWith(AndroidJUnit4::class)
@Config(sdk = [35], qualifiers = "w411dp-h891dp")
class BoletimConteudoTest {
    @get:Rule
    val compose = createComposeRule()

    private var voltas = 0
    private var tentativas = 0

    private val boletim =
        MeuBoletimDto(
            historico =
            listOf(
                HistoricoAdministracaoDto(
                    TipoAdministracao.Vacina,
                    "Vacina antirrábica",
                    LocalDate.of(2026, 10, 19),
                ),
                HistoricoAdministracaoDto(
                    TipoAdministracao.Desparasitacao,
                    "Desparasitante interno Y",
                    LocalDate.of(2026, 9, 1),
                ),
            ),
            proximas =
            listOf(
                ProximaAdministracaoDto(
                    TipoAdministracao.Desparasitacao,
                    "Desparasitante interno Y",
                    LocalDate.of(2027, 1, 17),
                ),
                ProximaAdministracaoDto(
                    TipoAdministracao.Vacina,
                    "Vacina antirrábica",
                    LocalDate.of(2027, 10, 19),
                ),
            ),
        )

    private fun mostrar(estado: EstadoCarregamento<MeuBoletimDto>) {
        compose.setContent {
            BoletimConteudo(
                estado = estado,
                onTentarNovamente = { tentativas++ },
                onVoltar = { voltas++ },
            )
        }
    }

    private fun naSeccao(etiqueta: String, texto: String) =
        compose.onNode(hasText(texto) and hasAnyAncestor(hasTestTag(etiqueta)))

    @Test
    fun `mostra as proximas administracoes com as datas da API`() {
        mostrar(EstadoCarregamento.Sucesso(boletim))

        compose.onNodeWithTag("m06-proximas").assertExists()
        naSeccao("m06-proximas", "Prevista para 17/01/2027").assertExists()
        naSeccao("m06-proximas", "Prevista para 19/10/2027").assertExists()
        compose
            .onAllNodes(hasText("Prevista para", substring = true))
            .assertCountEquals(2)
    }

    @Test
    fun `mostra o historico de vacinacoes e desparasitacoes`() {
        mostrar(EstadoCarregamento.Sucesso(boletim))

        compose.onNodeWithTag("m06-historico").assertExists()
        naSeccao("m06-historico", "Administrada em 19/10/2026").assertExists()
        naSeccao("m06-historico", "Administrada em 01/09/2026").assertExists()
        naSeccao("m06-historico", "Vacina").assertExists()
        naSeccao("m06-historico", "Desparasitação").assertExists()
    }

    @Test
    fun `as listas seguem a ordem devolvida pela API`() {
        mostrar(EstadoCarregamento.Sucesso(boletim))

        val proximas =
            compose.onAllNodes(
                hasText("Prevista para", substring = true) and
                    hasAnyAncestor(hasTestTag("m06-proximas")),
            )
        proximas[0].assertTextEquals("Prevista para 17/01/2027")
        proximas[1].assertTextEquals("Prevista para 19/10/2027")
    }

    @Test
    fun `secao sem registos mostra a indicacao propria`() {
        mostrar(
            EstadoCarregamento.Sucesso(
                boletim.copy(proximas = emptyList()),
            ),
        )

        naSeccao("m06-proximas", "Não há administrações previstas.").assertExists()
        naSeccao("m06-historico", "Administrada em 19/10/2026").assertExists()
    }

    @Test
    fun `animal sem registos de profilaxia mostra o estado vazio (UC17 A1)`() {
        mostrar(EstadoCarregamento.Vazio)

        compose
            .onNodeWithTag("m06-vazio")
            .assertTextEquals(
                "Este animal ainda não tem vacinações nem desparasitações registadas.",
            )
        compose.onNodeWithTag("m06-historico").assertDoesNotExist()
        compose.onNodeWithTag("m06-proximas").assertDoesNotExist()
    }

    @Test
    fun `boletim de animal de outro Cliente mostra so a mensagem (UC17 E1)`() {
        mostrar(EstadoCarregamento.Erro("Não tem acesso a esta informação."))

        compose.onNodeWithTag("estado-erro").assertTextEquals("Não tem acesso a esta informação.")
        compose.onNodeWithTag("m06-historico").assertDoesNotExist()
        compose.onNodeWithTag("m06-proximas").assertDoesNotExist()
        compose.onNodeWithTag("estado-tentar-novamente").performClick()
        assertEquals(1, tentativas)
    }

    @Test
    fun `so ha a acao Voltar, sem registar ou alterar administracoes`() {
        mostrar(EstadoCarregamento.Sucesso(boletim))

        compose.onAllNodes(hasClickAction()).assertCountEquals(1)
        compose.onNodeWithText("Voltar").assertIsDisplayed().performClick()
        assertEquals(1, voltas)
    }
}
