using ClinicaVeterinaria.Application.DTOs.Comum;

namespace ClinicaVeterinaria.Application.DTOs.Consultas;

/// <summary>
/// Marcação vista pelo Cliente em EP-030 (Modelo de Classes v02, <c>MinhaConsultaDto</c>). Só
/// informação administrativa: não tem motivo, notas de preparação nem nada do registo clínico.
/// </summary>
public class MinhaConsultaDto
{
    public int Id { get; set; }

    /// <summary>Início da marcação, em UTC.</summary>
    public DateTime DataHora { get; set; }

    /// <summary>Pendente, Confirmada, Realizada ou Cancelada.</summary>
    public string Estado { get; set; } = string.Empty;

    public AnimalRefDto Animal { get; set; } = new();

    /// <summary>Nome do Veterinário (texto, como no contrato de EP-030).</summary>
    public string Veterinario { get; set; } = string.Empty;
}
