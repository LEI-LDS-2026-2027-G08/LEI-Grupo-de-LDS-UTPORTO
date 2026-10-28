using System.Reflection;
using System.Security.Claims;
using ClinicaVeterinaria.Api.Controllers;
using ClinicaVeterinaria.Application.DTOs.RegistosClinicos;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.UnitTests.Consultas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.UnitTests.RegistosClinicos;

/// <summary>
/// I077 — critério 6: EP-031 e EP-032 só aceitam o perfil Veterinário (403 para os restantes,
/// incluindo o Auxiliar Veterinário). A política em si está verificada em UT-F001-041
/// (<c>PoliticasAutorizacaoTests</c>). O Veterinário passado ao serviço é sempre o do token.
/// </summary>
public class RegistosClinicosControllerTests
{
    [Fact(DisplayName = "I077 — EP-031 e EP-032 exigem a política Veterinário e nenhuma ação a dispensa")]
    public void ControladorExigePerfilVeterinario()
    {
        var tipo = typeof(RegistosClinicosController);

        var autorizacao = Assert.Single(tipo.GetCustomAttributes<AuthorizeAttribute>(inherit: true));
        Assert.Equal(PoliticasAutorizacao.Veterinario, autorizacao.Policy);
        Assert.Null(autorizacao.Roles);

        var acoes = tipo.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.Equal(2, acoes.Length);
        Assert.All(acoes, acao =>
        {
            Assert.Empty(acao.GetCustomAttributes<AllowAnonymousAttribute>(inherit: true));
            Assert.Empty(acao.GetCustomAttributes<AuthorizeAttribute>(inherit: true));
        });
    }

    [Fact(DisplayName = "I077 — As rotas são POST /registos-clinicos e PUT /registos-clinicos/{id}")]
    public void RotasDoContrato()
    {
        var tipo = typeof(RegistosClinicosController);

        Assert.Equal("registos-clinicos", Assert.Single(tipo.GetCustomAttributes<RouteAttribute>()).Template);
        Assert.Null(Assert.Single(tipo.GetMethod(nameof(RegistosClinicosController.Criar))!.GetCustomAttributes<HttpPostAttribute>()).Template);
        Assert.Equal(
            "{id:int}",
            Assert.Single(tipo.GetMethod(nameof(RegistosClinicosController.Atualizar))!.GetCustomAttributes<HttpPutAttribute>()).Template);
    }

    [Fact(DisplayName = "I077 — EP-031 devolve 201 com Location /registos-clinicos/{id} e usa o Veterinário do token")]
    public async Task CriarDevolve201()
    {
        var servico = new RegistosClinicosServiceDeTeste();
        var controlador = CriarControlador(servico, AcessoConsultasTests.Principal(Perfis.Veterinario, veterinarioId: 4));
        var pedido = new CreateRegistoClinicoDto { ConsultaId = 41, Diagnostico = "Otite externa" };

        var resposta = await controlador.Criar(pedido);

        var criado = Assert.IsType<CreatedResult>(resposta.Result);
        Assert.Equal("/registos-clinicos/18", criado.Location);
        Assert.Same(servico.Registo, criado.Value);
        Assert.Same(pedido, servico.UltimoPedido);
        Assert.Equal(4, servico.UltimoVeterinarioId);
    }

    [Fact(DisplayName = "I077 — EP-032 devolve 200 com o registo e usa o Veterinário do token")]
    public async Task AtualizarDevolve200()
    {
        var servico = new RegistosClinicosServiceDeTeste();
        var controlador = CriarControlador(servico, AcessoConsultasTests.Principal(Perfis.Veterinario, veterinarioId: 4));
        var pedido = new UpdateRegistoClinicoDto { Diagnostico = "Otite externa" };

        var resposta = await controlador.Atualizar(18, pedido);

        var ok = Assert.IsType<OkObjectResult>(resposta.Result);
        Assert.Same(servico.Registo, ok.Value);
        Assert.Equal(18, servico.UltimoId);
        Assert.Same(pedido, servico.UltimoPedido);
        Assert.Equal(4, servico.UltimoVeterinarioId);
    }

    [Fact(DisplayName = "I077 — um token de Veterinário sem a claim veterinarioId dá 403 sem chamar o serviço")]
    public async Task SemClaimVeterinarioDevolve403()
    {
        var servico = new RegistosClinicosServiceDeTeste();
        var controlador = CriarControlador(servico, AcessoConsultasTests.Principal(Perfis.Veterinario));

        await Assert.ThrowsAsync<ForbiddenException>(() => controlador.Criar(new CreateRegistoClinicoDto()));
        await Assert.ThrowsAsync<ForbiddenException>(() => controlador.Atualizar(18, new UpdateRegistoClinicoDto()));
        Assert.Null(servico.UltimoPedido);
    }

    private static RegistosClinicosController CriarControlador(RegistosClinicosServiceDeTeste servico, ClaimsPrincipal utilizador) =>
        new(servico)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = utilizador }
            }
        };

    private sealed class RegistosClinicosServiceDeTeste : IRegistosClinicosService
    {
        public RegistoClinicoDto Registo { get; } = new() { Id = 18, ConsultaId = 41 };

        public UpdateRegistoClinicoDto? UltimoPedido { get; private set; }

        public int? UltimoId { get; private set; }

        public int? UltimoVeterinarioId { get; private set; }

        public Task<RegistoClinicoDto> CriarAsync(CreateRegistoClinicoDto pedido, int veterinarioId)
        {
            UltimoPedido = pedido;
            UltimoVeterinarioId = veterinarioId;

            return Task.FromResult(Registo);
        }

        public Task<RegistoClinicoDto> AtualizarAsync(int id, UpdateRegistoClinicoDto pedido, int veterinarioId)
        {
            UltimoId = id;
            UltimoPedido = pedido;
            UltimoVeterinarioId = veterinarioId;

            return Task.FromResult(Registo);
        }
    }
}
