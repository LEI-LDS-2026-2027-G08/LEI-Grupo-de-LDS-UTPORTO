package pt.ipp.estg.g08.clinica.features.consultas

import java.time.Instant
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.test.runTest
import org.junit.Assert.assertEquals
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Test
import pt.ipp.estg.g08.clinica.core.api.ApiClient
import pt.ipp.estg.g08.clinica.core.api.ApiException
import pt.ipp.estg.g08.clinica.core.api.ERRO_403
import pt.ipp.estg.g08.clinica.core.api.ServidorFalso
import pt.ipp.estg.g08.clinica.core.api.esperarErro
import pt.ipp.estg.g08.clinica.core.api.respostaJson
import pt.ipp.estg.g08.clinica.core.config.AppConfig
import pt.ipp.estg.g08.clinica.features.consultas.dto.AnimalRefDto
import pt.ipp.estg.g08.clinica.features.consultas.dto.EstadoConsulta
import pt.ipp.estg.g08.clinica.features.consultas.dto.MinhaConsultaDto
import pt.ipp.estg.g08.clinica.features.consultas.dto.TipoConsultaFiltro

/** Resposta de EP-030 com o exemplo da Documentação da API v03 e mais uma marcação. */
private const val HISTORICO =
    """[""" +
        """{"id":33,"dataHora":"2026-09-02T15:00:00Z","estado":"Realizada",""" +
        """"animal":{"id":3,"nome":"Bobi","especie":"Cão"},"veterinario":"Dra. Marta Reis"},""" +
        """{"id":21,"dataHora":"2026-08-20T09:30:00Z","estado":"Cancelada",""" +
        """"animal":{"id":4,"nome":"Mia","especie":"Gato"},"veterinario":"Dr. Rui Lopes"}]"""

/** EP-030 com o `ApiClient` real e um servidor falso (sem rede). */
class ConsultasServiceTest {
    private fun api(servidor: ServidorFalso) = ApiClient(
        AppConfig(apiBaseUrl = "http://localhost:5159"),
        httpClient = servidor.cliente,
        dispatcher = Dispatchers.Unconfined,
    )

    @Test
    fun `futuras de todos os animais pedem EP-030 com tipo=futuras e sem animalId`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, "[]") }

        ConsultasService(api(servidor)).listar(TipoConsultaFiltro.Futuras)

        val pedido = servidor.pedidos.single()
        assertEquals("GET", pedido.method)
        assertEquals("http://localhost:5159/me/consultas?tipo=futuras", pedido.url.toString())
    }

    @Test
    fun `historico de um animal envia tipo=historico e o animalId`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, "[]") }

        ConsultasService(api(servidor)).listar(TipoConsultaFiltro.Historico, animalId = 3)

        assertEquals(
            "http://localhost:5159/me/consultas?tipo=historico&animalId=3",
            servidor.pedidos.single().url.toString(),
        )
    }

    @Test
    fun `nenhum pedido envia o identificador do Cliente (RNF002)`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, "[]") }

        ConsultasService(api(servidor)).listar(TipoConsultaFiltro.Futuras, animalId = 3)

        assertEquals(
            setOf("tipo", "animalId"),
            servidor.pedidos.single().url.queryParameterNames,
        )
    }

    @Test
    fun `le as marcacoes pela ordem da API, com a data em UTC`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, HISTORICO) }

        val consultas = ConsultasService(api(servidor)).listar(TipoConsultaFiltro.Historico)

        assertEquals(
            listOf(
                MinhaConsultaDto(
                    id = 33,
                    dataHora = Instant.parse("2026-09-02T15:00:00Z"),
                    estado = EstadoConsulta.Realizada,
                    animal = AnimalRefDto(id = 3, nome = "Bobi", especie = "Cão"),
                    veterinario = "Dra. Marta Reis",
                ),
                MinhaConsultaDto(
                    id = 21,
                    dataHora = Instant.parse("2026-08-20T09:30:00Z"),
                    estado = EstadoConsulta.Cancelada,
                    animal = AnimalRefDto(id = 4, nome = "Mia", especie = "Gato"),
                    veterinario = "Dr. Rui Lopes",
                ),
            ),
            consultas,
        )
    }

    @Test
    fun `especie do animal pode vir a null ou faltar`() = runTest {
        val corpo =
            """[{"id":1,"dataHora":"2026-12-01T10:00:00Z","estado":"Pendente",""" +
                """"animal":{"id":3,"nome":"Bobi","especie":null},"veterinario":"Dra. Ana"},""" +
                """{"id":2,"dataHora":"2026-12-02T10:00:00Z","estado":"Confirmada",""" +
                """"animal":{"id":3,"nome":"Bobi"},"veterinario":"Dra. Ana"}]"""
        val servidor = ServidorFalso { respostaJson(it, 200, corpo) }

        val consultas = ConsultasService(api(servidor)).listar(TipoConsultaFiltro.Futuras)

        assertNull(consultas[0].animal.especie)
        assertNull(consultas[1].animal.especie)
        assertEquals(EstadoConsulta.Confirmada, consultas[1].estado)
    }

    @Test
    fun `campos que a API venha a acrescentar sao ignorados`() = runTest {
        val corpo =
            """[{"id":1,"dataHora":"2026-12-01T10:00:00Z","estado":"Pendente","novo":"x",""" +
                """"animal":{"id":3,"nome":"Bobi","especie":"Cão"},"veterinario":"Dra. Ana"}]"""
        val servidor = ServidorFalso { respostaJson(it, 200, corpo) }

        val consultas = ConsultasService(api(servidor)).listar(TipoConsultaFiltro.Futuras)

        assertEquals(1, consultas.single().id)
    }

    @Test
    fun `sem marcacoes devolve lista vazia`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, "[]") }

        assertTrue(ConsultasService(api(servidor)).listar(TipoConsultaFiltro.Futuras).isEmpty())
    }

    @Test
    fun `animalId de outro Cliente da 403 (UC11 E1)`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 403, ERRO_403) }

        val erro =
            esperarErro<ApiException> {
                ConsultasService(api(servidor)).listar(TipoConsultaFiltro.Futuras, animalId = 99)
            }

        assertEquals(403, erro.status)
        assertEquals("Não tem acesso a esta informação.", erro.mensagemParaUtilizador())
    }

    @Test
    fun `estado desconhecido na resposta da erro de resposta inesperada`() = runTest {
        val servidor =
            ServidorFalso { respostaJson(it, 200, HISTORICO.replace("Realizada", "Adiada")) }

        val erro =
            esperarErro<ApiException> {
                ConsultasService(api(servidor)).listar(TipoConsultaFiltro.Historico)
            }

        assertEquals(ApiException.STATUS_SEM_RESPOSTA, erro.status)
        assertEquals("Resposta inesperada do servidor.", erro.mensagem)
    }

    @Test
    fun `data invalida na resposta da erro de resposta inesperada`() = runTest {
        val servidor =
            ServidorFalso {
                respostaJson(it, 200, HISTORICO.replace("2026-09-02T15:00:00Z", "ontem"))
            }

        val erro =
            esperarErro<ApiException> {
                ConsultasService(api(servidor)).listar(TipoConsultaFiltro.Historico)
            }

        assertEquals(ApiException.STATUS_SEM_RESPOSTA, erro.status)
    }
}
