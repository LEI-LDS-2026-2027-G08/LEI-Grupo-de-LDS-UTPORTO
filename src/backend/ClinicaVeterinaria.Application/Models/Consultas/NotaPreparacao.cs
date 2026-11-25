namespace ClinicaVeterinaria.Application.Models.Consultas;

public class NotaPreparacao
{
    public int Id { get; set; }

    public int? ConsultaId { get; set; }

    public string Texto { get; set; } = string.Empty;

    public string Autor { get; set; } = string.Empty;

    public DateTime CriadaEm { get; set; }
}
