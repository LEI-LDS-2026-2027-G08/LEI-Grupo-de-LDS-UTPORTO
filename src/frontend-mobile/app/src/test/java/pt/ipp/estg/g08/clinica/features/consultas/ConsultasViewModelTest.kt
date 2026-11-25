package pt.ipp.estg.g08.clinica.features.consultas

import io.mockk.coEvery
import io.mockk.coVerify
import io.mockk.mockk
import java.time.Instant
import kotlinx.coroutines.CompletableDeferred
import org.junit.Assert.assertEquals
import org.junit.Assert.assertNull
import org.junit.Rule
import org.junit.Test
import pt.ipp.estg.g08.clinica.core.api.ApiException
import pt.ipp.estg.g08.clinica.features.consultas.dto.AnimalRefDto
import pt.ipp.estg.g08.clinica.features.consultas.dto.EstadoConsulta
import pt.ipp.estg.g08.clinica.features.consultas.dto.MinhaConsultaDto
import pt.ipp.estg.g08.clinica.features.consultas.dto.TipoConsultaFiltro.Futuras
import pt.ipp.estg.g08.clinica.features.consultas.dto.TipoConsultaFiltro.Historico
import pt.ipp.estg.g08.clinica.sessao.RegraDispatcherPrincipal
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento

/** M04 — estados do [ConsultasViewModel] com o `ConsultasService` simulado (MockK). */
class ConsultasViewModelTest {
    @get:Rule
    val regra = RegraDispatcherPrincipal()

    private val service = mockk<ConsultasService>()

    private val futura =
        MinhaConsultaDto(
            id = 7,
            dataHora = Instant.parse("2026-12-01T10:00:00Z"),
            estado = EstadoConsulta.Confirmada,
            animal = AnimalRefDto(3, "Bobi", "Cão"),
            veterinario = "Dra. Ana",
        )
    private val passada =
        futura.copy(
            id = 1,
            dataHora = Instant.parse("2026-10-14T09:30:00Z"),
            estado = EstadoConsulta.Realizada,
        )

    @Test
    fun `ao abrir pede as futuras e o historico de todos os animais`() {
        coEvery { service.listar(Futuras, null) } returns listOf(futura)
        coEvery { service.listar(Historico, null) } returns listOf(passada)

        val viewModel = ConsultasViewModel(service)

        assertNull(viewModel.animalId)
        assertEquals(EstadoCarregamento.Sucesso(listOf(futura)), viewModel.estadoFuturas.value)
        assertEquals(EstadoCarregamento.Sucesso(listOf(passada)), viewModel.estadoHistorico.value)
        coVerify(exactly = 1) { service.listar(Futuras, null) }
        coVerify(exactly = 1) { service.listar(Historico, null) }
    }

    @Test
    fun `aberto a partir de M03 pede so as marcacoes desse animal`() {
        coEvery { service.listar(any(), 3) } returns emptyList()

        val viewModel = ConsultasViewModel(service, animalId = 3)

        assertEquals(3, viewModel.animalId)
        coVerify(exactly = 1) { service.listar(Futuras, 3) }
        coVerify(exactly = 1) { service.listar(Historico, 3) }
        coVerify(exactly = 0) { service.listar(any(), null) }
    }

    @Test
    fun `a ordem das listas e a da API, sem reordenar`() {
        val outra = futura.copy(id = 8, dataHora = Instant.parse("2026-11-30T10:00:00Z"))
        coEvery { service.listar(Futuras, null) } returns listOf(futura, outra)
        coEvery { service.listar(Historico, null) } returns emptyList()

        val viewModel = ConsultasViewModel(service)

        assertEquals(
            EstadoCarregamento.Sucesso(listOf(futura, outra)),
            viewModel.estadoFuturas.value,
        )
    }

    @Test
    fun `cada lista vazia tem o seu estado Vazio`() {
        coEvery { service.listar(Futuras, null) } returns emptyList()
        coEvery { service.listar(Historico, null) } returns listOf(passada)

        val viewModel = ConsultasViewModel(service)

        assertEquals(EstadoCarregamento.Vazio, viewModel.estadoFuturas.value)
        assertEquals(EstadoCarregamento.Sucesso(listOf(passada)), viewModel.estadoHistorico.value)
    }

    @Test
    fun `animal de outro Cliente mostra acesso recusado sem dados (UC11 E1)`() {
        coEvery { service.listar(any(), 99) } throws
            ApiException(status = 403, mensagem = "Não tem permissão para executar esta operação.")

        val viewModel = ConsultasViewModel(service, animalId = 99)

        val recusado = EstadoCarregamento.Erro("Não tem acesso a esta informação.")
        assertEquals(recusado, viewModel.estadoFuturas.value)
        assertEquals(recusado, viewModel.estadoHistorico.value)
    }

    @Test
    fun `um erro numa lista nao esconde a outra`() {
        coEvery { service.listar(Futuras, null) } throws ApiException.semLigacao()
        coEvery { service.listar(Historico, null) } returns listOf(passada)

        val viewModel = ConsultasViewModel(service)

        assertEquals(
            EstadoCarregamento.Erro("Sem ligação ao servidor. Verifique a sua ligação à rede."),
            viewModel.estadoFuturas.value,
        )
        assertEquals(EstadoCarregamento.Sucesso(listOf(passada)), viewModel.estadoHistorico.value)
    }

    @Test
    fun `Tentar novamente volta a pedir so a lista indicada`() {
        coEvery { service.listar(Futuras, null) } throws ApiException.semLigacao()
        coEvery { service.listar(Historico, null) } returns listOf(passada)
        val viewModel = ConsultasViewModel(service)

        coEvery { service.listar(Futuras, null) } returns listOf(futura)
        viewModel.carregar(Futuras)

        assertEquals(EstadoCarregamento.Sucesso(listOf(futura)), viewModel.estadoFuturas.value)
        coVerify(exactly = 2) { service.listar(Futuras, null) }
        coVerify(exactly = 1) { service.listar(Historico, null) }
    }

    @Test
    fun `pedidos repetidos da mesma lista sao ignorados enquanto decorrem`() {
        val resposta = CompletableDeferred<List<MinhaConsultaDto>>()
        coEvery { service.listar(Futuras, null) } coAnswers { resposta.await() }
        coEvery { service.listar(Historico, null) } returns emptyList()

        val viewModel = ConsultasViewModel(service)
        viewModel.carregar(Futuras)
        viewModel.carregar()

        assertEquals(EstadoCarregamento.ACarregar, viewModel.estadoFuturas.value)
        coVerify(exactly = 1) { service.listar(Futuras, null) }
        coVerify(exactly = 2) { service.listar(Historico, null) }

        resposta.complete(listOf(futura))
        assertEquals(EstadoCarregamento.Sucesso(listOf(futura)), viewModel.estadoFuturas.value)
    }
}
