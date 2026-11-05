using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Api.Middleware;

/// <summary>
/// Resposta 400 da validação automática do [ApiController] no mesmo formato
/// do ExceptionHandlingMiddleware (I058), em vez do ProblemDetails por omissão.
/// </summary>
public static class RespostaValidacaoModelo
{
    private const string DetalheCorpoInvalido =
        "O corpo do pedido está em falta ou não é um JSON válido.";

    public static IActionResult Criar(ActionContext contexto)
    {
        ArgumentNullException.ThrowIfNull(contexto);

        var mensagens = new List<string>();
        var corpoInvalido = false;

        foreach (var (chave, entrada) in contexto.ModelState)
        {
            if (entrada is null || entrada.Errors.Count == 0)
            {
                continue;
            }

            // Corpo ausente (chave vazia) ou JSON mal formado (chaves "$" / "$.campo"):
            // as mensagens do ASP.NET Core vêm em inglês e com detalhes internos.
            if (chave.Length == 0 || chave.StartsWith('$'))
            {
                corpoInvalido = true;
                continue;
            }

            foreach (var erro in entrada.Errors)
            {
                if (!string.IsNullOrWhiteSpace(erro.ErrorMessage) && !mensagens.Contains(erro.ErrorMessage))
                {
                    mensagens.Add(erro.ErrorMessage);
                }
            }
        }

        var detalhe = corpoInvalido || mensagens.Count == 0
            ? DetalheCorpoInvalido
            : string.Join(" ", mensagens);

        return new BadRequestObjectResult(new
        {
            code = StatusCodes.Status400BadRequest,
            message = "Pedido inválido.",
            detail = detalhe
        });
    }
}
