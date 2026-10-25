namespace ClinicaVeterinaria.Application.DTOs.Auth;

public class UtilizadorAutenticadoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Perfil { get; set; } = string.Empty;
    public int? ClienteId { get; set; }
}