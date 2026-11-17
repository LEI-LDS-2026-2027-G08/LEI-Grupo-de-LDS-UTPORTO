namespace ClinicaVeterinaria.Application.Models.Vacinacoes;

public class AdministracaoRegistada
{
    public int Id { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public string Produto { get; set; } = string.Empty;

    public string Lote { get; set; } = string.Empty;

    public DateOnly DataAdministracao { get; set; }

    public int? IntervaloDias { get; set; }

    public DateOnly? DataProximaPrevista { get; set; }

    public string Veterinario { get; set; } = string.Empty;
}
