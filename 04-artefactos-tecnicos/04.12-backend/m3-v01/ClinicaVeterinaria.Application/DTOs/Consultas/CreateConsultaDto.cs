using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.DTOs.Consultas;

/// <summary>
/// Corpo de EP-025 (POST /consultas): cria uma marcação no estado <c>Pendente</c> para um animal,
/// um Veterinário e uma data e hora (Documentação de API v02, EP-025).
/// </summary>
/// <remarks>
/// Os identificadores e a data são anuláveis só para distinguir um campo em falta, que tem de dar
/// 400, de um valor por omissão (0 ou 0001-01-01). A duração não vem no pedido: é a configuração
/// <c>DuracaoPadraoConsultaMinutos</c> gravada em EP-010 (30 minutos por omissão).
/// </remarks>
public class CreateConsultaDto
{
    /// <summary>Animal (paciente) da marcação.</summary>
    [Required(ErrorMessage = "O animalId é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O animalId deve ser superior a 0.")]
    public int? AnimalId { get; set; }

    /// <summary>Veterinário a quem a marcação fica atribuída.</summary>
    [Required(ErrorMessage = "O veterinarioId é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O veterinarioId deve ser superior a 0.")]
    public int? VeterinarioId { get; set; }

    /// <summary>
    /// Início da consulta, em UTC (por exemplo, <c>2026-11-25T10:00:00Z</c>). Uma data sem
    /// indicação de fuso é lida como UTC. Tem de ser estritamente futura.
    /// </summary>
    [Required(ErrorMessage = "A data e hora da marcação (dataHora) é obrigatória.")]
    public DateTime? DataHora { get; set; }

    /// <summary>Motivo da consulta (opcional, até 250 caracteres).</summary>
    [MaxLength(250, ErrorMessage = "O motivo não pode ter mais de 250 caracteres.")]
    public string? Motivo { get; set; }
}
