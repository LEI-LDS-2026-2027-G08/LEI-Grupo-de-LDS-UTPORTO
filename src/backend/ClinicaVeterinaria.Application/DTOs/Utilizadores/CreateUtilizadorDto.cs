using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.Application.DTOs.Utilizadores;

/// <summary>
/// Pedido de EP-006 (POST /utilizadores): os campos de EP-007 e a palavra-passe inicial.
/// As regras do perfil (Cliente não permitido, podeRegistarApoio só para
/// AuxiliarVeterinario) são verificadas no serviço (422).
/// </summary>
public class CreateUtilizadorDto : UpdateUtilizadorDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "A palavra-passe inicial é obrigatória.")]
    [PalavraPasseForte]
    public string PasswordInicial { get; set; } = string.Empty;
}
