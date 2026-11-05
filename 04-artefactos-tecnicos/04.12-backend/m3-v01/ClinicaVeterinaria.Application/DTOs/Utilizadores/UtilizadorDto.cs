namespace ClinicaVeterinaria.Application.DTOs.Utilizadores;

/// <summary>Conta de funcionário (EP-005). Nunca inclui o hash da palavra-passe.</summary>
public class UtilizadorDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string Perfil { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public bool PodeRegistarApoio { get; set; }

    /// <summary>Data de criação da conta, em UTC.</summary>
    public DateTime CriadoEm { get; set; }
}
