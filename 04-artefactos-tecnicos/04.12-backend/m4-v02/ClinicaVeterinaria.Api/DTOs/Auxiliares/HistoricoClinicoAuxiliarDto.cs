using ClinicaVeterinaria.Api.DTOs.Comum;

namespace ClinicaVeterinaria.Api.DTOs.RegistosClinicos;

/// <summary>Histórico autorizado ao Auxiliar, sem diagnóstico nem notas clínicas.</summary>
public class HistoricoClinicoAuxiliarDto
{
    public AnimalRefDto Animal { get; set; } = new();

    public IReadOnlyList<RegistoClinicoResumoAuxiliarDto> Registos { get; set; } = [];
}

public class RegistoClinicoResumoAuxiliarDto
{
    public int Id { get; set; }

    public int ConsultaId { get; set; }

    public DateOnly Data { get; set; }

    public string Veterinario { get; set; } = string.Empty;

    public int NumeroTratamentos { get; set; }

    public int NumeroPrescricoes { get; set; }
}
