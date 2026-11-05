using ClinicaVeterinaria.Application.DTOs.Comum;

namespace ClinicaVeterinaria.Application.DTOs.Vacinacoes;

/// <summary>
/// Boletim do paciente no web (resposta de EP-042): o animal, todas as administrações (da mais
/// recente para a mais antiga) e as próximas previstas a partir de hoje (da mais próxima para a
/// mais distante). Sem registos, as duas listas vêm vazias.
/// </summary>
public class BoletimDto
{
    public AnimalRefDto Animal { get; set; } = new();

    public IReadOnlyList<AdministracaoDto> Administracoes { get; set; } = [];

    public IReadOnlyList<ProximaAdministracaoDto> Proximas { get; set; } = [];
}
