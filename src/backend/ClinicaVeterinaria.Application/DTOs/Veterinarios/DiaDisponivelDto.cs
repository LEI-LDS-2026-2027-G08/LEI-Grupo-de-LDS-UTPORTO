namespace ClinicaVeterinaria.Application.DTOs.Veterinarios;

/// <summary>Horários livres de um dia (EP-022).</summary>
public class DiaDisponivelDto
{
    /// <summary>Dia, no formato AAAA-MM-DD.</summary>
    public DateOnly Data { get; set; }

    /// <summary>Inícios livres em hora local da clínica (Lisboa), no formato HH:mm, por ordem crescente.</summary>
    public IReadOnlyList<string> HorariosLivres { get; set; } = [];
}
