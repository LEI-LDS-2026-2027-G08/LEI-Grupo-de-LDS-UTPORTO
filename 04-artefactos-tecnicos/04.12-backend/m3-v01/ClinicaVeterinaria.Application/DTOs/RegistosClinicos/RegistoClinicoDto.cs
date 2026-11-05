namespace ClinicaVeterinaria.Application.DTOs.RegistosClinicos;

/// <summary>
/// Registo clínico de uma consulta (resposta de EP-031 e EP-032; Documentação de API, EP-031).
/// O Veterinário e o autor da última alteração vêm como texto (nome), como no contrato da API.
/// </summary>
public class RegistoClinicoDto
{
    public int Id { get; set; }

    /// <summary>Marcação (consulta) a que o registo pertence.</summary>
    public int ConsultaId { get; set; }

    /// <summary>Paciente da consulta.</summary>
    public int AnimalId { get; set; }

    /// <summary>Nome do Veterinário que criou o registo.</summary>
    public string Veterinario { get; set; } = string.Empty;

    /// <summary>Data e hora de criação, em UTC.</summary>
    public DateTime CriadoEm { get; set; }

    public string? NotasClinicas { get; set; }

    public string? Diagnostico { get; set; }

    public IReadOnlyList<TratamentoDto> Tratamentos { get; set; } = [];

    public IReadOnlyList<PrescricaoDto> Prescricoes { get; set; } = [];

    /// <summary>Data e hora da última alteração (EP-032), em UTC; nulo se nunca foi alterado.</summary>
    public DateTime? AtualizadoEm { get; set; }

    /// <summary>Nome de quem fez a última alteração; nulo se nunca foi alterado.</summary>
    public string? AtualizadoPor { get; set; }
}
