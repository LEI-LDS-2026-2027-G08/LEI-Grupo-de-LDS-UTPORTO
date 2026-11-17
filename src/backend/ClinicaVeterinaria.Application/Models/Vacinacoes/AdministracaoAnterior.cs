namespace ClinicaVeterinaria.Application.Models.Vacinacoes;

public class AdministracaoAnterior
{
    public string Tipo { get; set; } = string.Empty;

    public string Produto { get; set; } = string.Empty;

    public DateOnly DataAdministracao { get; set; }
}
