namespace ClinicaVeterinaria.Application.DTOs.Vacinacoes;

/// <summary>
/// Vacinação ou desparasitação de um paciente (resposta de EP-043 e item do boletim de EP-042).
/// O Veterinário vem como texto (nome), como no contrato da API.
/// </summary>
public class AdministracaoDto
{
    public int Id { get; set; }

    /// <summary>Vacina ou Desparasitacao.</summary>
    public string Tipo { get; set; } = string.Empty;

    public string Produto { get; set; } = string.Empty;

    public string Lote { get; set; } = string.Empty;

    public DateOnly DataAdministracao { get; set; }

    public int? IntervaloDias { get; set; }

    /// <summary>dataAdministracao + intervaloDias; nulo sem intervalo (RF045).</summary>
    public DateOnly? DataProximaPrevista { get; set; }

    /// <summary>Nome do Veterinário que registou a administração.</summary>
    public string Veterinario { get; set; } = string.Empty;
}
