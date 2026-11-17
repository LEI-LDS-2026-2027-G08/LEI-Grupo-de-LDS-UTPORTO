using ClinicaVeterinaria.Application.Models.Comum;

namespace ClinicaVeterinaria.Application.Models.Consultas;

public class DetalheConsulta
{
    public int Id { get; set; }

    public DateTime DataHora { get; set; }

    public int DuracaoMinutos { get; set; }

    public string Estado { get; set; } = string.Empty;

    public string? Motivo { get; set; }

    public string? Observacao { get; set; }

    public ReferenciaAnimal Animal { get; set; } = new();

    public ReferenciaCliente Cliente { get; set; } = new();

    public ReferenciaVeterinario Veterinario { get; set; } = new();

    public DateTime? ChegadaEm { get; set; }

    public IReadOnlyList<NotaPreparacao> NotasPreparacao { get; set; } = [];
}
