using System.Diagnostics;

namespace ClinicaVeterinaria.Api.Middleware;

public sealed partial class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        LogRequestStarted(
            _logger,
            context.Request.Method,
            context.Request.Path);

        await _next(context);

        stopwatch.Stop();

        LogRequestCompleted(
            _logger,
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds);
    }

    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "Pedido iniciado: {Method} {Path}")]
    private static partial void LogRequestStarted(
        ILogger logger,
        string method,
        PathString path);

    [LoggerMessage(
        EventId = 1002,
        Level = LogLevel.Information,
        Message = "Pedido concluído: {Method} {Path} -> {StatusCode} em {ElapsedMilliseconds} ms")]
    private static partial void LogRequestCompleted(
        ILogger logger,
        string method,
        PathString path,
        int statusCode,
        long elapsedMilliseconds);
}
