using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.DTOs.Utilizadores;

namespace ClinicaVeterinaria.Application.Interfaces;

public interface IUtilizadoresService
{
    /// <summary>EP-004 — contas de funcionários, filtradas por perfil, estado e nome ou e-mail.</summary>
    Task<PagedResultDto<UtilizadorListItemDto>> ListarAsync(UtilizadorFiltroQuery filtro);

    /// <summary>EP-005 — conta de funcionário; 404 se não existir ou não for de funcionário.</summary>
    Task<UtilizadorDto> ObterAsync(int id);

    /// <summary>EP-006 — cria a conta ativa e o registo de Funcionario na mesma transação.</summary>
    Task<UtilizadorListItemDto> CriarAsync(CreateUtilizadorDto pedido);
}
