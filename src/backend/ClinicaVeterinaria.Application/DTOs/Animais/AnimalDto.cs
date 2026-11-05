namespace ClinicaVeterinaria.Application.DTOs.Animais;

public class AnimalDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string? Raca { get; set; }
    public string Sexo { get; set; } = string.Empty;
    public DateOnly? DataNascimento { get; set; }
    public int? IdadeAnos { get; set; }
    public decimal? Peso { get; set; }
    public ClienteDetalheDto Cliente { get; set; } = new();
    public DateTime? AtualizadoEm { get; set; }
}

public class ClienteDetalheDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
}
