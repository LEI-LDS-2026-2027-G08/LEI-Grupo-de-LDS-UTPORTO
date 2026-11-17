namespace ClinicaVeterinaria.Application.Models.Utilizadores;

public class DetalheUtilizador
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Telefone { get; set; }

    public string Perfil { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public string? MotivoBloqueio { get; set; }

    public bool PodeRegistarApoio { get; set; }

    public DateTime CriadoEm { get; set; }
}
