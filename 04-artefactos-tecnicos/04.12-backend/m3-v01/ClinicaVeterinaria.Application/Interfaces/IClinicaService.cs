using ClinicaVeterinaria.Application.DTOs.Clinica;

namespace ClinicaVeterinaria.Application.Interfaces;

public interface IClinicaService
{
    /// <summary>EP-009 — dados gerais, duração das consultas e horário de funcionamento.</summary>
    Task<ClinicaDto> ObterAsync();

    /// <summary>EP-010 — grava tudo numa única transação; o horário enviado substitui o anterior.</summary>
    Task<ClinicaDto> AtualizarAsync(UpdateClinicaDto pedido);
}
