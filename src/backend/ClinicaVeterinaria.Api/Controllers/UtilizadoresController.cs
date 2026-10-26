using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.DTOs.Utilizadores;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Api.Controllers;

[ApiController]
[Route("utilizadores")]
[Produces("application/json")]
[Authorize(Policy = PoliticasAutorizacao.Administrador)]
public sealed class UtilizadoresController : ControllerBase
{
    private readonly IUtilizadoresService _utilizadoresService;

    public UtilizadoresController(IUtilizadoresService utilizadoresService)
    {
        _utilizadoresService = utilizadoresService;
    }

    /// <summary>EP-004 — Listar contas de funcionários (Administrador).</summary>
    /// <response code="200">Página de contas, ordenada por nome e identificador.</response>
    /// <response code="400">Perfil, estado ou paginação inválidos.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    [HttpGet]
    [ProducesResponseType<PagedResultDto<UtilizadorListItemDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PagedResultDto<UtilizadorListItemDto>>> Listar([FromQuery] UtilizadorFiltroQuery filtro)
    {
        return Ok(await _utilizadoresService.ListarAsync(filtro));
    }

    /// <summary>EP-005 — Consultar conta de funcionário (Administrador).</summary>
    /// <response code="200">Dados da conta, sem o hash da palavra-passe.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    /// <response code="404">Conta de funcionário inexistente.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType<UtilizadorDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UtilizadorDto>> Obter(int id)
    {
        return Ok(await _utilizadoresService.ObterAsync(id));
    }

    /// <summary>EP-006 — Criar conta de funcionário (Administrador).</summary>
    /// <response code="201">Conta criada e ativa; Location indica a conta.</response>
    /// <response code="400">Campos em falta, e-mail inválido, perfil inexistente ou palavra-passe fraca.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    /// <response code="409">E-mail já registado.</response>
    /// <response code="422">Perfil Cliente ou podeRegistarApoio para um perfil diferente de AuxiliarVeterinario.</response>
    [HttpPost]
    [ProducesResponseType<UtilizadorListItemDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<UtilizadorListItemDto>> Criar(CreateUtilizadorDto pedido)
    {
        var utilizador = await _utilizadoresService.CriarAsync(pedido);
        return CreatedAtAction(nameof(Obter), new { id = utilizador.Id }, utilizador);
    }
}
