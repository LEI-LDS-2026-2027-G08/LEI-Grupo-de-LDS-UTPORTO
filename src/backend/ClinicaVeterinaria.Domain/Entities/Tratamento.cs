namespace ClinicaVeterinaria.Domain.Entities;

public class Tratamento
{
    public int Id { get; set; }
    public int RegistoClinicoId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateOnly DataAplicacao { get; set; }
}
