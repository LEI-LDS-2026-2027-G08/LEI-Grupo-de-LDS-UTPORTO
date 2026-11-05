namespace ClinicaVeterinaria.Application.DTOs.Animais;

/// <summary>Animal do Cliente autenticado na lista de EP-019 (GET /me/animais).</summary>
public class MeuAnimalListItemDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string? Raca { get; set; }

    /// <summary>Vazio quando a data de nascimento não é conhecida (como em <see cref="AnimalDto"/>).</summary>
    public int? IdadeAnos { get; set; }

    public decimal? Peso { get; set; }
}
