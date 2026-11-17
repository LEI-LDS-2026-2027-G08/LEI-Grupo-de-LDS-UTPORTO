using ClinicaVeterinaria.Api.DTOs.Comum;
using ClinicaVeterinaria.Api.DTOs.Utilizadores;
using ClinicaVeterinaria.Api.Mapeamento;
using ClinicaVeterinaria.Application.Exceptions;
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
        var pagina = await _utilizadoresService.ListarAsync(filtro.ParaModelo());
        return Ok(pagina.ParaDto(item => item.ParaDto()));
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
        var resultado = await _utilizadoresService.ObterAsync(id);
        return Ok(resultado.ParaDto());
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
        var utilizador = await _utilizadoresService.CriarAsync(pedido.ParaModelo());
        return CreatedAtAction(nameof(Obter), new { id = utilizador.Id }, utilizador.ParaDto());
    }

    /// <summary>EP-007 — Atualizar conta de funcionário e atribuir perfil (Administrador).</summary>
    /// <remarks>
    /// Uma mudança de perfil tem efeito imediato: o token emitido com o perfil antigo é rejeitado no pedido seguinte.
    /// </remarks>
    /// <response code="200">Conta atualizada (formato de EP-005).</response>
    /// <response code="400">Dados inválidos, perfil inexistente ou perfil Cliente.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    /// <response code="404">Conta de funcionário inexistente.</response>
    /// <response code="409">E-mail já usado por outra conta.</response>
    /// <response code="422">O Administrador remove o próprio perfil de Administrador, ou podeRegistarApoio para um perfil diferente de AuxiliarVeterinario.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType<UtilizadorDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<UtilizadorDto>> Atualizar(int id, UpdateUtilizadorDto pedido)
    {
        var resultado = await _utilizadoresService.AtualizarAsync(id, AdministradorId(), pedido.ParaModelo());
        return Ok(resultado.ParaDto());
    }

    /// <summary>EP-008 — Bloquear conta de funcionário (Administrador).</summary>
    /// <remarks>
    /// A conta bloqueada deixa de iniciar sessão (EP-001 devolve 403) e os tokens já emitidos
    /// são rejeitados com 401 no pedido seguinte.
    /// </remarks>
    /// <response code="204">Conta bloqueada.</response>
    /// <response code="400">Estado diferente de Bloqueado.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    /// <response code="404">Conta inexistente.</response>
    /// <response code="422">Conta já bloqueada, a própria conta do Administrador ou conta de Cliente.</response>
    [HttpPatch("{id:int}/estado")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AlterarEstado(int id, AlterarEstadoUtilizadorDto pedido)
    {
        await _utilizadoresService.BloquearAsync(id, AdministradorId(), pedido.ParaModelo());
        return NoContent();
    }

    /// <summary>O Administrador autenticado vem sempre do token (RNF002).</summary>
    private int AdministradorId() =>
        User.ObterUtilizadorId()
        ?? throw new NaoAutenticadoException("O token não contém as claims obrigatórias.");
}
