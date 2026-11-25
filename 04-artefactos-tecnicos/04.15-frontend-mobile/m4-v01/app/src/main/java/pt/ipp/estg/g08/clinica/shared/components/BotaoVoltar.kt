package pt.ipp.estg.g08.clinica.shared.components

import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.size
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.ArrowBack
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import pt.ipp.estg.g08.clinica.R

/** Regressa ao ecrã anterior da área autenticada (o botão "Voltar" do protótipo). */
@Composable
fun BotaoVoltar(onVoltar: () -> Unit, modifier: Modifier = Modifier) {
    TextButton(onClick = onVoltar, modifier = modifier) {
        Icon(
            imageVector = Icons.AutoMirrored.Filled.ArrowBack,
            contentDescription = null,
            modifier = Modifier.size(ButtonDefaults.IconSize),
        )
        Spacer(modifier = Modifier.size(ButtonDefaults.IconSpacing))
        Text(text = stringResource(R.string.voltar))
    }
}
