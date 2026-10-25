namespace ClinicaVeterinaria.Domain.Entities;

public class Cliente
{
    public int Id { get; set; }
    public int UtilizadorId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? NIF { get; set; }
    public string Telefone { get; set; } = string.Empty;
    public string? Morada { get; set; }
    public DateTime DataRegisto { get; set; }
}