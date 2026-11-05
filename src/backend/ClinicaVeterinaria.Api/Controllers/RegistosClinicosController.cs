using System.Globalization;
using ClinicaVeterinaria.Application.DTOs.RegistosClinicos;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Api.Controllers;

/// <summary>
/// Registo clínico (EP-031 a EP-034). A leitura do registo e do histórico do paciente (EP-033,
/// EP-034) é do Veterinário e do Auxiliar Veterinário (I078); a Rececionista, o Administrador e o
/// Cliente recebem 403. A criação e a alteração (EP-031, EP-032) exigem também a política
/// Veterinário: o Auxiliar Veterinário recebe 403 (I077, critério 6; I078, critérios 2 e 3). Dentro
/// do perfil, só o Veterinário da consulta pode registar ou alterar (verificado no serviço).
/// </summary>
[ApiController]
[Route("registos-clinicos")]
[Produces("application/json")]
[Authorize(Roles = PerfisLeitura)]
public sealed class RegistosClinicosController : ControllerBase
{
    /// <summary>Perfis que leem o registo clínico e o histórico (EP-033, EP-034).</summary>
    public const string PerfisLeitura = Perfis.Veterinario + "," + Perfis.AuxiliarVeterinario;

    private const string MensagemSemVeterinario =
        "O utilizador autenticado não está associado a um Veterinário.";

    private readonly IRegistosClinicosService _registosClinicosService;

    public RegistosClinicosController(IRegistosClinicosService registosClinicosService)
    {
        _registosClinicosService = registosClinicosService;
    }

    /// <summary>EP-031 — Criar registo clínico (só Veterinário).</summary>
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
    // Soma-se ao [Authorize] do controlador: dos dois perfis de leitura, só o Veterinário regista.
    [Authorize(Policy = PoliticasAutorizacao.Veterinario)]
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

        var local = new Uri("/registos-clinicos/" + registo.Id.ToString(CultureInfo.InvariantCulture), UriKind.Relative);

        return Created(local, registo);
    }

    /// <summary>EP-032 — Atualizar registo clínico (só Veterinário).</summary>
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
    // Soma-se ao [Authorize] do controlador: o Auxiliar Veterinário não altera registos.
    [Authorize(Policy = PoliticasAutorizacao.Veterinario)]
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

    /// <summary>EP-033 — Consultar registo clínico (Veterinário e Auxiliar Veterinário).</summary>
    /// <remarks>
    /// Devolve o registo completo (notas, diagnóstico, tratamentos e prescrições), no formato da
    /// resposta de EP-031. Para o Auxiliar Veterinário, o acesso é apenas de leitura.
    /// </remarks>
    /// <response code="200">Registo clínico.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    /// <response code="404">Registo inexistente.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType<RegistoClinicoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RegistoClinicoDto>> Obter(int id)
    {
        return Ok(await _registosClinicosService.ObterAsync(id));
    }

    /// <summary>EP-034 — Consultar histórico clínico do paciente (Veterinário e Auxiliar Veterinário).</summary>
    /// <remarks>
    /// Devolve o animal e os seus registos clínicos, do mais recente para o mais antigo, com a data
    /// da consulta (hora de Lisboa), o Veterinário, o diagnóstico e o número de tratamentos e de
    /// prescrições. <c>dataInicio</c> e <c>dataFim</c> são opcionais (dias em hora de Lisboa, os
    /// dois incluídos). Um animal sem registos devolve a lista vazia.
    /// </remarks>
    /// <response code="200">Histórico clínico do animal.</response>
    /// <response code="400">Intervalo de datas inválido.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    /// <response code="404">Animal inexistente.</response>
    [HttpGet("/animais/{id:int}/historico-clinico")]
    [ProducesResponseType<HistoricoClinicoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HistoricoClinicoDto>> ObterHistorico(int id, [FromQuery] HistoricoClinicoQuery query)
    {
        return Ok(await _registosClinicosService.ObterHistoricoAsync(id, query));
    }
}
