namespace ClinicaVeterinaria.Domain.Entities;

public class RegistoProfilaxia
{
    public int Id { get; set; }
    public int PacienteId { get; set; }
    public int VeterinarioId { get; set; }
    public int? MarcacaoId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Produto { get; set; } = string.Empty;
    public string Lote { get; set; } = string.Empty;
    public DateOnly DataAdministracao { get; set; }
    public short? IntervaloDias { get; set; }
    public DateOnly? DataProximaAdministracao { get; set; }
    public DateTime DataRegisto { get; set; }
}