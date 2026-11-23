package pt.ipp.estg.g08.clinica

import android.app.Application

class ClinicaApplication : Application() {
    val container: AppContainer by lazy { AppContainer(this) }
}
