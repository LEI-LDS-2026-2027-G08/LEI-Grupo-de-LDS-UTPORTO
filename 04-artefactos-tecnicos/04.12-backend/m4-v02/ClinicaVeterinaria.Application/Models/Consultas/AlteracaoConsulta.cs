using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.Models.Consultas;

public class AlteracaoConsulta
{
    [Required(ErrorMessage = "O veterinarioId é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O veterinarioId deve ser superior a 0.")]
    public int? VeterinarioId { get; set; }

    [Required(ErrorMessage = "A data e hora da marcação (dataHora) é obrigatória.")]
    public DateTime? DataHora { get; set; }

    [MaxLength(250, ErrorMessage = "O motivo não pode ter mais de 250 caracteres.")]
    public string? Motivo { get; set; }
}
