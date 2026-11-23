namespace ClinicaVeterinaria.Application.Models.RegistosClinicos;

public class ResumoRegistoClinico
{
    public int Id { get; set; }

    public int ConsultaId { get; set; }

    public DateOnly Data { get; set; }

    public string Veterinario { get; set; } = string.Empty;

    public string? Diagnostico { get; set; }

    public int NumeroTratamentos { get; set; }

    public int NumeroPrescricoes { get; set; }
}
