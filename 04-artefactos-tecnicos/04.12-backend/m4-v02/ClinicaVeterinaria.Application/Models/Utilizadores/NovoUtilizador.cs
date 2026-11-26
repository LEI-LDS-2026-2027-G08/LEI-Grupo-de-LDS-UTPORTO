namespace ClinicaVeterinaria.Application.Models.Utilizadores;

public class NovoUtilizador : AlteracaoUtilizador
{
    public string PasswordInicial { get; set; } = string.Empty;
}
