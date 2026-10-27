using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.DTOs.Consultas;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Api.Controllers;

/// <summary>
/// Agenda e consulta de marcações (EP-023, EP-024): Rececionista, Veterinário e Auxiliar
/// Veterinário. O Veterinário só vê as marcações que lhe estão atribuídas, identificado pelo token.
/// </summary>
[ApiController]
[Route("consultas")]
[Produces("application/json")]
[Authorize(Roles = PerfisAgenda)]
public sealed class ConsultasController : ControllerBase
{
    public const string PerfisAgenda =
        Perfis.Rececionista + "," + Perfis.Veterinario + "," + Perfis.AuxiliarVeterinario;

    private readonly IConsultasService _consultasService;

    public ConsultasController(IConsultasService consultasService)
    {
        _consultasService = consultasService;
    }

    /// <summary>EP-023 — Consultar agenda.</summary>
    /// <remarks>
    /// Marcações que começam entre <c>dataInicio</c> e <c>dataFim</c> (dias em hora de Lisboa, os dois
    /// incluídos), por data e hora. Para o perfil Veterinário, <c>veterinarioId</c> é ignorado e
    /// só são devolvidas as marcações do Veterinário autenticado.
    /// </remarks>
    /// <response code="200">Página de marcações.</response>
    /// <response code="400">Intervalo de datas inválido, estado inválido ou paginação fora dos limites.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    [HttpGet]
    [ProducesResponseType<PagedResultDto<ConsultaListItemDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PagedResultDto<ConsultaListItemDto>>> GetConsultas(
        [FromQuery] ConsultaFiltroQuery filtro)
    {
        return Ok(await _consultasService.ObterConsultasAsync(filtro, AcessoConsultas.DoUtilizador(User)));
    }

    /// <summary>EP-024 — Consultar marcação.</summary>
    /// <remarks>
    /// Devolve a marcação e as notas de preparação não clínicas (só leitura), com a versão atual
    /// da marcação no cabeçalho <c>ETag</c>.
    /// </remarks>
    /// <response code="200">Marcação, com o cabeçalho ETag.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão ou marcação não atribuída ao Veterinário autenticado.</response>
    /// <response code="404">Marcação inexistente.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType<ConsultaDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConsultaDto>> GetConsulta(int id)
    {
        var resultado = await _consultasService.ObterConsultaAsync(id, AcessoConsultas.DoUtilizador(User));

        Response.Headers.ETag = resultado.ETag;

        return Ok(resultado.Consulta);
    }
}
