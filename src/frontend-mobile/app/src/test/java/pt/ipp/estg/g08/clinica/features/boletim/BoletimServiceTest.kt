package pt.ipp.estg.g08.clinica.features.boletim

import java.time.LocalDate
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.test.runTest
import org.junit.Assert.assertEquals
import org.junit.Assert.assertTrue
import org.junit.Test
import pt.ipp.estg.g08.clinica.core.api.ApiClient
import pt.ipp.estg.g08.clinica.core.api.ApiException
import pt.ipp.estg.g08.clinica.core.api.ERRO_403
import pt.ipp.estg.g08.clinica.core.api.ServidorFalso
import pt.ipp.estg.g08.clinica.core.api.esperarErro
import pt.ipp.estg.g08.clinica.core.api.respostaJson
import pt.ipp.estg.g08.clinica.core.config.AppConfig
import pt.ipp.estg.g08.clinica.features.boletim.dto.HistoricoAdministracaoDto
import pt.ipp.estg.g08.clinica.features.boletim.dto.ProximaAdministracaoDto
import pt.ipp.estg.g08.clinica.features.boletim.dto.TipoAdministracao

/** Resposta de EP-044 com a estrutura do exemplo da Documentação da API v03. */
private const val BOLETIM =
    """{"historico":[""" +
        """{"tipo":"Vacina","produto":"Vacina antirrábica","dataAdministracao":"2026-10-19"},""" +
        """{"tipo":"Desparasitacao","produto":"Desparasitante interno Y",""" +
        """"dataAdministracao":"2026-10-19"}],""" +
        """"proximas":[""" +
        """{"tipo":"Desparasitacao","produto":"Desparasitante interno Y",""" +
        """"dataProximaPrevista":"2027-01-17"},""" +
        """{"tipo":"Vacina","produto":"Vacina antirrábica","dataProximaPrevista":"2027-10-19"}]}"""

/** EP-044 com o `ApiClient` real e um servidor falso (sem rede). */
class BoletimServiceTest {
    private fun api(servidor: ServidorFalso) = ApiClient(
        AppConfig(apiBaseUrl = "http://localhost:5159"),
        httpClient = servidor.cliente,
        dispatcher = Dispatchers.Unconfined,
    )

    @Test
    fun `obter pede EP-044 do animal indicado`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, BOLETIM) }

        BoletimService(api(servidor)).obter(3)

        val pedido = servidor.pedidos.single()
        assertEquals("GET", pedido.method)
        assertEquals("http://localhost:5159/me/animais/3/vacinacoes", pedido.url.toString())
    }

    @Test
    fun `le o historico e as proximas administracoes pela ordem da API`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, BOLETIM) }

        val boletim = BoletimService(api(servidor)).obter(3)

        assertEquals(
            listOf(
                HistoricoAdministracaoDto(
                    TipoAdministracao.Vacina,
                    "Vacina antirrábica",
                    LocalDate.of(2026, 10, 19),
                ),
                HistoricoAdministracaoDto(
                    TipoAdministracao.Desparasitacao,
                    "Desparasitante interno Y",
                    LocalDate.of(2026, 10, 19),
                ),
            ),
            boletim.historico,
        )
        assertEquals(
            listOf(
                ProximaAdministracaoDto(
                    TipoAdministracao.Desparasitacao,
                    "Desparasitante interno Y",
                    LocalDate.of(2027, 1, 17),
                ),
                ProximaAdministracaoDto(
                    TipoAdministracao.Vacina,
                    "Vacina antirrábica",
                    LocalDate.of(2027, 10, 19),
                ),
            ),
            boletim.proximas,
        )
    }

    @Test
    fun `animal sem registos devolve as duas listas vazias`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, """{"historico":[],"proximas":[]}""") }

        val boletim = BoletimService(api(servidor)).obter(8)

        assertTrue(boletim.historico.isEmpty())
        assertTrue(boletim.proximas.isEmpty())
    }

    @Test
    fun `boletim de animal de outro Cliente da 403`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 403, ERRO_403) }

        val erro = esperarErro<ApiException> { BoletimService(api(servidor)).obter(99) }

        assertEquals(403, erro.status)
        assertEquals("Não tem acesso a esta informação.", erro.mensagemParaUtilizador())
    }

    @Test
    fun `data invalida na resposta da erro de resposta inesperada`() = runTest {
        val servidor =
            ServidorFalso { respostaJson(it, 200, BOLETIM.replace("2026-10-19", "ontem")) }

        val erro = esperarErro<ApiException> { BoletimService(api(servidor)).obter(3) }

        assertEquals(ApiException.STATUS_SEM_RESPOSTA, erro.status)
        assertEquals("Resposta inesperada do servidor.", erro.mensagem)
    }
}
