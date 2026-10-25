public class Paciente
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int EspecieId { get; set; }
    public int RacaId { get; set; }
    public string Sexo { get; set; } = "Desconhecido";
    public DateOnly? DataNascimento { get; set; }
    public decimal? Peso { get; set; }
    public DateTime DataRegisto { get; set; }
    public DateTime? DataUltimaAlteracao { get; set; }
}