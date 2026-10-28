using System.Text.Json;
using ClinicaVeterinaria.Api.Middleware;
using ClinicaVeterinaria.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace ClinicaVeterinaria.UnitTests.Middleware;

/// <summary>
/// I074 — tratamento HTTP de falhas de pré-condição usadas por If-Match.
/// </summary>
public class ExceptionHandlingMiddlewareTests
{
  [Fact(DisplayName = "I074 — PreconditionFailedException é convertida em HTTP 412")]
  public async Task PreconditionFailedDevolve412()
  {
    const string detalhe =
        "A versão indicada em If-Match não corresponde à versão atual da marcação.";

    RequestDelegate next =
        _ => throw new PreconditionFailedException(detalhe);

    var middleware = new ExceptionHandlingMiddleware(
        next,
        NullLogger<ExceptionHandlingMiddleware>.Instance);

    var contexto = new DefaultHttpContext();

    contexto.Response.Body = new MemoryStream();

    await middleware.InvokeAsync(contexto);

    Assert.Equal(
        StatusCodes.Status412PreconditionFailed,
        contexto.Response.StatusCode);

    Assert.Equal(
        "application/json",
        contexto.Response.ContentType);

    contexto.Response.Body.Position = 0;

    using var documento =
        await JsonDocument.ParseAsync(
            contexto.Response.Body);

    var raiz = documento.RootElement;

    Assert.Equal(
        StatusCodes.Status412PreconditionFailed,
        raiz.GetProperty("code").GetInt32());

    Assert.Equal(
        "A pré-condição indicada não corresponde à versão atual do recurso.",
        raiz.GetProperty("message").GetString());

    Assert.Equal(
        detalhe,
        raiz.GetProperty("detail").GetString());
  }
}
