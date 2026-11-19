package pt.ipp.estg.g08.clinica.features.consultas

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.Job
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.ipp.estg.g08.clinica.features.consultas.dto.MinhaConsultaDto
import pt.ipp.estg.g08.clinica.features.consultas.dto.TipoConsultaFiltro
import pt.ipp.estg.g08.clinica.shared.EstadoCarregamento
import pt.ipp.estg.g08.clinica.shared.carregarEstado

/**
 * M04 — Consultas futuras e histórico administrativo (UC11; RF028, RF029). Obtém as duas listas
 * de EP-030 (`tipo=futuras` e `tipo=historico`), de todos os animais do Cliente ou só do animal
 * [animalId] quando o ecrã é aberto a partir de M03.
 *
 * Cada lista tem o seu estado: uma lista vazia dá [EstadoCarregamento.Vazio] e um erro numa delas
 * não esconde a outra. Um `animalId` de outro Cliente (`403`, UC11 E1) dá
 * [EstadoCarregamento.Erro] sem qualquer dado.
 */
class ConsultasViewModel(private val service: ConsultasService, val animalId: Int? = null) :
    ViewModel() {
    private val estados =
        TipoConsultaFiltro.entries.associateWith {
            MutableStateFlow<EstadoCarregamento<List<MinhaConsultaDto>>>(EstadoCarregamento.Inicial)
        }
    private val pedidos = mutableMapOf<TipoConsultaFiltro, Job>()

    val estadoFuturas: StateFlow<EstadoCarregamento<List<MinhaConsultaDto>>> =
        estados.getValue(TipoConsultaFiltro.Futuras).asStateFlow()
    val estadoHistorico: StateFlow<EstadoCarregamento<List<MinhaConsultaDto>>> =
        estados.getValue(TipoConsultaFiltro.Historico).asStateFlow()

    init {
        carregar()
    }

    /** Pede as Consultas futuras e o histórico à API. */
    fun carregar() {
        TipoConsultaFiltro.entries.forEach(::carregar)
    }

    /** Pede uma das listas; também é a ação "Tentar novamente". Ignora pedidos repetidos. */
    fun carregar(tipo: TipoConsultaFiltro) {
        if (pedidos[tipo]?.isActive == true) return
        val estado = estados.getValue(tipo)
        estado.value = EstadoCarregamento.ACarregar
        pedidos[tipo] =
            viewModelScope.launch {
                estado.value =
                    carregarEstado(semDados = List<MinhaConsultaDto>::isEmpty) {
                        service.listar(tipo, animalId)
                    }
            }
    }
}
