using ClinicaVeterinaria.Application.DTOs.Auth;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ClinicaVeterinaria.UnitTests.Seguranca;

/// <summary>
/// Duplo do <see cref="IPasswordHasher{TUser}"/> (I028, secção 4.2): devolve o resultado
/// configurado em cada verificação e conta quantas vezes foi chamado.
/// </summary>
internal sealed class HasherDuplo : IPasswordHasher<Utilizador>
{
    public const string NovoHash = "hash-novo-do-duplo";

    public HasherDuplo(PasswordVerificationResult resultado = PasswordVerificationResult.Success)
    {
        Resultado = resultado;
    }

    public PasswordVerificationResult Resultado { get; }

    public int Verificacoes { get; private set; }

    public string HashPassword(Utilizador user, string password) => NovoHash;

    public PasswordVerificationResult VerifyHashedPassword(Utilizador user, string hashedPassword, string providedPassword)
    {
        Verificacoes++;
        return Resultado;
    }
}

/// <summary>Duplo do <see cref="IJwtTokenService"/>: devolve sempre o mesmo token e guarda os pedidos.</summary>
internal sealed class TokenServiceDuplo : IJwtTokenService
{
    public const string Token = "token-do-duplo";

    public static readonly DateTime ExpiraEm = new(2026, 10, 14, 16, 0, 0, DateTimeKind.Utc);

    public List<UtilizadorAutenticadoDto> Pedidos { get; } = [];

    public TokenEmitido GerarToken(UtilizadorAutenticadoDto utilizador)
    {
        Pedidos.Add(utilizador);
        return new TokenEmitido(Token, ExpiraEm, "jti-do-duplo");
    }
}

/// <summary>Logger que guarda o identificador e o texto de cada mensagem registada.</summary>
internal sealed class LoggerDeTeste<T> : ILogger<T>
{
    public List<(EventId Evento, string Mensagem)> Registos { get; } = [];

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        ArgumentNullException.ThrowIfNull(formatter);
        Registos.Add((eventId, formatter(state, exception)));
    }
}
