using ClinicaVeterinaria.Application.Models.Comum;
using ClinicaVeterinaria.Application.Models.Utilizadores;

namespace ClinicaVeterinaria.Application.Interfaces;

public interface IUtilizadoresService
{
    /// <summary>EP-004 — contas de funcionários, filtradas por perfil, estado e nome ou e-mail.</summary>
    Task<PaginaResultados<ResumoUtilizador>> ListarAsync(PesquisaUtilizadores filtro);

    /// <summary>EP-005 — conta de funcionário; 404 se não existir ou não for de funcionário.</summary>
    Task<DetalheUtilizador> ObterAsync(int id);

    /// <summary>EP-006 — cria a conta ativa e o registo de Funcionario na mesma transação.</summary>
    Task<ResumoUtilizador> CriarAsync(NovoUtilizador pedido);

    /// <summary>
    /// EP-007 — atualiza os dados e o perfil da conta de funcionário. O
    /// <paramref name="administradorId"/> é o do token e impede o Administrador de
    /// remover o próprio perfil de Administrador.
    /// </summary>
    Task<DetalheUtilizador> AtualizarAsync(int id, int administradorId, AlteracaoUtilizador pedido);

    /// <summary>
    /// EP-008 — bloqueia a conta de funcionário. O <paramref name="administradorId"/>
    /// é o do token e impede o Administrador de bloquear a própria conta.
    /// </summary>
    Task BloquearAsync(int id, int administradorId, BloqueioUtilizador pedido);
}
