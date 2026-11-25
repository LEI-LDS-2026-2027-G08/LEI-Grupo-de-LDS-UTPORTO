namespace ClinicaVeterinaria.Application.Models.Auth;

public class SessaoIniciada
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiraEm { get; set; }

    public IdentidadeSessao Utilizador { get; set; } = new();
}
