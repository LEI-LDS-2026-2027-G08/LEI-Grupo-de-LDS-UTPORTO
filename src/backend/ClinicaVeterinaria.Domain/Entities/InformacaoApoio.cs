namespace ClinicaVeterinaria.Domain.Entities;

public class InformacaoApoio
{
    public int Id { get; set; }
    public int MarcacaoId { get; set; }
    public int AuxiliarId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataRegisto { get; set; }
}