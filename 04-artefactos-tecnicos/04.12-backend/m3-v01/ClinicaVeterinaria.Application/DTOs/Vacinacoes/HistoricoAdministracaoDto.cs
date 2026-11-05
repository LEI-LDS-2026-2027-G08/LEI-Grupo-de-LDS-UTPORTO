namespace ClinicaVeterinaria.Application.DTOs.Vacinacoes;

/// <summary>Administração no boletim do Cliente (EP-044): só tipo, produto e data.</summary>
public class HistoricoAdministracaoDto
{
    /// <summary>Vacina ou Desparasitacao.</summary>
    public string Tipo { get; set; } = string.Empty;

    public string Produto { get; set; } = string.Empty;

    public DateOnly DataAdministracao { get; set; }
}
