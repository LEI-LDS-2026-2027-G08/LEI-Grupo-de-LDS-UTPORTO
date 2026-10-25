namespace ClinicaVeterinaria.Domain.Entities;

public class Utilizador
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Perfil { get; set; } = string.Empty;
    public string Estado { get; set; } = "Ativo";
    public DateTime DataCriacao { get; set; }
    public DateTime? DataUltimoAcesso { get; set; }
}