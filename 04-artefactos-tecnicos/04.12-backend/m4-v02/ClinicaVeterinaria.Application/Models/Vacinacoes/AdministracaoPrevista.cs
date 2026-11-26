namespace ClinicaVeterinaria.Application.Models.Vacinacoes;

public class AdministracaoPrevista
{
    public string Tipo { get; set; } = string.Empty;

    public string Produto { get; set; } = string.Empty;

    public DateOnly DataProximaPrevista { get; set; }
}
