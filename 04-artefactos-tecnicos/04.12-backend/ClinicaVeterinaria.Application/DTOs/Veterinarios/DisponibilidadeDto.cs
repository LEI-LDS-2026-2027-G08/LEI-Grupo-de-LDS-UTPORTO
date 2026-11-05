namespace ClinicaVeterinaria.Application.DTOs.Veterinarios;

/// <summary>Resposta de EP-022: horários livres de um Veterinário em cada dia do período.</summary>
public class DisponibilidadeDto
{
    public int VeterinarioId { get; set; }

    /// <summary>Duração de cada consulta e intervalo entre horários (EP-009/EP-010).</summary>
    public int DuracaoConsultaMinutos { get; set; }

    /// <summary>Um elemento por dia do período, pela ordem das datas, incluindo os dias sem horários livres.</summary>
    public IReadOnlyList<DiaDisponivelDto> Dias { get; set; } = [];
}
