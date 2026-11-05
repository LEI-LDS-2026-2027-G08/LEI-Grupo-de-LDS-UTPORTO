namespace ClinicaVeterinaria.Domain.Entities;

public class Clinica
{
    public byte Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string NIF { get; set; } = string.Empty;
    public string Morada { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DataUltimaAlteracao { get; set; }
}
