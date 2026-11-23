namespace ClinicaVeterinaria.Application.Models.Utilizadores;

public class PesquisaUtilizadores
{
    public string? Perfil { get; set; }

    public string? Estado { get; set; }

    public string? Pesquisa { get; set; }

    public int Pagina { get; set; } = 1;

    public int Tamanho { get; set; } = 20;
}
