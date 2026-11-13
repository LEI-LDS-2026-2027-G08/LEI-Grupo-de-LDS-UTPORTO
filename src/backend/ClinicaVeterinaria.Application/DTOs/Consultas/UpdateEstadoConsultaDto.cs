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
    /// Observação opcional da alteração de estado. Fica guardada em Marcacao.Observacao
    /// e é substituída na alteração de estado seguinte.
    /// </summary>
    [MaxLength(500, ErrorMessage = "A observação não pode ter mais de 500 caracteres.")]
    public string? Observacao { get; set; }
}
