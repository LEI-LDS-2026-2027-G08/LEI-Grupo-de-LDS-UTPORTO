package pt.ipp.estg.g08.clinica.core.config

import kotlin.time.Duration
import kotlin.time.Duration.Companion.seconds
import pt.ipp.estg.g08.clinica.BuildConfig

private const val SEGUNDOS_TIMEOUT_PADRAO = 30

/**
 * Configuração da aplicação.
 *
 * A URL base da API vem de `BuildConfig.API_BASE_URL`, definida em tempo de compilação
 * (propriedade Gradle `apiBaseUrl`), nunca escrita no código.
 */
data class AppConfig(
    val apiBaseUrl: String,
    val timeout: Duration = SEGUNDOS_TIMEOUT_PADRAO.seconds,
) {
    companion object {
        fun deBuildConfig(): AppConfig = AppConfig(apiBaseUrl = BuildConfig.API_BASE_URL)
    }
}
