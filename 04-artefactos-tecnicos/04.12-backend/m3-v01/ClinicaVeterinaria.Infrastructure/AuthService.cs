using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ClinicaVeterinaria.Application.DTOs.Auth;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicaVeterinaria.Infrastructure;

/// <summary>
/// Autenticação (EP-001, EP-003), fim de sessão com revogação do token (EP-002, I064)
/// e validação da sessão em cada pedido autenticado.
/// </summary>
public sealed class AuthService : IAuthService
{
    private const string MensagemCredenciaisInvalidas = "Credenciais inválidas.";
    private const string MensagemSessaoInvalida = "A sessão já não é válida. Inicie sessão novamente.";

    // Hash de referência usado quando o e-mail não existe, para que a resposta
    // demore o mesmo que uma palavra-passe errada e não revele se o e-mail existe.
    private static readonly Utilizador UtilizadorReferencia = new();
    private static readonly string HashReferencia =
        new PasswordHasher<Utilizador>().HashPassword(UtilizadorReferencia, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

    private static readonly Action<ILogger, string, string, Exception?> LogTentativaFalhada =
        LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            new EventId(3001, "TentativaLoginFalhada"),
            "Tentativa de início de sessão falhada para {Email}: {Motivo}");

    private static readonly Action<ILogger, int, string, Exception?> LogSessaoIniciada =
        LoggerMessage.Define<int, string>(
            LogLevel.Information,
            new EventId(3002, "SessaoIniciada"),
            "Sessão iniciada pelo utilizador {UtilizadorId} com o perfil {Perfil}");

    private static readonly Action<ILogger, int, Exception?> LogSessaoTerminada =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(3003, "SessaoTerminada"),
            "Sessão terminada pelo utilizador {UtilizadorId}; o token fica revogado até expirar");

    private readonly ClinicaDbContext _db;
    private readonly IPasswordHasher<Utilizador> _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<AuthService> _logger;
    private readonly TimeProvider _relogio;

    public AuthService(
        ClinicaDbContext db,
        IPasswordHasher<Utilizador> passwordHasher,
        IJwtTokenService jwtTokenService,
        ILogger<AuthService> logger,
        TimeProvider relogio)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
        _relogio = relogio;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto pedido)
    {
        ArgumentNullException.ThrowIfNull(pedido);

        var email = pedido.Email?.Trim();
        var password = pedido.Password;

        ValidarCamposObrigatorios(email, password);

        var utilizador = await _db.Utilizadores
            .FirstOrDefaultAsync(u => u.Email == email);

        if (utilizador is null)
        {
            _passwordHasher.VerifyHashedPassword(UtilizadorReferencia, HashReferencia, password);
            LogTentativaFalhada(_logger, email, "e-mail não registado", null);
            throw new UnauthorizedAccessException(MensagemCredenciaisInvalidas);
        }

        var resultado = _passwordHasher.VerifyHashedPassword(
            utilizador, utilizador.PasswordHash, password);

        if (resultado == PasswordVerificationResult.Failed)
        {
            LogTentativaFalhada(_logger, email, "palavra-passe incorreta", null);
            throw new UnauthorizedAccessException(MensagemCredenciaisInvalidas);
        }

        // Só depois de confirmar a palavra-passe, para que uma palavra-passe
        // errada nunca revele que a conta existe e está bloqueada.
        if (utilizador.Estado != EstadosUtilizador.Ativo)
        {
            LogTentativaFalhada(_logger, email, "conta bloqueada", null);
            throw new ForbiddenException("A conta está bloqueada. Contacte a clínica.");
        }

        if (resultado == PasswordVerificationResult.SuccessRehashNeeded)
        {
            utilizador.PasswordHash = _passwordHasher.HashPassword(utilizador, password);
        }

        var autenticado = await CriarUtilizadorAutenticadoAsync(utilizador);
        var token = _jwtTokenService.GerarToken(autenticado);

        utilizador.DataUltimoAcesso = _relogio.GetUtcNow().UtcDateTime;
        await _db.SaveChangesAsync();

        LogSessaoIniciada(_logger, utilizador.Id, utilizador.Perfil, null);

        return new LoginResponseDto
        {
            Token = token.Token,
            ExpiraEm = token.ExpiraEm,
            Utilizador = new UtilizadorResumoDto
            {
                Id = autenticado.Id,
                Nome = autenticado.Nome,
                Email = autenticado.Email,
                Perfil = autenticado.Perfil,
                ClienteId = autenticado.ClienteId
            }
        };
    }

    public async Task<UtilizadorAutenticadoDto> ObterAutenticadoAsync(int utilizadorId)
    {
        var utilizador = await _db.Utilizadores
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == utilizadorId);

        if (utilizador is null || utilizador.Estado != EstadosUtilizador.Ativo)
        {
            throw new UnauthorizedAccessException(MensagemSessaoInvalida);
        }

        return await CriarUtilizadorAutenticadoAsync(utilizador);
    }

    public async Task<bool> SessaoValidaAsync(int utilizadorId, string perfil, string jti)
    {
        var conta = await _db.Utilizadores
            .AsNoTracking()
            .Where(u => u.Id == utilizadorId)
            .Select(u => new { u.Perfil, u.Estado })
            .FirstOrDefaultAsync();

        // Conta removida, bloqueada depois da emissão do token (RF009) ou com
        // perfil alterado (I066): o token deixa de dar acesso de imediato.
        if (conta is null
            || conta.Estado != EstadosUtilizador.Ativo
            || conta.Perfil != perfil)
        {
            return false;
        }

        var revogado = await _db.TokensRevogados
            .AsNoTracking()
            .AnyAsync(t => t.Jti == jti);

        return !revogado;
    }

    public async Task TerminarSessaoAsync(int utilizadorId, string jti, DateTime dataExpiracao)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(jti);

        // Idempotente (I028, PA-15): um segundo pedido com o mesmo jti não cria outra linha.
        if (await JtiRevogadoAsync(jti))
        {
            return;
        }

        _db.TokensRevogados.Add(new TokenRevogado
        {
            Jti = jti,
            UtilizadorId = utilizadorId,
            DataExpiracao = dataExpiracao,
            DataRevogacao = SemFracoesDeSegundo(_relogio.GetUtcNow().UtcDateTime)
        });

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // Dois pedidos simultâneos com o mesmo token: o segundo falha na chave
            // primária (PK_TokenRevogado), mas o token já ficou revogado pelo primeiro.
            _db.ChangeTracker.Clear();

            if (!await JtiRevogadoAsync(jti))
            {
                throw;
            }
        }

        LogSessaoTerminada(_logger, utilizadorId, null);
    }

    public async Task<int> RemoverRevogacoesExpiradasAsync(CancellationToken cancelamento = default)
    {
        var agora = _relogio.GetUtcNow().UtcDateTime;

        // Só as que expiraram ANTES de agora. Com ClockSkew zero, um token ainda é
        // aceite no próprio instante da expiração (exp >= agora), por isso a revogação
        // tem de se manter até lá; depois disso o token é rejeitado por ter expirado.
        var expiradas = await _db.TokensRevogados
            .Where(t => t.DataExpiracao < agora)
            .ToListAsync(cancelamento);

        if (expiradas.Count == 0)
        {
            return 0;
        }

        _db.TokensRevogados.RemoveRange(expiradas);
        await _db.SaveChangesAsync(cancelamento);

        return expiradas.Count;
    }

    private Task<bool> JtiRevogadoAsync(string jti) =>
        _db.TokensRevogados
            .AsNoTracking()
            .AnyAsync(t => t.Jti == jti);

    // A coluna é DATETIME2(0): sem frações de segundo, o valor gravado é o valor lido.
    private static DateTime SemFracoesDeSegundo(DateTime data) =>
        new(data.Ticks - (data.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc);

    private static void ValidarCamposObrigatorios([NotNull] string? email, [NotNull] string? password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(password))
        {
            var emFalta = new List<string>(2);

            if (string.IsNullOrWhiteSpace(email))
            {
                emFalta.Add("O e-mail é obrigatório.");
            }

            if (string.IsNullOrEmpty(password))
            {
                emFalta.Add("A palavra-passe é obrigatória.");
            }

            throw new ArgumentException(string.Join(" ", emFalta));
        }
    }

    private async Task<UtilizadorAutenticadoDto> CriarUtilizadorAutenticadoAsync(Utilizador utilizador)
    {
        var nome = string.Empty;
        int? clienteId = null;

        if (utilizador.Perfil == Perfis.Cliente)
        {
            var cliente = await _db.Clientes
                .AsNoTracking()
                .Where(c => c.UtilizadorId == utilizador.Id)
                .Select(c => new { c.Id, c.Nome })
                .FirstOrDefaultAsync();

            if (cliente is not null)
            {
                nome = cliente.Nome;
                clienteId = cliente.Id;
            }
        }
        else
        {
            nome = await _db.Funcionarios
                .AsNoTracking()
                .Where(f => f.UtilizadorId == utilizador.Id)
                .Select(f => f.Nome)
                .FirstOrDefaultAsync() ?? string.Empty;
        }

        return new UtilizadorAutenticadoDto
        {
            Id = utilizador.Id,
            Nome = nome,
            Email = utilizador.Email,
            Perfil = utilizador.Perfil,
            ClienteId = clienteId
        };
    }
}
