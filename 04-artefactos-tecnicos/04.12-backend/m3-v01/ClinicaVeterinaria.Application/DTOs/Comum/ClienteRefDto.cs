namespace ClinicaVeterinaria.Application.DTOs.Comum;

/// <summary>
/// Referência ao proprietário em animais e consultas (Modelo de Classes do Backend v02, 3.3).
/// </summary>
public class ClienteRefDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    /// <summary>Só é preenchido na ficha do animal (EP-016); nas consultas vem a <c>null</c>.</summary>
    public string? Telefone { get; set; }
}
