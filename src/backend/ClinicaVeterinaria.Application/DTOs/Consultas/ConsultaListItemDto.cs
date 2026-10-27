using ClinicaVeterinaria.Application.DTOs.Comum;

namespace ClinicaVeterinaria.Application.DTOs.Consultas;

/// <summary>
/// Marcação na agenda (EP-023). Só tem dados administrativos: nenhum campo do registo clínico.
/// </summary>
public class ConsultaListItemDto
{
    public int Id { get; set; }

    /// <summary>Início da consulta, em UTC.</summary>
    public DateTime DataHora { get; set; }

    public int DuracaoMinutos { get; set; }

    /// <summary>Pendente, Confirmada, Realizada ou Cancelada.</summary>
    public string Estado { get; set; } = string.Empty;

    public string? Motivo { get; set; }

    public AnimalRefDto Animal { get; set; } = new();

    public ClienteRefDto Cliente { get; set; } = new();

    public VeterinarioRefDto Veterinario { get; set; } = new();

    /// <summary>Hora de chegada registada na receção (EP-028), em UTC; <c>null</c> sem chegada.</summary>
    public DateTime? ChegadaEm { get; set; }
}
