namespace ClinicaVeterinaria.Application.Agenda;

/// <summary>
/// Valores do parâmetro <c>tipo</c> de EP-030 (GET /me/consultas): <c>futuras</c>, por omissão,
/// ou <c>historico</c> (Documentação de API v02; Modelo de Classes v02, <c>MinhasConsultasQuery</c>).
/// </summary>
/// <remarks>
/// Regra da I076 (RF028, RF029): uma marcação é futura quando começa agora ou depois e ainda está
/// <c>Pendente</c> ou <c>Confirmada</c>. Tudo o resto vai para o histórico: marcações passadas e as
/// marcações <c>Canceladas</c> ou <c>Realizadas</c>, mesmo que a data ainda não tenha chegado.
/// </remarks>
public static class TiposMinhasConsultas
{
    public const string Futuras = "futuras";
    public const string Historico = "historico";

    public const string MensagemTipoInvalido = "O tipo deve ser «futuras» ou «historico».";

    /// <summary>
    /// Devolve <see cref="Futuras"/> ou <see cref="Historico"/>, sem distinguir maiúsculas nem espaços
    /// à volta; sem valor, <see cref="Futuras"/>. Valor desconhecido: <c>null</c>.
    /// </summary>
    public static string? Normalizar(string? tipo)
    {
        if (string.IsNullOrWhiteSpace(tipo))
        {
            return Futuras;
        }

        var texto = tipo.Trim();

        if (string.Equals(texto, Futuras, StringComparison.OrdinalIgnoreCase))
        {
            return Futuras;
        }

        if (string.Equals(texto, Historico, StringComparison.OrdinalIgnoreCase))
        {
            return Historico;
        }

        return null;
    }

    /// <summary>
    /// <c>true</c> quando a marcação aparece em <see cref="Futuras"/>; caso contrário aparece em
    /// <see cref="Historico"/>. A mesma regra está escrita na consulta à base de dados
    /// (<c>MinhasConsultasService</c>) e é verificada pelos testes.
    /// </summary>
    public static bool EFutura(DateTime inicioUtc, string estado, DateTime agoraUtc) =>
        inicioUtc >= agoraUtc
        && (estado == EstadosConsulta.Pendente || estado == EstadosConsulta.Confirmada);
}
