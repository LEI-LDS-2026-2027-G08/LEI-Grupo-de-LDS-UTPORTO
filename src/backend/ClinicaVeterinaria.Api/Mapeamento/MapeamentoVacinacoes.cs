using ClinicaVeterinaria.Api.DTOs.Vacinacoes;
using ClinicaVeterinaria.Application.Models.Vacinacoes;

namespace ClinicaVeterinaria.Api.Mapeamento;

internal static class MapeamentoVacinacoes
{
    public static FiltroBoletim ParaModelo(this BoletimQuery origem) => new()
    {
        Tipo = origem.Tipo,
    };

    public static DadosAdministracao ParaModelo(this UpdateAdministracaoDto origem) => new()
    {
        Tipo = origem.Tipo,
        Produto = origem.Produto,
        Lote = origem.Lote,
        DataAdministracao = origem.DataAdministracao,
        IntervaloDias = origem.IntervaloDias,
    };

    public static AdministracaoDto ParaDto(this AdministracaoRegistada origem) => new()
    {
        Id = origem.Id,
        Tipo = origem.Tipo,
        Produto = origem.Produto,
        Lote = origem.Lote,
        DataAdministracao = origem.DataAdministracao,
        IntervaloDias = origem.IntervaloDias,
        DataProximaPrevista = origem.DataProximaPrevista,
        Veterinario = origem.Veterinario,
    };

    public static BoletimDto ParaDto(this BoletimProfilaxia origem) => new()
    {
        Animal = origem.Animal.ParaDto(),
        Administracoes = origem.Administracoes.Select(item => item.ParaDto()).ToArray(),
        Proximas = origem.Proximas.Select(item => item.ParaDto()).ToArray(),
    };

    public static HistoricoAdministracaoDto ParaDto(this AdministracaoAnterior origem) => new()
    {
        Tipo = origem.Tipo,
        Produto = origem.Produto,
        DataAdministracao = origem.DataAdministracao,
    };

    public static MeuBoletimDto ParaDto(this BoletimDoCliente origem) => new()
    {
        Historico = origem.Historico.Select(item => item.ParaDto()).ToArray(),
        Proximas = origem.Proximas.Select(item => item.ParaDto()).ToArray(),
    };

    public static ProximaAdministracaoDto ParaDto(this AdministracaoPrevista origem) => new()
    {
        Tipo = origem.Tipo,
        Produto = origem.Produto,
        DataProximaPrevista = origem.DataProximaPrevista,
    };
}
