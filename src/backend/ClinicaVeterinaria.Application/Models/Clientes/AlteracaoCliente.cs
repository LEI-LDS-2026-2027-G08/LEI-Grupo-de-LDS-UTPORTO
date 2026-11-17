namespace ClinicaVeterinaria.Application.Models.Clientes;

public sealed record AlteracaoCliente(string Nome, string? Nif, string Telefone, string Email, string? Morada);
