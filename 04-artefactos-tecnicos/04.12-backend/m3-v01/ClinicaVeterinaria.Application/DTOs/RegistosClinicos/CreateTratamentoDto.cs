using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.DTOs.RegistosClinicos;

/// <summary>
/// Tratamento enviado em EP-031 e EP-032. Em EP-032, um tratamento que já existe mantém-se
/// indicando o respetivo <see cref="Id"/>; sem <see cref="Id"/>, é acrescentado.
/// </summary>
public class CreateTratamentoDto
{
    /// <summary>Só em EP-032: tratamento já existente neste registo, a manter e atualizar.</summary>
    [Range(1, int.MaxValue, ErrorMessage = "O id do tratamento deve ser superior a 0.")]
    public int? Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "A descrição do tratamento é obrigatória.")]
    [MaxLength(1000, ErrorMessage = "A descrição do tratamento não pode ter mais de 1000 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de aplicação do tratamento é obrigatória.")]
    public DateOnly? DataAplicacao { get; set; }
}
