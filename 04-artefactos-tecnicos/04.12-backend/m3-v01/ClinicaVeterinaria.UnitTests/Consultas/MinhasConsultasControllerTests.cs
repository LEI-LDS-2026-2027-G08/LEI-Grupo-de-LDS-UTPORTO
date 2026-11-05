using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using ClinicaVeterinaria.Api.Controllers;
using ClinicaVeterinaria.Api.Seguranca;
using ClinicaVeterinaria.Application.DTOs.Consultas;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ClinicaVeterinaria.UnitTests.Consultas;

/// <summary>
/// I076 — EP-030 (GET /me/consultas): só o perfil Cliente, através da política de propriedade de
/// <see cref="ClienteAutenticadoControllerBase"/> (UT-F001-044 e UT-F001-046 verificam a política);
/// o Cliente passado ao serviço é sempre o do token; não há operações de escrita (critério 4).
/// </summary>
public class MinhasConsultasControllerTests
{
    private static readonly string[] SoGet = ["GET"];

    [Fact(DisplayName = "I076 — EP-030 usa a política de propriedade do Cliente e nenhuma ação a dispensa")]
    public void ControladorExigePoliticaDoCliente()
    {
        var tipo = typeof(MinhasConsultasController);

        Assert.True(tipo.IsSubclassOf(typeof(ClienteAutenticadoControllerBase)));

        var autorizacao = Assert.Single(tipo.GetCustomAttributes<AuthorizeAttribute>(inherit: true));
        Assert.Equal(PoliticasAutorizacao.ClienteProprietario, autorizacao.Policy);

        var acao = Assert.Single(tipo.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
        Assert.Empty(acao.GetCustomAttributes<AllowAnonymousAttribute>(inherit: true));
        Assert.Empty(acao.GetCustomAttributes<AuthorizeAttribute>(inherit: true));
    }

    [Fact(DisplayName = "I076 — a única operação é GET /me/consultas (sem criar, alterar, confirmar ou cancelar)")]
    public void SoExisteOGet()
    {
        var tipo = typeof(MinhasConsultasController);

        Assert.Equal("me/consultas", Assert.Single(tipo.GetCustomAttributes<RouteAttribute>()).Template);

        var acao = Assert.Single(tipo.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
        Assert.Equal(nameof(MinhasConsultasController.Listar), acao.Name);
        var verbos = acao.GetCustomAttributes<HttpMethodAttribute>().SelectMany(a => a.HttpMethods);
        Assert.Equal(SoGet, verbos);
    }

    [Fact(DisplayName = "I076 — EP-030 devolve 200 com a lista do serviço e usa o Cliente do token")]
    public async Task ListarUsaOClienteDoToken()
    {
        var servico = new MinhasConsultasServiceDeTeste();
        var controlador = new MinhasConsultasController(servico)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = PrincipalCliente(clienteId: 7) }
            }
        };
        var query = new MinhasConsultasQuery { Tipo = "historico", AnimalId = 3 };

        var resposta = await controlador.Listar(query);

        var ok = Assert.IsType<OkObjectResult>(resposta.Result);
        Assert.Same(servico.Lista, ok.Value);
        Assert.Equal(7, servico.UltimoClienteId);
        Assert.Same(query, servico.UltimaQuery);
    }

    private static ClaimsPrincipal PrincipalCliente(int clienteId) =>
        new(new ClaimsIdentity(
            [
                new Claim(ClaimsClinica.UtilizadorId, "20"),
                new Claim(ClaimsClinica.Perfil, Perfis.Cliente),
                new Claim(ClaimsClinica.ClienteId, clienteId.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimsClinica.Jti, "jti-de-teste")
            ],
            authenticationType: "Bearer",
            nameType: ClaimsClinica.UtilizadorId,
            roleType: ClaimsClinica.Perfil));

    private sealed class MinhasConsultasServiceDeTeste : IMinhasConsultasService
    {
        public IReadOnlyList<MinhaConsultaDto> Lista { get; } = [new MinhaConsultaDto { Id = 33 }];

        public int? UltimoClienteId { get; private set; }

        public MinhasConsultasQuery? UltimaQuery { get; private set; }

        public Task<IReadOnlyList<MinhaConsultaDto>> ListarAsync(int clienteId, MinhasConsultasQuery query)
        {
            UltimoClienteId = clienteId;
            UltimaQuery = query;
            return Task.FromResult(Lista);
        }
    }
}
