package pt.ipp.estg.g08.clinica

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import pt.ipp.estg.g08.clinica.navegacao.AppNavHost
import pt.ipp.estg.g08.clinica.ui.theme.GestãoDeClínicaVeterináriaTheme

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        val container = (application as ClinicaApplication).container
        setContent {
            GestãoDeClínicaVeterináriaTheme {
                AppNavHost(container = container)
            }
        }
    }
}
