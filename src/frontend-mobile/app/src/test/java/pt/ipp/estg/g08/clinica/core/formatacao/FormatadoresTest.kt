package pt.ipp.estg.g08.clinica.core.formatacao

import java.time.Instant
import java.time.LocalDate
import java.time.ZoneId
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

    @Test
    fun `data e hora na hora de Lisboa, com e sem hora de verao`() {
        val lisboa = ZoneId.of("Europe/Lisbon")

        // 14/10/2026: hora de verão (UTC+1); 05/11/2026: hora de inverno (UTC+0).
        assertEquals(
            "14/10/2026 • 10:30",
            Formatadores.dataHora(Instant.parse("2026-10-14T09:30:00Z"), lisboa),
        )
        assertEquals(
            "05/11/2026 • 11:00",
            Formatadores.dataHora(Instant.parse("2026-11-05T11:00:00Z"), lisboa),
        )
    }

    @Test
    fun `data e hora seguem o fuso indicado, incluindo a mudanca de dia`() {
        val instante = Instant.parse("2026-12-31T23:30:00Z")

        assertEquals("31/12/2026 • 23:30", Formatadores.dataHora(instante, ZoneId.of("UTC")))
        assertEquals(
            "01/01/2027 • 00:30",
            Formatadores.dataHora(instante, ZoneId.of("Europe/Paris")),
        )
    }
}
