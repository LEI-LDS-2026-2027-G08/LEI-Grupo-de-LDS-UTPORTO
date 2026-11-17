using System.Reflection;
using System.Security.Claims;
using ClinicaVeterinaria.Api.Controllers;
using ClinicaVeterinaria.Api.DTOs.Vacinacoes;
using ClinicaVeterinaria.Api.Seguranca;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Models.Vacinacoes;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.UnitTests.Consultas;
using ClinicaVeterinaria.UnitTests.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.UnitTests.Vacinacao;

/// <summary>
/// I082 — autorização, rotas e respostas de <see cref="VacinacoesController"/> (EP-042, EP-043 e a
/// atualização do RF056: só Veterinário) e de <see cref="MinhasVacinacoesController"/> (EP-044: só
/// Cliente, com o Cliente do token). As políticas em si estão verificadas em UT-F001-041
/// (<c>PoliticasAutorizacaoTests</c>).
/// </summary>
public class VacinacoesControllerTests
{
    [Fact(DisplayName = "I082 — EP-042, EP-043 e RF056 exigem a política Veterinário e nenhuma ação a dispensa")]
    public void BoletimWebSoVeterinario()
    {
        var tipo = typeof(VacinacoesController);

        var autorizacao = Assert.Single(tipo.GetCustomAttributes<AuthorizeAttribute>(inherit: true));
        Assert.Equal(PoliticasAutorizacao.Veterinario, autorizacao.Policy);
        Assert.Null(autorizacao.Roles);

        var acoes = tipo.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.Equal(3, acoes.Length);
        Assert.All(acoes, acao =>
        {
            Assert.Empty(acao.GetCustomAttributes<AllowAnonymousAttribute>(inherit: true));
            Assert.Empty(acao.GetCustomAttributes<AuthorizeAttribute>(inherit: true));
        });
    }

    [Fact(DisplayName = "I082 — EP-044 usa a política de propriedade do Cliente (ClienteAutenticadoControllerBase)")]
    public void BoletimMobileSoCliente()
    {
        var tipo = typeof(MinhasVacinacoesController);

        Assert.True(tipo.IsSubclassOf(typeof(ClienteAutenticadoControllerBase)));
        var autorizacao = Assert.Single(tipo.GetCustomAttributes<AuthorizeAttribute>(inherit: true));
        Assert.Equal(PoliticasAutorizacao.ClienteProprietario, autorizacao.Policy);
        Assert.Empty(tipo.GetMethod(nameof(MinhasVacinacoesController.ObterBoletim))!.GetCustomAttributes<AllowAnonymousAttribute>(inherit: true));
    }

    [Fact(DisplayName = "I082 — As rotas são /animais/{id}/vacinacoes (GET, POST), /animais/{id}/vacinacoes/{administracaoId} (PUT) e /me/animais/{id}/vacinacoes (GET)")]
    public void RotasDoContrato()
    {
        var web = typeof(VacinacoesController);
        var mobile = typeof(MinhasVacinacoesController);

        Assert.Equal("animais/{id:int}/vacinacoes", Assert.Single(web.GetCustomAttributes<RouteAttribute>()).Template);
        Assert.Null(Assert.Single(web.GetMethod(nameof(VacinacoesController.ObterBoletim))!.GetCustomAttributes<HttpGetAttribute>()).Template);
        Assert.Null(Assert.Single(web.GetMethod(nameof(VacinacoesController.Registar))!.GetCustomAttributes<HttpPostAttribute>()).Template);
        Assert.Equal(
            "{administracaoId:int}",
            Assert.Single(web.GetMethod(nameof(VacinacoesController.Atualizar))!.GetCustomAttributes<HttpPutAttribute>()).Template);

        Assert.Equal("me/animais/{id:int}/vacinacoes", Assert.Single(mobile.GetCustomAttributes<RouteAttribute>()).Template);
        Assert.Null(Assert.Single(mobile.GetMethod(nameof(MinhasVacinacoesController.ObterBoletim))!.GetCustomAttributes<HttpGetAttribute>()).Template);
    }

    [Fact(DisplayName = "I082 — EP-042 devolve 200 com o boletim e passa o animal e o filtro ao serviço")]
    public async Task ObterBoletimDevolve200()
    {
        var servico = new VacinacoesServiceDeTeste();
        var controlador = CriarControladorWeb(servico, AcessoConsultasTests.Principal(Perfis.Veterinario, veterinarioId: 4));
        var query = new BoletimQuery { Tipo = "Vacina" };

        var resposta = await controlador.ObterBoletim(3, query);

        var ok = Assert.IsType<OkObjectResult>(resposta.Result);
        Assert.Equivalent(servico.Boletim, Assert.IsType<BoletimDto>(ok.Value), strict: true);
        Assert.Equal(3, servico.UltimoAnimalId);
        Assert.Equivalent(query, servico.UltimaQuery);
    }

    [Fact(DisplayName = "I082 — EP-043 devolve 201 com Location /animais/{id}/vacinacoes/{administracaoId} e usa o Veterinário do token")]
    public async Task RegistarDevolve201()
    {
        var servico = new VacinacoesServiceDeTeste();
        var controlador = CriarControladorWeb(servico, AcessoConsultasTests.Principal(Perfis.Veterinario, veterinarioId: 4));
        var pedido = new CreateAdministracaoDto { Tipo = "Vacina", Produto = "Vacina antirrábica", Lote = "RB-2291" };

        var resposta = await controlador.Registar(3, pedido);

        var criado = Assert.IsType<CreatedResult>(resposta.Result);
        Assert.Equal("/animais/3/vacinacoes/22", criado.Location);
        Assert.Equivalent(servico.Administracao, Assert.IsType<AdministracaoDto>(criado.Value), strict: true);
        Assert.Equivalent(pedido, servico.UltimoPedido);
        Assert.Equal(3, servico.UltimoAnimalId);
        Assert.Equal(4, servico.UltimoVeterinarioId);
    }

    [Fact(DisplayName = "I082 — RF056 devolve 200 com a administração e passa o animal, o registo e o Veterinário do token")]
    public async Task AtualizarDevolve200()
    {
        var servico = new VacinacoesServiceDeTeste();
        var controlador = CriarControladorWeb(servico, AcessoConsultasTests.Principal(Perfis.Veterinario, veterinarioId: 4));
        var pedido = new UpdateAdministracaoDto { Tipo = "Vacina", Produto = "Vacina antirrábica", Lote = "RB-2292" };

        var resposta = await controlador.Atualizar(3, 22, pedido);

        var ok = Assert.IsType<OkObjectResult>(resposta.Result);
        Assert.Equivalent(servico.Administracao, Assert.IsType<AdministracaoDto>(ok.Value), strict: true);
        Assert.Equal(3, servico.UltimoAnimalId);
        Assert.Equal(22, servico.UltimaAdministracaoId);
        Assert.Equivalent(pedido, servico.UltimoPedido);
        Assert.Equal(4, servico.UltimoVeterinarioId);
    }

    [Fact(DisplayName = "I082 — um token de Veterinário sem a claim veterinarioId dá 403 sem chamar o serviço")]
    public async Task SemClaimVeterinarioDevolve403()
    {
        var servico = new VacinacoesServiceDeTeste();
        var controlador = CriarControladorWeb(servico, AcessoConsultasTests.Principal(Perfis.Veterinario));

        await Assert.ThrowsAsync<ForbiddenException>(() => controlador.Registar(3, new CreateAdministracaoDto()));
        await Assert.ThrowsAsync<ForbiddenException>(() => controlador.Atualizar(3, 22, new UpdateAdministracaoDto()));
        Assert.Null(servico.UltimoPedido);
    }

    [Fact(DisplayName = "I082 — EP-044 devolve 200 e usa o Cliente do token, nunca do pedido")]
    public async Task ObterBoletimDoClienteDevolve200()
    {
        var servico = new VacinacoesServiceDeTeste();
        var controlador = new MinhasVacinacoesController(servico)
        {
            ControllerContext = Contexto(AjudaSeguranca.Principal(Perfis.Cliente, clienteId: 7)),
        };

        var resposta = await controlador.ObterBoletim(3);

        var ok = Assert.IsType<OkObjectResult>(resposta.Result);
        Assert.Equivalent(servico.MeuBoletim, Assert.IsType<MeuBoletimDto>(ok.Value), strict: true);
        Assert.Equal(7, servico.UltimoClienteId);
        Assert.Equal(3, servico.UltimoAnimalId);
    }

    [Fact(DisplayName = "I082 — EP-044 com um token de Cliente sem a claim clienteId dá 403 sem chamar o serviço")]
    public async Task SemClaimClienteDevolve403()
    {
        var servico = new VacinacoesServiceDeTeste();
        var controlador = new MinhasVacinacoesController(servico)
        {
            ControllerContext = Contexto(AjudaSeguranca.Principal(Perfis.Cliente)),
        };

        await Assert.ThrowsAsync<ForbiddenException>(() => controlador.ObterBoletim(3));
        Assert.Null(servico.UltimoClienteId);
    }

    private static VacinacoesController CriarControladorWeb(VacinacoesServiceDeTeste servico, ClaimsPrincipal utilizador) =>
        new(servico) { ControllerContext = Contexto(utilizador) };

    private static ControllerContext Contexto(ClaimsPrincipal utilizador) =>
        new() { HttpContext = new DefaultHttpContext { User = utilizador } };

    private sealed class VacinacoesServiceDeTeste : IVacinacoesService
    {
        public BoletimProfilaxia Boletim { get; } = new();

        public BoletimDoCliente MeuBoletim { get; } = new();

        public AdministracaoRegistada Administracao { get; } = new() { Id = 22, Tipo = "Vacina" };

        public FiltroBoletim? UltimaQuery { get; private set; }

        public DadosAdministracao? UltimoPedido { get; private set; }

        public int? UltimoAnimalId { get; private set; }

        public int? UltimaAdministracaoId { get; private set; }

        public int? UltimoVeterinarioId { get; private set; }

        public int? UltimoClienteId { get; private set; }

        public Task<BoletimProfilaxia> ObterBoletimAsync(int animalId, FiltroBoletim query)
        {
            UltimoAnimalId = animalId;
            UltimaQuery = query;

            return Task.FromResult(Boletim);
        }

        public Task<AdministracaoRegistada> RegistarAsync(int animalId, DadosAdministracao pedido, int veterinarioId)
        {
            UltimoAnimalId = animalId;
            UltimoPedido = pedido;
            UltimoVeterinarioId = veterinarioId;

            return Task.FromResult(Administracao);
        }

        public Task<AdministracaoRegistada> AtualizarAsync(int animalId, int administracaoId, DadosAdministracao pedido, int veterinarioId)
        {
            UltimoAnimalId = animalId;
            UltimaAdministracaoId = administracaoId;
            UltimoPedido = pedido;
            UltimoVeterinarioId = veterinarioId;

            return Task.FromResult(Administracao);
        }

        public Task<BoletimDoCliente> ObterBoletimDoClienteAsync(int clienteId, int animalId)
        {
            UltimoClienteId = clienteId;
            UltimoAnimalId = animalId;

            return Task.FromResult(MeuBoletim);
        }
    }
}
