using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Vacinacao;

namespace ClinicaVeterinaria.Application.Models.Vacinacoes;

public class DadosAdministracao
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "O tipo é obrigatório.")]
    [RegularExpression(TiposAdministracao.ExpressaoValida, ErrorMessage = TiposAdministracao.MensagemTipoInvalido)]
    public string Tipo { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "O produto é obrigatório.")]
    [MaxLength(150, ErrorMessage = "O produto não pode ter mais de 150 caracteres.")]
    public string Produto { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "O lote é obrigatório.")]
    [MaxLength(50, ErrorMessage = "O lote não pode ter mais de 50 caracteres.")]
    public string Lote { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de administração é obrigatória.")]
    public DateOnly? DataAdministracao { get; set; }

    [Range(1, short.MaxValue, ErrorMessage = "O intervalo (intervaloDias), quando indicado, deve ser superior a 0 dias.")]
    public int? IntervaloDias { get; set; }
}
