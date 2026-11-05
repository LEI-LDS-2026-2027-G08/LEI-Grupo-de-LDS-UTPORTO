namespace ClinicaVeterinaria.Application.DTOs.Comum;

/// <summary>Envelope das listas paginadas (Documentação de API, 2.1).</summary>
public class PagedResultDto<T>
{
    public IReadOnlyList<T> Itens { get; set; } = [];
    public int Pagina { get; set; }
    public int Tamanho { get; set; }
    public int Total { get; set; }
}
