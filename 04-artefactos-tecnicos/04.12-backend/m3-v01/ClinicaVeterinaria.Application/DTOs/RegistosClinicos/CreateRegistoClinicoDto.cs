using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.DTOs.RegistosClinicos;

/// <summary>
/// Corpo de EP-031 (POST /registos-clinicos): registo clínico de uma consulta, com notas,
/// diagnóstico, tratamentos e prescrições. Tem de haver pelo menos um elemento clínico.
/// </summary>
/// <remarks>
/// <c>ConsultaId</c> é anulável só para distinguir um campo em falta, que tem de dar 400, de 0.
/// </remarks>
public class CreateRegistoClinicoDto : UpdateRegistoClinicoDto
{
    [Required(ErrorMessage = "O consultaId é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O consultaId deve ser superior a 0.")]
    public int? ConsultaId { get; set; }
}
