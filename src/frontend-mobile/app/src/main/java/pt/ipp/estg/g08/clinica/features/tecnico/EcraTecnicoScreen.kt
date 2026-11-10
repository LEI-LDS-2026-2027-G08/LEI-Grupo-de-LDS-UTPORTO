package pt.ipp.estg.g08.clinica.features.tecnico

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.statusBarsPadding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.Button
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import pt.ipp.estg.g08.clinica.AppContainer
import pt.ipp.estg.g08.clinica.R
import pt.ipp.estg.g08.clinica.shared.components.EstadoVista

/** Ecrã técnico (só em desenvolvimento): diagnóstico da configuração da API e da sessão. */
@Composable
fun EcraTecnicoScreen(
    container: AppContainer,
    onAbrirPrincipal: () -> Unit,
    modifier: Modifier = Modifier,
) {
    val viewModel: EcraTecnicoViewModel =
        viewModel(
            factory =
            viewModelFactory {
                initializer {
                    EcraTecnicoViewModel(
                        config = container.config,
                        tokenStorage = container.tokenStorage,
                        apiClient = container.apiClient,
                    )
                }
            },
        )
    val estado by viewModel.estado.collectAsStateWithLifecycle()

    Column(
        modifier =
        modifier
            .fillMaxSize()
            .statusBarsPadding()
            .padding(16.dp)
            .verticalScroll(rememberScrollState()),
        verticalArrangement = Arrangement.spacedBy(12.dp),
    ) {
        Text(
            text = stringResource(R.string.tecnico_titulo),
            style = MaterialTheme.typography.headlineSmall,
        )
        Text(text = stringResource(R.string.tecnico_nota))
        Text(
            text = stringResource(R.string.tecnico_url_api, estado.urlApi),
            modifier = Modifier.testTag("tecnico-url-api"),
        )
        Text(
            text =
            stringResource(
                R.string.tecnico_sessao_guardada,
                stringResource(if (estado.temSessao) R.string.sim else R.string.nao),
            ),
            modifier = Modifier.testTag("tecnico-sessao"),
        )
        if (estado.temSessao) {
            Text(
                text =
                stringResource(
                    R.string.tecnico_sessao_detalhe,
                    estado.perfil.toString(),
                    estado.expiraEm.toString(),
                ),
            )
        }
        Button(
            onClick = viewModel::guardarSessaoDeTeste,
            modifier = Modifier.testTag("tecnico-guardar-teste"),
        ) {
            Text(text = stringResource(R.string.tecnico_guardar_teste))
        }
        Button(
            onClick = viewModel::testarLigacao,
            modifier = Modifier.testTag("tecnico-testar-ligacao"),
        ) {
            Text(text = stringResource(R.string.tecnico_testar_ligacao))
        }
        EstadoVista(estado = estado.teste, onTentarNovamente = viewModel::testarLigacao) {
            Text(
                text = stringResource(R.string.tecnico_ligacao_ok),
                modifier = Modifier.testTag("tecnico-ligacao-ok"),
            )
        }
        OutlinedButton(
            onClick = viewModel::limparSessao,
            modifier = Modifier.testTag("tecnico-limpar"),
        ) {
            Text(text = stringResource(R.string.tecnico_limpar))
        }
        OutlinedButton(
            onClick = onAbrirPrincipal,
            modifier = Modifier.testTag("tecnico-abrir-principal"),
        ) {
            Text(text = stringResource(R.string.tecnico_abrir_principal))
        }
    }
}
