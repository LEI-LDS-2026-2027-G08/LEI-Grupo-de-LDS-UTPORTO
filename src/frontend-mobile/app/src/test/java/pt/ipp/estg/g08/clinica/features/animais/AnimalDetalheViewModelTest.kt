package pt.ipp.estg.g08.clinica.features.animais

import io.mockk.coEvery
import io.mockk.coVerify
import io.mockk.mockk
import java.time.LocalDate
import org.junit.Assert.assertEquals
import org.junit.Rule
import org.junit.Test
import pt.ipp.estg.g08.clinica.core.api.ApiException
import pt.ipp.estg.g08.clinica.features.animais.dto.MeuAnimalDto
import pt.ipp.estg.g08.clinica.features.animais.dto.Sexo
import pt.ipp.estg.g08.clinica.sessao.RegraDispatcherPrincipal
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento

/** M03 — estados do [AnimalDetalheViewModel] com o `AnimaisService` simulado (MockK). */
class AnimalDetalheViewModelTest {
    @get:Rule
    val regra = RegraDispatcherPrincipal()

    private val service = mockk<AnimaisService>()

    private val bobi =
        MeuAnimalDto(3, "Bobi", "Cão", "Labrador", Sexo.Macho, LocalDate.of(2022, 5, 10), 4, 29.1)

    @Test
    fun `ao abrir pede o animal indicado e mostra os dados basicos`() {
        coEvery { service.obter(3) } returns bobi

        val viewModel = AnimalDetalheViewModel(service, animalId = 3)

        assertEquals(3, viewModel.animalId)
        assertEquals(EstadoCarregamento.Sucesso(bobi), viewModel.estado.value)
        coVerify(exactly = 1) { service.obter(3) }
    }

    @Test
    fun `animal de outro Cliente mostra acesso recusado sem dados (UC08 E1)`() {
        coEvery { service.obter(99) } throws
            ApiException(status = 403, mensagem = "Não tem permissão para executar esta operação.")

        val viewModel = AnimalDetalheViewModel(service, animalId = 99)

        assertEquals(
            EstadoCarregamento.Erro("Não tem acesso a esta informação."),
            viewModel.estado.value,
        )
    }

    @Test
    fun `animal inexistente mostra que a informacao ja nao existe (UC08 E2)`() {
        coEvery { service.obter(77) } throws
            ApiException(status = 404, mensagem = "Recurso não encontrado.")

        val viewModel = AnimalDetalheViewModel(service, animalId = 77)

        assertEquals(
            EstadoCarregamento.Erro("A informação pedida já não existe."),
            viewModel.estado.value,
        )
    }

    @Test
    fun `Tentar novamente volta a pedir o mesmo animal`() {
        coEvery { service.obter(3) } throws ApiException.semLigacao() andThen bobi

        val viewModel = AnimalDetalheViewModel(service, animalId = 3)
        viewModel.carregar()

        assertEquals(EstadoCarregamento.Sucesso(bobi), viewModel.estado.value)
        coVerify(exactly = 2) { service.obter(3) }
    }
}
