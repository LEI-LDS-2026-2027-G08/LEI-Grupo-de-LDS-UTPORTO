namespace ClinicaVeterinaria.Domain.Entities;

public class Marcacao
{
    public int Id { get; set; }
    public int PacienteId { get; set; }
    public int VeterinarioId { get; set; }
    public DateTime DataHoraInicio { get; set; }
    public short DuracaoMinutos { get; set; }
    public string? Motivo { get; set; }
    public string Estado { get; set; } = "Pendente";
    public DateTime? DataHoraChegada { get; set; }
    public int CriadaPorId { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataUltimaAlteracao { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

}
