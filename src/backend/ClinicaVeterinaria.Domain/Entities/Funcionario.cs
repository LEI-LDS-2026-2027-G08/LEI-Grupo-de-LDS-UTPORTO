namespace ClinicaVeterinaria.Domain.Entities;

public class Funcionario
{
    public int UtilizadorId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public bool PodeRegistarApoio { get; set; }
}
