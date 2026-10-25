namespace ClinicaVeterinaria.Domain.Entities;

public class Prescricao
{
    public int Id { get; set; }
    public int RegistoClinicoId { get; set; }
    public string Medicamento { get; set; } = string.Empty;
    public string Dosagem { get; set; } = string.Empty;
    public string Frequencia { get; set; } = string.Empty;
    public short? DuracaoDias { get; set; }
}