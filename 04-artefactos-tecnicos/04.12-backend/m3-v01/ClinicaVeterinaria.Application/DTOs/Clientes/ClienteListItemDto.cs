namespace ClinicaVeterinaria.Application.DTOs.Clientes;

public class ClienteListItemDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Nif { get; set; }
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int NumeroAnimais { get; set; }
}
