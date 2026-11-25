package pt.ipp.estg.g08.clinica.features.boletim

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.Job
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.ipp.estg.g08.clinica.features.boletim.dto.MeuBoletimDto
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento
import pt.ipp.estg.g08.clinica.shared.carregarEstado

/**
 * M06 — Boletim de vacinação e desparasitação (UC17; RF047). Obtém o boletim do animal
 * [animalId] (EP-044): histórico e próximas administrações previstas.
 *
 * Quando as duas listas vêm vazias, o estado é [EstadoCarregamento.Vazio] (UC17, A1). Um animal
 * de outro Cliente (`403`, UC17 E1) dá [EstadoCarregamento.Erro] sem qualquer dado.
 */
class BoletimViewModel(private val service: BoletimService, val animalId: Int) : ViewModel() {
    private val estadoInterno =
        MutableStateFlow<EstadoCarregamento<MeuBoletimDto>>(EstadoCarregamento.Inicial)
    val estado: StateFlow<EstadoCarregamento<MeuBoletimDto>> = estadoInterno.asStateFlow()

    private var pedido: Job? = null

    init {
        carregar()
    }

    /** Pede o boletim à API; também é a ação "Tentar novamente". Ignora pedidos repetidos. */
    fun carregar() {
        if (pedido?.isActive == true) return
        estadoInterno.value = EstadoCarregamento.ACarregar
        pedido =
            viewModelScope.launch {
                estadoInterno.value =
                    carregarEstado(semDados = ::semRegistos) { service.obter(animalId) }
            }
    }

    private fun semRegistos(boletim: MeuBoletimDto): Boolean =
        boletim.historico.isEmpty() && boletim.proximas.isEmpty()
}
