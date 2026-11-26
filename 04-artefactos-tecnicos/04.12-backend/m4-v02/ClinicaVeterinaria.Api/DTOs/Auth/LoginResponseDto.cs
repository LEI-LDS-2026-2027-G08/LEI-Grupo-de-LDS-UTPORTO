namespace ClinicaVeterinaria.Api.DTOs.Auth;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiraEm { get; set; }

    public UtilizadorResumoDto Utilizador { get; set; } = new();
}
