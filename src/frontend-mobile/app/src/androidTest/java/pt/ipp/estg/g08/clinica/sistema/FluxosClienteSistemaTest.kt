package pt.ipp.estg.g08.clinica.sistema

import android.os.ParcelFileDescriptor
import androidx.activity.compose.setContent
import androidx.compose.ui.test.SemanticsMatcher
import androidx.compose.ui.test.assertCountEquals
import androidx.compose.ui.test.assertHasNoClickAction
import androidx.compose.ui.test.assertIsNotSelected
import androidx.compose.ui.test.assertIsSelected
import androidx.compose.ui.test.assertTextEquals
import androidx.compose.ui.test.click
import androidx.compose.ui.test.hasAnyAncestor
import androidx.compose.ui.test.hasTestTag
import androidx.compose.ui.test.hasText
import androidx.compose.ui.test.junit4.createAndroidComposeRule
import androidx.compose.ui.test.onAllNodesWithText
import androidx.compose.ui.test.onNodeWithTag
import androidx.compose.ui.test.performClick
import androidx.compose.ui.test.performScrollTo
import androidx.compose.ui.test.performTextReplacement
import androidx.compose.ui.test.performTouchInput
import androidx.navigation.NavHostController
import androidx.test.ext.junit.runners.AndroidJUnit4
import androidx.test.platform.app.InstrumentationRegistry
import java.net.HttpURLConnection
import java.net.URL
import java.time.LocalDate
import java.time.format.DateTimeFormatter
import java.util.concurrent.CopyOnWriteArrayList
import okhttp3.OkHttpClient
import org.json.JSONObject
import org.junit.Assert.assertEquals
import org.junit.Assert.assertFalse
import org.junit.Assert.assertTrue
import org.junit.Rule
import org.junit.Test
import org.junit.rules.RuleChain
import org.junit.rules.TestRule
import org.junit.runner.Description
import org.junit.runner.RunWith
import org.junit.runners.model.Statement
import pt.ipp.estg.g08.clinica.AppContainer
import pt.ipp.estg.g08.clinica.core.config.AppConfig
import pt.ipp.estg.g08.clinica.navegacao.AppNavHost
import pt.ipp.estg.g08.clinica.navegacao.Rotas
import pt.ipp.estg.g08.clinica.ui.theme.GestãoDeClínicaVeterináriaTheme

/** I136, secção 13.1: os 18 cenários atribuídos à I137, contra a API e SQL reais. */
@RunWith(AndroidJUnit4::class)
class FluxosClienteSistemaTest {
    private val compose = createAndroidComposeRule<SistemaTestActivity>()
    private val instrumentacao = InstrumentationRegistry.getInstrumentation()
    private val argumentos = InstrumentationRegistry.getArguments()
    private val pedidos = CopyOnWriteArrayList<Pair<String, Int>>()
    private lateinit var dados: JSONObject
    private lateinit var nav: NavHostController

    /** Reposição antes de cada cenário; a falha é capturada antes de fechar a Activity. */
    private val ambiente = TestRule { base, descricao ->
        object : Statement() {
            override fun evaluate() {
                try {
                    preparar()
                    base.evaluate()
                } catch (falha: Throwable) {
                    capturarFalha(descricao)
                    throw falha
                }
            }
        }
    }

    @get:Rule
    val regras: RuleChain = RuleChain.outerRule(compose).around(ambiente)

    private fun preparar() {
        val ligacao = URL("http://localhost:5160/reset").openConnection() as HttpURLConnection
        ligacao.requestMethod = "POST"
        ligacao.setRequestProperty("Authorization", "Bearer ${argumento("tokenPreparacao")}")
        ligacao.connectTimeout = 120_000
        ligacao.readTimeout = 120_000
        try {
            check(ligacao.responseCode == 200) { "Falhou a preparação da base isolada." }
            dados = JSONObject(ligacao.inputStream.bufferedReader().use { it.readText() })
        } finally {
            ligacao.disconnect()
        }
        val contexto = instrumentacao.targetContext
        check(contexto.getSharedPreferences("sessao", 0).edit().clear().commit())
        pedidos.clear()
        montarInterface()
    }

    private fun montarInterface() {
        val contexto = instrumentacao.targetContext
        val cliente = OkHttpClient.Builder().addInterceptor { cadeia ->
            val resposta = cadeia.proceed(cadeia.request())
            pedidos += cadeia.request().url.encodedPath to resposta.code
            resposta
        }.build()
        val container = AppContainer(contexto, AppConfig("http://localhost:5159"), cliente)
        compose.activityRule.scenario.onActivity { activity ->
            activity.setContent {
                GestãoDeClínicaVeterináriaTheme {
                    AppNavHost(container, aoCriarNavegacaoAutenticada = { nav = it })
                }
            }
        }
        esperar(hasTestTag("m01-email"))
    }

    private fun argumento(nome: String): String =
        requireNotNull(argumentos.getString(nome)) { "Argumento de instrumentação em falta: $nome" }

    private fun esperar(matcher: SemanticsMatcher) {
        compose.waitUntil(30_000) {
            compose.onAllNodes(matcher, useUnmergedTree = true).fetchSemanticsNodes().isNotEmpty()
        }
    }

    private fun texto(valor: String) {
        esperar(hasText(valor, substring = true))
    }

    private fun ausente(valor: String) {
        compose.onAllNodesWithText(valor, substring = true, useUnmergedTree = true)
            .assertCountEquals(0)
    }

    private fun clicar(tag: String) {
        esperar(hasTestTag(tag))
        compose.onNodeWithTag(tag).performClick()
    }

    private fun credenciais(email: String, senha: String) {
        compose.onNodeWithTag("m01-email").performTextReplacement(email)
        compose.onNodeWithTag("m01-password").performTextReplacement(senha)
        clicar("m01-entrar")
    }

    private fun entrar(conta: String = "joao") {
        val email = if (conta ==
            "vazio"
        ) {
            dados.getString("emailVazio")
        } else {
            "cliente.$conta@clinica-g08.test"
        }
        val senha = when (conta) {
            "vazio" -> dados.getString("senhaVazio")
            "maria" -> argumento("senhaClienteMaria")
            else -> argumento("senhaClienteJoao")
        }
        credenciais(email, senha)
        esperar(hasTestTag(if (conta == "vazio") "m02-vazio" else "m02-lista-animais"))
    }

    private fun sair() {
        clicar("nav-terminar-sessao")
        esperar(hasTestTag("m01-email"))
    }

    private fun detalhe(animal: String = "bobi") {
        clicar("m02-animal-${dados.getInt(animal)}")
        esperar(hasTestTag("m03-nome"))
    }

    private fun boletim(animal: String = "bobi") {
        detalhe(animal)
        clicar("m03-ver-boletim")
        esperar(hasTestTag(if (animal == "mia") "m06-vazio" else "m06-proximas"))
    }

    private fun navegar(rota: String) {
        compose.runOnIdle { nav.navigate(rota) }
    }

    private fun semEscrita() {
        listOf("Criar", "Alterar", "Eliminar", "Marcar consulta", "Confirmar", "Cancelar")
            .forEach(::ausente)
    }

    private fun capturarFalha(descricao: Description) {
        runCatching {
            val execucao = argumento("idExecucao")
            require(execucao.matches(Regex("[A-Za-z0-9-]+")))
            val pasta = "/sdcard/Download/i137/$execucao"
            val nome = descricao.methodName.replace(Regex("[^A-Za-z0-9_-]"), "_")
            // O AGP desinstala o APK no fim: guardar fora do diretório privado da aplicação.
            for (comando in listOf("mkdir -p $pasta", "screencap -p $pasta/$nome.png")) {
                ParcelFileDescriptor.AutoCloseInputStream(
                    instrumentacao.uiAutomation.executeShellCommand(comando),
                ).use { it.readBytes() }
            }
        }.onFailure { println("Não foi possível guardar a captura: ${it.javaClass.simpleName}") }
    }

    @Test
    fun `ST-MOB-F001-01`() {
        entrar()
        texto("Bobi")
        texto("Mia")
        listOf("nav-animais", "nav-consultas", "nav-terminar-sessao").forEach {
            compose.onNodeWithTag(it).assertExists()
        }
        ausente("Funcionários")
        ausente("Preparação")
    }

    @Test
    fun `ST-MOB-F001-02`() {
        for (email in listOf(
            "cliente.joao@clinica-g08.test",
            "naoexiste.${dados.getString("execucao")}@clinica-g08.test",
        )) {
            val anteriores = pedidos.count { it.first == "/auth/login" }
            credenciais(email, "Incorreta_${dados.getString("execucao")}")
            compose.waitUntil(30_000) { pedidos.count { it.first == "/auth/login" } > anteriores }
            esperar(hasTestTag("m01-erro"))
            compose.onNodeWithTag("m01-erro").assertTextEquals("Email ou palavra-passe incorretos.")
            compose.onNodeWithTag("nav-animais").assertDoesNotExist()
        }
        compose.activityRule.scenario.recreate()
        montarInterface()
        esperar(hasTestTag("m01-email"))
        compose.onNodeWithTag("nav-animais").assertDoesNotExist()
    }

    @Test
    fun `ST-MOB-F001-03`() {
        credenciais("rececao.bloqueada@clinica-g08.test", argumento("senhaRececaoBloqueada"))
        texto("A conta encontra-se bloqueada.")
        compose.onNodeWithTag("nav-animais").assertDoesNotExist()
    }

    @Test
    fun `ST-MOB-F001-04`() {
        clicar("m01-entrar")
        texto("Indique o email.")
        texto("Indique a palavra-passe.")
        compose.onNodeWithTag("m01-email").performTextReplacement("cliente.joao")
        clicar("m01-entrar")
        texto("Indique um email válido.")
        texto("Indique a palavra-passe.")
        assertFalse(pedidos.any { it.first == "/auth/login" })
    }

    @Test
    fun `ST-MOB-F002-01`() {
        entrar()
        listOf(
            "Bobi",
            "Mia",
            "Labrador Retriever",
            "Europeu Comum",
            "28,4 kg",
            "4,2 kg",
        ).forEach(::texto)
        ausente("Kiko")
        semEscrita()
    }

    @Test
    fun `ST-MOB-F002-02`() {
        entrar()
        detalhe()
        compose.onNodeWithTag("m03-nome").assertTextEquals("Bobi")
        listOf(
            "Cão",
            "Labrador Retriever",
            "Macho",
            "10/03/2022",
            "Idade",
            "28,4 kg",
        ).forEach(::texto)
        compose.onNodeWithTag("m03-ver-boletim").assertExists()
        compose.onNodeWithTag("m03-ver-consultas").assertExists()
        semEscrita()
        clicar("m03-voltar")
        esperar(hasTestTag("m02-lista-animais"))
    }

    @Test
    fun `ST-MOB-F002-03`() {
        entrar("vazio")
        texto("Ainda não tem animais associados à sua conta.")
        compose.onNodeWithTag("m02-lista-animais").assertDoesNotExist()
    }

    @Test
    fun `ST-MOB-F002-04`() {
        entrar()
        navegar(Rotas.animalDetalhe(dados.getInt("kiko")))
        texto("Não tem acesso a esta informação.")
        ausente("Kiko")
        assertTrue(
            pedidos.any {
                it.first == "/me/animais/${dados.getInt("kiko")}" &&
                    it.second == 403
            },
        )
        compose.onNodeWithTag("nav-terminar-sessao").assertExists()
    }

    @Test
    fun `ST-MOB-F003-01`() {
        entrar()
        clicar("nav-consultas")
        esperar(hasTestTag("m04-lista-futuras"))
        compose.onNodeWithTag("m04-separador-futuras").assertIsSelected()
        listOf(
            "Bobi • Cão",
            "Pendente",
            dados.getString("dataConsultaTexto"),
            "10:00",
            "Dra. Ana Veterinária",
        ).forEach(::texto)
        ausente("Kiko")
        ausente("Teste ST-MOB")
        ausente("05/11/2026")
    }

    @Test
    fun `ST-MOB-F003-02`() {
        entrar()
        clicar("nav-consultas")
        clicar("m04-separador-historico")
        esperar(hasTestTag("m04-lista-historico"))
        listOf(
            "05/11/2026",
            "20/10/2026",
            "14/10/2026",
            "Bobi",
            "Mia",
            "Pendente",
            "Cancelada",
            "Realizada",
        ).forEach(::texto)
        listOf(
            "Kiko",
            "Otite externa",
            "Notas clínicas",
            "Tratamento",
            "Prescrição",
        ).forEach(::ausente)
        val posicoes = listOf("05/11/2026", "20/10/2026", "14/10/2026").map {
            compose.onNode(hasText(it, substring = true), useUnmergedTree = true)
                .fetchSemanticsNode().boundsInRoot.top
        }
        assertTrue(posicoes.zipWithNext().all { it.first < it.second })
    }

    @Test
    fun `ST-MOB-F003-03`() {
        entrar()
        clicar("nav-consultas")
        esperar(hasTestTag("m04-lista-futuras"))
        semEscrita()
        compose.onNodeWithTag("m04-consulta-${dados.getInt("consultaBobi")}")
            .assertHasNoClickAction().performTouchInput { click() }
        compose.onNodeWithTag("m04-lista-futuras").assertExists()
        clicar("m04-separador-historico")
        esperar(hasTestTag("m04-lista-historico"))
        semEscrita()
        compose.onNodeWithTag("m04-consulta-${dados.getInt("consultaPassadaBobi")}")
            .assertHasNoClickAction().performTouchInput { click() }
        compose.onNodeWithTag("m04-lista-historico").assertExists()
    }

    @Test
    fun `ST-MOB-F003-04`() {
        entrar()
        detalhe()
        clicar("m03-ver-consultas")
        esperar(hasTestTag("m04-lista-futuras"))
        texto("Só as Consultas deste animal.")
        texto("Bobi")
        clicar("m04-separador-historico")
        esperar(hasTestTag("m04-lista-historico"))
        texto("05/11/2026")
        texto("14/10/2026")
        ausente("Mia")
        compose.onNodeWithTag("nav-animais").assertIsSelected()
        compose.onNodeWithTag("nav-consultas").assertIsNotSelected()
        clicar("m04-voltar")
        compose.onNodeWithTag("m03-nome").assertTextEquals("Bobi")
    }

    @Test
    fun `ST-MOB-F003-05`() {
        entrar()
        detalhe("mia")
        clicar("m03-ver-consultas")
        texto("Este animal não tem Consultas marcadas.")
        sair()
        entrar("vazio")
        clicar("nav-consultas")
        texto("Não tem Consultas marcadas.")
        clicar("m04-separador-historico")
        texto("Ainda não há Consultas no histórico.")
    }

    @Test
    fun `ST-MOB-F003-06`() {
        entrar()
        navegar(Rotas.consultasDoAnimal(dados.getInt("kiko")))
        texto("Não tem acesso a esta informação.")
        clicar("m04-separador-historico")
        texto("Não tem acesso a esta informação.")
        ausente("Kiko")
        assertEquals(2, pedidos.count { it.first == "/me/consultas" && it.second == 403 })
        compose.onNodeWithTag("nav-terminar-sessao").assertExists()
    }

    @Test
    fun `ST-MOB-F006-01`() {
        entrar()
        boletim()
        texto("Próximas administrações previstas")
        listOf("Vacina", "Desparasitação", "Vacina polivalente (fictícia)").forEach(::texto)
        compose.onNode(
            hasText("Vacina antirrábica (fictícia)"),
            useUnmergedTree = true,
        ).performScrollTo()
        texto("Administrada em")
        semEscrita()
        clicar("m06-voltar")
        compose.onNodeWithTag("m03-nome").assertTextEquals("Bobi")
    }

    @Test
    fun `ST-MOB-F006-02`() {
        entrar()
        boletim()
        val proximas = hasAnyAncestor(hasTestTag("m06-proximas"))
        val hoje = LocalDate.parse(dados.getString("hoje"))
        val carga = LocalDate.parse(dados.getString("carga"))
        val formato = DateTimeFormatter.ofPattern("dd/MM/yyyy")
        listOf(hoje.plusDays(30), carga.plusDays(60), carga.plusDays(335)).forEach { data ->
            compose.onNode(
                hasText("Prevista para ${data.format(formato)}") and proximas,
                useUnmergedTree = true,
            ).assertExists()
        }
        compose.onNode(
            hasText(dados.getString("produtoVacina")) and proximas,
            useUnmergedTree = true,
        ).assertExists()
        compose.onNode(
            hasText("Vacina antirrábica (fictícia)") and proximas,
            useUnmergedTree = true,
        ).assertDoesNotExist()
        compose.onNode(
            hasText(dados.getString("produtoVacina")) and
                hasAnyAncestor(hasTestTag("m06-historico")),
            useUnmergedTree = true,
        ).assertExists()
    }

    @Test
    fun `ST-MOB-F006-03`() {
        entrar("maria")
        boletim("kiko")
        texto("Não há administrações previstas.")
        texto("Desparasitante externo (fictício)")
        sair()
        entrar()
        boletim("mia")
        texto("Este animal ainda não tem vacinações nem desparasitações registadas.")
    }

    @Test
    fun `ST-MOB-F006-04`() {
        entrar()
        navegar(Rotas.boletim(dados.getInt("kiko")))
        texto("Não tem acesso a esta informação.")
        ausente("Desparasitante externo")
        assertTrue(
            pedidos.any {
                it.first == "/me/animais/${dados.getInt("kiko")}/vacinacoes" &&
                    it.second == 403
            },
        )
        compose.onNodeWithTag("nav-terminar-sessao").assertExists()
    }
}
