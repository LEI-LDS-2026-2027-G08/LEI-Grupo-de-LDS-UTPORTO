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
import androidx.compose.ui.text.style.TextAlign
import pt.ipp.estg.g08.clinica.R
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento

/**
 * Apresentação uniforme dos estados de carregamento: indicador de progresso, mensagem de vazio,
 * erro com "Tentar novamente" ou o [conteudo] quando há dados.
 *
 * [etiquetaVazio] permite a cada ecrã usar a etiqueta de vazio prevista na secção 13 do Modelo
 * de Classes Mobile (por exemplo, `m02-vazio` e `m06-vazio`).
 */
@Composable
fun <T> EstadoVista(
    estado: EstadoCarregamento<T>,
    onTentarNovamente: () -> Unit,
    modifier: Modifier = Modifier,
    mensagemVazio: String = "",
    etiquetaVazio: String = "estado-vazio",
    conteudo: @Composable (T) -> Unit,
) {
    Box(modifier = modifier.fillMaxWidth(), contentAlignment = Alignment.Center) {
        when (estado) {
            EstadoCarregamento.Inicial -> Unit
            EstadoCarregamento.ACarregar ->
                CircularProgressIndicator(modifier = Modifier.testTag("estado-a-carregar"))
            EstadoCarregamento.Vazio ->
                Text(
                    text = mensagemVazio,
                    textAlign = TextAlign.Center,
                    modifier = Modifier.testTag(etiquetaVazio),
                )
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
