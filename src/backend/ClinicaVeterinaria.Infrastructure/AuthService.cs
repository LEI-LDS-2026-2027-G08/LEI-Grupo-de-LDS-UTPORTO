using ClinicaVeterinaria.Application.DTOs.Auth;
using ClinicaVeterinaria.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Application.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace ClinicaVeterinaria.Infrastructure;

public class AuthService : IAuthService
{
    private readonly ClinicaDbContext _db;
    private readonly IConfiguration _configuracao;

    public AuthService(ClinicaDbContext db, IConfiguration configuracao)
    {
        _db = db;
        _configuracao = configuracao;
    }
    public Task<UtilizadorAutenticadoDto> ObterAutenticadoAsync(int utilizadorId)
    {
        throw new NotImplementedException();
    }
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto pedido)
    {
        if (string.IsNullOrWhiteSpace(pedido.Email) || string.IsNullOrWhiteSpace(pedido.Password))
            throw new ArgumentException("O e-mail e a palavra-passe são obrigatórios.");

        var utilizador = await _db.Utilizadores
            .FirstOrDefaultAsync(u => u.Email == pedido.Email);

        var credenciaisInvalidas = new UnauthorizedAccessException("Credenciais inválidas.");

        if (utilizador is null)
            throw credenciaisInvalidas;

        var verificador = new PasswordHasher<Utilizador>();
        var resultado = verificador.VerifyHashedPassword(
            utilizador, utilizador.PasswordHash, pedido.Password);

        if (resultado == PasswordVerificationResult.Failed)
            throw credenciaisInvalidas;

        if (utilizador.Estado == "Bloqueado")
            throw new ForbiddenException("A conta está bloqueada.");

        var chave = Convert.FromBase64String(_configuracao["Jwt:Key"]!);
        var expiraEm = DateTime.UtcNow.AddHours(int.Parse(_configuracao["Jwt:ExpirationHours"]!));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, utilizador.Id.ToString()),
            new Claim(ClaimTypes.Role, utilizador.Perfil),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var credenciais = new SigningCredentials(
            new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            _configuracao["Jwt:Issuer"], _configuracao["Jwt:Audience"],
            claims, expires: expiraEm, signingCredentials: credenciais);

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        var nome = await _db.Clientes.Where(c => c.UtilizadorId == utilizador.Id)
        .Select(c => c.Nome).FirstOrDefaultAsync()
    ?? await _db.Funcionarios.Where(f => f.UtilizadorId == utilizador.Id)
        .Select(f => f.Nome).FirstOrDefaultAsync()
    ?? string.Empty;

        return new LoginResponseDto
        {
            Token = token,
            ExpiraEm = expiraEm,
            Utilizador = new UtilizadorResumoDto
            {
                Id = utilizador.Id,
                Nome = nome,
                Email = utilizador.Email,
                Perfil = utilizador.Perfil
            }
        };
    }
}