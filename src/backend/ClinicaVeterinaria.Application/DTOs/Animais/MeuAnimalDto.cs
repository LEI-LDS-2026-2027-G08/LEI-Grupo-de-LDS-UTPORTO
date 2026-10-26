namespace ClinicaVeterinaria.Application.DTOs.Animais;

/// <summary>
/// Dados básicos de um animal do Cliente autenticado (EP-020, GET /me/animais/{id}).
/// Só dados administrativos: não inclui o proprietário nem informação clínica.
/// </summary>
public class MeuAnimalDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string? Raca { get; set; }
    public string Sexo { get; set; } = string.Empty;
    public DateOnly? DataNascimento { get; set; }

    /// <summary>Vazio quando a data de nascimento não é conhecida (como em <see cref="AnimalDto"/>).</summary>
    public int? IdadeAnos { get; set; }

    public decimal? Peso { get; set; }
}
