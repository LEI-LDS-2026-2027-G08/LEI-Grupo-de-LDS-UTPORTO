using ClinicaVeterinaria.Application.DTOs.Auth;
using ClinicaVeterinaria.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto pedido)
    {
        return Ok(await _authService.LoginAsync(pedido));
    }
}