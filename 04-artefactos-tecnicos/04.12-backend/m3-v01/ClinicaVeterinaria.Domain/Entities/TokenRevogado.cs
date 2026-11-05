namespace ClinicaVeterinaria.Domain.Entities;

public class TokenRevogado
{
    public string Jti { get; set; } = string.Empty;
    public int UtilizadorId { get; set; }
    public DateTime DataExpiracao { get; set; }
    public DateTime DataRevogacao { get; set; }
}
