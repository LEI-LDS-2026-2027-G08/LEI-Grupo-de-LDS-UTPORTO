namespace ClinicaVeterinaria.Application.Models.Animais;

public class PesquisaAnimais
{
    public int? ClienteId { get; set; }

    public string? Pesquisa { get; set; }

    public string? Especie { get; set; }

    public int Pagina { get; set; } = 1;

    public int Tamanho { get; set; } = 20;
}
