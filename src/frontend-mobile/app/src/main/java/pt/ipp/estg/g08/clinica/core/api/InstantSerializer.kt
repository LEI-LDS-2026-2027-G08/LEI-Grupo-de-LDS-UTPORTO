package pt.ipp.estg.g08.clinica.core.api

import java.time.Instant
import java.time.OffsetDateTime
import java.time.format.DateTimeParseException
import kotlinx.serialization.KSerializer
import kotlinx.serialization.SerializationException
import kotlinx.serialization.descriptors.PrimitiveKind
import kotlinx.serialization.descriptors.PrimitiveSerialDescriptor
import kotlinx.serialization.descriptors.SerialDescriptor
import kotlinx.serialization.encoding.Decoder
import kotlinx.serialization.encoding.Encoder

/**
 * Datas ISO 8601 da API (por exemplo, `2026-10-09T22:00:00Z`) convertidas para [Instant].
 *
 * Aceita `Z` ou um desvio explícito (`+01:00`). Uma data inválida dá [SerializationException],
 * que o `ApiClient` trata como resposta inesperada, em vez de terminar a aplicação.
 */
object InstantSerializer : KSerializer<Instant> {
    override val descriptor: SerialDescriptor =
        PrimitiveSerialDescriptor("Instant", PrimitiveKind.STRING)

    override fun serialize(encoder: Encoder, value: Instant) {
        encoder.encodeString(value.toString())
    }

    override fun deserialize(decoder: Decoder): Instant {
        val texto = decoder.decodeString()
        return try {
            OffsetDateTime.parse(texto).toInstant()
        } catch (e: DateTimeParseException) {
            throw SerializationException("Data inválida: '$texto'.", e)
        }
    }
}
