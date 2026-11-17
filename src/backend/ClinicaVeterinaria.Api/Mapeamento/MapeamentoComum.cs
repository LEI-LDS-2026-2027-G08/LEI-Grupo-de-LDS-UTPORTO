using ClinicaVeterinaria.Api.DTOs.Comum;
using ClinicaVeterinaria.Application.Models.Comum;

namespace ClinicaVeterinaria.Api.Mapeamento;

internal static class MapeamentoComum
{
    public static AnimalRefDto ParaDto(this ReferenciaAnimal origem) => new()
    {
        Id = origem.Id,
        Nome = origem.Nome,
        Especie = origem.Especie,
    };

    public static ClienteRefDto ParaDto(this ReferenciaCliente origem) => new()
    {
        Id = origem.Id,
        Nome = origem.Nome,
        Telefone = origem.Telefone,
    };

    public static VeterinarioRefDto ParaDto(this ReferenciaVeterinario origem) => new()
    {
        Id = origem.Id,
        Nome = origem.Nome,
    };

    public static PagedResultDto<TDto> ParaDto<TModelo, TDto>(
        this PaginaResultados<TModelo> origem,
        Func<TModelo, TDto> converter) => new()
        {
            Itens = origem.Itens.Select(converter).ToArray(),
            Pagina = origem.Pagina,
            Tamanho = origem.Tamanho,
            Total = origem.Total,
        };
}
