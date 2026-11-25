namespace ClinicaVeterinaria.Application.Models.Clientes;

public sealed record PaginaClientes(IReadOnlyList<ResumoCliente> Itens, int Pagina, int Tamanho, int Total);
