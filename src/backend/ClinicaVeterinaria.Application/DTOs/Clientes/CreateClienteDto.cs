using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.DTOs.Clientes;

/// <summary>Pedido de EP-013 (POST /clientes): os dados do Cliente e a palavra-passe inicial da conta.</summary>
public class CreateClienteDto : UpdateClienteDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "A palavra-passe inicial é obrigatória.")]
    [StringLength(128, MinimumLength = 8,
        ErrorMessage = "A palavra-passe inicial deve ter entre 8 e 128 caracteres.")]
    public string PasswordInicial { get; set; } = string.Empty;
}
