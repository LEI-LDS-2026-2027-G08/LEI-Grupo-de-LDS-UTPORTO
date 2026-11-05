namespace ClinicaVeterinaria.Application.DTOs.Comum;

/// <summary>Referência ao Veterinário na lista de Veterinários (EP-021) e nas consultas.</summary>
public class VeterinarioRefDto
{
    /// <summary>Identificador do Utilizador, que é também o do Funcionario.</summary>
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;
}
