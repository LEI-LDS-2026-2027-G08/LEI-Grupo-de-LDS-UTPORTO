namespace ClinicaVeterinaria.Application.DTOs.RegistosClinicos;

/// <summary>
/// Linha do histórico clínico de um paciente (EP-034; Modelo de Classes do Backend v02). O
/// registo completo obtém-se em EP-033 com o <see cref="Id"/>.
/// </summary>
public class RegistoClinicoResumoDto
{
    public int Id { get; set; }

    /// <summary>Marcação (consulta) a que o registo pertence.</summary>
    public int ConsultaId { get; set; }

    /// <summary>Dia da consulta, em hora de Lisboa.</summary>
    public DateOnly Data { get; set; }

    /// <summary>Nome do Veterinário que fez o registo.</summary>
    public string Veterinario { get; set; } = string.Empty;

    public string? Diagnostico { get; set; }

    public int NumeroTratamentos { get; set; }

    public int NumeroPrescricoes { get; set; }
}
