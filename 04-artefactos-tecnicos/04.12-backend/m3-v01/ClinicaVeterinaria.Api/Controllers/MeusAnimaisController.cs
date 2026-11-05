using ClinicaVeterinaria.Api.Seguranca;
using ClinicaVeterinaria.Application.DTOs.Animais;
using ClinicaVeterinaria.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Api.Controllers;

/// <summary>
/// Animais do Cliente autenticado (aplicação móvel). A política de propriedade
/// vem de <see cref="ClienteAutenticadoControllerBase"/>: só o perfil Cliente
/// entra e o identificador do Cliente é sempre o do token, nunca do pedido.
/// </summary>
[Route("me/animais")]
[Produces("application/json")]
public sealed class MeusAnimaisController : ClienteAutenticadoControllerBase
{
    private readonly IAnimaisService _animaisService;

    public MeusAnimaisController(IAnimaisService animaisService)
    {
        _animaisService = animaisService;
    }

    /// <summary>EP-019 — Listar os meus animais (Cliente).</summary>
    /// <response code="200">Animais da conta do Cliente autenticado; lista vazia se não tiver animais.</response>
    /// <response code="401">Token ausente, inválido, expirado ou revogado.</response>
    /// <response code="403">Perfil diferente de Cliente.</response>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<MeuAnimalListItemDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<MeuAnimalListItemDto>>> Listar()
    {
        return Ok(await _animaisService.ListarDoClienteAsync(ClienteIdAutenticado));
    }

    /// <summary>EP-020 — Consultar o meu animal (Cliente).</summary>
    /// <response code="200">Dados básicos do animal.</response>
    /// <response code="401">Token ausente, inválido, expirado ou revogado.</response>
    /// <response code="403">Perfil diferente de Cliente ou animal de outro Cliente.</response>
    /// <response code="404">Animal inexistente.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType<MeuAnimalDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MeuAnimalDto>> Obter(int id)
    {
        return Ok(await _animaisService.ObterDoClienteAsync(ClienteIdAutenticado, id));
    }
}
