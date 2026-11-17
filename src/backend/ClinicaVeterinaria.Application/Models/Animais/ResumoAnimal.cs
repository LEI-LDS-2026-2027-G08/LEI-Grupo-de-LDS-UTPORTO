namespace ClinicaVeterinaria.Application.Models.Animais;

public class ResumoAnimal
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Especie { get; set; } = string.Empty;

    public string? Raca { get; set; }

    public int? IdadeAnos { get; set; }

    public decimal? Peso { get; set; }

    public DonoAnimal Cliente { get; set; } = new();
}

public class DonoAnimal
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;
}
