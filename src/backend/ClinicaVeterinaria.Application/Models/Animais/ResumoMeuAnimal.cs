namespace ClinicaVeterinaria.Application.Models.Animais;

public class ResumoMeuAnimal
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Especie { get; set; } = string.Empty;

    public string? Raca { get; set; }

    public int? IdadeAnos { get; set; }

    public decimal? Peso { get; set; }
}
