using ClinicaVeterinaria.Application.DTOs.Clientes;
using ClinicaVeterinaria.Application.DTOs.Comum;

namespace ClinicaVeterinaria.Application.Interfaces;

public interface IClientesService
{
    /// <summary>EP-011 — pesquisa por correspondência parcial no nome, NIF, telefone ou e-mail.</summary>
    Task<PagedResultDto<ClienteListItemDto>> PesquisarAsync(ClienteFiltroQuery filtro);

    /// <summary>EP-012 — ficha do Cliente com os animais associados.</summary>
    Task<ClienteDto> ObterAsync(int id);

    /// <summary>EP-013 — cria o Cliente e a conta de acesso com o perfil Cliente.</summary>
    Task<ClienteDto> CriarAsync(CreateClienteDto pedido);

    /// <summary>EP-014 — atualiza o Cliente e, se mudar, o e-mail da conta associada.</summary>
    Task<ClienteDto> AtualizarAsync(int id, UpdateClienteDto pedido);
}
