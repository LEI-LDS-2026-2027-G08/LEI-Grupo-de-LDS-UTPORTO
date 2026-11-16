namespace ClinicaVeterinaria.Application.Clientes;

public sealed record ResumoCliente(int Id, string Nome, string? Nif, string Telefone, string Email, int NumeroAnimais);
