using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.DTOs.RegistosClinicos;

/// <summary>
/// Prescrição enviada em EP-031 e EP-032. Medicamento, dosagem e frequência são obrigatórios;
/// a duração é opcional e, quando indicada, superior a 0 (<c>CK_Prescricao_Duracao</c>).
/// </summary>
public class CreatePrescricaoDto
{
    /// <summary>Só em EP-032: prescrição já existente neste registo, a manter e atualizar.</summary>
    [Range(1, int.MaxValue, ErrorMessage = "O id da prescrição deve ser superior a 0.")]
    public int? Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "O medicamento da prescrição é obrigatório.")]
    [MaxLength(150, ErrorMessage = "O medicamento não pode ter mais de 150 caracteres.")]
    public string Medicamento { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "A dosagem da prescrição é obrigatória.")]
    [MaxLength(100, ErrorMessage = "A dosagem não pode ter mais de 100 caracteres.")]
    public string Dosagem { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "A frequência da prescrição é obrigatória.")]
    [MaxLength(100, ErrorMessage = "A frequência não pode ter mais de 100 caracteres.")]
    public string Frequencia { get; set; } = string.Empty;

    /// <summary>Duração em dias (opcional). A coluna é <c>smallint</c>: de 1 a 32767.</summary>
    [Range(1, short.MaxValue, ErrorMessage = "A duração da prescrição, quando indicada, deve ser superior a 0 dias.")]
    public int? DuracaoDias { get; set; }
}
