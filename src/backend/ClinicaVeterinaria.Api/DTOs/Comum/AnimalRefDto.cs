namespace ClinicaVeterinaria.Api.DTOs.Comum;

public class AnimalRefDto
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Especie { get; set; }
}
