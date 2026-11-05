using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.DTOs.Animais;

/// <summary>Parâmetros de EP-015 (GET /animais).</summary>
public class AnimalFiltroQuery
{
    /// <summary>Filtra os pacientes pelo proprietário.</summary>
    [Range(1, int.MaxValue, ErrorMessage = "O clienteId deve ser superior a 0.")]
    public int? ClienteId { get; set; }

    /// <summary>Pesquisa parcial pelo nome do animal ou pelo nome do proprietário.</summary>
    [MaxLength(100, ErrorMessage = "A pesquisa não pode ter mais de 100 caracteres.")]
    public string? Pesquisa { get; set; }

    /// <summary>Filtra pela espécie do paciente.</summary>
    [MaxLength(50, ErrorMessage = "A espécie não pode ter mais de 50 caracteres.")]
    public string? Especie { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A página deve ser igual ou superior a 1.")]
    public int Pagina { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "O tamanho da página deve estar entre 1 e 100.")]
    public int Tamanho { get; set; } = 20;
}
