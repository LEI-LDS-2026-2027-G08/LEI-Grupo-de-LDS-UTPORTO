using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.DTOs.Utilizadores;

/// <summary>
/// Pedido de EP-008 (PATCH /utilizadores/{id}/estado). Só é aceite o estado
/// <c>Bloqueado</c>; outro valor dá 400 no serviço.
/// </summary>
public class AlterarEstadoUtilizadorDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "O estado é obrigatório.")]
    public string Estado { get; set; } = string.Empty;

    /// <summary>
    /// Motivo do bloqueio indicado pelo Administrador. O Modelo de Dados não tem coluna
    /// para o guardar, por isso não é persistido (divergência registada no README).
    /// </summary>
    [MaxLength(500, ErrorMessage = "O motivo não pode ter mais de 500 caracteres.")]
    public string? Motivo { get; set; }
}
