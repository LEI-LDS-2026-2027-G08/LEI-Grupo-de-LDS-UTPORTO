using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.Models.Consultas;

public class NovaNotaPreparacao
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "O texto da nota é obrigatório.")]
    [MaxLength(1000, ErrorMessage = "O texto da nota não pode ter mais de 1000 caracteres.")]
    public string? Texto { get; set; }
}
