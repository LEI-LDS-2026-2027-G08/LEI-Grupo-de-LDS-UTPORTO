package pt.ipp.estg.g08.clinica.features.login

import androidx.annotation.StringRes
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.imePadding
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.statusBarsPadding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.text.KeyboardActions
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.Button
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalSoftwareKeyboardController
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.semantics.LiveRegionMode
import androidx.compose.ui.semantics.liveRegion
import androidx.compose.ui.semantics.semantics
import androidx.compose.ui.text.input.ImeAction
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import pt.ipp.estg.g08.clinica.R
import pt.ipp.estg.g08.clinica.sessao.ErrosLogin
import pt.ipp.estg.g08.clinica.sessao.MotivoFimSessao
import pt.ipp.estg.g08.clinica.sessao.SessaoViewModel

/**
 * M01 — Início de sessão (UC01; RF001, RF002, RF004 e RF005).
 *
 * Liga o [SessaoViewModel] ao formulário [LoginConteudo]. A palavra-passe só existe em memória
 * enquanto o ecrã está visível: não é guardada no estado restaurável nem no dispositivo.
 */
@Composable
fun LoginScreen(
    sessao: SessaoViewModel,
    modifier: Modifier = Modifier,
    onAbrirTecnico: (() -> Unit)? = null,
) {
    val errosCampos by sessao.errosCampos.collectAsStateWithLifecycle()
    val erro by sessao.erro.collectAsStateWithLifecycle()
    val aviso by sessao.aviso.collectAsStateWithLifecycle()
    val aProcessar by sessao.aProcessar.collectAsStateWithLifecycle()

    var email by rememberSaveable { mutableStateOf("") }
    var password by remember { mutableStateOf("") }

    LoginConteudo(
        email = email,
        password = password,
        errosCampos = errosCampos,
        erro = erro,
        aviso = aviso,
        aProcessar = aProcessar,
        onEmailAlterado = {
            email = it
            sessao.limparErros()
        },
        onPasswordAlterada = {
            password = it
            sessao.limparErros()
        },
        onEntrar = { sessao.login(email, password) },
        modifier = modifier,
        onAbrirTecnico = onAbrirTecnico,
    )
}

/** Formulário M01 sem estado próprio (testável com Compose UI Test). */
@Composable
fun LoginConteudo(
    email: String,
    password: String,
    errosCampos: ErrosLogin,
    erro: String?,
    aviso: MotivoFimSessao?,
    aProcessar: Boolean,
    onEmailAlterado: (String) -> Unit,
    onPasswordAlterada: (String) -> Unit,
    onEntrar: () -> Unit,
    modifier: Modifier = Modifier,
    onAbrirTecnico: (() -> Unit)? = null,
) {
    val teclado = LocalSoftwareKeyboardController.current
    val erroEmail = errosCampos.email
    val erroPassword = errosCampos.password
    val submeter = {
        teclado?.hide()
        onEntrar()
    }

    Column(
        modifier =
        modifier
            .fillMaxSize()
            .statusBarsPadding()
            .imePadding()
            .verticalScroll(rememberScrollState())
            .padding(24.dp),
        verticalArrangement = Arrangement.spacedBy(16.dp, Alignment.CenterVertically),
        horizontalAlignment = Alignment.CenterHorizontally,
    ) {
        Text(
            text = stringResource(R.string.app_name),
            style = MaterialTheme.typography.titleMedium,
            textAlign = TextAlign.Center,
        )
        Text(
            text = stringResource(R.string.login_titulo),
            style = MaterialTheme.typography.headlineSmall,
        )

        if (aviso != null) {
            Text(
                text = stringResource(textoDoAviso(aviso)),
                color = MaterialTheme.colorScheme.primary,
                textAlign = TextAlign.Center,
                modifier =
                Modifier
                    .testTag("m01-aviso")
                    .semantics { liveRegion = LiveRegionMode.Polite },
            )
        }

        OutlinedTextField(
            value = email,
            onValueChange = onEmailAlterado,
            label = { Text(text = stringResource(R.string.login_email)) },
            singleLine = true,
            enabled = !aProcessar,
            isError = erroEmail != null,
            supportingText =
            if (erroEmail != null) {
                { Text(text = erroEmail) }
            } else {
                null
            },
            keyboardOptions =
            KeyboardOptions(keyboardType = KeyboardType.Email, imeAction = ImeAction.Next),
            modifier = Modifier.fillMaxWidth().testTag("m01-email"),
        )

        OutlinedTextField(
            value = password,
            onValueChange = onPasswordAlterada,
            label = { Text(text = stringResource(R.string.login_password)) },
            singleLine = true,
            enabled = !aProcessar,
            isError = erroPassword != null,
            supportingText =
            if (erroPassword != null) {
                { Text(text = erroPassword) }
            } else {
                null
            },
            visualTransformation = PasswordVisualTransformation(),
            keyboardOptions =
            KeyboardOptions(keyboardType = KeyboardType.Password, imeAction = ImeAction.Done),
            keyboardActions = KeyboardActions(onDone = { submeter() }),
            modifier = Modifier.fillMaxWidth().testTag("m01-password"),
        )

        Button(
            onClick = submeter,
            enabled = !aProcessar,
            modifier = Modifier.fillMaxWidth().testTag("m01-entrar"),
        ) {
            if (aProcessar) {
                CircularProgressIndicator(
                    strokeWidth = 2.dp,
                    modifier = Modifier.size(20.dp).testTag("estado-a-carregar"),
                )
            } else {
                Text(text = stringResource(R.string.login_entrar))
            }
        }

        if (erro != null) {
            Text(
                text = erro,
                color = MaterialTheme.colorScheme.error,
                textAlign = TextAlign.Center,
                modifier =
                Modifier
                    .testTag("m01-erro")
                    .semantics { liveRegion = LiveRegionMode.Assertive },
            )
        }

        if (onAbrirTecnico != null) {
            OutlinedButton(
                onClick = onAbrirTecnico,
                modifier = Modifier.testTag("m01-abrir-tecnico"),
            ) {
                Text(text = stringResource(R.string.login_abrir_tecnico))
            }
        }
    }
}

@StringRes
private fun textoDoAviso(aviso: MotivoFimSessao): Int = when (aviso) {
    MotivoFimSessao.NaoAutenticado -> R.string.aviso_sessao_terminada
    MotivoFimSessao.Utilizador -> R.string.aviso_sessao_terminada_utilizador
}
