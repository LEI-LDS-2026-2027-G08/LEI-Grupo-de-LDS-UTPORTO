using ClinicaVeterinaria.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace ClinicaVeterinaria.Api.Seguranca;

/// <summary>
/// Rotina periódica que apaga as revogações de tokens já expirados (I064, critério 4).
/// </summary>
/// <remarks>
/// <para>
/// Corre uma vez no arranque da API e depois de <see cref="LimpezaTokensRevogadosOptions.IntervaloMinutos"/>
/// em <see cref="LimpezaTokensRevogadosOptions.IntervaloMinutos"/> minutos.
/// </para>
/// <para>
/// Só apaga revogações de tokens cuja expiração já passou. Esses tokens já são rejeitados
/// pela validação da data de expiração, por isso a limpeza nunca devolve o acesso a um
/// token terminado por logout.
/// </para>
/// <para>
/// Uma falha (por exemplo, base de dados indisponível) fica registada no log e não pára
/// a API: a limpeza é repetida no ciclo seguinte.
/// </para>
/// </remarks>
public sealed partial class LimpezaTokensRevogadosService : BackgroundService
{
    private readonly IServiceScopeFactory _fabricaScopes;
    private readonly TimeProvider _relogio;
    private readonly ILogger<LimpezaTokensRevogadosService> _logger;
    private readonly TimeSpan _intervalo;

    public LimpezaTokensRevogadosService(
        IServiceScopeFactory fabricaScopes,
        IOptions<LimpezaTokensRevogadosOptions> opcoes,
        TimeProvider relogio,
        ILogger<LimpezaTokensRevogadosService> logger)
    {
        ArgumentNullException.ThrowIfNull(opcoes);

        _fabricaScopes = fabricaScopes;
        _relogio = relogio;
        _logger = logger;
        _intervalo = TimeSpan.FromMinutes(opcoes.Value.IntervaloMinutos);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var temporizador = new PeriodicTimer(_intervalo, _relogio);

        try
        {
            do
            {
                await LimparAsync(stoppingToken);
            }
            while (await temporizador.WaitForNextTickAsync(stoppingToken));
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Paragem normal da API.
        }
    }

    private async Task LimparAsync(CancellationToken cancelamento)
    {
        try
        {
            // O AuthService e o ClinicaDbContext são scoped: um scope novo por limpeza.
            await using var scope = _fabricaScopes.CreateAsyncScope();
            var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

            var removidas = await authService.RemoverRevogacoesExpiradasAsync(cancelamento);

            LogLimpezaConcluida(_logger, removidas);
        }
        catch (OperationCanceledException) when (cancelamento.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception excecao)
        {
            LogLimpezaFalhou(_logger, excecao);
        }
    }

    [LoggerMessage(
        EventId = 3004,
        Level = LogLevel.Information,
        Message = "Limpeza das revogações de tokens expirados: {Removidas} registo(s) apagado(s)")]
    private static partial void LogLimpezaConcluida(ILogger logger, int removidas);

    [LoggerMessage(
        EventId = 3005,
        Level = LogLevel.Warning,
        Message = "A limpeza das revogações de tokens expirados falhou; será repetida no ciclo seguinte")]
    private static partial void LogLimpezaFalhou(ILogger logger, Exception excecao);
}
