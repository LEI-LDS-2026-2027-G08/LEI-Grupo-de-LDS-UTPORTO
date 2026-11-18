package pt.ipp.estg.g08.clinica.core.formatacao

import java.text.DecimalFormat
import java.text.DecimalFormatSymbols
import java.time.LocalDate
import java.time.format.DateTimeFormatter
import java.util.Locale

/**
 * Formatação dos valores apresentados ao Cliente em português europeu (secção 5 do Modelo de
 * Classes Mobile). Só altera a apresentação: os valores vêm da API tal como foram calculados.
 */
object Formatadores {
    private val PORTUGUES: Locale = Locale.forLanguageTag("pt-PT")
    private val FORMATO_DATA: DateTimeFormatter =
        DateTimeFormatter.ofPattern("dd/MM/yyyy", PORTUGUES)

    /** Data simples no formato `dd/MM/aaaa` (por exemplo, `10/05/2022`). */
    fun data(data: LocalDate): String = FORMATO_DATA.format(data)

    /** Peso em quilogramas com vírgula decimal e até duas casas (por exemplo, `29,1 kg`). */
    fun peso(quilogramas: Double): String {
        // DecimalFormat não é seguro entre threads: cria-se um por chamada.
        val formato = DecimalFormat("0.##", DecimalFormatSymbols.getInstance(PORTUGUES))
        return "${formato.format(quilogramas)} kg"
    }
}
