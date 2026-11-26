using ClinicaVeterinaria.Api.DTOs.Auth;
using ClinicaVeterinaria.Application.Models.Auth;

namespace ClinicaVeterinaria.Api.Mapeamento;

internal static class MapeamentoAuth
{
    public static Credenciais ParaModelo(this LoginRequestDto origem) => new()
    {
        Email = origem.Email,
        Password = origem.Password,
    };

    public static LoginResponseDto ParaDto(this SessaoIniciada origem) => new()
    {
        Token = origem.Token,
        ExpiraEm = origem.ExpiraEm,
        Utilizador = origem.Utilizador.ParaDto(),
    };

    public static UtilizadorAutenticadoDto ParaDto(this UtilizadorAutenticado origem) => new()
    {
        Id = origem.Id,
        Nome = origem.Nome,
        Email = origem.Email,
        Perfil = origem.Perfil,
        ClienteId = origem.ClienteId,
    };

    public static UtilizadorResumoDto ParaDto(this IdentidadeSessao origem) => new()
    {
        Id = origem.Id,
        Nome = origem.Nome,
        Email = origem.Email,
        Perfil = origem.Perfil,
        ClienteId = origem.ClienteId,
    };
}
