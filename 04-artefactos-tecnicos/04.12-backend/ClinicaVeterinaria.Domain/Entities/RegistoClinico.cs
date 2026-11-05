namespace ClinicaVeterinaria.Domain.Entities;

public class RegistoClinico
{
    public int Id { get; set; }
    public int MarcacaoId { get; set; }
    public int VeterinarioId { get; set; }
    public string? NotasClinicas { get; set; }
    public string? Diagnostico { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
    public int? AtualizadoPorId { get; set; }
}
