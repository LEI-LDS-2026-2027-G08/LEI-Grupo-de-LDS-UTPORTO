using System.Reflection;
using ClinicaVeterinaria.Api.Controllers;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.UnitTests.Veterinarios;

/// <summary>
/// I071 — critério 7: EP-021 e EP-022 só aceitam a Rececionista. A política em si (só o perfil
/// Rececionista passa) está verificada em UT-F001-041 (<c>PoliticasAutorizacaoTests</c>).
/// </summary>
public class VeterinariosControllerTests
{
    [Fact(DisplayName = "I071 — EP-021 e EP-022 exigem a política Rececionista e nenhuma ação a dispensa")]
    public void ControladorExigePerfilRececionista()
    {
        var tipo = typeof(VeterinariosController);

        var autorizacao = Assert.Single(tipo.GetCustomAttributes<AuthorizeAttribute>(inherit: true));
        Assert.Equal(PoliticasAutorizacao.Rececionista, autorizacao.Policy);

        var acoes = tipo.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.Equal(2, acoes.Length);
        Assert.All(acoes, acao =>
        {
            Assert.Empty(acao.GetCustomAttributes<AllowAnonymousAttribute>(inherit: true));
            Assert.Empty(acao.GetCustomAttributes<AuthorizeAttribute>(inherit: true));
        });
    }

    [Fact(DisplayName = "I071 — As rotas são GET /veterinarios e GET /veterinarios/{id}/disponibilidade")]
    public void RotasDoContrato()
    {
        var tipo = typeof(VeterinariosController);

        Assert.Equal("veterinarios", Assert.Single(tipo.GetCustomAttributes<RouteAttribute>()).Template);
        Assert.Null(Assert.Single(tipo.GetMethod(nameof(VeterinariosController.Listar))!.GetCustomAttributes<HttpGetAttribute>()).Template);
        Assert.Equal(
            "{id:int}/disponibilidade",
            Assert.Single(tipo.GetMethod(nameof(VeterinariosController.ObterDisponibilidade))!.GetCustomAttributes<HttpGetAttribute>()).Template);
    }
}
