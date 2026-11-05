using System.Net;
using System.Text.Json;
using ClinicaVeterinaria.Application.Exceptions;

namespace ClinicaVeterinaria.Api.Middleware;

public sealed partial class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            LogUnhandledException(
                _logger,
                exception,
                context.Request.Method,
                context.Request.Path);

            await HandleExceptionAsync(context, exception);
        }
    }

    [LoggerMessage(
        EventId = 2001,
        Level = LogLevel.Error,
        Message = "Erro não tratado durante o pedido {Method} {Path}")]
    private static partial void LogUnhandledException(
        ILogger logger,
        Exception exception,
        string method,
        PathString path);

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var statusCode = exception switch
        {
            ArgumentException => HttpStatusCode.BadRequest,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            ForbiddenException => HttpStatusCode.Forbidden,
            KeyNotFoundException => HttpStatusCode.NotFound,
            InvalidOperationException => HttpStatusCode.Conflict,
            PreconditionFailedException => HttpStatusCode.PreconditionFailed,
            UnprocessableEntityException => HttpStatusCode.UnprocessableEntity,
            _ => HttpStatusCode.InternalServerError
        };

        var response = new
        {
            code = (int)statusCode,
            message = GetMessage(statusCode),
            detail = exception.Message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }

    private static string GetMessage(HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.BadRequest => "Pedido inválido.",
            HttpStatusCode.Unauthorized => "Autenticação necessária.",
            HttpStatusCode.Forbidden => "Não tem permissão para executar esta operação.",
            HttpStatusCode.NotFound => "Recurso não encontrado.",
            HttpStatusCode.Conflict => "Conflito ao processar o pedido.",
            HttpStatusCode.PreconditionFailed => "A pré-condição indicada não corresponde à versão atual do recurso.",
            HttpStatusCode.UnprocessableEntity => "Não foi possível processar os dados fornecidos.",
            _ => "Ocorreu um erro interno no servidor."
        };
    }
}
