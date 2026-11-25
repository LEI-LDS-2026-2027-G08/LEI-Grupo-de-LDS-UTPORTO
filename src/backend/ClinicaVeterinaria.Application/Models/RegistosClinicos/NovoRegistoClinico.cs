using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.Models.RegistosClinicos;

public class NovoRegistoClinico : AlteracaoRegistoClinico
{
    [Required(ErrorMessage = "O consultaId é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O consultaId deve ser superior a 0.")]
    public int? ConsultaId { get; set; }
}
