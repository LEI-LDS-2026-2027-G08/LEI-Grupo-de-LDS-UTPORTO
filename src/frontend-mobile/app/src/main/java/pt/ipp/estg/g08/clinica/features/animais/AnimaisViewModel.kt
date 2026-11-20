package pt.ipp.estg.g08.clinica.features.animais

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.Job
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.ipp.estg.g08.clinica.features.animais.dto.MeuAnimalListItemDto
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento
import pt.ipp.estg.g08.clinica.shared.carregarEstado

/**
 * M02 — Os meus animais (UC08; RF018). Obtém os animais do Cliente (EP-019) e indica
 * [EstadoCarregamento.Vazio] quando a lista é vazia (UC08, A1).
 */
class AnimaisViewModel(private val service: AnimaisService) : ViewModel() {
    private val estadoInterno =
        MutableStateFlow<EstadoCarregamento<List<MeuAnimalListItemDto>>>(EstadoCarregamento.Inicial)
    val estado: StateFlow<EstadoCarregamento<List<MeuAnimalListItemDto>>> =
        estadoInterno.asStateFlow()

    private var pedido: Job? = null

    init {
        carregar()
    }

    /** Pede a lista à API; também é a ação "Tentar novamente". Ignora pedidos repetidos. */
    fun carregar() {
        if (pedido?.isActive == true) return
        estadoInterno.value = EstadoCarregamento.ACarregar
        pedido =
            viewModelScope.launch {
                estadoInterno.value =
                    carregarEstado(semDados = List<MeuAnimalListItemDto>::isEmpty) {
                        service.listar()
                    }
            }
    }
}
