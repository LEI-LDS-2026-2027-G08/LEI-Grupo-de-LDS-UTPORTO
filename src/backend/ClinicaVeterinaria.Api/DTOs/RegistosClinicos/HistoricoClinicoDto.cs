using ClinicaVeterinaria.Api.DTOs.Comum;

namespace ClinicaVeterinaria.Api.DTOs.RegistosClinicos;

/// <summary>
/// Histórico clínico de um paciente (resposta de EP-034): o animal e os seus registos clínicos,
/// do mais recente para o mais antigo. Um paciente sem registos tem a lista vazia.
/// </summary>
public class HistoricoClinicoDto
{
    public AnimalRefDto Animal { get; set; } = new();

    public IReadOnlyList<RegistoClinicoResumoDto> Registos { get; set; } = [];
}
