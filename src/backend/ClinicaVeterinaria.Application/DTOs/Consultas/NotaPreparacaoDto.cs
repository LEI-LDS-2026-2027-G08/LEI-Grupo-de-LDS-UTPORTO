namespace ClinicaVeterinaria.Application.DTOs.Consultas;

/// <summary>
/// Nota de preparação não clínica registada por um Auxiliar Veterinário (entidade
/// <c>InformacaoApoio</c>): <c>Descricao</c> ↔ <see cref="Texto"/>, <c>DataRegisto</c> ↔
/// <see cref="CriadaEm"/>, e o autor é o nome do Auxiliar.
/// </summary>
public class NotaPreparacaoDto
{
    public int Id { get; set; }

    /// <summary>Só vem preenchido na resposta de criação (EP-029); em EP-024 é <c>null</c>.</summary>
    public int? ConsultaId { get; set; }

    public string Texto { get; set; } = string.Empty;

    /// <summary>Nome do Auxiliar Veterinário que registou a nota.</summary>
    public string Autor { get; set; } = string.Empty;

    /// <summary>Data e hora do registo, em UTC.</summary>
    public DateTime CriadaEm { get; set; }
}
