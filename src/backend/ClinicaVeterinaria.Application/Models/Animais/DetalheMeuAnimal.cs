namespace ClinicaVeterinaria.Application.Models.Animais;

public class DetalheMeuAnimal
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Especie { get; set; } = string.Empty;

    public string? Raca { get; set; }

    public string Sexo { get; set; } = string.Empty;

    public DateOnly? DataNascimento { get; set; }

    public int? IdadeAnos { get; set; }

    public decimal? Peso { get; set; }
}
