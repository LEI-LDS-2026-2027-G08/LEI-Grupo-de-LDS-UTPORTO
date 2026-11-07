namespace ClinicaVeterinaria.Application.DTOs.Clinica;

/// <summary>Período de funcionamento de um dia da semana (EP-009 e EP-010).</summary>
public class PeriodoHorarioDto
{
    /// <summary>Segunda, Terca, Quarta, Quinta, Sexta, Sabado ou Domingo.</summary>
    public string DiaSemana { get; set; } = string.Empty;

    /// <summary>Hora de abertura, no formato HH:mm.</summary>
    public string Abertura { get; set; } = string.Empty;

    /// <summary>Hora de fecho, no formato HH:mm; posterior à abertura.</summary>
    public string Fecho { get; set; } = string.Empty;
}
