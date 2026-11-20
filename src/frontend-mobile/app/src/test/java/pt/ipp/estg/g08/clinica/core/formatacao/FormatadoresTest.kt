package pt.ipp.estg.g08.clinica.core.formatacao

import java.time.LocalDate
import org.junit.Assert.assertEquals
import org.junit.Test

class FormatadoresTest {
    @Test
    fun `datas no formato dd-MM-aaaa`() {
        assertEquals("10/05/2022", Formatadores.data(LocalDate.of(2022, 5, 10)))
        assertEquals("01/12/2026", Formatadores.data(LocalDate.of(2026, 12, 1)))
    }

    @Test
    fun `peso com virgula decimal e ate duas casas`() {
        assertEquals("29,1 kg", Formatadores.peso(29.1))
        assertEquals("4,25 kg", Formatadores.peso(4.25))
        assertEquals("0,5 kg", Formatadores.peso(0.5))
    }

    @Test
    fun `peso inteiro sem casas decimais e sem separador de milhares`() {
        assertEquals("30 kg", Formatadores.peso(30.0))
        assertEquals("9999,99 kg", Formatadores.peso(9999.99))
    }
}
