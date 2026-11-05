namespace ClinicaVeterinaria.Application.Agenda;

/// <summary>
/// Estados de uma marcação, com os valores da enumeração <c>EstadoConsulta</c> da Documentação
/// de API v02 (2.2) e da restrição CK_Marcacao_Estado.
/// </summary>
public static class EstadosConsulta
{
    public const string Pendente = "Pendente";
    public const string Confirmada = "Confirmada";
    public const string Realizada = "Realizada";
    public const string Cancelada = "Cancelada";

    public static readonly IReadOnlyList<string> Todos =
    [
        Pendente,
        Confirmada,
        Realizada,
        Cancelada
    ];
}
