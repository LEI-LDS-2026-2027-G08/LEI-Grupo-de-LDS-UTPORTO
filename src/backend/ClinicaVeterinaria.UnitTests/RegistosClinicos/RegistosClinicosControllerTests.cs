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
/// I077 e I078 — EP-031 e EP-032 só aceitam o perfil Veterinário (403 para os restantes, incluindo
/// o Auxiliar Veterinário; I077, critério 6). EP-033 e EP-034 aceitam o Veterinário e o Auxiliar
/// Veterinário, só de leitura (I078, critérios 2, 3 e 5). As políticas em si estão verificadas em
/// UT-F001-041 (<c>PoliticasAutorizacaoTests</c>). O Veterinário passado ao serviço é sempre o do
/// token.
/// </summary>
public class RegistosClinicosControllerTests
{
    [Fact(DisplayName = "I078 — o controlador aceita só o Veterinário e o Auxiliar Veterinário e nenhuma ação dispensa a autenticação")]
    public void ControladorSoVeterinarioEAuxiliar()
    {
        var tipo = typeof(RegistosClinicosController);

        var autorizacao = Assert.Single(tipo.GetCustomAttributes<AuthorizeAttribute>(inherit: true));
        Assert.Null(autorizacao.Policy);
        Assert.Equal(
            Perfis.AuxiliarVeterinario + "," + Perfis.Veterinario,
            string.Join(',', autorizacao.Roles!.Split(',').Order(StringComparer.Ordinal)));

        var acoes = tipo.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Assert.Equal(4, acoes.Length);
        Assert.All(acoes, acao => Assert.Empty(acao.GetCustomAttributes<AllowAnonymousAttribute>(inherit: true)));
    }

    [Theory(DisplayName = "I077 e I078 — EP-031 e EP-032 exigem também a política Veterinário (o Auxiliar recebe 403)")]
    [InlineData(nameof(RegistosClinicosController.Criar))]
    [InlineData(nameof(RegistosClinicosController.Atualizar))]
    public void EscritaSoVeterinario(string nomeAcao)
    {
        var acao = typeof(RegistosClinicosController).GetMethod(nomeAcao)!;

        var autorizacao = Assert.Single(acao.GetCustomAttributes<AuthorizeAttribute>(inherit: true));
        Assert.Equal(PoliticasAutorizacao.Veterinario, autorizacao.Policy);
        Assert.Null(autorizacao.Roles);
    }

    [Theory(DisplayName = "I078 — EP-033 e EP-034 não acrescentam restrições: Veterinário e Auxiliar Veterinário leem")]
    [InlineData(nameof(RegistosClinicosController.Obter))]
    [InlineData(nameof(RegistosClinicosController.ObterHistorico))]
    public void LeituraVeterinarioEAuxiliar(string nomeAcao)
    {
        var acao = typeof(RegistosClinicosController).GetMethod(nomeAcao)!;

        Assert.Empty(acao.GetCustomAttributes<AuthorizeAttribute>(inherit: true));
    }

    [Fact(DisplayName = "I078 — o Auxiliar Veterinário só tem operações de leitura (GET) neste controlador")]
    public void AuxiliarSoTemLeitura()
    {
        var acoes = typeof(RegistosClinicosController)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        // As ações que o Auxiliar alcança (sem política própria) são todas GET.
        var semPolitica = acoes.Where(a => !a.GetCustomAttributes<AuthorizeAttribute>(inherit: true).Any()).ToList();
        Assert.Equal(2, semPolitica.Count);
        Assert.All(semPolitica, acao =>
        {
            Assert.Single(acao.GetCustomAttributes<HttpGetAttribute>());
            Assert.Empty(acao.GetCustomAttributes<HttpPostAttribute>());
            Assert.Empty(acao.GetCustomAttributes<HttpPutAttribute>());
            Assert.Empty(acao.GetCustomAttributes<HttpPatchAttribute>());
            Assert.Empty(acao.GetCustomAttributes<HttpDeleteAttribute>());
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

    [Fact(DisplayName = "I078 — As rotas são GET /registos-clinicos/{id} e GET /animais/{id}/historico-clinico")]
    public void RotasDeLeituraDoContrato()
    {
        var tipo = typeof(RegistosClinicosController);

        Assert.Equal(
            "{id:int}",
            Assert.Single(tipo.GetMethod(nameof(RegistosClinicosController.Obter))!.GetCustomAttributes<HttpGetAttribute>()).Template);

        // Começa por «/»: não se junta ao prefixo «registos-clinicos» do controlador.
        Assert.Equal(
            "/animais/{id:int}/historico-clinico",
            Assert.Single(tipo.GetMethod(nameof(RegistosClinicosController.ObterHistorico))!.GetCustomAttributes<HttpGetAttribute>()).Template);
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

    [Fact(DisplayName = "I078 — EP-033 devolve 200 com o registo do serviço")]
    public async Task ObterDevolve200()
    {
        var servico = new RegistosClinicosServiceDeTeste();
        var controlador = CriarControlador(servico, AcessoConsultasTests.Principal(Perfis.AuxiliarVeterinario));

        var resposta = await controlador.Obter(18);

        var ok = Assert.IsType<OkObjectResult>(resposta.Result);
        Assert.Same(servico.Registo, ok.Value);
        Assert.Equal(18, servico.UltimoId);
    }

    [Fact(DisplayName = "I078 — EP-034 devolve 200 com o histórico e passa o animal e as datas ao serviço")]
    public async Task ObterHistoricoDevolve200()
    {
        var servico = new RegistosClinicosServiceDeTeste();
        var controlador = CriarControlador(servico, AcessoConsultasTests.Principal(Perfis.Veterinario, veterinarioId: 4));
        var query = new HistoricoClinicoQuery { DataInicio = new DateOnly(2026, 9, 1), DataFim = new DateOnly(2026, 10, 31) };

        var resposta = await controlador.ObterHistorico(3, query);

        var ok = Assert.IsType<OkObjectResult>(resposta.Result);
        Assert.Same(servico.Historico, ok.Value);
        Assert.Equal(3, servico.UltimoId);
        Assert.Same(query, servico.UltimaQuery);
    }

    private static RegistosClinicosController CriarControlador(RegistosClinicosServiceDeTeste servico, ClaimsPrincipal utilizador) =>
        new(servico)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = utilizador },
            },
        };

    private sealed class RegistosClinicosServiceDeTeste : IRegistosClinicosService
    {
        public RegistoClinicoDto Registo { get; } = new() { Id = 18, ConsultaId = 41 };

        public HistoricoClinicoDto Historico { get; } = new();

        public HistoricoClinicoQuery? UltimaQuery { get; private set; }

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

        public Task<RegistoClinicoDto> ObterAsync(int id)
        {
            UltimoId = id;

            return Task.FromResult(Registo);
        }

        public Task<HistoricoClinicoDto> ObterHistoricoAsync(int animalId, HistoricoClinicoQuery query)
        {
            UltimoId = animalId;
            UltimaQuery = query;

            return Task.FromResult(Historico);
        }
    }
}
