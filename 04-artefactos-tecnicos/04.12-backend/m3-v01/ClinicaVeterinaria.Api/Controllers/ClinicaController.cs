using ClinicaVeterinaria.Application.DTOs.Clinica;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Api.Controllers;

[ApiController]
[Route("clinica")]
[Produces("application/json")]
[Authorize(Policy = PoliticasAutorizacao.Administrador)]
public sealed class ClinicaController : ControllerBase
{
    private readonly IClinicaService _clinicaService;

    public ClinicaController(IClinicaService clinicaService)
    {
        _clinicaService = clinicaService;
    }

    /// <summary>EP-009 — Consultar dados gerais, configurações e horário da clínica (Administrador).</summary>
    /// <response code="200">Dados gerais, duração das consultas e horário ordenado por dia e hora.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    [HttpGet]
    [ProducesResponseType<ClinicaDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ClinicaDto>> Obter()
    {
        return Ok(await _clinicaService.ObterAsync());
    }

    /// <summary>EP-010 — Atualizar dados gerais, configurações e horário da clínica (Administrador).</summary>
    /// <response code="200">Dados gravados, no formato de EP-009.</response>
    /// <response code="400">Campos inválidos, períodos sobrepostos no mesmo dia ou fecho igual ou anterior à abertura.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    [HttpPut]
    [ProducesResponseType<ClinicaDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ClinicaDto>> Atualizar(UpdateClinicaDto pedido)
    {
        return Ok(await _clinicaService.AtualizarAsync(pedido));
    }
}
