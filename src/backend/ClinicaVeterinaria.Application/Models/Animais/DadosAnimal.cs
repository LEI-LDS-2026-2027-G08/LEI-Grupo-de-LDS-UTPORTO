namespace ClinicaVeterinaria.Application.Models.Animais;

public class DadosAnimal
{
    public string Nome { get; set; } = string.Empty;

    public string Especie { get; set; } = string.Empty;

    public string? Raca { get; set; }

    public string Sexo { get; set; } = "Desconhecido";

    public DateOnly? DataNascimento { get; set; }

    public decimal? Peso { get; set; }

    public int ClienteId { get; set; }
}
