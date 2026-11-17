using ClinicaVeterinaria.Api.DTOs.Animais;
using ClinicaVeterinaria.Api.DTOs.Comum;
using ClinicaVeterinaria.Api.Mapeamento;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Api.Controllers;

[ApiController]
[Route("animais")]
[Produces("application/json")]
public sealed class AnimaisController : ControllerBase
{
    private const string PerfisPesquisa =
        Perfis.Rececionista + "," + Perfis.Veterinario;

    private const string PerfisConsulta =
        Perfis.Rececionista + "," + Perfis.Veterinario + "," + Perfis.AuxiliarVeterinario;

    private readonly IAnimaisService _animaisService;

    public AnimaisController(IAnimaisService animaisService)
    {
        _animaisService = animaisService;
    }

    /// <summary>EP-015 — Pesquisar pacientes.</summary>
    [HttpGet]
    [Authorize(Roles = PerfisPesquisa)]
    [ProducesResponseType<PagedResultDto<AnimalListItemDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PagedResultDto<AnimalListItemDto>>> Pesquisar(
        [FromQuery] AnimalFiltroQuery filtro)
    {
        var pagina = await _animaisService.PesquisarAsync(filtro.ParaModelo());
        return Ok(pagina.ParaDto(item => item.ParaDto()));
    }

    /// <summary>EP-016 — Consultar ficha administrativa do paciente.</summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = PerfisConsulta)]
    [ProducesResponseType<AnimalDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AnimalDto>> Obter(int id)
    {
        var resultado = await _animaisService.ObterAsync(id);
        return Ok(resultado.ParaDto());
    }

    /// <summary>EP-017 — Registar paciente.</summary>
    [HttpPost]
    [Authorize(Roles = PerfisPesquisa)]
    [ProducesResponseType<AnimalDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AnimalDto>> Criar(CreateAnimalDto pedido)
    {
        var animal = await _animaisService.CriarAsync(pedido.ParaModelo());

        return CreatedAtAction(
            nameof(Obter),
            new { id = animal.Id },
            animal.ParaDto());
    }

    /// <summary>EP-018 — Atualizar dados administrativos do paciente.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = PerfisPesquisa)]
    [ProducesResponseType<AnimalDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AnimalDto>> Atualizar(
        int id,
        UpdateAnimalDto pedido)
    {
        var resultado = await _animaisService.AtualizarAsync(id, pedido.ParaModelo());
        return Ok(resultado.ParaDto());
    }
}
