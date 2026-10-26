using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.Application.DTOs.Clinica;

/// <summary>
/// Pedido de EP-010 (PUT /clinica). O horário enviado substitui o anterior;
/// as regras dos períodos são verificadas por <see cref="ValidadorHorario"/>.
/// </summary>
public class UpdateClinicaDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(150, ErrorMessage = "O nome não pode ter mais de 150 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "O NIF é obrigatório.")]
    [NifValido]
    public string Nif { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "A morada é obrigatória.")]
    [MaxLength(250, ErrorMessage = "A morada não pode ter mais de 250 caracteres.")]
    public string Morada { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "O telefone é obrigatório.")]
    // 9 a 20 caracteres no total (coluna Clinica.Telefone NVARCHAR(20)), contando o + e os espaços.
    [RegularExpression(@"^(?=.{9,20}$)\+?[0-9 ]+$",
        ErrorMessage = "O telefone deve ter entre 9 e 20 caracteres: dígitos, espaços e, opcionalmente, o indicativo + no início.")]
    public string Telefone { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail é inválido.")]
    [MaxLength(256, ErrorMessage = "O e-mail não pode ter mais de 256 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Range(5, 240, ErrorMessage = "A duração da consulta deve estar entre 5 e 240 minutos.")]
    public int DuracaoConsultaMinutos { get; set; }

    [Required(ErrorMessage = "O horário de funcionamento é obrigatório (lista vazia: clínica sempre fechada).")]
    public List<PeriodoHorarioDto>? HorarioFuncionamento { get; set; }
}
