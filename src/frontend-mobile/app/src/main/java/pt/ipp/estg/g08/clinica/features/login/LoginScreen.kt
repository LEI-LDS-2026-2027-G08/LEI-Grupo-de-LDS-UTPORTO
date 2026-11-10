package pt.ipp.estg.g08.clinica.features.login

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import pt.ipp.estg.g08.clinica.BuildConfig
import pt.ipp.estg.g08.clinica.R
import pt.ipp.estg.g08.clinica.sessao.MotivoFimSessao

/**
 * M01 — Início de sessão. Ecrã provisório: o formulário, as validações e o pedido à API
 * (RF001 a RF005) são implementados na I121. Já mostra o aviso quando a sessão termina.
 */
@Composable
fun LoginScreen(
    aviso: MotivoFimSessao?,
    onAbrirTecnico: () -> Unit,
    modifier: Modifier = Modifier,
) {
    Column(
        modifier = modifier.fillMaxSize().padding(16.dp),
        verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally,
    ) {
        Text(
            text = stringResource(R.string.login_por_implementar),
            style = MaterialTheme.typography.titleLarge,
        )
        when (aviso) {
            MotivoFimSessao.NaoAutenticado ->
                Text(
                    text = stringResource(R.string.aviso_sessao_terminada),
                    modifier = Modifier.testTag("m01-aviso"),
                )
            MotivoFimSessao.Utilizador ->
                Text(
                    text = stringResource(R.string.aviso_sessao_terminada_utilizador),
                    modifier = Modifier.testTag("m01-aviso"),
                )
            null -> Unit
        }
        if (BuildConfig.DEBUG) {
            OutlinedButton(
                onClick = onAbrirTecnico,
                modifier = Modifier.testTag("m01-abrir-tecnico"),
            ) {
                Text(text = stringResource(R.string.login_abrir_tecnico))
            }
        }
    }
}
