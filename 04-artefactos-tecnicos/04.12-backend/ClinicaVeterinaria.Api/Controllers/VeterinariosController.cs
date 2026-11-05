using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.DTOs.Veterinarios;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Api.Controllers;

/// <summary>Veterinários e disponibilidade para marcação (EP-021, EP-022). Só a Rececionista tem acesso.</summary>
[ApiController]
[Route("veterinarios")]
[Produces("application/json")]
[Authorize(Policy = PoliticasAutorizacao.Rececionista)]
public sealed class VeterinariosController : ControllerBase
{
    private readonly IVeterinariosService _veterinariosService;

    public VeterinariosController(IVeterinariosService veterinariosService)
    {
        _veterinariosService = veterinariosService;
    }

    /// <summary>EP-021 — Listar veterinários (Rececionista).</summary>
    /// <response code="200">Veterinários com conta ativa, ordenados por nome.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<VeterinarioRefDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<VeterinarioRefDto>>> Listar()
    {
        return Ok(await _veterinariosService.ListarAsync());
    }

    /// <summary>EP-022 — Consultar disponibilidade de um veterinário (Rececionista).</summary>
    /// <remarks>
    /// Horários livres de cada dia, em hora local da clínica (HH:mm), a partir do horário de
    /// funcionamento e das marcações não canceladas do Veterinário. Os horários que já começaram
    /// não são devolvidos.
    /// </remarks>
    /// <response code="200">Horários livres de cada dia do período.</response>
    /// <response code="400">dataInicio em falta ou inválida, dataFim anterior a dataInicio ou período superior a 31 dias.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    /// <response code="404">Veterinário inexistente ou com a conta bloqueada.</response>
    [HttpGet("{id:int}/disponibilidade")]
    [ProducesResponseType<DisponibilidadeDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DisponibilidadeDto>> ObterDisponibilidade(int id, [FromQuery] DisponibilidadeQuery query)
    {
        return Ok(await _veterinariosService.ObterDisponibilidadeAsync(id, query));
    }
}
