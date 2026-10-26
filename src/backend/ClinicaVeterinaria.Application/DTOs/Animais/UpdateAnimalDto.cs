using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.DTOs.Animais;

/// <summary>Pedido de EP-018 (PUT /animais/{id}); base do pedido de EP-017.</summary>
public class UpdateAnimalDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(80, ErrorMessage = "O nome não pode ter mais de 80 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "A espécie é obrigatória.")]
    [MaxLength(50, ErrorMessage = "A espécie não pode ter mais de 50 caracteres.")]
    public string Especie { get; set; } = string.Empty;

    [MaxLength(80, ErrorMessage = "A raça não pode ter mais de 80 caracteres.")]
    public string? Raca { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "O sexo é obrigatório.")]
    [RegularExpression("^(Macho|Femea|Desconhecido)$",
        ErrorMessage = "O sexo deve ser Macho, Femea ou Desconhecido.")]
    public string Sexo { get; set; } = "Desconhecido";

    public DateOnly? DataNascimento { get; set; }

[Range(0.01, 9999.99,
    ErrorMessage = "O peso deve ser superior a 0.")]
public decimal? Peso { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O clienteId deve ser superior a 0.")]
    public int ClienteId { get; set; }
}
