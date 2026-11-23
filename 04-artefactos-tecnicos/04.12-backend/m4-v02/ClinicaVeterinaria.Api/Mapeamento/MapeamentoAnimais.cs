using ClinicaVeterinaria.Api.DTOs.Animais;
using ClinicaVeterinaria.Application.Models.Animais;

namespace ClinicaVeterinaria.Api.Mapeamento;

internal static class MapeamentoAnimais
{
    public static PesquisaAnimais ParaModelo(this AnimalFiltroQuery origem) => new()
    {
        ClienteId = origem.ClienteId,
        Pesquisa = origem.Pesquisa,
        Especie = origem.Especie,
        Pagina = origem.Pagina,
        Tamanho = origem.Tamanho,
    };

    public static DadosAnimal ParaModelo(this UpdateAnimalDto origem) => new()
    {
        Nome = origem.Nome,
        Especie = origem.Especie,
        Raca = origem.Raca,
        Sexo = origem.Sexo,
        DataNascimento = origem.DataNascimento,
        Peso = origem.Peso,
        ClienteId = origem.ClienteId,
    };

    public static AnimalDto ParaDto(this DetalheAnimal origem) => new()
    {
        Id = origem.Id,
        Nome = origem.Nome,
        Especie = origem.Especie,
        Raca = origem.Raca,
        Sexo = origem.Sexo,
        DataNascimento = origem.DataNascimento,
        IdadeAnos = origem.IdadeAnos,
        Peso = origem.Peso,
        Cliente = origem.Cliente.ParaDto(),
        AtualizadoEm = origem.AtualizadoEm,
    };

    public static AnimalListItemDto ParaDto(this ResumoAnimal origem) => new()
    {
        Id = origem.Id,
        Nome = origem.Nome,
        Especie = origem.Especie,
        Raca = origem.Raca,
        IdadeAnos = origem.IdadeAnos,
        Peso = origem.Peso,
        Cliente = origem.Cliente.ParaDto(),
    };

    public static ClienteDetalheDto ParaDto(this ContactoDonoAnimal origem) => new()
    {
        Id = origem.Id,
        Nome = origem.Nome,
        Telefone = origem.Telefone,
    };

    public static ClienteResumoDto ParaDto(this DonoAnimal origem) => new()
    {
        Id = origem.Id,
        Nome = origem.Nome,
    };

    public static MeuAnimalDto ParaDto(this DetalheMeuAnimal origem) => new()
    {
        Id = origem.Id,
        Nome = origem.Nome,
        Especie = origem.Especie,
        Raca = origem.Raca,
        Sexo = origem.Sexo,
        DataNascimento = origem.DataNascimento,
        IdadeAnos = origem.IdadeAnos,
        Peso = origem.Peso,
    };

    public static MeuAnimalListItemDto ParaDto(this ResumoMeuAnimal origem) => new()
    {
        Id = origem.Id,
        Nome = origem.Nome,
        Especie = origem.Especie,
        Raca = origem.Raca,
        IdadeAnos = origem.IdadeAnos,
        Peso = origem.Peso,
    };
}
