using ClinicaVeterinaria.Application.DTOs.Comum;

namespace ClinicaVeterinaria.Application.DTOs.Consultas;

/// <summary>
/// Marcação com as notas de preparação (EP-024). É a mesma resposta para a Rececionista, o
/// Veterinário e o Auxiliar Veterinário e só tem dados administrativos e notas não clínicas:
/// nenhum campo do registo clínico, tratamentos ou prescrições.
/// </summary>
/// <remarks>A versão da marcação não é um campo: vem no cabeçalho <c>ETag</c> da resposta.</remarks>
public class ConsultaDto
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

    /// <summary>Notas de preparação dos Auxiliares (só leitura), da mais antiga para a mais recente.</summary>
    public IReadOnlyList<NotaPreparacaoDto> NotasPreparacao { get; set; } = [];
}
