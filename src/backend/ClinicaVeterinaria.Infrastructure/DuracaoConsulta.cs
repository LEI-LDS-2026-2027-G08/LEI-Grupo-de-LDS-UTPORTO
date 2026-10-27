using System.Globalization;

namespace ClinicaVeterinaria.Infrastructure;

/// <summary>
/// Duração das consultas usada na disponibilidade (I071). É a configuração gravada em EP-010
/// (I067) na tabela <c>ConfiguracaoSistema</c>, com a mesma chave e o mesmo valor por omissão
/// do <see cref="ClinicaService"/>.
/// </summary>
internal static class DuracaoConsulta
{
    public const string Chave = "DuracaoPadraoConsultaMinutos";
    public const int PorOmissao = 30;

    /// <summary>Minutos guardados na configuração; sem valor, ou com um valor inválido, 30.</summary>
    public static int Ler(string? valor) =>
        int.TryParse(valor, NumberStyles.Integer, CultureInfo.InvariantCulture, out var minutos) && minutos > 0
            ? minutos
            : PorOmissao;
}
