using System.Globalization;
using ClinicaVeterinaria.Application.DTOs.Auth;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace ClinicaVeterinaria.Api.Seguranca;

/// <summary>
/// Emite JWT assinados com HS256, com as claims da Documentação de API (3.2)
/// e validade de <see cref="JwtOptions.ExpirationHours"/> horas (8, sem refresh token).
/// </summary>
public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _opcoes;
    private readonly SigningCredentials _credenciais;
    private readonly TimeProvider _relogio;
    private readonly JsonWebTokenHandler _handler = new();

    public JwtTokenService(IOptions<JwtOptions> opcoes, TimeProvider relogio)
    {
        ArgumentNullException.ThrowIfNull(opcoes);

        _opcoes = opcoes.Value;
        _credenciais = new SigningCredentials(_opcoes.CriarChaveAssinatura(), SecurityAlgorithms.HmacSha256);
        _relogio = relogio;
    }

    public TokenEmitido GerarToken(UtilizadorAutenticadoDto utilizador)
    {
        ArgumentNullException.ThrowIfNull(utilizador);

        // Sem frações de segundo, para que expiraEm coincida com a claim exp.
        var agora = _relogio.GetUtcNow().UtcDateTime;
        var emitidoEm = new DateTime(agora.Ticks - (agora.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc);
        var expiraEm = emitidoEm.AddHours(_opcoes.ExpirationHours);
        var jti = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

        var claims = new Dictionary<string, object>
        {
            [ClaimsClinica.UtilizadorId] = utilizador.Id.ToString(CultureInfo.InvariantCulture),
            [ClaimsClinica.Email] = utilizador.Email,
            [ClaimsClinica.Perfil] = utilizador.Perfil,
            [ClaimsClinica.Jti] = jti
        };

        if (utilizador.ClienteId is int clienteId)
        {
            claims[ClaimsClinica.ClienteId] = clienteId.ToString(CultureInfo.InvariantCulture);
        }

        // No modelo de dados o Veterinário é o Funcionario com o mesmo Id do Utilizador.
        if (utilizador.Perfil == Perfis.Veterinario)
        {
            claims[ClaimsClinica.VeterinarioId] = utilizador.Id.ToString(CultureInfo.InvariantCulture);
        }

        var descritor = new SecurityTokenDescriptor
        {
            Issuer = _opcoes.Issuer,
            Audience = _opcoes.Audience,
            IssuedAt = emitidoEm,
            NotBefore = emitidoEm,
            Expires = expiraEm,
            Claims = claims,
            SigningCredentials = _credenciais
        };

        return new TokenEmitido(_handler.CreateToken(descritor), expiraEm, jti);
    }
}
