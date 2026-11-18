package pt.ipp.estg.g08.clinica.features.boletim

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.Card
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.semantics.heading
import androidx.compose.ui.semantics.semantics
import androidx.compose.ui.unit.dp
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import pt.ipp.estg.g08.clinica.R
import pt.ipp.estg.g08.clinica.core.formatacao.Formatadores
import pt.ipp.estg.g08.clinica.features.boletim.dto.MeuBoletimDto
import pt.ipp.estg.g08.clinica.features.boletim.dto.TipoAdministracao
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento
import pt.ipp.estg.g08.clinica.shared.components.BotaoVoltar
import pt.ipp.estg.g08.clinica.shared.components.EstadoVista

/**
 * M06 — Boletim de vacinação e desparasitação (UC17; RF047). Liga o [BoletimViewModel] ao
 * [BoletimConteudo].
 *
 * Só consulta: não há ações para registar ou alterar administrações.
 */
@Composable
fun BoletimScreen(
    viewModel: BoletimViewModel,
    onVoltar: () -> Unit,
    modifier: Modifier = Modifier,
) {
    val estado by viewModel.estado.collectAsStateWithLifecycle()

    BoletimConteudo(
        estado = estado,
        onTentarNovamente = viewModel::carregar,
        onVoltar = onVoltar,
        modifier = modifier,
    )
}

/**
 * Boletim M06 sem estado próprio (testável com Compose UI Test): próximas administrações
 * previstas e histórico, pela ordem devolvida pela API. As datas das próximas administrações
 * são as calculadas pelo servidor (RF045).
 */
@Composable
fun BoletimConteudo(
    estado: EstadoCarregamento<MeuBoletimDto>,
    onTentarNovamente: () -> Unit,
    onVoltar: () -> Unit,
    modifier: Modifier = Modifier,
) {
    Column(modifier = modifier.fillMaxSize().padding(16.dp)) {
        BotaoVoltar(onVoltar = onVoltar, modifier = Modifier.testTag("m06-voltar"))
        Text(
            text = stringResource(R.string.m06_titulo),
            style = MaterialTheme.typography.headlineSmall,
            modifier = Modifier.padding(bottom = 12.dp),
        )
        EstadoVista(
            estado = estado,
            onTentarNovamente = onTentarNovamente,
            modifier = Modifier.weight(1f),
            mensagemVazio = stringResource(R.string.m06_vazio),
            etiquetaVazio = "m06-vazio",
        ) { boletim ->
            Column(
                modifier = Modifier.fillMaxSize().verticalScroll(rememberScrollState()),
                verticalArrangement = Arrangement.spacedBy(24.dp),
            ) {
                Seccao(
                    titulo = stringResource(R.string.m06_proximas),
                    semRegistos = stringResource(R.string.m06_sem_proximas),
                    registos =
                    boletim.proximas.map {
                        Registo(
                            tipo = it.tipo,
                            produto = it.produto,
                            data =
                            stringResource(
                                R.string.m06_prevista_para,
                                Formatadores.data(it.dataProximaPrevista),
                            ),
                        )
                    },
                    modifier = Modifier.testTag("m06-proximas"),
                )
                Seccao(
                    titulo = stringResource(R.string.m06_historico),
                    semRegistos = stringResource(R.string.m06_sem_historico),
                    registos =
                    boletim.historico.map {
                        Registo(
                            tipo = it.tipo,
                            produto = it.produto,
                            data =
                            stringResource(
                                R.string.m06_administrada_em,
                                Formatadores.data(it.dataAdministracao),
                            ),
                        )
                    },
                    modifier = Modifier.testTag("m06-historico"),
                )
            }
        }
    }
}

/** Uma administração pronta a apresentar. */
private data class Registo(val tipo: TipoAdministracao, val produto: String, val data: String)

/** Secção do boletim com o seu título; sem registos, mostra [semRegistos]. */
@Composable
private fun Seccao(
    titulo: String,
    semRegistos: String,
    registos: List<Registo>,
    modifier: Modifier = Modifier,
) {
    Column(modifier = modifier.fillMaxWidth(), verticalArrangement = Arrangement.spacedBy(8.dp)) {
        Text(
            text = titulo,
            style = MaterialTheme.typography.titleMedium,
            modifier = Modifier.semantics { heading() },
        )
        if (registos.isEmpty()) {
            Text(text = semRegistos, style = MaterialTheme.typography.bodyMedium)
        } else {
            registos.forEach { registo -> CartaoRegisto(registo = registo) }
        }
    }
}

/** Tipo (Vacina ou Desparasitação), produto e data de uma administração. */
@Composable
private fun CartaoRegisto(registo: Registo, modifier: Modifier = Modifier) {
    Card(modifier = modifier.fillMaxWidth()) {
        Column(
            modifier = Modifier.padding(16.dp),
            verticalArrangement = Arrangement.spacedBy(4.dp),
        ) {
            Text(
                text = registo.tipo.rotulo,
                style = MaterialTheme.typography.labelMedium,
                color = MaterialTheme.colorScheme.primary,
            )
            Text(text = registo.produto, style = MaterialTheme.typography.titleSmall)
            Text(text = registo.data, style = MaterialTheme.typography.bodyMedium)
        }
    }
}
