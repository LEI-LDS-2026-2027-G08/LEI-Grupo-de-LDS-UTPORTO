using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.DTOs.Consultas;

/// <summary>Corpo de EP-029 (POST /consultas/{id}/notas-preparacao).</summary>
public class CreateNotaPreparacaoDto
{
    /// <summary>Informação não clínica de apoio; texto só com espaços conta como vazio.</summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "O texto da nota é obrigatório.")]
    [MaxLength(1000, ErrorMessage = "O texto da nota não pode ter mais de 1000 caracteres.")]
    public string? Texto { get; set; }
}
