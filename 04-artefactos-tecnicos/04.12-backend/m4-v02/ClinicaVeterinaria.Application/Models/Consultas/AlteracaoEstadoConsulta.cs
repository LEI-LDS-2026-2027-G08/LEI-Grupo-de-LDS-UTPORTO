using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.Models.Consultas;

public class AlteracaoEstadoConsulta
{
    [Required(ErrorMessage = "O estado é obrigatório.")]
    public string? Estado { get; set; }

    [MaxLength(500, ErrorMessage = "A observação não pode ter mais de 500 caracteres.")]
    public string? Observacao { get; set; }
}
