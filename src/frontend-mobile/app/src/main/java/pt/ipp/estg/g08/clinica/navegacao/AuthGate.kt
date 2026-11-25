package pt.ipp.estg.g08.clinica.navegacao

import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.statusBarsPadding
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import pt.ipp.estg.g08.clinica.sessao.EstadoSessao
import pt.ipp.estg.g08.clinica.sessao.SessaoViewModel
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento
import pt.ipp.estg.g08.clinica.shared.components.EstadoVista

/**
 * Mostra o ecrã de arranque, o início de sessão ou a área autenticada consoante o
 * [EstadoSessao] (secção 6.1 do Modelo de Classes Mobile). A área autenticada não tem outra
 * entrada: sem sessão de Cliente válida não há forma de a abrir.
 */
@Composable
fun AuthGate(
    sessao: SessaoViewModel,
    areaNaoAutenticada: @Composable () -> Unit,
    areaAutenticada: @Composable () -> Unit,
    modifier: Modifier = Modifier,
) {
    val estado by sessao.estado.collectAsStateWithLifecycle()
    val erro by sessao.erro.collectAsStateWithLifecycle()

    Box(modifier = modifier.fillMaxSize()) {
        when (estado) {
            EstadoSessao.Desconhecida ->
                EcraArranque(erro = erro, onTentarNovamente = sessao::restaurarSessao)
            EstadoSessao.NaoAutenticada -> areaNaoAutenticada()
            EstadoSessao.Autenticada -> areaAutenticada()
        }
    }
}

/** Arranque: indicador de progresso enquanto a sessão é confirmada; erro com nova tentativa. */
@Composable
private fun EcraArranque(
    erro: String?,
    onTentarNovamente: () -> Unit,
    modifier: Modifier = Modifier,
) {
    val estado: EstadoCarregamento<Unit> =
        if (erro == null) EstadoCarregamento.ACarregar else EstadoCarregamento.Erro(erro)
    EstadoVista(
        estado = estado,
        onTentarNovamente = onTentarNovamente,
        modifier = modifier.fillMaxSize().statusBarsPadding().padding(16.dp),
    ) {}
}
