namespace ClinicaVeterinaria.Application.DTOs.Vacinacoes;

/// <summary>
/// Boletim de um animal do Cliente autenticado (resposta de EP-044): o histórico (da administração
/// mais recente para a mais antiga) e as próximas previstas a partir de hoje. Sem registos, as duas
/// listas vêm vazias.
/// </summary>
public class MeuBoletimDto
{
    public IReadOnlyList<HistoricoAdministracaoDto> Historico { get; set; } = [];

    public IReadOnlyList<ProximaAdministracaoDto> Proximas { get; set; } = [];
}
