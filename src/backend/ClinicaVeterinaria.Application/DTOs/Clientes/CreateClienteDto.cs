using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.Application.DTOs.Clientes;

/// <summary>Pedido de EP-013 (POST /clientes): os dados do Cliente e a palavra-passe inicial da conta.</summary>
public class CreateClienteDto : UpdateClienteDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "A palavra-passe inicial é obrigatória.")]
    [PalavraPasseForte]
    public string PasswordInicial { get; set; } = string.Empty;
}
