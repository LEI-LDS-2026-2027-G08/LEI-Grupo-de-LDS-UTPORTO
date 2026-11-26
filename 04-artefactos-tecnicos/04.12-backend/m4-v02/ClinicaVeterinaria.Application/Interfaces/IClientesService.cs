using ClinicaVeterinaria.Application.Models.Clientes;

namespace ClinicaVeterinaria.Application.Interfaces;

public interface IClientesService
{
    /// <summary>EP-011 — pesquisa por correspondência parcial no nome, NIF, telefone ou e-mail.</summary>
    Task<PaginaClientes> PesquisarAsync(PesquisaClientes filtro);

    /// <summary>EP-012 — ficha do Cliente com os animais associados.</summary>
    Task<FichaCliente> ObterAsync(int id);

    /// <summary>EP-013 — cria o Cliente e a conta de acesso com o perfil Cliente.</summary>
    Task<FichaCliente> CriarAsync(NovoCliente pedido);

    /// <summary>EP-014 — atualiza o Cliente e, se mudar, o e-mail da conta associada.</summary>
    Task<FichaCliente> AtualizarAsync(int id, AlteracaoCliente pedido);
}
