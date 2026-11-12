using System.Text.Json;
using ClinicaVeterinaria.Api.Middleware;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.UnitTests.Seguranca;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging.Abstractions;

namespace ClinicaVeterinaria.UnitTests.Middleware;

/// <summary>
/// I086 — tratamento de erros e registos dos pedidos: casos UT-TRV-006 a UT-TRV-010 da
/// Especificação de Testes Unitários (I028, secção 7.6.2), com o
/// <see cref="ExceptionHandlingMiddleware"/>, a <see cref="RespostaValidacaoModelo"/> e o
/// <see cref="RequestLoggingMiddleware"/>.
/// </summary>
public class ErrosERegistosTests
{
    [Theory(DisplayName =
        "UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro")]
    [InlineData(nameof(ArgumentException), StatusCodes.Status400BadRequest)]
    [InlineData(nameof(UnauthorizedAccessException), StatusCodes.Status401Unauthorized)]
    [InlineData(nameof(ForbiddenException), StatusCodes.Status403Forbidden)]
    [InlineData(nameof(KeyNotFoundException), StatusCodes.Status404NotFound)]
    [InlineData(nameof(InvalidOperationException), StatusCodes.Status409Conflict)]
    [InlineData(nameof(UnprocessableEntityException), StatusCodes.Status422UnprocessableEntity)]
    public async Task ExcecaoPrevistaDaOCodigoHttp(string tipo, int esperado)
    {
        const string mensagem = "Mensagem da regra violada.";

        var (estado, corpo) = await ExecutarComExcecaoAsync(CriarExcecao(tipo, mensagem));

        Assert.Equal(esperado, estado);
        Assert.Equal(esperado, corpo.GetProperty("code").GetInt32());
        Assert.False(string.IsNullOrWhiteSpace(corpo.GetProperty("message").GetString()));
        Assert.Equal(mensagem, corpo.GetProperty("detail").GetString());
    }

    [Fact(DisplayName = "UT-TRV-007 — Uma exceção inesperada dá 500 com mensagem genérica, sem a mensagem interna")]
    public async Task ExcecaoInesperadaNaoExpoeAMensagemInterna()
    {
        // A I028 usa uma NullReferenceException; o analisador (CA2201) não deixa criá-la, por isso
        // usa-se outra exceção que o middleware também não conhece e que, como ela, dá 500.
        var (estado, corpo) = await ExecutarComExcecaoAsync(new NotSupportedException("segredo interno"));

        Assert.Equal(StatusCodes.Status500InternalServerError, estado);
        Assert.DoesNotContain("segredo interno", corpo.GetRawText(), StringComparison.Ordinal);
    }

    [Fact(DisplayName =
        "UT-TRV-008 — Erros de validação em email e password dão 400 com as duas mensagens em português no detail")]
    public void ErrosDeValidacaoDaoAsDuasMensagens()
    {
        var estado = new ModelStateDictionary();
        estado.AddModelError("Email", "O e-mail é obrigatório.");
        estado.AddModelError("Password", "A palavra-passe é obrigatória.");

        var corpo = CorpoDaValidacao(estado);

        Assert.Equal("O e-mail é obrigatório. A palavra-passe é obrigatória.", corpo.GetProperty("detail").GetString());
    }

    [Theory(DisplayName =
        "UT-TRV-009 — Corpo em falta ou JSON mal formado dão 400 com a mensagem genérica, sem mensagens internas")]
    [InlineData("")]
    [InlineData("$.email")]
    public void CorpoInvalidoDaMensagemGenerica(string chave)
    {
        var estado = new ModelStateDictionary();
        estado.AddModelError(chave, "The JSON value could not be converted to System.String. Path: $.email");

        var corpo = CorpoDaValidacao(estado);

        Assert.Equal("O corpo do pedido está em falta ou não é um JSON válido.",
            corpo.GetProperty("detail").GetString());
    }

    [Fact(DisplayName =
        "UT-TRV-010 — O registo do pedido tem método, caminho, código e duração, mas nunca o token nem o corpo")]
    public async Task RegistoDoPedidoNaoTemTokenNemCorpo()
    {
        var logger = new LoggerDeTeste<RequestLoggingMiddleware>();
        var middleware = new RequestLoggingMiddleware(
            contexto =>
            {
                contexto.Response.StatusCode = StatusCodes.Status200OK;
                return Task.CompletedTask;
            },
            logger);
        var pedido = new DefaultHttpContext();
        pedido.Request.Method = HttpMethods.Post;
        pedido.Request.Path = "/auth/login";
        pedido.Request.Headers.Authorization = "Bearer abc.def.ghi";
        pedido.Request.Body = new MemoryStream("{\"email\":\"a@b.pt\",\"password\":\"Segredo#123\"}"u8.ToArray());

        await middleware.InvokeAsync(pedido);

        var concluido = logger.Registos.Single(r => r.Evento.Id == 1002).Mensagem;
        Assert.Contains("POST /auth/login -> 200 em", concluido, StringComparison.Ordinal);
        Assert.DoesNotContain(logger.Registos, r => r.Mensagem.Contains("abc.def.ghi", StringComparison.Ordinal)
                                                    || r.Mensagem.Contains("Segredo#123", StringComparison.Ordinal));
    }

    private static Exception CriarExcecao(string tipo, string mensagem) => tipo switch
    {
        nameof(ArgumentException) => new ArgumentException(mensagem),
        nameof(UnauthorizedAccessException) => new UnauthorizedAccessException(mensagem),
        nameof(ForbiddenException) => new ForbiddenException(mensagem),
        nameof(KeyNotFoundException) => new KeyNotFoundException(mensagem),
        nameof(InvalidOperationException) => new InvalidOperationException(mensagem),
        nameof(UnprocessableEntityException) => new UnprocessableEntityException(mensagem),
        _ => throw new ArgumentOutOfRangeException(nameof(tipo), tipo, "Tipo de exceção sem caso de teste.")
    };

    private static async Task<(int Estado, JsonElement Corpo)> ExecutarComExcecaoAsync(Exception excecao)
    {
        var middleware = new ExceptionHandlingMiddleware(
            _ => throw excecao,
            NullLogger<ExceptionHandlingMiddleware>.Instance);
        var contexto = new DefaultHttpContext();
        contexto.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(contexto);

        contexto.Response.Body.Position = 0;
        using var documento = await JsonDocument.ParseAsync(contexto.Response.Body);

        return (contexto.Response.StatusCode, documento.RootElement.Clone());
    }

    private static JsonElement CorpoDaValidacao(ModelStateDictionary estado)
    {
        var contexto = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor(), estado);

        var resultado = Assert.IsType<BadRequestObjectResult>(RespostaValidacaoModelo.Criar(contexto));

        return JsonSerializer.SerializeToElement(resultado.Value);
    }
}
