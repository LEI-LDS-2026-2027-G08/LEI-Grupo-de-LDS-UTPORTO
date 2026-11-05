using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.DTOs.Veterinarios;

namespace ClinicaVeterinaria.Application.Interfaces;

/// <summary>Veterinários e disponibilidade para marcação (EP-021, EP-022).</summary>
public interface IVeterinariosService
{
    /// <summary>EP-021 — Veterinários com conta ativa, ordenados por nome e, em caso de empate, por identificador.</summary>
    Task<IReadOnlyList<VeterinarioRefDto>> ListarAsync();

    /// <summary>
    /// EP-022 — horários livres do Veterinário em cada dia do período. Período inválido: 400;
    /// identificador que não é de um Veterinário ativo: 404.
    /// </summary>
    Task<DisponibilidadeDto> ObterDisponibilidadeAsync(int veterinarioId, DisponibilidadeQuery query);
}
