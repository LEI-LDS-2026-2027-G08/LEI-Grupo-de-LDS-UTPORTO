namespace ClinicaVeterinaria.Application.Models.Comum;

public class PaginaResultados<T>
{
    public IReadOnlyList<T> Itens { get; set; } = [];

    public int Pagina { get; set; }

    public int Tamanho { get; set; }

    public int Total { get; set; }
}
