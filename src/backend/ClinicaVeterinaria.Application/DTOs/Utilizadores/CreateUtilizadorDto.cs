using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.Application.DTOs.Utilizadores;

/// <summary>
/// Pedido de EP-006 (POST /utilizadores). As regras do perfil (Cliente não permitido,
/// podeRegistarApoio só para AuxiliarVeterinario) são verificadas no serviço (422).
/// </summary>
public class CreateUtilizadorDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(150, ErrorMessage = "O nome não pode ter mais de 150 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "O e-mail é obrigatório.")]
    // Regra R-EMAIL (I028): o [EmailAddress] do .NET aceitava, por exemplo, "joao@mail".
    [EmailValido]
    public string Email { get; set; } = string.Empty;

    // 9 a 20 caracteres no total (coluna Funcionario.Telefone NVARCHAR(20)), contando o + e os espaços.
    [RegularExpression(@"^(?=.{9,20}$)\+?[0-9 ]+$",
        ErrorMessage = "O telefone deve ter entre 9 e 20 caracteres: dígitos, espaços e, opcionalmente, o indicativo + no início.")]
    public string? Telefone { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "O perfil é obrigatório.")]
    public string Perfil { get; set; } = string.Empty;

    public bool PodeRegistarApoio { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "A palavra-passe inicial é obrigatória.")]
    [PalavraPasseForte]
    public string PasswordInicial { get; set; } = string.Empty;
}
