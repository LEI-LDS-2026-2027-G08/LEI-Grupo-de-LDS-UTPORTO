using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.DTOs.Consultas;
using ClinicaVeterinaria.Application.Seguranca;

namespace ClinicaVeterinaria.Application.Interfaces;

/// <summary>Agenda e consulta de marcações (EP-023, EP-024).</summary>
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
}
