using ClinicaVeterinaria.Application.DTOs.Auth;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Api.Controllers;

[ApiController]
[Route("auth")]
[Produces("application/json")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>EP-001 — Iniciar sessão (todos os perfis).</summary>
    /// <response code="200">Credenciais válidas de conta ativa; devolve o JWT (HS256, 8 horas).</response>
    /// <response code="400">E-mail ou palavra-passe em falta.</response>
    /// <response code="401">Credenciais incorretas.</response>
    /// <response code="403">Conta bloqueada.</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType<LoginResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto pedido)
    {
        return Ok(await _authService.LoginAsync(pedido));
    }

    /// <summary>EP-003 — Consultar utilizador autenticado (todos os perfis).</summary>
    /// <response code="200">Identidade e perfil atual do utilizador do token.</response>
    /// <response code="401">Token ausente, inválido, expirado ou revogado.</response>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType<UtilizadorAutenticadoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UtilizadorAutenticadoDto>> ObterUtilizadorAutenticado()
    {
        var utilizadorId = User.ObterUtilizadorId()
            ?? throw new UnauthorizedAccessException("O token não identifica o utilizador.");

        return Ok(await _authService.ObterAutenticadoAsync(utilizadorId));
    }
}
