package pt.ipp.estg.g08.clinica.features.boletim

import io.mockk.coEvery
import io.mockk.coVerify
import io.mockk.mockk
import java.time.LocalDate
import kotlinx.coroutines.CompletableDeferred
import org.junit.Assert.assertEquals
import org.junit.Rule
import org.junit.Test
import pt.ipp.estg.g08.clinica.core.api.ApiException
import pt.ipp.estg.g08.clinica.features.boletim.dto.HistoricoAdministracaoDto
import pt.ipp.estg.g08.clinica.features.boletim.dto.MeuBoletimDto
import pt.ipp.estg.g08.clinica.features.boletim.dto.ProximaAdministracaoDto
import pt.ipp.estg.g08.clinica.features.boletim.dto.TipoAdministracao
import pt.ipp.estg.g08.clinica.sessao.RegraDispatcherPrincipal
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento

/** M06 — estados do [BoletimViewModel] com o `BoletimService` simulado (MockK). */
class BoletimViewModelTest {
    @get:Rule
    val regra = RegraDispatcherPrincipal()

    private val service = mockk<BoletimService>()

    private val vacina =
        HistoricoAdministracaoDto(
            TipoAdministracao.Vacina,
            "Vacina antirrábica",
            LocalDate.of(2026, 10, 19),
        )
    private val proximaVacina =
        ProximaAdministracaoDto(
            TipoAdministracao.Vacina,
            "Vacina antirrábica",
            LocalDate.of(2027, 10, 19),
        )

    @Test
    fun `ao abrir pede o boletim do animal e mostra historico e proximas`() {
        val boletim = MeuBoletimDto(historico = listOf(vacina), proximas = listOf(proximaVacina))
        coEvery { service.obter(3) } returns boletim

        val viewModel = BoletimViewModel(service, animalId = 3)

        assertEquals(3, viewModel.animalId)
        assertEquals(EstadoCarregamento.Sucesso(boletim), viewModel.estado.value)
        coVerify(exactly = 1) { service.obter(3) }
    }

    @Test
    fun `sem historico nem proximas o estado e Vazio (UC17 A1)`() {
        coEvery { service.obter(8) } returns MeuBoletimDto(emptyList(), emptyList())

        val viewModel = BoletimViewModel(service, animalId = 8)

        assertEquals(EstadoCarregamento.Vazio, viewModel.estado.value)
    }

    @Test
    fun `com historico e sem proximas o boletim e mostrado`() {
        val boletim = MeuBoletimDto(historico = listOf(vacina), proximas = emptyList())
        coEvery { service.obter(3) } returns boletim

        val viewModel = BoletimViewModel(service, animalId = 3)

        assertEquals(EstadoCarregamento.Sucesso(boletim), viewModel.estado.value)
    }

    @Test
    fun `sem historico e com proximas o boletim e mostrado`() {
        val boletim = MeuBoletimDto(historico = emptyList(), proximas = listOf(proximaVacina))
        coEvery { service.obter(3) } returns boletim

        val viewModel = BoletimViewModel(service, animalId = 3)

        assertEquals(EstadoCarregamento.Sucesso(boletim), viewModel.estado.value)
    }

    @Test
    fun `boletim de animal de outro Cliente mostra acesso recusado sem dados (UC17 E1)`() {
        coEvery { service.obter(99) } throws
            ApiException(status = 403, mensagem = "Não tem permissão para executar esta operação.")

        val viewModel = BoletimViewModel(service, animalId = 99)

        assertEquals(
            EstadoCarregamento.Erro("Não tem acesso a esta informação."),
            viewModel.estado.value,
        )
    }

    @Test
    fun `pedidos repetidos sao ignorados e Tentar novamente volta a pedir`() {
        val resposta = CompletableDeferred<MeuBoletimDto>()
        coEvery { service.obter(3) } coAnswers { resposta.await() }

        val viewModel = BoletimViewModel(service, animalId = 3)
        viewModel.carregar()

        assertEquals(EstadoCarregamento.ACarregar, viewModel.estado.value)
        coVerify(exactly = 1) { service.obter(3) }

        resposta.completeExceptionally(ApiException.semLigacao())
        assertEquals(
            EstadoCarregamento.Erro("Sem ligação ao servidor. Verifique a sua ligação à rede."),
            viewModel.estado.value,
        )

        coEvery { service.obter(3) } returns MeuBoletimDto(listOf(vacina), emptyList())
        viewModel.carregar()

        assertEquals(
            EstadoCarregamento.Sucesso(MeuBoletimDto(listOf(vacina), emptyList())),
            viewModel.estado.value,
        )
        coVerify(exactly = 2) { service.obter(3) }
    }
}
