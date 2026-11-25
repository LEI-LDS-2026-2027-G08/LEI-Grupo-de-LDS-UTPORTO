package pt.ipp.estg.g08.clinica.core.api

import java.time.Instant
import kotlinx.serialization.Serializable
import kotlinx.serialization.SerializationException
import org.junit.Assert.assertEquals
import org.junit.Test

@Serializable
private data class ComData(
    @Serializable(with = InstantSerializer::class)
    val quando: Instant,
)

class InstantSerializerTest {
    private val json = ApiJson.json

    @Test
    fun `le datas em UTC e com desvio explicito`() {
        assertEquals(
            Instant.parse("2026-11-10T22:00:00Z"),
            json.decodeFromString<ComData>("""{"quando":"2026-11-10T22:00:00Z"}""").quando,
        )
        assertEquals(
            Instant.parse("2026-11-10T22:00:00Z"),
            json.decodeFromString<ComData>("""{"quando":"2026-11-10T22:00:00+00:00"}""").quando,
        )
        assertEquals(
            Instant.parse("2026-11-10T22:00:00Z"),
            json.decodeFromString<ComData>("""{"quando":"2026-11-10T23:00:00+01:00"}""").quando,
        )
    }

    @Test
    fun `escreve em UTC e volta a ler o mesmo instante`() {
        val original = ComData(Instant.parse("2026-11-10T22:00:00Z"))

        val texto = json.encodeToString(ComData.serializer(), original)

        assertEquals("""{"quando":"2026-11-10T22:00:00Z"}""", texto)
        assertEquals(original, json.decodeFromString<ComData>(texto))
    }

    @Test(expected = SerializationException::class)
    fun `data invalida da SerializationException`() {
        json.decodeFromString<ComData>("""{"quando":"amanha"}""")
    }
}
