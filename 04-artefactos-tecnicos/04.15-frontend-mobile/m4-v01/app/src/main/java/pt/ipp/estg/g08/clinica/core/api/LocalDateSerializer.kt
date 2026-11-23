package pt.ipp.estg.g08.clinica.core.api

import java.time.LocalDate
import java.time.format.DateTimeParseException
import kotlinx.serialization.KSerializer
import kotlinx.serialization.SerializationException
import kotlinx.serialization.descriptors.PrimitiveKind
import kotlinx.serialization.descriptors.PrimitiveSerialDescriptor
import kotlinx.serialization.descriptors.SerialDescriptor
import kotlinx.serialization.encoding.Decoder
import kotlinx.serialization.encoding.Encoder

/**
 * Datas simples da API no formato ISO 8601 `AAAA-MM-DD` (por exemplo, `2026-10-14`) convertidas
 * para [LocalDate] (DM-07; secção 2 da Documentação da API).
 *
 * Uma data inválida dá [SerializationException], que o `ApiClient` trata como resposta
 * inesperada, em vez de terminar a aplicação.
 */
object LocalDateSerializer : KSerializer<LocalDate> {
    override val descriptor: SerialDescriptor =
        PrimitiveSerialDescriptor("LocalDate", PrimitiveKind.STRING)

    override fun serialize(encoder: Encoder, value: LocalDate) {
        encoder.encodeString(value.toString())
    }

    override fun deserialize(decoder: Decoder): LocalDate {
        val texto = decoder.decodeString()
        return try {
            LocalDate.parse(texto)
        } catch (e: DateTimeParseException) {
            throw SerializationException("Data inválida: '$texto'.", e)
        }
    }
}
