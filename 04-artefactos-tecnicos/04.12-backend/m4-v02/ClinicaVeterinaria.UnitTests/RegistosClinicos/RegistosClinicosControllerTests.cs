using System.Reflection;
using System.Security.Claims;
using ClinicaVeterinaria.Api.Controllers;
using ClinicaVeterinaria.Api.DTOs.RegistosClinicos;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Models.RegistosClinicos;
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

    [Fact(DisplayName = "I078 — EP-033 devolve ao Auxiliar o registo reduzido")]
    public async Task ObterDevolve200()
    {
        var servico = new RegistosClinicosServiceDeTeste();
        var controlador = CriarControlador(
            servico,
            AcessoConsultasTests.Principal(Perfis.AuxiliarVeterinario));

        var resposta = await controlador.Obter(18);

        var ok = Assert.IsType<OkObjectResult>(resposta);
        var dto = Assert.IsType<RegistoClinicoAuxiliarDto>(ok.Value);

        Assert.Equal(servico.Registo.Id, dto.Id);
        Assert.Equal(servico.Registo.ConsultaId, dto.ConsultaId);
        Assert.Equal(servico.Registo.AnimalId, dto.AnimalId);
        Assert.Equivalent(servico.Registo.Tratamentos, dto.Tratamentos);
        Assert.Equivalent(servico.Registo.Prescricoes, dto.Prescricoes);
        Assert.Equal(18, servico.UltimoId);
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
        Assert.Equivalent(servico.Registo, Assert.IsType<RegistoClinicoDto>(criado.Value), strict: true);
        Assert.Equivalent(pedido, servico.UltimoPedido);
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
        Assert.Equivalent(servico.Registo, Assert.IsType<RegistoClinicoDto>(ok.Value), strict: true);
        Assert.Equal(18, servico.UltimoId);
        Assert.Equivalent(pedido, servico.UltimoPedido);
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

    [Fact(DisplayName = "I078 — EP-033 devolve ao Auxiliar o registo reduzido, sem notas clínicas nem diagnóstico")]
    public async Task ObterComoAuxiliarDevolveRegistoReduzido()
    {
        var servico = new RegistosClinicosServiceDeTeste();
        servico.Registo.NotasClinicas = "Notas reservadas ao Veterinário";
        servico.Registo.Diagnostico = "Diagnóstico reservado ao Veterinário";
        servico.Registo.Tratamentos =
        [
            new TratamentoRegistado { Id = 1, Descricao = "Limpeza", DataAplicacao = new DateOnly(2026, 11, 21) },
        ];
        servico.Registo.Prescricoes =
        [
            new PrescricaoRegistada { Id = 2, Medicamento = "Medicamento de teste", Dosagem = "Dose de teste", Frequencia = "Diária", DuracaoDias = 3 },
        ];
        var controlador = CriarControlador(servico, AcessoConsultasTests.Principal(Perfis.AuxiliarVeterinario));

        var resposta = await controlador.Obter(18);

        var ok = Assert.IsType<OkObjectResult>(resposta);
        var dto = Assert.IsType<RegistoClinicoAuxiliarDto>(ok.Value);
        Assert.Equal(servico.Registo.Id, dto.Id);
        Assert.Equal(servico.Registo.ConsultaId, dto.ConsultaId);
        Assert.Equal(servico.Registo.AnimalId, dto.AnimalId);
        Assert.Equal(servico.Registo.Veterinario, dto.Veterinario);
        Assert.Equal(servico.Registo.CriadoEm, dto.CriadoEm);
        Assert.Equal(servico.Registo.AtualizadoEm, dto.AtualizadoEm);
        Assert.Equal(servico.Registo.AtualizadoPor, dto.AtualizadoPor);
        Assert.Equivalent(servico.Registo.Tratamentos, dto.Tratamentos, strict: true);
        Assert.Equivalent(servico.Registo.Prescricoes, dto.Prescricoes, strict: true);
        var json = System.Text.Json.JsonSerializer.SerializeToElement(dto);
        Assert.False(json.TryGetProperty("NotasClinicas", out _));
        Assert.False(json.TryGetProperty("Diagnostico", out _));
        Assert.Equal(18, servico.UltimoId);
    }

    [Fact(DisplayName = "I078 — EP-033 mantém o registo completo para o Veterinário")]
    public async Task ObterComoVeterinarioDevolveRegistoCompleto()
    {
        var servico = new RegistosClinicosServiceDeTeste();
        servico.Registo.NotasClinicas = "Notas reservadas ao Veterinário";
        servico.Registo.Diagnostico = "Diagnóstico reservado ao Veterinário";
        var controlador = CriarControlador(servico, AcessoConsultasTests.Principal(Perfis.Veterinario, veterinarioId: 4));

        var resposta = await controlador.Obter(18);

        var ok = Assert.IsType<OkObjectResult>(resposta);
        Assert.Equivalent(servico.Registo, Assert.IsType<RegistoClinicoDto>(ok.Value), strict: true);
        Assert.Equal(18, servico.UltimoId);
    }

    [Fact(DisplayName = "I078 — EP-034 devolve 200 com o histórico e passa o animal e as datas ao serviço")]
    public async Task ObterHistoricoDevolve200()
    {
        var servico = new RegistosClinicosServiceDeTeste();
        var controlador = CriarControlador(servico, AcessoConsultasTests.Principal(Perfis.Veterinario, veterinarioId: 4));
        var query = new HistoricoClinicoQuery { DataInicio = new DateOnly(2026, 9, 1), DataFim = new DateOnly(2026, 10, 31) };

        var resposta = await controlador.ObterHistorico(3, query);

        var ok = Assert.IsType<OkObjectResult>(resposta);
        Assert.Equivalent(servico.Historico, Assert.IsType<HistoricoClinicoDto>(ok.Value), strict: true);
        Assert.Equal(3, servico.UltimoId);
        Assert.Equivalent(query, servico.UltimaQuery);
    }

    [Fact(DisplayName = "I125 — histórico do Auxiliar omite diagnóstico e mantém as contagens")]
    public async Task HistoricoComoAuxiliarDevolveRespostaReduzida()
    {
        var servico = new RegistosClinicosServiceDeTeste();
        servico.Historico.Registos =
        [
            new ResumoRegistoClinico { Id = 18, ConsultaId = 41, Diagnostico = "Reservado", NumeroTratamentos = 1, NumeroPrescricoes = 2 },
        ];
        var controlador = CriarControlador(servico, AcessoConsultasTests.Principal(Perfis.AuxiliarVeterinario));

        var resposta = await controlador.ObterHistorico(3, new HistoricoClinicoQuery());

        var dto = Assert.IsType<HistoricoClinicoAuxiliarDto>(Assert.IsType<OkObjectResult>(resposta).Value);
        var linha = Assert.Single(dto.Registos);
        Assert.Equal(18, linha.Id);
        Assert.Equal(41, linha.ConsultaId);
        Assert.Equal(1, linha.NumeroTratamentos);
        Assert.Equal(2, linha.NumeroPrescricoes);
        Assert.False(System.Text.Json.JsonSerializer.SerializeToElement(linha).TryGetProperty("Diagnostico", out _));
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
        public DetalheRegistoClinico Registo { get; } = new() { Id = 18, ConsultaId = 41 };

        public HistoricoClinico Historico { get; } = new();

        public PeriodoHistoricoClinico? UltimaQuery { get; private set; }

        public AlteracaoRegistoClinico? UltimoPedido { get; private set; }

        public int? UltimoId { get; private set; }

        public int? UltimoVeterinarioId { get; private set; }

        public Task<DetalheRegistoClinico> CriarAsync(NovoRegistoClinico pedido, int veterinarioId)
        {
            UltimoPedido = pedido;
            UltimoVeterinarioId = veterinarioId;

            return Task.FromResult(Registo);
        }

        public Task<DetalheRegistoClinico> AtualizarAsync(int id, AlteracaoRegistoClinico pedido, int veterinarioId)
        {
            UltimoId = id;
            UltimoPedido = pedido;
            UltimoVeterinarioId = veterinarioId;

            return Task.FromResult(Registo);
        }

        public Task<DetalheRegistoClinico> ObterAsync(int id)
        {
            UltimoId = id;

            return Task.FromResult(Registo);
        }

        public Task<HistoricoClinico> ObterHistoricoAsync(int animalId, PeriodoHistoricoClinico query)
        {
            UltimoId = animalId;
            UltimaQuery = query;

            return Task.FromResult(Historico);
        }
    }
}
