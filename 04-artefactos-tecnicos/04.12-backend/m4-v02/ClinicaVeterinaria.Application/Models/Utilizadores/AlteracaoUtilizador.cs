namespace ClinicaVeterinaria.Application.Models.Utilizadores;

public class AlteracaoUtilizador
{
    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Telefone { get; set; }

    public string Perfil { get; set; } = string.Empty;

    public bool PodeRegistarApoio { get; set; }
}
