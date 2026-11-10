package pt.ipp.estg.g08.clinica.shared.components

import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.material3.Button
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.res.stringResource
import pt.ipp.estg.g08.clinica.R
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento

/**
 * Apresentação uniforme dos estados de carregamento: indicador de progresso, mensagem de vazio,
 * erro com "Tentar novamente" ou o [conteudo] quando há dados.
 */
@Composable
fun <T> EstadoVista(
    estado: EstadoCarregamento<T>,
    onTentarNovamente: () -> Unit,
    modifier: Modifier = Modifier,
    mensagemVazio: String = "",
    conteudo: @Composable (T) -> Unit,
) {
    Box(modifier = modifier.fillMaxWidth(), contentAlignment = Alignment.Center) {
        when (estado) {
            EstadoCarregamento.Inicial -> Unit
            EstadoCarregamento.ACarregar ->
                CircularProgressIndicator(modifier = Modifier.testTag("estado-a-carregar"))
            EstadoCarregamento.Vazio ->
                Text(text = mensagemVazio, modifier = Modifier.testTag("estado-vazio"))
            is EstadoCarregamento.Erro ->
                Column(horizontalAlignment = Alignment.CenterHorizontally) {
                    Text(text = estado.mensagem, modifier = Modifier.testTag("estado-erro"))
                    Button(
                        onClick = onTentarNovamente,
                        modifier = Modifier.testTag("estado-tentar-novamente"),
                    ) {
                        Text(text = stringResource(R.string.estado_tentar_novamente))
                    }
                }
            is EstadoCarregamento.Sucesso -> conteudo(estado.dados)
        }
    }
}
