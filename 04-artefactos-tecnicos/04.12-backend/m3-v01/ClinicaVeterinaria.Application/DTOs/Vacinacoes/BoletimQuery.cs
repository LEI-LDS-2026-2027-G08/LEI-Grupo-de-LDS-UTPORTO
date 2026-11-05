using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Vacinacao;

namespace ClinicaVeterinaria.Application.DTOs.Vacinacoes;

/// <summary>
/// Parâmetros de EP-042 (GET /animais/{id}/vacinacoes). <c>tipo</c> é opcional e filtra as
/// administrações e as próximas previstas; um valor fora de <c>TipoAdministracao</c> dá 400.
/// </summary>
public class BoletimQuery
{
    /// <summary>Vacina ou Desparasitacao (opcional).</summary>
    [RegularExpression(TiposAdministracao.ExpressaoValida, ErrorMessage = TiposAdministracao.MensagemTipoInvalido)]
    public string? Tipo { get; set; }
}
