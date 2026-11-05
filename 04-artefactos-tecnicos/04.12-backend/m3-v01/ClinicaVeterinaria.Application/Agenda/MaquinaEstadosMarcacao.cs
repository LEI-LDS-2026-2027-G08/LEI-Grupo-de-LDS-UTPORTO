using ClinicaVeterinaria.Application.Exceptions;

namespace ClinicaVeterinaria.Application.Agenda;

/// <summary>
/// Regras de transição de estado das marcações (EP-027).
/// Centraliza todas as transições permitidas para evitar regras duplicadas.
/// </summary>
public static class MaquinaEstadosMarcacao
{
    public const string MensagemPendenteNaoPermitido =
        "O estado Pendente não pode ser escolhido manualmente.";

    public static string NormalizarDestino(string? estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
        {
            throw new ArgumentException("O estado é obrigatório.");
        }

        var texto = estado.Trim();

        var normalizado = EstadosConsulta.Todos.FirstOrDefault(
            e => string.Equals(e, texto, StringComparison.OrdinalIgnoreCase));

        if (normalizado is null)
        {
            throw new ArgumentException(
                $"O estado '{texto}' é inválido. Valores permitidos: " +
                $"{EstadosConsulta.Confirmada}, {EstadosConsulta.Realizada}, {EstadosConsulta.Cancelada}.");
        }

        if (normalizado == EstadosConsulta.Pendente)
        {
            throw new ArgumentException(MensagemPendenteNaoPermitido);
        }

        return normalizado;
    }

    public static bool PodeTransitar(string estadoAtual, string novoEstado)
    {
        return (estadoAtual, novoEstado) switch
        {
            (EstadosConsulta.Pendente, EstadosConsulta.Confirmada) => true,
            (EstadosConsulta.Pendente, EstadosConsulta.Cancelada) => true,

            (EstadosConsulta.Confirmada, EstadosConsulta.Realizada) => true,
            (EstadosConsulta.Confirmada, EstadosConsulta.Cancelada) => true,

            _ => false
        };
    }

    public static void ValidarTransicao(string estadoAtual, string novoEstado)
    {
        if (!PodeTransitar(estadoAtual, novoEstado))
        {
            throw new UnprocessableEntityException(
                $"A transição de '{estadoAtual}' para '{novoEstado}' não é permitida.");
        }
    }
}
