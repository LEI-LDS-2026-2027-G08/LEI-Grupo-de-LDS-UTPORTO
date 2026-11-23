package pt.ipp.estg.g08.clinica.features.animais

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
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
import pt.ipp.estg.g08.clinica.R
import pt.ipp.estg.g08.clinica.core.formatacao.Formatadores
import pt.ipp.estg.g08.clinica.features.animais.dto.MeuAnimalDto
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento
import pt.ipp.estg.g08.clinica.shared.components.BotaoVoltar
import pt.ipp.estg.g08.clinica.shared.components.EstadoVista

/**
 * M03 — Detalhe do animal (UC08; RF018). Liga o [AnimalDetalheViewModel] ao
 * [AnimalDetalheConteudo].
 *
 * Só consulta: as únicas ações são ver o boletim (M06) e as Consultas do animal (M04).
 */
@Composable
fun AnimalDetalheScreen(
    viewModel: AnimalDetalheViewModel,
    onVerBoletim: () -> Unit,
    onVerConsultas: () -> Unit,
    onVoltar: () -> Unit,
    modifier: Modifier = Modifier,
) {
    val estado by viewModel.estado.collectAsStateWithLifecycle()

    AnimalDetalheConteudo(
        estado = estado,
        onTentarNovamente = viewModel::carregar,
        onVerBoletim = onVerBoletim,
        onVerConsultas = onVerConsultas,
        onVoltar = onVoltar,
        modifier = modifier,
    )
}

/**
 * Detalhe M03 sem estado próprio (testável com Compose UI Test). Com erro (por exemplo, `403`
 * num animal de outro Cliente) só aparece a mensagem: nenhum dado do animal é mostrado.
 */
@Composable
fun AnimalDetalheConteudo(
    estado: EstadoCarregamento<MeuAnimalDto>,
    onTentarNovamente: () -> Unit,
    onVerBoletim: () -> Unit,
    onVerConsultas: () -> Unit,
    onVoltar: () -> Unit,
    modifier: Modifier = Modifier,
) {
    Column(modifier = modifier.fillMaxSize().padding(16.dp)) {
        BotaoVoltar(onVoltar = onVoltar, modifier = Modifier.testTag("m03-voltar"))
        EstadoVista(
            estado = estado,
            onTentarNovamente = onTentarNovamente,
            modifier = Modifier.weight(1f),
        ) { animal ->
            DadosAnimal(
                animal = animal,
                onVerBoletim = onVerBoletim,
                onVerConsultas = onVerConsultas,
                modifier = Modifier.fillMaxSize(),
            )
        }
    }
}

/** Dados básicos do animal (UC08, passo 5) e as ligações para o boletim e as Consultas. */
@Composable
private fun DadosAnimal(
    animal: MeuAnimalDto,
    onVerBoletim: () -> Unit,
    onVerConsultas: () -> Unit,
    modifier: Modifier = Modifier,
) {
    val semInformacao = stringResource(R.string.sem_informacao)
    Column(
        modifier = modifier.verticalScroll(rememberScrollState()),
        verticalArrangement = Arrangement.spacedBy(12.dp),
    ) {
        Text(
            text = animal.nome,
            style = MaterialTheme.typography.headlineSmall,
            modifier = Modifier.testTag("m03-nome"),
        )
        Campo(rotulo = stringResource(R.string.m03_especie), valor = animal.especie)
        Campo(rotulo = stringResource(R.string.m03_raca), valor = animal.raca ?: semInformacao)
        Campo(rotulo = stringResource(R.string.m03_sexo), valor = animal.sexo.rotulo)
        Campo(
            rotulo = stringResource(R.string.m03_data_nascimento),
            valor = animal.dataNascimento?.let(Formatadores::data) ?: semInformacao,
        )
        Campo(
            rotulo = stringResource(R.string.m03_idade),
            valor = animal.idadeAnos?.let { textoIdade(it) } ?: semInformacao,
        )
        Campo(
            rotulo = stringResource(R.string.m03_peso),
            valor = animal.peso?.let(Formatadores::peso) ?: semInformacao,
        )
        Button(
            onClick = onVerBoletim,
            modifier = Modifier.fillMaxWidth().testTag("m03-ver-boletim"),
        ) {
            Text(text = stringResource(R.string.m03_ver_boletim))
        }
        OutlinedButton(
            onClick = onVerConsultas,
            modifier = Modifier.fillMaxWidth().testTag("m03-ver-consultas"),
        ) {
            Text(text = stringResource(R.string.m03_ver_consultas))
        }
    }
}

/** Um dado do animal: rótulo e valor. */
@Composable
private fun Campo(rotulo: String, valor: String, modifier: Modifier = Modifier) {
    Column(modifier = modifier) {
        Text(
            text = rotulo,
            style = MaterialTheme.typography.labelMedium,
            color = MaterialTheme.colorScheme.onSurfaceVariant,
        )
        Text(text = valor, style = MaterialTheme.typography.bodyLarge)
    }
}
