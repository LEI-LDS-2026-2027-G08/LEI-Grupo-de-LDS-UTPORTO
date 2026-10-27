using System.Reflection;
using System.Security.Claims;
using ClinicaVeterinaria.Api.Controllers;
using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.DTOs.Consultas;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.UnitTests.Consultas;

/// <summary>
/// I072 — <see cref="ConsultasController"/>: perfis (critério 1), rotas, acesso lido do token e o
/// cabeçalho <c>ETag</c> de EP-024 (Documentação de API v02, secção 5.1).
/// </summary>
public class ConsultasControllerTests
{
    [Fact(DisplayName = "I072 — EP-023 e EP-024 só aceitam Rececionista, Veterinário e Auxiliar Veterinário")]
    public void ControladorExigeOsTresPerfis()
    {
        var tipo = typeof(ConsultasController);

        var autorizacao = Assert.Single(tipo.GetCustomAttributes<AuthorizeAttribute>(inherit: true));
        Assert.Null(autorizacao.Policy);
        Assert.Equal(
            Perfis.AuxiliarVeterinario + "," + Perfis.Rececionista + "," + Perfis.Veterinario,
            string.Join(',', autorizacao.Roles!.Split(',').Order(StringComparer.Ordinal)));

        var acoes = tipo.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.Equal(2, acoes.Length);
        Assert.All(acoes, acao =>
        {
            Assert.Empty(acao.GetCustomAttributes<AllowAnonymousAttribute>(inherit: true));
            Assert.Empty(acao.GetCustomAttributes<AuthorizeAttribute>(inherit: true));
        });
    }

    [Fact(DisplayName = "I072 — As rotas são GET /consultas e GET /consultas/{id}")]
    public void RotasDoContrato()
    {
        var tipo = typeof(ConsultasController);

        Assert.Equal("consultas", Assert.Single(tipo.GetCustomAttributes<RouteAttribute>()).Template);
        Assert.Null(Assert.Single(tipo.GetMethod(nameof(ConsultasController.GetConsultas))!.GetCustomAttributes<HttpGetAttribute>()).Template);
        Assert.Equal(
            "{id:int}",
            Assert.Single(tipo.GetMethod(nameof(ConsultasController.GetConsulta))!.GetCustomAttributes<HttpGetAttribute>()).Template);
    }

    [Fact(DisplayName = "I072 — EP-023 passa ao serviço o Veterinário do token, e não o do pedido")]
    public async Task AgendaUsaOVeterinarioDoToken()
    {
        var servico = new ConsultasServiceDeTeste();
        var controlador = CriarControlador(servico, AcessoConsultasTests.Principal(Perfis.Veterinario, veterinarioId: 4));
        var filtro = new ConsultaFiltroQuery { VeterinarioId = 5 };

        var resposta = await controlador.GetConsultas(filtro);

        Assert.IsType<OkObjectResult>(resposta.Result);
        Assert.Same(filtro, servico.UltimoFiltro);
        Assert.Equal(4, servico.UltimoAcesso!.VeterinarioId);
    }

    [Fact(DisplayName = "I072 — EP-024 devolve a marcação no corpo e a versão no cabeçalho ETag")]
    public async Task MarcacaoComETag()
    {
        var servico = new ConsultasServiceDeTeste();
        var controlador = CriarControlador(servico, AcessoConsultasTests.Principal(Perfis.Rececionista));

        var resposta = await controlador.GetConsulta(41);

        var ok = Assert.IsType<OkObjectResult>(resposta.Result);
        Assert.Same(servico.Consulta, ok.Value);
        Assert.Equal("\"AAAAAAAAB9E=\"", controlador.Response.Headers.ETag.ToString());
        Assert.Equal(41, servico.UltimoId);
        Assert.Null(servico.UltimoAcesso!.VeterinarioId);
    }

    [Fact(DisplayName = "I072 — perfil sem acesso à agenda dá 403 antes de chamar o serviço")]
    public async Task PerfilSemAcessoNaoChamaOServico()
    {
        var servico = new ConsultasServiceDeTeste();
        var controlador = CriarControlador(servico, AcessoConsultasTests.Principal(Perfis.Administrador));

        await Assert.ThrowsAsync<ForbiddenException>(() => controlador.GetConsulta(41));
        Assert.Null(servico.UltimoAcesso);
    }

    private static ConsultasController CriarControlador(ConsultasServiceDeTeste servico, ClaimsPrincipal utilizador) =>
        new(servico)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = utilizador }
            }
        };

    private sealed class ConsultasServiceDeTeste : IConsultasService
    {
        public ConsultaDto Consulta { get; } = new() { Id = 41 };

        public ConsultaFiltroQuery? UltimoFiltro { get; private set; }

        public AcessoConsultas? UltimoAcesso { get; private set; }

        public int? UltimoId { get; private set; }

        public Task<PagedResultDto<ConsultaListItemDto>> ObterConsultasAsync(ConsultaFiltroQuery filtro, AcessoConsultas acesso)
        {
            UltimoFiltro = filtro;
            UltimoAcesso = acesso;

            return Task.FromResult(new PagedResultDto<ConsultaListItemDto> { Pagina = 1, Tamanho = 20 });
        }

        public Task<ConsultaComVersao> ObterConsultaAsync(int id, AcessoConsultas acesso)
        {
            UltimoId = id;
            UltimoAcesso = acesso;

            return Task.FromResult(new ConsultaComVersao(Consulta, "\"AAAAAAAAB9E=\""));
        }
    }
}
