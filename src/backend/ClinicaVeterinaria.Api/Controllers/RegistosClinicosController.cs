using System.Globalization;
using ClinicaVeterinaria.Application.DTOs.RegistosClinicos;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Api.Controllers;

/// <summary>
/// Registo clínico (EP-031, EP-032): só o perfil Veterinário. O Auxiliar Veterinário, a
/// Rececionista, o Administrador e o Cliente recebem 403 (I077, critério 6). Dentro do perfil,
/// só o Veterinário da consulta pode registar ou alterar (verificado no serviço).
/// </summary>
[ApiController]
[Route("registos-clinicos")]
[Produces("application/json")]
[Authorize(Policy = PoliticasAutorizacao.Veterinario)]
public sealed class RegistosClinicosController : ControllerBase
{
    private const string MensagemSemVeterinario =
        "O utilizador autenticado não está associado a um Veterinário.";

    private readonly IRegistosClinicosService _registosClinicosService;

    public RegistosClinicosController(IRegistosClinicosService registosClinicosService)
    {
        _registosClinicosService = registosClinicosService;
    }

    /// <summary>EP-031 — Criar registo clínico.</summary>
    /// <remarks>
    /// Regista notas clínicas, diagnóstico, tratamentos e prescrições de uma consulta Confirmada
    /// ou Realizada do Veterinário autenticado. No máximo um registo por consulta; tratamentos e
    /// prescrições adicionais acrescentam-se com EP-032. O estado da marcação não muda.
    /// </remarks>
    /// <response code="201">Registo criado, com o cabeçalho Location.</response>
    /// <response code="400">consultaId em falta, nenhum elemento clínico ou tratamento/prescrição incompleto.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão ou consulta de outro Veterinário.</response>
    /// <response code="404">Consulta inexistente.</response>
    /// <response code="409">A consulta já tem registo clínico.</response>
    /// <response code="422">Consulta Pendente ou Cancelada.</response>
    [HttpPost]
    [ProducesResponseType<RegistoClinicoDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<RegistoClinicoDto>> Criar(CreateRegistoClinicoDto pedido)
    {
        var veterinarioId = User.ObterVeterinarioId() ?? throw new ForbiddenException(MensagemSemVeterinario);

        var registo = await _registosClinicosService.CriarAsync(pedido, veterinarioId);

        // A leitura do registo (GET /registos-clinicos/{id}, EP-033) é da I078.
        var local = new Uri("/registos-clinicos/" + registo.Id.ToString(CultureInfo.InvariantCulture), UriKind.Relative);

        return Created(local, registo);
    }

    /// <summary>EP-032 — Atualizar registo clínico.</summary>
    /// <remarks>
    /// Altera notas, diagnóstico, tratamentos e prescrições. As listas substituem as anteriores:
    /// um elemento existente mantém-se indicando o seu <c>id</c>; os que não vierem são removidos.
    /// A consulta e o paciente não mudam; ficam registados <c>atualizadoEm</c> e <c>atualizadoPor</c>.
    /// </remarks>
    /// <response code="200">Registo atualizado.</response>
    /// <response code="400">Dados inválidos, nenhum elemento clínico ou id de outro registo.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão ou registo de outro Veterinário.</response>
    /// <response code="404">Registo inexistente.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType<RegistoClinicoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RegistoClinicoDto>> Atualizar(int id, UpdateRegistoClinicoDto pedido)
    {
        var veterinarioId = User.ObterVeterinarioId() ?? throw new ForbiddenException(MensagemSemVeterinario);

        return Ok(await _registosClinicosService.AtualizarAsync(id, pedido, veterinarioId));
    }
}
