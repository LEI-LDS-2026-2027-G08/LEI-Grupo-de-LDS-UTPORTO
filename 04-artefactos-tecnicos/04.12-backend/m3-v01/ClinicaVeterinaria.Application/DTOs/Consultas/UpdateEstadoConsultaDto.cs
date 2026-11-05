using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.DTOs.Consultas;

/// <summary>
/// Corpo de EP-027 (PATCH /consultas/{id}/estado):
/// altera o estado de uma marcação.
/// </summary>
public class UpdateEstadoConsultaDto
{
    /// <summary>
    /// Novo estado da marcação: Confirmada, Realizada ou Cancelada.
    /// Pendente não pode ser escolhido manualmente.
    /// </summary>
    [Required(ErrorMessage = "O estado é obrigatório.")]
    public string? Estado { get; set; }

    /// <summary>
    /// Observação opcional associada à alteração de estado.
    /// </summary>
    public string? Observacao { get; set; }
}
