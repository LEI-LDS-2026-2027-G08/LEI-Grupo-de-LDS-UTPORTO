using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.Models.RegistosClinicos;

public class DadosTratamento
{
    [Range(1, int.MaxValue, ErrorMessage = "O id do tratamento deve ser superior a 0.")]
    public int? Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "A descrição do tratamento é obrigatória.")]
    [MaxLength(1000, ErrorMessage = "A descrição do tratamento não pode ter mais de 1000 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de aplicação do tratamento é obrigatória.")]
    public DateOnly? DataAplicacao { get; set; }
}
