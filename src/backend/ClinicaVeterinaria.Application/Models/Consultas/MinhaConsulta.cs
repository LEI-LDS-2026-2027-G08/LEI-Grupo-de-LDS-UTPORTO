using ClinicaVeterinaria.Application.Models.Comum;

namespace ClinicaVeterinaria.Application.Models.Consultas;

public class MinhaConsulta
{
    public int Id { get; set; }

    public DateTime DataHora { get; set; }

    public string Estado { get; set; } = string.Empty;

    public ReferenciaAnimal Animal { get; set; } = new();

    public string Veterinario { get; set; } = string.Empty;
}
