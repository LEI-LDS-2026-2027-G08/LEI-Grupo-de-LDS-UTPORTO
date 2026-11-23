package pt.ipp.estg.g08.clinica.features.animais

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.Job
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.ipp.estg.g08.clinica.features.animais.dto.MeuAnimalDto
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento
import pt.ipp.estg.g08.clinica.shared.carregarEstado

/**
 * M03 — Detalhe do animal (UC08; RF018). Obtém os dados básicos do animal [animalId] (EP-020).
 *
 * Um animal de outro Cliente (`403`, UC08 E1) ou inexistente (`404`, UC08 E2) dá
 * [EstadoCarregamento.Erro] com a mensagem da API, sem qualquer dado do animal.
 */
class AnimalDetalheViewModel(private val service: AnimaisService, val animalId: Int) : ViewModel() {
    private val estadoInterno =
        MutableStateFlow<EstadoCarregamento<MeuAnimalDto>>(EstadoCarregamento.Inicial)
    val estado: StateFlow<EstadoCarregamento<MeuAnimalDto>> = estadoInterno.asStateFlow()

    private var pedido: Job? = null

    init {
        carregar()
    }

    /** Pede o animal à API; também é a ação "Tentar novamente". Ignora pedidos repetidos. */
    fun carregar() {
        if (pedido?.isActive == true) return
        estadoInterno.value = EstadoCarregamento.ACarregar
        pedido =
            viewModelScope.launch {
                estadoInterno.value = carregarEstado { service.obter(animalId) }
            }
    }
}
