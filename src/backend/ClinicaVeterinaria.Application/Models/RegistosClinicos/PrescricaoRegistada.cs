namespace ClinicaVeterinaria.Application.Models.RegistosClinicos;

public class PrescricaoRegistada
{
    public int Id { get; set; }

    public string Medicamento { get; set; } = string.Empty;

    public string Dosagem { get; set; } = string.Empty;

    public string Frequencia { get; set; } = string.Empty;

    public int? DuracaoDias { get; set; }
}
