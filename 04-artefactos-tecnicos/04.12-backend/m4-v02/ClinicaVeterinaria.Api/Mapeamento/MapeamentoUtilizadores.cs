using ClinicaVeterinaria.Api.DTOs.Utilizadores;
using ClinicaVeterinaria.Application.Models.Utilizadores;

namespace ClinicaVeterinaria.Api.Mapeamento;

internal static class MapeamentoUtilizadores
{
    public static PesquisaUtilizadores ParaModelo(this UtilizadorFiltroQuery origem) => new()
    {
        Perfil = origem.Perfil,
        Estado = origem.Estado,
        Pesquisa = origem.Pesquisa,
        Pagina = origem.Pagina,
        Tamanho = origem.Tamanho,
    };

    public static BloqueioUtilizador ParaModelo(this AlterarEstadoUtilizadorDto origem) => new()
    {
        Estado = origem.Estado,
        Motivo = origem.Motivo,
    };

    public static NovoUtilizador ParaModelo(this CreateUtilizadorDto origem) => new()
    {
        Nome = origem.Nome,
        Email = origem.Email,
        Telefone = origem.Telefone,
        Perfil = origem.Perfil,
        PodeRegistarApoio = origem.PodeRegistarApoio,
        PasswordInicial = origem.PasswordInicial,
    };

    public static AlteracaoUtilizador ParaModelo(this UpdateUtilizadorDto origem) => new()
    {
        Nome = origem.Nome,
        Email = origem.Email,
        Telefone = origem.Telefone,
        Perfil = origem.Perfil,
        PodeRegistarApoio = origem.PodeRegistarApoio,
    };

    public static UtilizadorDto ParaDto(this DetalheUtilizador origem) => new()
    {
        Id = origem.Id,
        Nome = origem.Nome,
        Email = origem.Email,
        Telefone = origem.Telefone,
        Perfil = origem.Perfil,
        Estado = origem.Estado,
        MotivoBloqueio = origem.MotivoBloqueio,
        PodeRegistarApoio = origem.PodeRegistarApoio,
        CriadoEm = origem.CriadoEm,
    };

    public static UtilizadorListItemDto ParaDto(this ResumoUtilizador origem) => new()
    {
        Id = origem.Id,
        Nome = origem.Nome,
        Email = origem.Email,
        Perfil = origem.Perfil,
        Estado = origem.Estado,
        PodeRegistarApoio = origem.PodeRegistarApoio,
    };
}
