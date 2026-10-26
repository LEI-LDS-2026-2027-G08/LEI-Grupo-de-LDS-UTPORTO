using ClinicaVeterinaria.Application.DTOs.Animais;
using ClinicaVeterinaria.Application.DTOs.Comum;

namespace ClinicaVeterinaria.Application.Interfaces;

public interface IAnimaisService
{
    /// <summary>EP-015 — pesquisa pacientes por proprietário, nome e espécie.</summary>
    Task<PagedResultDto<AnimalListItemDto>> PesquisarAsync(AnimalFiltroQuery filtro);

    /// <summary>EP-016 — consulta a ficha administrativa do paciente.</summary>
    Task<AnimalDto> ObterAsync(int id);

    /// <summary>EP-017 — regista um paciente associado a um Cliente existente.</summary>
    Task<AnimalDto> CriarAsync(CreateAnimalDto pedido);

    /// <summary>EP-018 — atualiza os dados administrativos do paciente.</summary>
    Task<AnimalDto> AtualizarAsync(int id, UpdateAnimalDto pedido);
}
