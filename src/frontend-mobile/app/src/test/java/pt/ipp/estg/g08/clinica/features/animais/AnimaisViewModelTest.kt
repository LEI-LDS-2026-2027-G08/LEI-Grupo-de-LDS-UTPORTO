package pt.ipp.estg.g08.clinica.features.animais

import io.mockk.coEvery
import io.mockk.coVerify
import io.mockk.mockk
import kotlinx.coroutines.CompletableDeferred
import org.junit.Assert.assertEquals
import org.junit.Rule
import org.junit.Test
import pt.ipp.estg.g08.clinica.core.api.ApiException
import pt.ipp.estg.g08.clinica.features.animais.dto.MeuAnimalListItemDto
import pt.ipp.estg.g08.clinica.sessao.RegraDispatcherPrincipal
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento

/** M02 — estados do [AnimaisViewModel] com o `AnimaisService` simulado (MockK). */
class AnimaisViewModelTest {
    @get:Rule
    val regra = RegraDispatcherPrincipal()

    private val service = mockk<AnimaisService>()

    private val animais =
        listOf(
            MeuAnimalListItemDto(3, "Bobi", "Cão", "Labrador", 4, 29.1),
            MeuAnimalListItemDto(8, "Mia", "Gato", "Europeu Comum", 2, 4.2),
        )

    @Test
    fun `ao abrir pede a lista e mostra os animais do Cliente`() {
        coEvery { service.listar() } returns animais

        val viewModel = AnimaisViewModel(service)

        assertEquals(EstadoCarregamento.Sucesso(animais), viewModel.estado.value)
        coVerify(exactly = 1) { service.listar() }
    }

    @Test
    fun `sem animais o estado e Vazio (UC08 A1)`() {
        coEvery { service.listar() } returns emptyList()

        val viewModel = AnimaisViewModel(service)

        assertEquals(EstadoCarregamento.Vazio, viewModel.estado.value)
    }

    @Test
    fun `enquanto espera pela API o estado e ACarregar e pedidos repetidos sao ignorados`() {
        val resposta = CompletableDeferred<List<MeuAnimalListItemDto>>()
        coEvery { service.listar() } coAnswers { resposta.await() }

        val viewModel = AnimaisViewModel(service)
        viewModel.carregar()

        assertEquals(EstadoCarregamento.ACarregar, viewModel.estado.value)
        coVerify(exactly = 1) { service.listar() }

        resposta.complete(animais)

        assertEquals(EstadoCarregamento.Sucesso(animais), viewModel.estado.value)
    }

    @Test
    fun `sem ligacao mostra o erro e Tentar novamente volta a pedir`() {
        coEvery { service.listar() } throws ApiException.semLigacao() andThen animais

        val viewModel = AnimaisViewModel(service)

        assertEquals(
            EstadoCarregamento.Erro("Sem ligação ao servidor. Verifique a sua ligação à rede."),
            viewModel.estado.value,
        )

        viewModel.carregar()

        assertEquals(EstadoCarregamento.Sucesso(animais), viewModel.estado.value)
        coVerify(exactly = 2) { service.listar() }
    }

    @Test
    fun `erro do servidor mostra a mensagem em portugues sem detalhes tecnicos`() {
        coEvery { service.listar() } throws
            ApiException(status = 500, mensagem = "Ocorreu um erro interno no servidor.")

        val viewModel = AnimaisViewModel(service)

        assertEquals(
            EstadoCarregamento.Erro("Ocorreu um erro no servidor. Tente novamente mais tarde."),
            viewModel.estado.value,
        )
    }
}
