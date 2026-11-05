using ClinicaVeterinaria.Api.Seguranca;
using ClinicaVeterinaria.Application.DTOs.Consultas;
using ClinicaVeterinaria.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Api.Controllers;

/// <summary>
/// Marcações do Cliente autenticado (aplicação móvel). A política de propriedade vem de
/// <see cref="ClienteAutenticadoControllerBase"/>: só o perfil Cliente entra e o identificador do
/// Cliente é sempre o do token, nunca do pedido. Não há operações de escrita (I076, critério 4).
/// </summary>
[Route("me/consultas")]
[Produces("application/json")]
public sealed class MinhasConsultasController : ClienteAutenticadoControllerBase
{
    private readonly IMinhasConsultasService _minhasConsultasService;

    public MinhasConsultasController(IMinhasConsultasService minhasConsultasService)
    {
        _minhasConsultasService = minhasConsultasService;
    }

    /// <summary>EP-030 — Consultar as minhas marcações (Cliente).</summary>
    /// <remarks>
    /// <c>tipo=futuras</c> (por omissão): marcações Pendentes ou Confirmadas que ainda não começaram,
    /// por data crescente. <c>tipo=historico</c>: marcações passadas e as Canceladas ou Realizadas,
    /// mesmo com data futura, por data decrescente. <c>animalId</c> é opcional. Só dados
    /// administrativos: data, estado, animal e Veterinário.
    /// </remarks>
    /// <response code="200">Lista das marcações; vazia se não houver nenhuma.</response>
    /// <response code="400">Tipo diferente de futuras ou historico, ou animalId inferior a 1.</response>
    /// <response code="401">Token ausente, inválido, expirado ou revogado.</response>
    /// <response code="403">Perfil diferente de Cliente ou animalId que não é de um animal do Cliente.</response>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<MinhaConsultaDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<MinhaConsultaDto>>> Listar([FromQuery] MinhasConsultasQuery query)
    {
        return Ok(await _minhasConsultasService.ListarAsync(ClienteIdAutenticado, query));
    }
}
