using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.Application.DTOs.Clientes;

/// <summary>Pedido de EP-014 (PUT /clientes/{id}); base do pedido de EP-013.</summary>
public class UpdateClienteDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(150, ErrorMessage = "O nome não pode ter mais de 150 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [NifValido]
    public string? Nif { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "O telefone é obrigatório.")]
    [RegularExpression(@"^\+?[0-9 ]{9,20}$",
        ErrorMessage = "O telefone deve ter entre 9 e 20 dígitos, opcionalmente com o indicativo +.")]
    public string Telefone { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail é inválido.")]
    [MaxLength(256, ErrorMessage = "O e-mail não pode ter mais de 256 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [MaxLength(250, ErrorMessage = "A morada não pode ter mais de 250 caracteres.")]
    public string? Morada { get; set; }
}
