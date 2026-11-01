using System.Security.Claims;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ClinicaVeterinaria.UnitTests.Seguranca;

/// <summary>
/// I063 — critérios 5 e 8: cada política aceita apenas os perfis previstos e rejeita os restantes;
/// sem autenticação nenhum endpoint é acessível. Casos UT-F001-040 a UT-F001-049 da Especificação
/// de Testes Unitários (I028, secção 7.1.4), um por política e com uma linha por perfil (I086).
/// </summary>
public class PoliticasAutorizacaoTests
{
    [Theory(DisplayName = "UT-F001-040 — A política Administrador só aceita o perfil Administrador")]
    [InlineData(Perfis.Administrador, true)]
    [InlineData(Perfis.Rececionista, false)]
    [InlineData(Perfis.Veterinario, false)]
    [InlineData(Perfis.AuxiliarVeterinario, false)]
    [InlineData(Perfis.Cliente, false)]
    public Task PoliticaAdministrador(string perfil, bool aceite) =>
        VerificarPoliticaAsync(PoliticasAutorizacao.Administrador, perfil, aceite);

    [Theory(DisplayName = "UT-F001-041 — A política Rececionista só aceita o perfil Rececionista")]
    [InlineData(Perfis.Administrador, false)]
    [InlineData(Perfis.Rececionista, true)]
    [InlineData(Perfis.Veterinario, false)]
    [InlineData(Perfis.AuxiliarVeterinario, false)]
    [InlineData(Perfis.Cliente, false)]
    public Task PoliticaRececionista(string perfil, bool aceite) =>
        VerificarPoliticaAsync(PoliticasAutorizacao.Rececionista, perfil, aceite);

    [Theory(DisplayName = "UT-F001-042 — A política Veterinario só aceita o perfil Veterinario")]
    [InlineData(Perfis.Administrador, false)]
    [InlineData(Perfis.Rececionista, false)]
    [InlineData(Perfis.Veterinario, true)]
    [InlineData(Perfis.AuxiliarVeterinario, false)]
    [InlineData(Perfis.Cliente, false)]
    public Task PoliticaVeterinario(string perfil, bool aceite) =>
        VerificarPoliticaAsync(PoliticasAutorizacao.Veterinario, perfil, aceite);

    [Theory(DisplayName = "UT-F001-043 — A política AuxiliarVeterinario só aceita o perfil AuxiliarVeterinario")]
    [InlineData(Perfis.Administrador, false)]
    [InlineData(Perfis.Rececionista, false)]
    [InlineData(Perfis.Veterinario, false)]
    [InlineData(Perfis.AuxiliarVeterinario, true)]
    [InlineData(Perfis.Cliente, false)]
    public Task PoliticaAuxiliarVeterinario(string perfil, bool aceite) =>
        VerificarPoliticaAsync(PoliticasAutorizacao.AuxiliarVeterinario, perfil, aceite);

    [Theory(DisplayName = "UT-F001-044 — A política Cliente só aceita o perfil Cliente")]
    [InlineData(Perfis.Administrador, false)]
    [InlineData(Perfis.Rececionista, false)]
    [InlineData(Perfis.Veterinario, false)]
    [InlineData(Perfis.AuxiliarVeterinario, false)]
    [InlineData(Perfis.Cliente, true)]
    public Task PoliticaCliente(string perfil, bool aceite) =>
        VerificarPoliticaAsync(PoliticasAutorizacao.Cliente, perfil, aceite);

    [Theory(DisplayName = "UT-F001-045 — A política Funcionario aceita os quatro perfis internos e recusa o Cliente")]
    [InlineData(Perfis.Administrador, true)]
    [InlineData(Perfis.Rececionista, true)]
    [InlineData(Perfis.Veterinario, true)]
    [InlineData(Perfis.AuxiliarVeterinario, true)]
    [InlineData(Perfis.Cliente, false)]
    public Task PoliticaFuncionario(string perfil, bool aceite) =>
        VerificarPoliticaAsync(PoliticasAutorizacao.Funcionario, perfil, aceite);

    [Fact(DisplayName = "UT-F001-046 — A política de propriedade aceita um Cliente com a claim clienteId")]
    public async Task PoliticaDePropriedadeAceitaClienteComClienteId()
    {
        using var servicos = AjudaSeguranca.CriarServicos(AjudaSeguranca.NovaChave());
        var autorizacao = servicos.GetRequiredService<IAuthorizationService>();

        var resultado = await autorizacao.AuthorizeAsync(
            AjudaSeguranca.Principal(Perfis.Cliente, clienteId: 7),
            PoliticasAutorizacao.ClienteProprietario);

        Assert.True(resultado.Succeeded);
    }

    [Fact(DisplayName = "UT-F001-047 — A política de propriedade recusa um Cliente sem a claim clienteId")]
    public async Task PoliticaDePropriedadeRejeitaClienteSemClienteId()
    {
        using var servicos = AjudaSeguranca.CriarServicos(AjudaSeguranca.NovaChave());
        var autorizacao = servicos.GetRequiredService<IAuthorizationService>();

        var resultado = await autorizacao.AuthorizeAsync(
            AjudaSeguranca.Principal(Perfis.Cliente),
            PoliticasAutorizacao.ClienteProprietario);

        Assert.False(resultado.Succeeded);
    }

    [Theory(DisplayName = "UT-F001-048 — A política de propriedade recusa os perfis internos mesmo com clienteId")]
    [InlineData(Perfis.Administrador)]
    [InlineData(Perfis.Rececionista)]
    [InlineData(Perfis.Veterinario)]
    [InlineData(Perfis.AuxiliarVeterinario)]
    public async Task PoliticaDePropriedadeRejeitaFuncionariosMesmoComClienteId(string perfil)
    {
        using var servicos = AjudaSeguranca.CriarServicos(AjudaSeguranca.NovaChave());
        var autorizacao = servicos.GetRequiredService<IAuthorizationService>();

        var resultado = await autorizacao.AuthorizeAsync(
            AjudaSeguranca.Principal(perfil, clienteId: 7),
            PoliticasAutorizacao.ClienteProprietario);

        Assert.False(resultado.Succeeded);
    }

    [Fact(DisplayName = "UT-F001-049 — Sem autenticação, a política por omissão recusa o acesso (401)")]
    public async Task SemAutenticacaoAPoliticaPorOmissaoRecusaOAcesso()
    {
        using var servicos = AjudaSeguranca.CriarServicos(AjudaSeguranca.NovaChave());
        var autorizacao = servicos.GetRequiredService<IAuthorizationService>();
        var opcoes = servicos.GetRequiredService<IOptions<AuthorizationOptions>>().Value;

        Assert.NotNull(opcoes.FallbackPolicy);

        var anonimo = new ClaimsPrincipal(new ClaimsIdentity());
        var resultado = await autorizacao.AuthorizeAsync(anonimo, null, opcoes.FallbackPolicy);

        Assert.False(resultado.Succeeded);
    }

    private static async Task VerificarPoliticaAsync(string politica, string perfil, bool aceite)
    {
        using var servicos = AjudaSeguranca.CriarServicos(AjudaSeguranca.NovaChave());
        var autorizacao = servicos.GetRequiredService<IAuthorizationService>();

        var resultado = await autorizacao.AuthorizeAsync(AjudaSeguranca.Principal(perfil), politica);

        Assert.Equal(aceite, resultado.Succeeded);
    }
}
