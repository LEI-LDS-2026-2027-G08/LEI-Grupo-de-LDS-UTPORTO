using ClinicaVeterinaria.Api.Seguranca;
using ClinicaVeterinaria.Application.DTOs.Vacinacoes;
using ClinicaVeterinaria.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Api.Controllers;

/// <summary>
/// Boletim de vacinas e desparasitações de um animal do Cliente autenticado (aplicação móvel;
/// EP-044). A política de propriedade vem de <see cref="ClienteAutenticadoControllerBase"/>: só o
/// perfil Cliente entra e o identificador do Cliente é sempre o do token, nunca do pedido (RNF002).
/// </summary>
[Route("me/animais/{id:int}/vacinacoes")]
[Produces("application/json")]
public sealed class MinhasVacinacoesController : ClienteAutenticadoControllerBase
{
    private readonly IVacinacoesService _vacinacoesService;

    public MinhasVacinacoesController(IVacinacoesService vacinacoesService)
    {
        _vacinacoesService = vacinacoesService;
    }

    /// <summary>EP-044 — Consultar boletim do meu animal (Cliente).</summary>
    /// <remarks>
    /// Histórico de vacinação e desparasitação do animal (da administração mais recente para a mais
    /// antiga) e próximas administrações previstas a partir de hoje. Sem registos, as duas listas
    /// vêm vazias.
    /// </remarks>
    /// <response code="200">Boletim do animal.</response>
    /// <response code="401">Token ausente, inválido, expirado ou revogado.</response>
    /// <response code="403">Perfil diferente de Cliente ou animal de outro Cliente.</response>
    /// <response code="404">Animal inexistente.</response>
    [HttpGet]
    [ProducesResponseType<MeuBoletimDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MeuBoletimDto>> ObterBoletim(int id)
    {
        return Ok(await _vacinacoesService.ObterBoletimDoClienteAsync(ClienteIdAutenticado, id));
    }
}
