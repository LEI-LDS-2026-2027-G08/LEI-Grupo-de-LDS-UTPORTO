namespace ClinicaVeterinaria.Application.DTOs.Animais;

public class AnimalListItemDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string? Raca { get; set; }
    public int? IdadeAnos { get; set; }
    public decimal? Peso { get; set; }
    public ClienteResumoDto Cliente { get; set; } = new();
}

public class ClienteResumoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}
