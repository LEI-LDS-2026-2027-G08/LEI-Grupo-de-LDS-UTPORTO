using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Application.DTOs.Clientes;

/// <summary>Parâmetros de EP-011 (GET /clientes).</summary>
public class ClienteFiltroQuery
{
    /// <summary>Texto procurado, por correspondência parcial, no nome, NIF, telefone ou e-mail.</summary>
    [MaxLength(100, ErrorMessage = "A pesquisa não pode ter mais de 100 caracteres.")]
    public string? Pesquisa { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A página deve ser igual ou superior a 1.")]
    public int Pagina { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "O tamanho da página deve estar entre 1 e 100.")]
    public int Tamanho { get; set; } = 20;
}
