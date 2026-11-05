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
/// I072–I074 — ConsultasController:
/// EP-023/024 consulta da agenda;
/// EP-025 criação;
/// EP-026 alteração;
/// EP-027 mudança de estado.
/// Valida rotas, autorização, passagem de If-Match e ETag.
/// </summary>
public class ConsultasControllerTests
{
    [Fact(DisplayName = "I072 — EP-023 e EP-024 só aceitam Rececionista, Veterinário e Auxiliar Veterinário")]
    public void ControladorExigeOsTresPerfis()
    {
        var tipo = typeof(ConsultasController);

        var autorizacao = Assert.Single(
            tipo.GetCustomAttributes<AuthorizeAttribute>(inherit: true));

        Assert.Null(autorizacao.Policy);

        Assert.Equal(
            Perfis.AuxiliarVeterinario + "," +
            Perfis.Rececionista + "," +
            Perfis.Veterinario,
            string.Join(
                ',',
                autorizacao.Roles!
                    .Split(',')
                    .Order(StringComparer.Ordinal)));

        var getConsultas =
            tipo.GetMethod(nameof(ConsultasController.GetConsultas))!;

        var getConsulta =
            tipo.GetMethod(nameof(ConsultasController.GetConsulta))!;

        Assert.Empty(
            getConsultas.GetCustomAttributes<AllowAnonymousAttribute>(
                inherit: true));

        Assert.Empty(
            getConsulta.GetCustomAttributes<AllowAnonymousAttribute>(
                inherit: true));

        Assert.Empty(
            getConsultas.GetCustomAttributes<AuthorizeAttribute>(
                inherit: true));

        Assert.Empty(
            getConsulta.GetCustomAttributes<AuthorizeAttribute>(
                inherit: true));
    }

    [Fact(DisplayName = "I073 — EP-025 é POST /consultas e exige também a política Rececionista")]
    public void CriarConsultaSoRececionista()
    {
        var acao = typeof(ConsultasController)
            .GetMethod(nameof(ConsultasController.CriarConsulta))!;

        Assert.Null(
            Assert.Single(
                acao.GetCustomAttributes<HttpPostAttribute>())
            .Template);

        var autorizacao = Assert.Single(
            acao.GetCustomAttributes<AuthorizeAttribute>(
                inherit: true));

        Assert.Equal(
            PoliticasAutorizacao.Rececionista,
            autorizacao.Policy);

        Assert.Null(autorizacao.Roles);
    }

    [Fact(DisplayName = "I073 — EP-025 devolve 201 com Location, ETag e o utilizador do token como autor")]
    public async Task CriarConsultaDevolve201()
    {
        var servico = new ConsultasServiceDeTeste();

        var controlador = CriarControlador(
            servico,
            AcessoConsultasTests.Principal(
                Perfis.Rececionista));

        var pedido = new CreateConsultaDto
        {
            AnimalId = 3,
            VeterinarioId = 5,
            DataHora = new DateTime(
                2026,
                11,
                25,
                10,
                0,
                0,
                DateTimeKind.Utc)
        };

        var resposta =
            await controlador.CriarConsulta(pedido);

        var criada =
            Assert.IsType<CreatedAtActionResult>(
                resposta.Result);

        Assert.Equal(
            nameof(ConsultasController.GetConsulta),
            criada.ActionName);

        Assert.Equal(
            41,
            criada.RouteValues!["id"]);

        Assert.Same(
            servico.Consulta,
            criada.Value);

        Assert.Equal(
            "\"AAAAAAAAB9E=\"",
            controlador.Response.Headers.ETag.ToString());

        Assert.Same(
            pedido,
            servico.UltimoPedido);

        Assert.Equal(
            1,
            servico.UltimoCriadaPorId);
    }

    [Fact(DisplayName = "I073 — EP-025 com um token sem identificador de utilizador dá 403 sem chamar o serviço")]
    public async Task CriarConsultaSemUtilizadorNoToken()
    {
        var servico = new ConsultasServiceDeTeste();

        var semSub = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                    new Claim(
                        ClaimsClinica.Perfil,
                        Perfis.Rececionista)
                },
                authenticationType: "Bearer",
                nameType: ClaimsClinica.UtilizadorId,
                roleType: ClaimsClinica.Perfil));

        var controlador =
            CriarControlador(servico, semSub);

        await Assert.ThrowsAsync<ForbiddenException>(
            () => controlador.CriarConsulta(
                new CreateConsultaDto()));

        Assert.Null(servico.UltimoPedido);
    }

    [Fact(DisplayName = "I074 — EP-026 é PUT /consultas/{id} e exige Rececionista")]
    public void AtualizarConsultaSoRececionista()
    {
        var acao = typeof(ConsultasController)
            .GetMethod(
                nameof(ConsultasController.AtualizarConsulta))!;

        var http = Assert.Single(
            acao.GetCustomAttributes<HttpPutAttribute>());

        Assert.Equal(
            "{id:int}",
            http.Template);

        var autorizacao = Assert.Single(
            acao.GetCustomAttributes<AuthorizeAttribute>(
                inherit: true));

        Assert.Equal(
            PoliticasAutorizacao.Rececionista,
            autorizacao.Policy);

        Assert.Null(autorizacao.Roles);
    }

    [Fact(DisplayName = "I074 — EP-026 passa If-Match ao serviço e devolve 200 com ETag")]
    public async Task AtualizarConsultaPassaIfMatchEDevolveETag()
    {
        var servico = new ConsultasServiceDeTeste();

        var controlador = CriarControlador(
            servico,
            AcessoConsultasTests.Principal(
                Perfis.Rececionista));

        var pedido = new UpdateConsultaDto
        {
            VeterinarioId = 5,
            DataHora = new DateTime(
                2026,
                11,
                25,
                10,
                0,
                0,
                DateTimeKind.Utc),
            Motivo = "Consulta reagendada"
        };

        const string ifMatch =
            "\"AAAAAAAAB9E=\"";

        var resposta =
            await controlador.AtualizarConsulta(
                41,
                pedido,
                ifMatch);

        var ok =
            Assert.IsType<OkObjectResult>(
                resposta.Result);

        Assert.Same(
            servico.Consulta,
            ok.Value);

        Assert.Equal(
            41,
            servico.UltimoId);

        Assert.Same(
            pedido,
            servico.UltimoUpdateConsulta);

        Assert.Equal(
            ifMatch,
            servico.UltimoIfMatch);

        Assert.Equal(
            "\"AAAAAAAAB9E=\"",
            controlador.Response.Headers.ETag.ToString());
    }

    [Fact(DisplayName = "I074 — EP-027 é PATCH /consultas/{id}/estado e exige Rececionista")]
    public void AtualizarEstadoSoRececionista()
    {
        var acao = typeof(ConsultasController)
            .GetMethod(
                nameof(ConsultasController.AtualizarEstado))!;

        var http = Assert.Single(
            acao.GetCustomAttributes<HttpPatchAttribute>());

        Assert.Equal(
            "{id:int}/estado",
            http.Template);

        var autorizacao = Assert.Single(
            acao.GetCustomAttributes<AuthorizeAttribute>(
                inherit: true));

        Assert.Equal(
            PoliticasAutorizacao.Rececionista,
            autorizacao.Policy);

        Assert.Null(autorizacao.Roles);
    }

    [Fact(DisplayName = "I074 — EP-027 passa If-Match ao serviço e devolve 204")]
    public async Task AtualizarEstadoPassaIfMatchEDevolve204()
    {
        var servico = new ConsultasServiceDeTeste();

        var controlador = CriarControlador(
            servico,
            AcessoConsultasTests.Principal(
                Perfis.Rececionista));

        var pedido = new UpdateEstadoConsultaDto
        {
            Estado = "Confirmada"
        };

        const string ifMatch =
            "\"AAAAAAAAB9E=\"";

        var resposta =
            await controlador.AtualizarEstado(
                41,
                pedido,
                ifMatch);

        Assert.IsType<NoContentResult>(resposta);

        Assert.Equal(
            "\"AAAAAAAAB9E=\"",
            controlador.Response.Headers.ETag.ToString());

        Assert.Equal(
            41,
            servico.UltimoId);

        Assert.Same(
            pedido,
            servico.UltimoUpdateEstado);

        Assert.Equal(
            ifMatch,
            servico.UltimoIfMatch);
    }

    [Fact(DisplayName = "I072 — As rotas são GET /consultas e GET /consultas/{id}")]
    public void RotasDoContrato()
    {
        var tipo = typeof(ConsultasController);

        Assert.Equal(
            "consultas",
            Assert.Single(
                tipo.GetCustomAttributes<RouteAttribute>())
            .Template);

        Assert.Null(
            Assert.Single(
                tipo.GetMethod(
                    nameof(ConsultasController.GetConsultas))!
                    .GetCustomAttributes<HttpGetAttribute>())
            .Template);

        Assert.Equal(
            "{id:int}",
            Assert.Single(
                tipo.GetMethod(
                    nameof(ConsultasController.GetConsulta))!
                    .GetCustomAttributes<HttpGetAttribute>())
            .Template);
    }

    [Fact(DisplayName = "I072 — EP-023 passa ao serviço o Veterinário do token, e não o do pedido")]
    public async Task AgendaUsaOVeterinarioDoToken()
    {
        var servico =
            new ConsultasServiceDeTeste();

        var controlador = CriarControlador(
            servico,
            AcessoConsultasTests.Principal(
                Perfis.Veterinario,
                veterinarioId: 4));

        var filtro = new ConsultaFiltroQuery
        {
            VeterinarioId = 5
        };

        var resposta =
            await controlador.GetConsultas(filtro);

        Assert.IsType<OkObjectResult>(
            resposta.Result);

        Assert.Same(
            filtro,
            servico.UltimoFiltro);

        Assert.Equal(
            4,
            servico.UltimoAcesso!.VeterinarioId);
    }

    [Fact(DisplayName = "I072 — EP-024 devolve a marcação no corpo e a versão no cabeçalho ETag")]
    public async Task MarcacaoComETag()
    {
        var servico =
            new ConsultasServiceDeTeste();

        var controlador = CriarControlador(
            servico,
            AcessoConsultasTests.Principal(
                Perfis.Rececionista));

        var resposta =
            await controlador.GetConsulta(41);

        var ok =
            Assert.IsType<OkObjectResult>(
                resposta.Result);

        Assert.Same(
            servico.Consulta,
            ok.Value);

        Assert.Equal(
            "\"AAAAAAAAB9E=\"",
            controlador.Response.Headers.ETag.ToString());

        Assert.Equal(
            41,
            servico.UltimoId);

        Assert.Null(
            servico.UltimoAcesso!.VeterinarioId);
    }

    [Fact(DisplayName = "I072 — perfil sem acesso à agenda dá 403 antes de chamar o serviço")]
    public async Task PerfilSemAcessoNaoChamaOServico()
    {
        var servico =
            new ConsultasServiceDeTeste();

        var controlador = CriarControlador(
            servico,
            AcessoConsultasTests.Principal(
                Perfis.Administrador));

        await Assert.ThrowsAsync<ForbiddenException>(
            () => controlador.GetConsulta(41));

        Assert.Null(servico.UltimoAcesso);
    }

    private static ConsultasController CriarControlador(
        ConsultasServiceDeTeste servico,
        ClaimsPrincipal utilizador) =>
        new(servico)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = utilizador
                }
            }
        };

    private sealed class ConsultasServiceDeTeste
        : IConsultasService
    {
        public ConsultaDto Consulta { get; } =
            new()
            {
                Id = 41
            };

        public ConsultaFiltroQuery? UltimoFiltro
        {
            get;
            private set;
        }

        public AcessoConsultas? UltimoAcesso
        {
            get;
            private set;
        }

        public int? UltimoId
        {
            get;
            private set;
        }

        public CreateConsultaDto? UltimoPedido
        {
            get;
            private set;
        }

        public int? UltimoCriadaPorId
        {
            get;
            private set;
        }

        public UpdateConsultaDto? UltimoUpdateConsulta
        {
            get;
            private set;
        }

        public UpdateEstadoConsultaDto? UltimoUpdateEstado
        {
            get;
            private set;
        }

        public string? UltimoIfMatch
        {
            get;
            private set;
        }

        public Task<PagedResultDto<ConsultaListItemDto>>
            ObterConsultasAsync(
                ConsultaFiltroQuery filtro,
                AcessoConsultas acesso)
        {
            UltimoFiltro = filtro;
            UltimoAcesso = acesso;

            return Task.FromResult(
                new PagedResultDto<ConsultaListItemDto>
                {
                    Pagina = 1,
                    Tamanho = 20
                });
        }

        public Task<ConsultaComVersao>
            ObterConsultaAsync(
                int id,
                AcessoConsultas acesso)
        {
            UltimoId = id;
            UltimoAcesso = acesso;

            return Task.FromResult(
                new ConsultaComVersao(
                    Consulta,
                    "\"AAAAAAAAB9E=\""));
        }

        public Task<ConsultaComVersao>
            CriarConsultaAsync(
                CreateConsultaDto pedido,
                int criadaPorId)
        {
            UltimoPedido = pedido;
            UltimoCriadaPorId = criadaPorId;

            return Task.FromResult(
                new ConsultaComVersao(
                    Consulta,
                    "\"AAAAAAAAB9E=\""));
        }

        public Task<ConsultaComVersao>
            AtualizarConsultaAsync(
                int id,
                UpdateConsultaDto pedido,
                string? ifMatch)
        {
            UltimoId = id;
            UltimoUpdateConsulta = pedido;
            UltimoIfMatch = ifMatch;

            return Task.FromResult(
                new ConsultaComVersao(
                    Consulta,
                    "\"AAAAAAAAB9E=\""));
        }

        public Task<string> AtualizarEstadoAsync(
            int id,
            UpdateEstadoConsultaDto pedido,
            string? ifMatch)
        {
            UltimoId = id;
            UltimoUpdateEstado = pedido;
            UltimoIfMatch = ifMatch;

            return Task.FromResult("\"AAAAAAAAB9E=\"");
        }

        public Task<ChegadaComVersao> RegistarChegadaAsync(int id, string? ifMatch)
        {
            UltimoId = id;
            UltimoIfMatch = ifMatch;

            return Task.FromResult(new ChegadaComVersao(
                new ChegadaDto { Id = id, ChegadaEm = DateTime.UnixEpoch },
                "\"AAAAAAAAB9E=\""));
        }

        public Task<NotaPreparacaoDto> CriarNotaPreparacaoAsync(
            int id,
            CreateNotaPreparacaoDto pedido,
            int auxiliarId)
        {
            UltimoId = id;

            return Task.FromResult(new NotaPreparacaoDto
            {
                Id = 2,
                ConsultaId = id,
                Texto = pedido.Texto!,
                Autor = "Carla Auxiliar"
            });
        }
    }
}
