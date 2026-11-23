namespace ClinicaVeterinaria.Application.Models.RegistosClinicos;

public class DetalheRegistoClinico
{
    public int Id { get; set; }

    public int ConsultaId { get; set; }

    public int AnimalId { get; set; }

    public string Veterinario { get; set; } = string.Empty;

    public DateTime CriadoEm { get; set; }

    public string? NotasClinicas { get; set; }

    public string? Diagnostico { get; set; }

    public IReadOnlyList<TratamentoRegistado> Tratamentos { get; set; } = [];

    public IReadOnlyList<PrescricaoRegistada> Prescricoes { get; set; } = [];

    public DateTime? AtualizadoEm { get; set; }

    public string? AtualizadoPor { get; set; }
}
