package pt.ipp.estg.g08.clinica.features.consultas

import androidx.annotation.StringRes
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.Card
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.PrimaryTabRow
import androidx.compose.material3.Tab
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.key
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import java.time.ZoneId
import pt.ipp.estg.g08.clinica.R
import pt.ipp.estg.g08.clinica.core.formatacao.Formatadores
import pt.ipp.estg.g08.clinica.features.consultas.dto.MinhaConsultaDto
import pt.ipp.estg.g08.clinica.features.consultas.dto.TipoConsultaFiltro
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento
import pt.ipp.estg.g08.clinica.shared.components.BotaoVoltar
import pt.ipp.estg.g08.clinica.shared.components.EstadoVista

/**
 * M04 — Consultas futuras e histórico administrativo (UC11; RF028, RF029). Liga o
 * [ConsultasViewModel] ao [ConsultasConteudo].
 *
 * Aberto pelo separador Consultas, mostra todos os animais do Cliente; aberto a partir de M03,
 * mostra só esse animal e tem o botão [onVoltar]. Só consulta: não há ações para marcar,
 * alterar, confirmar ou cancelar Consultas.
 */
@Composable
fun ConsultasScreen(
    viewModel: ConsultasViewModel,
    onVoltar: (() -> Unit)?,
    modifier: Modifier = Modifier,
) {
    val futuras by viewModel.estadoFuturas.collectAsStateWithLifecycle()
    val historico by viewModel.estadoHistorico.collectAsStateWithLifecycle()

    ConsultasConteudo(
        estadoFuturas = futuras,
        estadoHistorico = historico,
        doAnimal = viewModel.animalId != null,
        onTentarNovamente = viewModel::carregar,
        onVoltar = onVoltar,
        modifier = modifier,
    )
}

/**
 * M04 sem estado de dados próprio (testável com Compose UI Test): separadores "Próximas" e
 * "Histórico", cada um com a sua lista pela ordem devolvida pela API (futuras por data
 * crescente, histórico por data decrescente). As horas são apresentadas no [fuso] do
 * dispositivo.
 *
 * [doAnimal] indica que as listas são só de um animal (aberto a partir de M03), o que muda o
 * subtítulo e as mensagens de lista vazia.
 */
@Composable
fun ConsultasConteudo(
    estadoFuturas: EstadoCarregamento<List<MinhaConsultaDto>>,
    estadoHistorico: EstadoCarregamento<List<MinhaConsultaDto>>,
    doAnimal: Boolean,
    onTentarNovamente: (TipoConsultaFiltro) -> Unit,
    onVoltar: (() -> Unit)?,
    modifier: Modifier = Modifier,
    fuso: ZoneId = ZoneId.systemDefault(),
) {
    var separador by rememberSaveable { mutableStateOf(TipoConsultaFiltro.Futuras) }
    val futuras = separador == TipoConsultaFiltro.Futuras

    Column(modifier = modifier.fillMaxSize().padding(16.dp)) {
        if (onVoltar != null) {
            BotaoVoltar(onVoltar = onVoltar, modifier = Modifier.testTag("m04-voltar"))
        }
        Text(
            text = stringResource(R.string.m04_titulo),
            style = MaterialTheme.typography.headlineSmall,
        )
        if (doAnimal) {
            Text(
                text = stringResource(R.string.m04_so_deste_animal),
                style = MaterialTheme.typography.bodyMedium,
                modifier = Modifier.testTag("m04-so-deste-animal"),
            )
        }
        PrimaryTabRow(
            selectedTabIndex = separador.ordinal,
            modifier = Modifier.padding(vertical = 12.dp),
        ) {
            TipoConsultaFiltro.entries.forEach { tipo ->
                Tab(
                    selected = separador == tipo,
                    onClick = { separador = tipo },
                    text = { Text(text = stringResource(tipo.rotuloSeparador())) },
                    modifier = Modifier.testTag("m04-separador-${tipo.valorApi}"),
                )
            }
        }
        // Cada separador abre a sua lista no topo, sem herdar o deslocamento do outro.
        key(separador) {
            ListaDoSeparador(
                estado = if (futuras) estadoFuturas else estadoHistorico,
                tipo = separador,
                doAnimal = doAnimal,
                onTentarNovamente = { onTentarNovamente(separador) },
                fuso = fuso,
                modifier = Modifier.weight(1f),
            )
        }
    }
}

/** Lista de um separador com os estados comuns (a carregar, vazio, erro). */
@Composable
private fun ListaDoSeparador(
    estado: EstadoCarregamento<List<MinhaConsultaDto>>,
    tipo: TipoConsultaFiltro,
    doAnimal: Boolean,
    onTentarNovamente: () -> Unit,
    fuso: ZoneId,
    modifier: Modifier = Modifier,
) {
    EstadoVista(
        estado = estado,
        onTentarNovamente = onTentarNovamente,
        modifier = modifier,
        mensagemVazio = stringResource(mensagemVazio(tipo, doAnimal)),
        etiquetaVazio = "m04-vazio-${tipo.valorApi}",
    ) { consultas ->
        LazyColumn(
            modifier = Modifier.fillMaxSize().testTag("m04-lista-${tipo.valorApi}"),
            verticalArrangement = Arrangement.spacedBy(8.dp),
        ) {
            items(items = consultas, key = { it.id }) { consulta ->
                CartaoConsulta(consulta = consulta, fuso = fuso)
            }
        }
    }
}

@StringRes
private fun TipoConsultaFiltro.rotuloSeparador(): Int = when (this) {
    TipoConsultaFiltro.Futuras -> R.string.m04_separador_futuras
    TipoConsultaFiltro.Historico -> R.string.m04_separador_historico
}

@StringRes
private fun mensagemVazio(tipo: TipoConsultaFiltro, doAnimal: Boolean): Int = when (tipo) {
    TipoConsultaFiltro.Futuras ->
        if (doAnimal) R.string.m04_sem_futuras_animal else R.string.m04_sem_futuras
    TipoConsultaFiltro.Historico ->
        if (doAnimal) R.string.m04_sem_historico_animal else R.string.m04_sem_historico
}

/**
 * Uma marcação: estado, data e hora, animal e Veterinário. Só informação administrativa — o
 * DTO não tem motivo, notas nem nada do registo clínico (RF029).
 */
@Composable
private fun CartaoConsulta(
    consulta: MinhaConsultaDto,
    fuso: ZoneId,
    modifier: Modifier = Modifier,
) {
    Card(modifier = modifier.fillMaxWidth().testTag("m04-consulta-${consulta.id}")) {
        Column(
            modifier = Modifier.padding(16.dp),
            verticalArrangement = Arrangement.spacedBy(4.dp),
        ) {
            Text(
                text = consulta.estado.rotulo,
                style = MaterialTheme.typography.labelMedium,
                color = MaterialTheme.colorScheme.primary,
            )
            Text(
                text = Formatadores.dataHora(consulta.dataHora, fuso),
                style = MaterialTheme.typography.titleMedium,
            )
            Text(
                text =
                listOfNotNull(consulta.animal.nome, consulta.animal.especie).joinToString(" • "),
            )
            Text(text = consulta.veterinario, style = MaterialTheme.typography.bodyMedium)
        }
    }
}
