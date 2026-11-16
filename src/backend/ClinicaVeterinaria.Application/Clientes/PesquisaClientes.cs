namespace ClinicaVeterinaria.Application.Clientes;

public sealed record PesquisaClientes(string? Pesquisa = null, int Pagina = 1, int Tamanho = 20);
