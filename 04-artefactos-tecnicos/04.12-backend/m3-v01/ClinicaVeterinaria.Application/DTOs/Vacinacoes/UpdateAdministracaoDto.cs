using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Vacinacao;

namespace ClinicaVeterinaria.Application.DTOs.Vacinacoes;

/// <summary>
/// Corpo da atualização de uma vacinação ou desparasitação (RF056; I082, critério 4) e base do
/// pedido de EP-043. Os tamanhos são os das colunas de <c>RegistoProfilaxia</c>.
/// </summary>
/// <remarks>
/// A data de administração não pode ser futura (hoje em hora de Lisboa); essa regra depende do
/// relógio e é verificada no serviço. <c>dataProximaPrevista</c> nunca vem do pedido: é sempre
/// calculada pelo servidor (RF045).
/// </remarks>
public class UpdateAdministracaoDto
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

    /// <summary>Data de administração (AAAA-MM-DD), hoje ou no passado.</summary>
    [Required(ErrorMessage = "A data de administração é obrigatória.")]
    public DateOnly? DataAdministracao { get; set; }

    /// <summary>
    /// Intervalo até à próxima administração, em dias (opcional). A coluna é <c>smallint</c> com
    /// <c>CK_Profilaxia_Intervalo</c>: de 1 a 32767.
    /// </summary>
    [Range(1, short.MaxValue, ErrorMessage = "O intervalo (intervaloDias), quando indicado, deve ser superior a 0 dias.")]
    public int? IntervaloDias { get; set; }
}
