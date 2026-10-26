using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using ClinicaVeterinaria.Api.Seguranca;
using ClinicaVeterinaria.Application.DTOs.Auth;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ClinicaVeterinaria.UnitTests.Seguranca;

/// <summary>Relógio fixo para testar a validade dos tokens sem esperar 8 horas.</summary>
internal sealed class RelogioFixo : TimeProvider
{
    private readonly DateTimeOffset _agora;

    public RelogioFixo(DateTimeOffset agora)
    {
        _agora = agora;
    }

    public override DateTimeOffset GetUtcNow() => _agora;
}

internal static class AjudaSeguranca
{
    public const string Issuer = "ClinicaVeterinaria.Api";
    public const string Audience = "ClinicaVeterinaria.Clients";

    public static string NovaChave() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

    public static JwtOptions CriarOpcoes(string chave) => new()
    {
        Key = chave,
        Issuer = Issuer,
        Audience = Audience,
        ExpirationHours = 8
    };

    public static JwtTokenService CriarTokenService(string chave, TimeProvider relogio) =>
        new(Options.Create(CriarOpcoes(chave)), relogio);

    /// <summary>Contentor de serviços com a configuração real de autenticação e autorização da API.</summary>
    public static ServiceProvider CriarServicos(string chave)
    {
        var configuracao = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = chave,
                ["Jwt:Issuer"] = Issuer,
                ["Jwt:Audience"] = Audience,
                ["Jwt:ExpirationHours"] = "8"
            })
            .Build();

        var servicos = new ServiceCollection();
        servicos.AddLogging();
        servicos.AddAutenticacaoJwt(configuracao);

        return servicos.BuildServiceProvider();
    }

    public static UtilizadorAutenticadoDto Utilizador(int id, string perfil, int? clienteId = null) => new()
    {
        Id = id,
        Nome = "Utilizador de teste",
        Email = "teste" + id.ToString(CultureInfo.InvariantCulture) + "@clinica-g08.test",
        Perfil = perfil,
        ClienteId = clienteId
    };

    /// <summary>Utilizador autenticado tal como o JwtBearer o constrói a partir do token.</summary>
    public static ClaimsPrincipal Principal(string perfil, int? clienteId = null)
    {
        var claims = new List<Claim>
        {
            new(ClaimsClinica.UtilizadorId, "1"),
            new(ClaimsClinica.Perfil, perfil),
            new(ClaimsClinica.Jti, "jti-de-teste")
        };

        if (clienteId is int id)
        {
            claims.Add(new Claim(ClaimsClinica.ClienteId, id.ToString(CultureInfo.InvariantCulture)));
        }

        var identidade = new ClaimsIdentity(
            claims,
            authenticationType: "Bearer",
            nameType: ClaimsClinica.UtilizadorId,
            roleType: ClaimsClinica.Perfil);

        return new ClaimsPrincipal(identidade);
    }
}
