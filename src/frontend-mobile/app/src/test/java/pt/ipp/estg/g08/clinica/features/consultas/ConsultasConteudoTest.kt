package pt.ipp.estg.g08.clinica.features.consultas

import androidx.compose.ui.test.assertCountEquals
import androidx.compose.ui.test.assertIsSelected
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
import java.time.Instant
import java.time.ZoneId
import org.junit.Assert.assertEquals
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.annotation.Config
import pt.ipp.estg.g08.clinica.features.consultas.dto.AnimalRefDto
import pt.ipp.estg.g08.clinica.features.consultas.dto.EstadoConsulta
import pt.ipp.estg.g08.clinica.features.consultas.dto.MinhaConsultaDto
import pt.ipp.estg.g08.clinica.features.consultas.dto.TipoConsultaFiltro
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento

/** M04 com Compose UI Test e Robolectric (QG-07), sem backend. */
@RunWith(AndroidJUnit4::class)
@Config(sdk = [35], qualifiers = "w411dp-h891dp")
class ConsultasConteudoTest {
    @get:Rule
    val compose = createComposeRule()

    private val lisboa = ZoneId.of("Europe/Lisbon")
    private val tentativas = mutableListOf<TipoConsultaFiltro>()
    private var voltas = 0

    private val bobi = AnimalRefDto(3, "Bobi", "Cão")
    private val futuras =
        listOf(
            MinhaConsultaDto(
                id = 7,
                dataHora = Instant.parse("2026-12-01T10:00:00Z"),
                estado = EstadoConsulta.Confirmada,
                animal = bobi,
                veterinario = "Dra. Ana Veterinária",
            ),
            MinhaConsultaDto(
                id = 8,
                dataHora = Instant.parse("2026-12-15T14:30:00Z"),
                estado = EstadoConsulta.Pendente,
                animal = AnimalRefDto(4, "Mia", null),
                veterinario = "Dr. Bruno Veterinário",
            ),
        )
    private val historico =
        listOf(
            MinhaConsultaDto(
                id = 5,
                dataHora = Instant.parse("2026-11-05T11:00:00Z"),
                estado = EstadoConsulta.Pendente,
                animal = bobi,
                veterinario = "Dra. Ana Veterinária",
            ),
            MinhaConsultaDto(
                id = 1,
                dataHora = Instant.parse("2026-10-14T09:30:00Z"),
                estado = EstadoConsulta.Realizada,
                animal = bobi,
                veterinario = "Dra. Ana Veterinária",
            ),
        )

    private fun mostrar(
        estadoFuturas: EstadoCarregamento<List<MinhaConsultaDto>> =
            EstadoCarregamento.Sucesso(futuras),
        estadoHistorico: EstadoCarregamento<List<MinhaConsultaDto>> =
            EstadoCarregamento.Sucesso(historico),
        doAnimal: Boolean = false,
    ) {
        compose.setContent {
            ConsultasConteudo(
                estadoFuturas = estadoFuturas,
                estadoHistorico = estadoHistorico,
                doAnimal = doAnimal,
                onTentarNovamente = { tentativas.add(it) },
                onVoltar = if (doAnimal) ({ voltas++ }) else null,
                fuso = lisboa,
            )
        }
    }

    private fun naConsulta(id: Int, texto: String) =
        compose.onNode(hasText(texto) and hasAnyAncestor(hasTestTag("m04-consulta-$id")))

    private fun abrirHistorico() {
        compose.onNodeWithTag("m04-separador-historico").performClick()
    }

    @Test
    fun `abre no separador Proximas com as Consultas futuras (RF028)`() {
        mostrar()

        compose.onNodeWithTag("m04-separador-futuras").assertIsSelected()
        compose.onNodeWithTag("m04-lista-futuras").assertExists()
        compose.onNodeWithTag("m04-lista-historico").assertDoesNotExist()
        naConsulta(7, "01/12/2026 • 10:00").assertExists()
        naConsulta(7, "Bobi • Cão").assertExists()
        naConsulta(7, "Dra. Ana Veterinária").assertExists()
        naConsulta(7, "Confirmada").assertExists()
    }

    @Test
    fun `especie em falta e omitida e a hora e a de Lisboa`() {
        mostrar()

        naConsulta(8, "Mia").assertExists()
        naConsulta(8, "15/12/2026 • 14:30").assertExists()
        naConsulta(8, "Pendente").assertExists()
    }

    @Test
    fun `o separador Historico mostra as marcacoes passadas (RF029)`() {
        mostrar()

        abrirHistorico()

        compose.onNodeWithTag("m04-separador-historico").assertIsSelected()
        compose.onNodeWithTag("m04-lista-historico").assertExists()
        compose.onNodeWithTag("m04-lista-futuras").assertDoesNotExist()
        // 14/10/2026 09:30 UTC é 10:30 em Lisboa (hora de verão).
        naConsulta(1, "14/10/2026 • 10:30").assertExists()
        naConsulta(1, "Realizada").assertExists()
        naConsulta(5, "05/11/2026 • 11:00").assertExists()
    }

    @Test
    fun `as listas seguem a ordem devolvida pela API`() {
        mostrar()
        abrirHistorico()

        val datas =
            compose.onAllNodes(
                hasText(" • ", substring = true) and hasText(":", substring = true) and
                    hasAnyAncestor(hasTestTag("m04-lista-historico")),
            )
        datas[0].assertTextEquals("05/11/2026 • 11:00")
        datas[1].assertTextEquals("14/10/2026 • 10:30")
    }

    @Test
    fun `so mostra informacao administrativa (RF029)`() {
        mostrar()
        abrirHistorico()

        // Estado, data e hora, animal e Veterinário: quatro textos por marcação, nada mais.
        compose
            .onAllNodes(
                hasAnyAncestor(hasTestTag("m04-consulta-1")) and hasText("", substring = true),
            )
            .assertCountEquals(4)
    }

    @Test
    fun `sem acoes de marcar, alterar, confirmar ou cancelar (RF028)`() {
        mostrar()

        // Só os dois separadores são acionáveis; as marcações não têm ações.
        compose.onAllNodes(hasClickAction()).assertCountEquals(2)
        abrirHistorico()
        compose.onAllNodes(hasClickAction()).assertCountEquals(2)
    }

    @Test
    fun `sem Consultas futuras mostra a mensagem do separador`() {
        mostrar(estadoFuturas = EstadoCarregamento.Vazio)

        compose.onNodeWithTag("m04-vazio-futuras").assertTextEquals("Não tem Consultas marcadas.")
        abrirHistorico()
        compose.onNodeWithTag("m04-lista-historico").assertExists()
    }

    @Test
    fun `sem historico mostra a mensagem do separador`() {
        mostrar(estadoHistorico = EstadoCarregamento.Vazio)

        abrirHistorico()

        compose
            .onNodeWithTag("m04-vazio-historico")
            .assertTextEquals("Ainda não há Consultas no histórico.")
    }

    @Test
    fun `so de um animal tem subtitulo, mensagens proprias e Voltar`() {
        mostrar(
            estadoFuturas = EstadoCarregamento.Vazio,
            estadoHistorico = EstadoCarregamento.Vazio,
            doAnimal = true,
        )

        compose
            .onNodeWithTag("m04-so-deste-animal")
            .assertTextEquals("Só as Consultas deste animal.")
        compose
            .onNodeWithTag("m04-vazio-futuras")
            .assertTextEquals("Este animal não tem Consultas marcadas.")
        abrirHistorico()
        compose
            .onNodeWithTag("m04-vazio-historico")
            .assertTextEquals("Este animal ainda não tem Consultas no histórico.")

        compose.onNodeWithTag("m04-voltar").performClick()
        assertEquals(1, voltas)
    }

    @Test
    fun `de todos os animais nao tem Voltar nem subtitulo`() {
        mostrar()

        compose.onNodeWithTag("m04-voltar").assertDoesNotExist()
        compose.onNodeWithTag("m04-so-deste-animal").assertDoesNotExist()
    }

    @Test
    fun `a carregar mostra o indicador de progresso`() {
        mostrar(estadoFuturas = EstadoCarregamento.ACarregar)

        compose.onNodeWithTag("estado-a-carregar").assertExists()
    }

    @Test
    fun `erro mostra so a mensagem e Tentar novamente pede a lista do separador`() {
        val recusado = EstadoCarregamento.Erro("Não tem acesso a esta informação.")
        mostrar(estadoFuturas = recusado, estadoHistorico = recusado, doAnimal = true)

        compose.onNodeWithTag("estado-erro").assertTextEquals("Não tem acesso a esta informação.")
        compose.onNodeWithTag("m04-lista-futuras").assertDoesNotExist()
        compose.onNodeWithTag("estado-tentar-novamente").performClick()
        abrirHistorico()
        compose.onNodeWithTag("m04-lista-historico").assertDoesNotExist()
        compose.onNodeWithText("Tentar novamente").performClick()

        assertEquals(listOf(TipoConsultaFiltro.Futuras, TipoConsultaFiltro.Historico), tentativas)
    }
}
