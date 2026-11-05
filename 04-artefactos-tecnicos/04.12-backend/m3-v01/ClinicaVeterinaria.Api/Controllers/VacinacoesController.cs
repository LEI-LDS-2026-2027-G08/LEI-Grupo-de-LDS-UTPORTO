using System.Globalization;
using ClinicaVeterinaria.Application.DTOs.Vacinacoes;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Api.Controllers;

/// <summary>
/// Boletim de vacinas e desparasitações no web (EP-042, EP-043 e a atualização do RF056): só o
/// perfil Veterinário (Documentação de API v02, secção 4). O Administrador, a Rececionista, o
/// Auxiliar Veterinário e o Cliente recebem 403; o Cliente consulta os seus animais em EP-044
/// (<see cref="MinhasVacinacoesController"/>). O Veterinário vem sempre da claim
/// <c>veterinarioId</c> do token.
/// </summary>
[ApiController]
[Route("animais/{id:int}/vacinacoes")]
[Produces("application/json")]
[Authorize(Policy = PoliticasAutorizacao.Veterinario)]
public sealed class VacinacoesController : ControllerBase
{
    private const string MensagemSemVeterinario =
        "O utilizador autenticado não está associado a um Veterinário.";

    private readonly IVacinacoesService _vacinacoesService;

    public VacinacoesController(IVacinacoesService vacinacoesService)
    {
        _vacinacoesService = vacinacoesService;
    }

    /// <summary>EP-042 — Consultar boletim do paciente (web).</summary>
    /// <remarks>
    /// Todas as vacinações e desparasitações do animal, da mais recente para a mais antiga, e as
    /// próximas administrações previstas a partir de hoje (hora de Lisboa), da mais próxima para a
    /// mais distante. <c>tipo</c> (Vacina ou Desparasitacao) filtra as duas listas.
    /// </remarks>
    /// <response code="200">Boletim do animal; listas vazias se não tiver registos.</response>
    /// <response code="400">Tipo inválido.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    /// <response code="404">Animal inexistente.</response>
    [HttpGet]
    [ProducesResponseType<BoletimDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BoletimDto>> ObterBoletim(int id, [FromQuery] BoletimQuery query)
    {
        return Ok(await _vacinacoesService.ObterBoletimAsync(id, query));
    }

    /// <summary>EP-043 — Registar vacinação ou desparasitação.</summary>
    /// <remarks>
    /// Regista a administração com tipo, produto, lote, data (hoje ou no passado) e, quando
    /// aplicável, o intervalo em dias até à próxima. Com intervalo, o servidor calcula
    /// <c>dataProximaPrevista = dataAdministracao + intervaloDias</c>; sem intervalo, fica nula.
    /// </remarks>
    /// <response code="201">Administração registada, com o cabeçalho Location.</response>
    /// <response code="400">Tipo, produto, lote ou data em falta ou inválidos, data futura ou intervaloDias não superior a 0.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    /// <response code="404">Animal inexistente.</response>
    [HttpPost]
    [ProducesResponseType<AdministracaoDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AdministracaoDto>> Registar(int id, CreateAdministracaoDto pedido)
    {
        var veterinarioId = User.ObterVeterinarioId() ?? throw new ForbiddenException(MensagemSemVeterinario);

        var administracao = await _vacinacoesService.RegistarAsync(id, pedido, veterinarioId);

        var local = new Uri(
            "/animais/" + id.ToString(CultureInfo.InvariantCulture)
            + "/vacinacoes/" + administracao.Id.ToString(CultureInfo.InvariantCulture),
            UriKind.Relative);

        return Created(local, administracao);
    }

    /// <summary>RF056 — Atualizar vacinação ou desparasitação (I082, critério 4).</summary>
    /// <remarks>
    /// Substitui tipo, produto, lote, data e intervalo de uma administração do animal e recalcula
    /// <c>dataProximaPrevista</c>. Só o Veterinário que a registou a altera. Operação ainda não
    /// descrita na Documentação de API v02 (ponto em aberto 2: prevista para a v03).
    /// </remarks>
    /// <response code="200">Administração atualizada.</response>
    /// <response code="400">Dados em falta ou inválidos, data futura ou intervaloDias não superior a 0.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão ou administração registada por outro Veterinário.</response>
    /// <response code="404">Animal inexistente, ou administração inexistente neste animal.</response>
    [HttpPut("{administracaoId:int}")]
    [ProducesResponseType<AdministracaoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AdministracaoDto>> Atualizar(int id, int administracaoId, UpdateAdministracaoDto pedido)
    {
        var veterinarioId = User.ObterVeterinarioId() ?? throw new ForbiddenException(MensagemSemVeterinario);

        return Ok(await _vacinacoesService.AtualizarAsync(id, administracaoId, pedido, veterinarioId));
    }
}
