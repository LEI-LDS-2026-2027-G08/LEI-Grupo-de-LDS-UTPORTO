namespace ClinicaVeterinaria.Application.DTOs.Vacinacoes;

/// <summary>Próxima administração prevista (EP-042 e EP-044).</summary>
public class ProximaAdministracaoDto
{
    /// <summary>Vacina ou Desparasitacao.</summary>
    public string Tipo { get; set; } = string.Empty;

    public string Produto { get; set; } = string.Empty;

    public DateOnly DataProximaPrevista { get; set; }
}
