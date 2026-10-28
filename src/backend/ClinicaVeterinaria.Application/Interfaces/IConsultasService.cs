using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.DTOs.Consultas;
using ClinicaVeterinaria.Application.Seguranca;

namespace ClinicaVeterinaria.Application.Interfaces;

/// <summary>Agenda, consulta e criação de marcações (EP-023 a EP-025).</summary>
public interface IConsultasService
{
    /// <summary>
    /// EP-023 — marcações que começam no período, por data e hora e, em caso de empate, por
    /// identificador. Para um Veterinário, só as que lhe estão atribuídas, qualquer que seja o
    /// <c>veterinarioId</c> do filtro. Filtro inválido: 400.
    /// </summary>
    Task<PagedResultDto<ConsultaListItemDto>> ObterConsultasAsync(ConsultaFiltroQuery filtro, AcessoConsultas acesso);

    /// <summary>
    /// EP-024 — marcação com as notas de preparação e a versão (<c>ETag</c>). Inexistente: 404;
    /// marcação de outro Veterinário, para um Veterinário: 403.
    /// </summary>
    Task<ConsultaComVersao> ObterConsultaAsync(int id, AcessoConsultas acesso);

    /// <summary>
    /// EP-025 — cria a marcação no estado <c>Pendente</c>, com a duração configurada, se o intervalo
    /// couber inteiro num período do horário e não se sobrepuser a outra marcação não cancelada do
    /// Veterinário. Devolve a marcação no formato de EP-024, com a versão (<c>ETag</c>).
    /// Campos em falta ou data não futura: 400; animal ou Veterinário ativo inexistente: 404;
    /// fora do horário ou Veterinário indisponível: 422.
    /// </summary>
    /// <param name="pedido">Animal, Veterinário, data e hora (UTC) e motivo.</param>
    /// <param name="criadaPorId">Utilizador autenticado (Rececionista) que cria a marcação.</param>
    Task<ConsultaComVersao> CriarConsultaAsync(CreateConsultaDto pedido, int criadaPorId);

    /// <summary>
    /// EP-026 — altera o Veterinário, a data/hora e o motivo de uma marcação Pendente ou Confirmada.
    /// Revalida horário e disponibilidade.
    /// If-Match é opcional; quando indicado e não corresponde à versão atual: 412.
    /// </summary>
    Task<ConsultaComVersao> AtualizarConsultaAsync(
        int id,
        UpdateConsultaDto pedido,
        string? ifMatch);

    /// <summary>
    /// EP-027 — altera o estado da marcação segundo a máquina de estados.
    /// If-Match é opcional; quando indicado e não corresponde à versão atual: 412.
    /// </summary>
    Task<string> AtualizarEstadoAsync(
        int id,
        UpdateEstadoConsultaDto pedido,
        string? ifMatch);
}
