package pt.ipp.estg.g08.clinica.features.animais

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.semantics.Role
import androidx.compose.ui.unit.dp
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import pt.ipp.estg.g08.clinica.R
import pt.ipp.estg.g08.clinica.core.formatacao.Formatadores
import pt.ipp.estg.g08.clinica.features.animais.dto.MeuAnimalListItemDto
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento
import pt.ipp.estg.g08.clinica.shared.components.EstadoVista

/**
 * M02 — Os meus animais (UC08; RF018). Liga o [AnimaisViewModel] à lista [AnimaisConteudo].
 *
 * Só consulta: não há ações para criar, alterar ou eliminar animais.
 */
@Composable
fun AnimaisScreen(
    viewModel: AnimaisViewModel,
    onAbrirAnimal: (animalId: Int) -> Unit,
    modifier: Modifier = Modifier,
) {
    val estado by viewModel.estado.collectAsStateWithLifecycle()

    AnimaisConteudo(
        estado = estado,
        onTentarNovamente = viewModel::carregar,
        onAbrirAnimal = onAbrirAnimal,
        modifier = modifier,
    )
}

/** Lista M02 sem estado próprio (testável com Compose UI Test). */
@Composable
fun AnimaisConteudo(
    estado: EstadoCarregamento<List<MeuAnimalListItemDto>>,
    onTentarNovamente: () -> Unit,
    onAbrirAnimal: (animalId: Int) -> Unit,
    modifier: Modifier = Modifier,
) {
    Column(
        modifier = modifier.fillMaxSize().padding(16.dp),
        verticalArrangement = Arrangement.spacedBy(12.dp),
    ) {
        Text(
            text = stringResource(R.string.m02_titulo),
            style = MaterialTheme.typography.headlineSmall,
        )
        EstadoVista(
            estado = estado,
            onTentarNovamente = onTentarNovamente,
            modifier = Modifier.weight(1f),
            mensagemVazio = stringResource(R.string.m02_vazio),
            etiquetaVazio = "m02-vazio",
        ) { animais ->
            LazyColumn(
                modifier = Modifier.fillMaxSize().testTag("m02-lista-animais"),
                verticalArrangement = Arrangement.spacedBy(8.dp),
            ) {
                items(items = animais, key = { it.id }) { animal ->
                    CartaoAnimal(animal = animal, onAbrir = { onAbrirAnimal(animal.id) })
                }
            }
        }
    }
}

/** Um animal da lista: nome, espécie e raça, idade e peso; toque abre o detalhe (M03). */
@Composable
private fun CartaoAnimal(
    animal: MeuAnimalListItemDto,
    onAbrir: () -> Unit,
    modifier: Modifier = Modifier,
) {
    val verDetalhe = stringResource(R.string.m02_ver_detalhe)
    Card(
        modifier =
        modifier
            .fillMaxWidth()
            .clip(CardDefaults.shape)
            .clickable(onClickLabel = verDetalhe, role = Role.Button, onClick = onAbrir)
            .testTag("m02-animal-${animal.id}"),
    ) {
        Column(
            modifier = Modifier.padding(16.dp),
            verticalArrangement = Arrangement.spacedBy(4.dp),
        ) {
            Text(text = animal.nome, style = MaterialTheme.typography.titleMedium)
            Text(text = listOfNotNull(animal.especie, animal.raca).joinToString(" • "))
            val idade = animal.idadeAnos?.let { textoIdade(it) }
            val complemento = listOfNotNull(idade, animal.peso?.let(Formatadores::peso))
            if (complemento.isNotEmpty()) {
                Text(
                    text = complemento.joinToString(" • "),
                    style = MaterialTheme.typography.bodySmall,
                )
            }
            Text(
                text = verDetalhe,
                color = MaterialTheme.colorScheme.primary,
                style = MaterialTheme.typography.labelLarge,
            )
        }
    }
}
