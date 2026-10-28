using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.DTOs.Consultas;

/// <summary>
/// Corpo de EP-026 (PUT /consultas/{id}): altera o Veterinário,
/// a data e hora e o motivo de uma marcação.
/// </summary>
public class UpdateConsultaDto
{
    /// <summary>Novo Veterinário atribuído à marcação.</summary>
    [Required(ErrorMessage = "O veterinarioId é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O veterinarioId deve ser superior a 0.")]
    public int? VeterinarioId { get; set; }

    /// <summary>
    /// Nova data e hora da consulta, em UTC.
    /// Tem de ser estritamente futura.
    /// </summary>
    [Required(ErrorMessage = "A data e hora da marcação (dataHora) é obrigatória.")]
    public DateTime? DataHora { get; set; }

    /// <summary>Motivo da consulta (opcional, até 250 caracteres).</summary>
    [MaxLength(250, ErrorMessage = "O motivo não pode ter mais de 250 caracteres.")]
    public string? Motivo { get; set; }
}
