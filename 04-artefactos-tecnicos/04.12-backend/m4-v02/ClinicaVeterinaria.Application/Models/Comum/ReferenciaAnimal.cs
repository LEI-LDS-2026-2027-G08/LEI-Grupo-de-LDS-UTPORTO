namespace ClinicaVeterinaria.Application.Models.Comum;

public class ReferenciaAnimal
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Especie { get; set; }
}
