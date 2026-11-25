package pt.ipp.estg.g08.clinica.features.login

import androidx.compose.ui.test.assertIsDisplayed
import androidx.compose.ui.test.assertIsEnabled
import androidx.compose.ui.test.assertIsNotEnabled
import androidx.compose.ui.test.assertTextEquals
import androidx.compose.ui.test.junit4.createComposeRule
import androidx.compose.ui.test.onNodeWithTag
import androidx.compose.ui.test.onNodeWithText
import androidx.compose.ui.test.performClick
import androidx.compose.ui.test.performTextInput
import androidx.test.ext.junit.runners.AndroidJUnit4
import org.junit.Assert.assertEquals
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.annotation.Config
import pt.ipp.estg.g08.clinica.sessao.ErrosLogin
import pt.ipp.estg.g08.clinica.sessao.MSG_CREDENCIAIS_INCORRETAS
import pt.ipp.estg.g08.clinica.sessao.MSG_EMAIL_EM_FALTA
import pt.ipp.estg.g08.clinica.sessao.MSG_PASSWORD_EM_FALTA
import pt.ipp.estg.g08.clinica.sessao.MotivoFimSessao

/** M01 com Compose UI Test e Robolectric (QG-07), sem backend. */
@RunWith(AndroidJUnit4::class)
@Config(sdk = [35])
class LoginConteudoTest {
    @get:Rule
    val compose = createComposeRule()

    private var entradas = 0
    private var emailEscrito = ""

    private fun mostrar(
        errosCampos: ErrosLogin = ErrosLogin(),
        erro: String? = null,
        aviso: MotivoFimSessao? = null,
        aProcessar: Boolean = false,
        onAbrirTecnico: (() -> Unit)? = null,
    ) {
        compose.setContent {
            LoginConteudo(
                email = "",
                password = "",
                errosCampos = errosCampos,
                erro = erro,
                aviso = aviso,
                aProcessar = aProcessar,
                onEmailAlterado = { emailEscrito = it },
                onPasswordAlterada = {},
                onEntrar = { entradas++ },
                onAbrirTecnico = onAbrirTecnico,
            )
        }
    }

    @Test
    fun `mostra os campos e o botao Entrar com as etiquetas do modelo`() {
        mostrar()

        compose.onNodeWithTag("m01-email").assertIsDisplayed()
        compose.onNodeWithTag("m01-password").assertIsDisplayed()
        compose.onNodeWithTag("m01-entrar").assertIsEnabled().performClick()
        assertEquals(1, entradas)
        compose.onNodeWithTag("m01-erro").assertDoesNotExist()
        compose.onNodeWithTag("m01-aviso").assertDoesNotExist()
        compose.onNodeWithTag("m01-abrir-tecnico").assertDoesNotExist()
    }

    @Test
    fun `o texto escrito no email chega ao ecra`() {
        mostrar()

        compose.onNodeWithTag("m01-email").performTextInput("cliente.joao@clinica-g08.test")

        assertEquals("cliente.joao@clinica-g08.test", emailEscrito)
    }

    @Test
    fun `mostra os erros de preenchimento por campo`() {
        mostrar(errosCampos = ErrosLogin(MSG_EMAIL_EM_FALTA, MSG_PASSWORD_EM_FALTA))

        compose.onNodeWithText(MSG_EMAIL_EM_FALTA).assertIsDisplayed()
        compose.onNodeWithText(MSG_PASSWORD_EM_FALTA).assertIsDisplayed()
    }

    @Test
    fun `mostra o erro do inicio de sessao`() {
        mostrar(erro = MSG_CREDENCIAIS_INCORRETAS)

        compose.onNodeWithTag("m01-erro").assertTextEquals(MSG_CREDENCIAIS_INCORRETAS)
    }

    @Test
    fun `mostra o aviso de sessao terminada`() {
        mostrar(aviso = MotivoFimSessao.NaoAutenticado)

        compose
            .onNodeWithTag("m01-aviso")
            .assertTextEquals("A sua sessão terminou. Inicie sessão novamente.")
    }

    @Test
    fun `durante o pedido o botao fica inativo`() {
        mostrar(aProcessar = true)

        compose.onNodeWithTag("m01-entrar").assertIsNotEnabled()
        compose.onNodeWithTag("estado-a-carregar", useUnmergedTree = true).assertExists()
    }

    @Test
    fun `o acesso ao ecra tecnico so aparece quando e fornecido`() {
        var aberto = false
        mostrar(onAbrirTecnico = { aberto = true })

        compose.onNodeWithTag("m01-abrir-tecnico").performClick()

        assertEquals(true, aberto)
    }
}
