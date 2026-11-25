package pt.ipp.estg.g08.clinica.core.api

import java.time.LocalDate
import kotlinx.serialization.Serializable
import kotlinx.serialization.SerializationException
import org.junit.Assert.assertEquals
import org.junit.Assert.assertNull
import org.junit.Test

@Serializable
private data class ComDia(
    @Serializable(with = LocalDateSerializer::class)
    val dia: LocalDate,
)

@Serializable
private data class ComDiaOpcional(
    @Serializable(with = LocalDateSerializer::class)
    val dia: LocalDate? = null,
)

class LocalDateSerializerTest {
    private val json = ApiJson.json

    @Test
    fun `le datas simples AAAA-MM-DD`() {
        assertEquals(
            LocalDate.of(2026, 10, 14),
            json.decodeFromString<ComDia>("""{"dia":"2026-10-14"}""").dia,
        )
    }

    @Test
    fun `escreve AAAA-MM-DD e volta a ler a mesma data`() {
        val original = ComDia(LocalDate.of(2027, 1, 2))

        val texto = json.encodeToString(ComDia.serializer(), original)

        assertEquals("""{"dia":"2027-01-02"}""", texto)
        assertEquals(original, json.decodeFromString<ComDia>(texto))
    }

    @Test
    fun `aceita null nos campos opcionais`() {
        assertNull(json.decodeFromString<ComDiaOpcional>("""{"dia":null}""").dia)
    }

    @Test(expected = SerializationException::class)
    fun `data noutro formato da SerializationException`() {
        json.decodeFromString<ComDia>("""{"dia":"14/10/2026"}""")
    }
}
