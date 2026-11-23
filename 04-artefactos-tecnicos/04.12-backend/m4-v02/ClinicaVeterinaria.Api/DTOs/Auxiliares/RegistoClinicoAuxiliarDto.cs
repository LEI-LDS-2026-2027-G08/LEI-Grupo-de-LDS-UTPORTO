namespace ClinicaVeterinaria.Api.DTOs.RegistosClinicos;

public class RegistoClinicoAuxiliarDto
{
    public int Id { get; set; }

    public int ConsultaId { get; set; }

    public int AnimalId { get; set; }

    public string Veterinario { get; set; } = string.Empty;

    public DateTime CriadoEm { get; set; }

    public IReadOnlyList<TratamentoDto> Tratamentos { get; set; } = [];

    public IReadOnlyList<PrescricaoDto> Prescricoes { get; set; } = [];

    public DateTime? AtualizadoEm { get; set; }

    public string? AtualizadoPor { get; set; }
}
