package pt.ipp.estg.g08.clinica.features.animais

import androidx.compose.runtime.Composable
import androidx.compose.ui.res.pluralStringResource
import androidx.compose.ui.res.stringResource
import pt.ipp.estg.g08.clinica.R

/**
 * Idade calculada pela API (`idadeAnos`) em texto: "Menos de 1 ano", "1 ano" ou "4 anos".
 * A aplicação não calcula a idade: só a apresenta.
 */
@Composable
fun textoIdade(anos: Int): String = if (anos < 1) {
    stringResource(R.string.idade_menos_de_um_ano)
} else {
    pluralStringResource(R.plurals.idade_anos, anos, anos)
}
