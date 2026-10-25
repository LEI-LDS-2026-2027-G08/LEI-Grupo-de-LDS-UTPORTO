namespace ClinicaVeterinaria.Domain.Entities;

public class Raca
{
    public int Id { get; set; }
    public int EspecieId { get; set; }
    public string Nome { get; set; } = string.Empty;
}