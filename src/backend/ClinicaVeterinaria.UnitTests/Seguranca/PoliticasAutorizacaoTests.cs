using System.Security.Claims;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ClinicaVeterinaria.UnitTests.Seguranca;

/// <summary>
/// I063 — critérios 5 e 8: cada política aceita apenas os perfis previstos
/// e rejeita os restantes; sem autenticação nenhum endpoint é acessível.
/// </summary>
public class PoliticasAutorizacaoTests
{
    [Theory]
    [InlineData(PoliticasAutorizacao.Administrador, Perfis.Administrador)]
    [InlineData(PoliticasAutorizacao.Rececionista, Perfis.Rececionista)]
    [InlineData(PoliticasAutorizacao.Veterinario, Perfis.Veterinario)]
    [InlineData(PoliticasAutorizacao.AuxiliarVeterinario, Perfis.AuxiliarVeterinario)]
    [InlineData(PoliticasAutorizacao.Cliente, Perfis.Cliente)]
    [InlineData(PoliticasAutorizacao.Funcionario, Perfis.Funcionarios)]
    public async Task PoliticaAceitaSoOsPerfisPrevistos(string politica, string perfisAceites)
    {
        using var servicos = AjudaSeguranca.CriarServicos(AjudaSeguranca.NovaChave());
        var autorizacao = servicos.GetRequiredService<IAuthorizationService>();
        var aceites = perfisAceites.Split(',');

        foreach (var perfil in Perfis.Todos)
        {
            var resultado = await autorizacao.AuthorizeAsync(AjudaSeguranca.Principal(perfil), politica);

            Assert.True(
                resultado.Succeeded == aceites.Contains(perfil),
                $"Política {politica} com o perfil {perfil}: esperado {aceites.Contains(perfil)}.");
        }
    }

    [Fact]
    public async Task PoliticaDePropriedadeAceitaClienteComClienteId()
    {
        using var servicos = AjudaSeguranca.CriarServicos(AjudaSeguranca.NovaChave());
        var autorizacao = servicos.GetRequiredService<IAuthorizationService>();

        var resultado = await autorizacao.AuthorizeAsync(
            AjudaSeguranca.Principal(Perfis.Cliente, clienteId: 7),
            PoliticasAutorizacao.ClienteProprietario);

        Assert.True(resultado.Succeeded);
    }

    [Fact]
    public async Task PoliticaDePropriedadeRejeitaClienteSemClienteId()
    {
        using var servicos = AjudaSeguranca.CriarServicos(AjudaSeguranca.NovaChave());
        var autorizacao = servicos.GetRequiredService<IAuthorizationService>();

        var resultado = await autorizacao.AuthorizeAsync(
            AjudaSeguranca.Principal(Perfis.Cliente),
            PoliticasAutorizacao.ClienteProprietario);

        Assert.False(resultado.Succeeded);
    }

    [Fact]
    public async Task PoliticaDePropriedadeRejeitaFuncionariosMesmoComClienteId()
    {
        using var servicos = AjudaSeguranca.CriarServicos(AjudaSeguranca.NovaChave());
        var autorizacao = servicos.GetRequiredService<IAuthorizationService>();

        foreach (var perfil in Perfis.ListaFuncionarios)
        {
            var resultado = await autorizacao.AuthorizeAsync(
                AjudaSeguranca.Principal(perfil, clienteId: 7),
                PoliticasAutorizacao.ClienteProprietario);

            Assert.False(resultado.Succeeded, $"O perfil {perfil} não deve passar na política de propriedade.");
        }
    }

    [Fact]
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
}
