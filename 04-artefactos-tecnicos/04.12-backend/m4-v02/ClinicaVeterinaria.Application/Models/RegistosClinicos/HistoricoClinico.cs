using ClinicaVeterinaria.Application.Models.Comum;

namespace ClinicaVeterinaria.Application.Models.RegistosClinicos;

public class HistoricoClinico
{
    public ReferenciaAnimal Animal { get; set; } = new();

    public IReadOnlyList<ResumoRegistoClinico> Registos { get; set; } = [];
}
