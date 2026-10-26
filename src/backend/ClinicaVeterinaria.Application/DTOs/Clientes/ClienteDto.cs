using ClinicaVeterinaria.Application.DTOs.Comum;

namespace ClinicaVeterinaria.Application.DTOs.Clientes;

public class ClienteDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Nif { get; set; }
    public string Telefone { get; set; } = string.Empty;

    /// <summary>E-mail da conta <c>Utilizador</c> associada (fonte única do dado).</summary>
    public string Email { get; set; } = string.Empty;

    public string? Morada { get; set; }
    public int? UtilizadorId { get; set; }

    /// <summary><c>Utilizador.Estado</c>: Ativo ou Bloqueado.</summary>
    public string? EstadoConta { get; set; }

    public IReadOnlyList<AnimalRefDto> Animais { get; set; } = [];
}
