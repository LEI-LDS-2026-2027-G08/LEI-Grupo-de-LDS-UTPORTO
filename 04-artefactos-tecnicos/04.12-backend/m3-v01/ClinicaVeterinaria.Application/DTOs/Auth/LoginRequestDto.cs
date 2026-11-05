using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.DTOs.Auth;

public class LoginRequestDto
{
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A palavra-passe é obrigatória.")]
    public string Password { get; set; } = string.Empty;
}
