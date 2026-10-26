using System.Collections.Concurrent;
using ClinicaVeterinaria.Api.Seguranca;
using ClinicaVeterinaria.Application.DTOs.Auth;
using ClinicaVeterinaria.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ClinicaVeterinaria.UnitTests.Seguranca;

/// <summary>
/// I064 — critério 4: a limpeza das revogações expiradas corre no arranque da API,
/// regista o resultado, não pára a API quando falha e o intervalo é validado.
/// </summary>
public class LimpezaTokensRevogadosServiceTests
{
    private const int EventoLimpezaConcluida = 3004;
    private const int EventoLimpezaFalhou = 3005;

    private static readonly TimeSpan TempoMaximo = TimeSpan.FromSeconds(10);

    [Fact(DisplayName = "I064 — A limpeza das revogações expiradas corre logo no arranque da API")]
    public async Task LimpezaCorreNoArranque()
    {
        var authService = new AuthServiceDeTeste();
        var logger = new LoggerDeTeste<LimpezaTokensRevogadosService>();
        using var servicos = CriarServicos(authService);
        using var limpeza = CriarLimpeza(servicos, logger);

        await limpeza.StartAsync(CancellationToken.None);
        await authService.LimpezaChamada.WaitAsync(TempoMaximo);
        await limpeza.StopAsync(CancellationToken.None);

        Assert.Equal(1, authService.Chamadas);
        Assert.Contains(logger.Eventos, e => e.Id == EventoLimpezaConcluida);
    }

    [Fact(DisplayName = "I064 — Uma falha da limpeza fica no log e não pára a API")]
    public async Task FalhaDaLimpezaNaoParaOServico()
    {
        var authService = new AuthServiceDeTeste { Falhar = true };
        var logger = new LoggerDeTeste<LimpezaTokensRevogadosService>();
        using var servicos = CriarServicos(authService);
        using var limpeza = CriarLimpeza(servicos, logger);

        await limpeza.StartAsync(CancellationToken.None);
        await authService.LimpezaChamada.WaitAsync(TempoMaximo);
        await limpeza.StopAsync(CancellationToken.None);

        // Se a exceção tivesse escapado, a tarefa do serviço terminava em erro (Faulted).
        Assert.True(limpeza.ExecuteTask?.IsCompletedSuccessfully);
        Assert.Contains(logger.Eventos, e => e.Id == EventoLimpezaFalhou);
    }

    [Fact(DisplayName = "I064 — Ao parar a API, a rotina de limpeza termina sem erro")]
    public async Task ParagemDaApiTerminaARotinaSemErro()
    {
        var authService = new AuthServiceDeTeste();
        using var servicos = CriarServicos(authService);
        using var limpeza = CriarLimpeza(servicos, new LoggerDeTeste<LimpezaTokensRevogadosService>());

        await limpeza.StartAsync(CancellationToken.None);
        await authService.LimpezaChamada.WaitAsync(TempoMaximo);
        await limpeza.StopAsync(CancellationToken.None);

        Assert.True(limpeza.ExecuteTask?.IsCompletedSuccessfully);
    }

    [Theory(DisplayName = "I064 — Um intervalo de limpeza fora de 1 a 1440 minutos impede o arranque")]
    [InlineData("0")]
    [InlineData("-5")]
    [InlineData("1441")]
    public void IntervaloForaDosLimitesERecusado(string intervalo)
    {
        using var servicos = CriarServicosComConfiguracao(intervalo);

        var excecao = Assert.Throws<OptionsValidationException>(
            () => servicos.GetRequiredService<IOptions<LimpezaTokensRevogadosOptions>>().Value);

        Assert.Contains("IntervaloMinutos", excecao.Message, StringComparison.Ordinal);
    }

    [Theory(DisplayName = "I064 — Um intervalo de limpeza entre 1 e 1440 minutos é aceite (60 por omissão)")]
    [InlineData(null, 60)]
    [InlineData("1", 1)]
    [InlineData("1440", 1440)]
    public void IntervaloDentroDosLimitesEAceite(string? intervalo, int esperado)
    {
        using var servicos = CriarServicosComConfiguracao(intervalo);

        var opcoes = servicos.GetRequiredService<IOptions<LimpezaTokensRevogadosOptions>>().Value;

        Assert.Equal(esperado, opcoes.IntervaloMinutos);
    }

    private static ServiceProvider CriarServicos(IAuthService authService)
    {
        var servicos = new ServiceCollection();
        servicos.AddScoped(_ => authService);

        return servicos.BuildServiceProvider();
    }

    private static LimpezaTokensRevogadosService CriarLimpeza(
        ServiceProvider servicos,
        ILogger<LimpezaTokensRevogadosService> logger) =>
        new(
            servicos.GetRequiredService<IServiceScopeFactory>(),
            Options.Create(new LimpezaTokensRevogadosOptions()),
            TimeProvider.System,
            logger);

    private static ServiceProvider CriarServicosComConfiguracao(string? intervalo)
    {
        var valores = new Dictionary<string, string?>();

        if (intervalo is not null)
        {
            valores[LimpezaTokensRevogadosOptions.Seccao + ":IntervaloMinutos"] = intervalo;
        }

        var configuracao = new ConfigurationBuilder()
            .AddInMemoryCollection(valores)
            .Build();

        var servicos = new ServiceCollection();
        servicos.AddLogging();
        servicos.AddLimpezaTokensRevogados(configuracao);

        return servicos.BuildServiceProvider();
    }

    /// <summary>Duplo do <see cref="IAuthService"/> que só implementa a limpeza.</summary>
    private sealed class AuthServiceDeTeste : IAuthService
    {
        private readonly TaskCompletionSource _limpezaChamada =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        private int _chamadas;

        public bool Falhar { get; init; }

        public int Chamadas => Volatile.Read(ref _chamadas);

        public Task LimpezaChamada => _limpezaChamada.Task;

        public Task<int> RemoverRevogacoesExpiradasAsync(CancellationToken cancelamento = default)
        {
            Interlocked.Increment(ref _chamadas);
            _limpezaChamada.TrySetResult();

            return Falhar
                ? Task.FromException<int>(new InvalidOperationException("Base de dados indisponível (simulada)."))
                : Task.FromResult(3);
        }

        public Task<LoginResponseDto> LoginAsync(LoginRequestDto pedido) =>
            throw new NotSupportedException();

        public Task<UtilizadorAutenticadoDto> ObterAutenticadoAsync(int utilizadorId) =>
            throw new NotSupportedException();

        public Task<bool> SessaoValidaAsync(int utilizadorId, string perfil, string jti) =>
            throw new NotSupportedException();

        public Task TerminarSessaoAsync(int utilizadorId, string jti, DateTime dataExpiracao) =>
            throw new NotSupportedException();
    }

    /// <summary>Logger que guarda os identificadores dos eventos registados.</summary>
    private sealed class LoggerDeTeste<T> : ILogger<T>
    {
        private readonly ConcurrentQueue<EventId> _eventos = new();

        public IReadOnlyCollection<EventId> Eventos => _eventos;

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter) =>
            _eventos.Enqueue(eventId);
    }
}
