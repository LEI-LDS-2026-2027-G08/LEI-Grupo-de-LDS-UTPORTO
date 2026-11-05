using ClinicaVeterinaria.Application.DTOs.Consultas;

namespace ClinicaVeterinaria.Application.Interfaces;

/// <summary>Marcações do Cliente autenticado na aplicação móvel (EP-030; RF028, RF029).</summary>
public interface IMinhasConsultasService
{
    /// <summary>
    /// EP-030 — marcações futuras (por data crescente) ou histórico (por data decrescente) dos animais
    /// do Cliente, só com dados administrativos. Tipo inválido ou animalId inferior a 1: 400;
    /// animalId que não é de um animal do Cliente: 403.
    /// </summary>
    /// <param name="clienteId">Cliente do token (nunca do pedido, RNF002).</param>
    /// <param name="query">Tipo e filtro opcional por animal.</param>
    Task<IReadOnlyList<MinhaConsultaDto>> ListarAsync(int clienteId, MinhasConsultasQuery query);
}
