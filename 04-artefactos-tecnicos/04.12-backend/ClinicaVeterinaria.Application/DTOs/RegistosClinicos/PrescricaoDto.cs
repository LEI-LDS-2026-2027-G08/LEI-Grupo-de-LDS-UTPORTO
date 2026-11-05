namespace ClinicaVeterinaria.Application.DTOs.RegistosClinicos;

/// <summary>Prescrição de um registo clínico (resposta de EP-031 e EP-032).</summary>
public class PrescricaoDto
{
    public int Id { get; set; }

    public string Medicamento { get; set; } = string.Empty;

    public string Dosagem { get; set; } = string.Empty;

    public string Frequencia { get; set; } = string.Empty;

    public int? DuracaoDias { get; set; }
}
