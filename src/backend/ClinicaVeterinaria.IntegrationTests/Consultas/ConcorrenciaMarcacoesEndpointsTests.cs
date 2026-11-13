using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.Consultas;

/// <summary>
/// I093 — concorrência das marcações (Especificação de Testes de Integração, I030, secção 15:
/// IT-CONC-01 a IT-CONC-05), com a API real e o SQL Server dos testes de integração (I091), onde a
/// <c>RowVersion</c> é gerada pela base de dados. Inclui os pedidos em paralelo do critério 3 da I093.
/// </summary>
/// <remarks>
/// Marcações de demonstração usadas (dados da I061): M4 (Luna, Dr. Bruno, Pendente) e M3 (Kiko,
/// Dra. Ana, Confirmada, sem chegada). A API usa o relógio real: as alterações de data e hora são
/// feitas numa quarta-feira pelo menos 28 dias à frente, em hora de Lisboa, onde não há marcações.
/// <para>
/// Nos pedidos em paralelo, a ordem em que o SQL Server os executa não é controlada pelo teste. As
/// verificações valem para qualquer ordem: com o mesmo <c>If-Match</c>, o pedido que perde recebe
/// sempre 412 (ou porque a versão já não corresponde, ou porque a gravação deteta a
/// <c>RowVersion</c> alterada); sem <c>If-Match</c>, recebe 412 (conflito de versão) ou 422 (já viu
/// o estado gravado pelo outro). Em nenhum caso os dois são aceites.
/// </para>
/// </remarks>
public sealed class ConcorrenciaMarcacoesEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailRececao = "rececao@clinica-g08.test";
    private const string EmailVetAna = "vet.ana@clinica-g08.test";
    private const string EmailVetBruno = "vet.bruno@clinica-g08.test";

    private const string AlterarMarcacao = "EP-026";
    private const string AtualizarEstado = "EP-027";
    private const string RegistarChegada = "EP-028";

    private static readonly HttpStatusCode[] RecusasSemIfMatch =
        [HttpStatusCode.PreconditionFailed, HttpStatusCode.UnprocessableEntity];

    public ConcorrenciaMarcacoesEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    [Fact(DisplayName = "IT-CONC-01 — alteração com o ETag atual: 200, alteração persistida e novo ETag")]
    public async Task AlteracaoComETagAtual()
    {
        var marcacaoId = await ObterMarcacaoAsync(EmailVetBruno, "Pendente");
        var vetBruno = await ObterUtilizadorIdAsync(EmailVetBruno);
        var novaData = InicioUtc(QuartaFeiraFutura(), 10, 0);
        using var cliente = await ClienteDeAsync(EmailRececao);

        var etagLido = await LerETagAsync(cliente, marcacaoId);

        using var resposta = await EnviarAsync(
            cliente, HttpMethod.Put, $"/consultas/{marcacaoId}", CorpoAlteracao(vetBruno, novaData, "IT-CONC-01"), etagLido);

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        var novoETag = Assert.IsType<EntityTagHeaderValue>(resposta.Headers.ETag).Tag;
        Assert.NotEqual(etagLido, novoETag);

        var gravada = await LerMarcacaoAsync(marcacaoId);
        Assert.Equal(novaData, gravada.DataHoraInicio);
        Assert.Equal("IT-CONC-01", gravada.Motivo);
        Assert.Equal(VersaoMarcacao.ParaETag(gravada.RowVersion), novoETag);
        Assert.Equal(novoETag, await LerETagAsync(cliente, marcacaoId));
    }

    [Fact(DisplayName = "IT-CONC-02 — alteração com o ETag desatualizado: 412 e a alteração do primeiro pedido permanece")]
    public async Task AlteracaoComETagDesatualizado()
    {
        var marcacaoId = await ObterMarcacaoAsync(EmailVetBruno, "Pendente");
        var vetBruno = await ObterUtilizadorIdAsync(EmailVetBruno);
        var dia = QuartaFeiraFutura();
        var url = $"/consultas/{marcacaoId}";
        using var primeiro = await ClienteDeAsync(EmailRececao);
        using var segundo = await ClienteDeAsync(EmailRececao);

        // Os dois leem a mesma versão.
        var etagPrimeiro = await LerETagAsync(primeiro, marcacaoId);
        var etagSegundo = await LerETagAsync(segundo, marcacaoId);
        Assert.Equal(etagPrimeiro, etagSegundo);

        using var alteracao = await EnviarAsync(
            primeiro, HttpMethod.Put, url, CorpoAlteracao(vetBruno, InicioUtc(dia, 10, 0), "Alteração do primeiro"), etagPrimeiro);
        Assert.Equal(HttpStatusCode.OK, alteracao.StatusCode);
        var depoisDoPrimeiro = await LerMarcacaoAsync(marcacaoId);

        using var desatualizado = await EnviarAsync(
            segundo, HttpMethod.Put, url, CorpoAlteracao(vetBruno, InicioUtc(dia, 11, 0), "Alteração do segundo"), etagSegundo);

        await AssertErroAsync(desatualizado, HttpStatusCode.PreconditionFailed);

        var final = await LerMarcacaoAsync(marcacaoId);
        Assert.Equal(InicioUtc(dia, 10, 0), final.DataHoraInicio);
        Assert.Equal("Alteração do primeiro", final.Motivo);
        Assert.Equal(depoisDoPrimeiro.RowVersion, final.RowVersion);
    }

    [Fact(DisplayName = "IT-CONC-03 — mudança de estado com o ETag desatualizado: 412 e o estado gravado pelo primeiro pedido permanece")]
    public async Task MudancaDeEstadoComETagDesatualizado()
    {
        var marcacaoId = await ObterMarcacaoAsync(EmailVetBruno, "Pendente");
        var url = $"/consultas/{marcacaoId}/estado";
        using var primeiro = await ClienteDeAsync(EmailRececao);
        using var segundo = await ClienteDeAsync(EmailRececao);

        var etagPrimeiro = await LerETagAsync(primeiro, marcacaoId);
        var etagSegundo = await LerETagAsync(segundo, marcacaoId);
        Assert.Equal(etagPrimeiro, etagSegundo);

        using var confirmacao = await EnviarAsync(primeiro, HttpMethod.Patch, url, CorpoEstado("Confirmada"), etagPrimeiro);
        Assert.Equal(HttpStatusCode.NoContent, confirmacao.StatusCode);
        var depoisDoPrimeiro = await LerMarcacaoAsync(marcacaoId);

        using var desatualizado = await EnviarAsync(segundo, HttpMethod.Patch, url, CorpoEstado("Cancelada"), etagSegundo);

        await AssertErroAsync(desatualizado, HttpStatusCode.PreconditionFailed);

        var final = await LerMarcacaoAsync(marcacaoId);
        Assert.Equal("Confirmada", final.Estado);
        Assert.Equal(depoisDoPrimeiro.RowVersion, final.RowVersion);
    }

    [Fact(DisplayName = "IT-CONC-04 — registo de chegada com o ETag desatualizado: 412 e a hora de chegada original mantém-se")]
    public async Task ChegadaComETagDesatualizado()
    {
        var marcacaoId = await ObterMarcacaoAsync(EmailVetAna, "Confirmada");
        var url = $"/consultas/{marcacaoId}/chegada";
        using var primeiro = await ClienteDeAsync(EmailRececao);
        using var segundo = await ClienteDeAsync(EmailRececao);

        var etagPrimeiro = await LerETagAsync(primeiro, marcacaoId);
        var etagSegundo = await LerETagAsync(segundo, marcacaoId);
        Assert.Equal(etagPrimeiro, etagSegundo);

        using var chegada = await EnviarAsync(primeiro, HttpMethod.Patch, url, null, etagPrimeiro);
        Assert.Equal(HttpStatusCode.OK, chegada.StatusCode);
        var chegadaOriginal = await LerChegadaDaRespostaAsync(chegada);
        var depoisDoPrimeiro = await LerMarcacaoAsync(marcacaoId);

        using var desatualizado = await EnviarAsync(segundo, HttpMethod.Patch, url, null, etagSegundo);

        await AssertErroAsync(desatualizado, HttpStatusCode.PreconditionFailed);

        var final = await LerMarcacaoAsync(marcacaoId);
        Assert.Equal(chegadaOriginal, final.DataHoraChegada);
        Assert.Equal(depoisDoPrimeiro.RowVersion, final.RowVersion);
    }

    [Theory(DisplayName = "IT-CONC-02, IT-CONC-03 e IT-CONC-04 (critério 3 da I093) — dois pedidos em paralelo com o mesmo If-Match: só um é aceite, o outro recebe 412 e a base fica coerente")]
    [InlineData(AlterarMarcacao)]
    [InlineData(AtualizarEstado)]
    [InlineData(RegistarChegada)]
    public async Task PedidosEmParaleloComOMesmoIfMatch(string operacao)
    {
        var cenario = await PrepararCenarioAsync(operacao);
        using var primeiro = await ClienteDeAsync(EmailRececao);
        using var segundo = await ClienteDeAsync(EmailRececao);
        var etag = await LerETagAsync(primeiro, cenario.MarcacaoId);

        var respostas = await Task.WhenAll(
            EnviarAsync(primeiro, cenario.Metodo, cenario.Url, cenario.CorpoPrimeiro, etag),
            EnviarAsync(segundo, cenario.Metodo, cenario.Url, cenario.CorpoSegundo, etag));

        try
        {
            var aceite = Assert.Single(respostas, r => r.StatusCode == cenario.Sucesso);
            var recusado = Assert.Single(respostas, r => r.StatusCode != cenario.Sucesso);

            await AssertErroAsync(recusado, HttpStatusCode.PreconditionFailed);
            await AssertBaseCoerenteAsync(cenario, aceite, ReferenceEquals(aceite, respostas[0]));
        }
        finally
        {
            foreach (var resposta in respostas)
            {
                resposta.Dispose();
            }
        }
    }

    [Theory(DisplayName = "IT-CONC-05 — duas gravações simultâneas sem If-Match: só uma é persistida e a outra é recusada")]
    [InlineData(AtualizarEstado)]
    [InlineData(RegistarChegada)]
    public async Task GravacoesSimultaneasSemIfMatch(string operacao)
    {
        // Os dois pedidos pedem a mesma alteração: se fossem os dois aceites, haveria uma gravação perdida.
        var cenario = await PrepararCenarioAsync(operacao, mesmaAlteracao: true);
        using var primeiro = await ClienteDeAsync(EmailRececao);
        using var segundo = await ClienteDeAsync(EmailRececao);

        var respostas = await Task.WhenAll(
            EnviarAsync(primeiro, cenario.Metodo, cenario.Url, cenario.CorpoPrimeiro, null),
            EnviarAsync(segundo, cenario.Metodo, cenario.Url, cenario.CorpoSegundo, null));

        try
        {
            var aceite = Assert.Single(respostas, r => r.StatusCode == cenario.Sucesso);
            var recusado = Assert.Single(respostas, r => r.StatusCode != cenario.Sucesso);

            Assert.Contains(recusado.StatusCode, RecusasSemIfMatch);
            await AssertErroAsync(recusado, recusado.StatusCode);
            await AssertBaseCoerenteAsync(cenario, aceite, primeiroAceite: true);
        }
        finally
        {
            foreach (var resposta in respostas)
            {
                resposta.Dispose();
            }
        }
    }

    // ----- Cenários -----
    private sealed record Cenario(
        string Operacao,
        int MarcacaoId,
        HttpMethod Metodo,
        string Url,
        object? CorpoPrimeiro,
        object? CorpoSegundo,
        HttpStatusCode Sucesso,
        DateTime DataPrimeiro,
        DateTime DataSegundo,
        string EstadoPrimeiro,
        string EstadoSegundo);

    private sealed record MarcacaoBd(
        string Estado,
        DateTime DataHoraInicio,
        string? Motivo,
        DateTime? DataHoraChegada,
        byte[] RowVersion);

    /// <summary>
    /// EP-026: M4 para as 10:00 (primeiro) ou para as 11:00 (segundo). EP-027: M4 para Confirmada
    /// (primeiro) ou Cancelada (segundo). EP-028: chegada à M3. Com <paramref name="mesmaAlteracao"/>,
    /// os dois pedidos de EP-027 pedem Confirmada.
    /// </summary>
    private async Task<Cenario> PrepararCenarioAsync(string operacao, bool mesmaAlteracao = false)
    {
        var dia = QuartaFeiraFutura();
        var dez = InicioUtc(dia, 10, 0);
        var onze = InicioUtc(dia, 11, 0);

        switch (operacao)
        {
            case AlterarMarcacao:
                {
                    var id = await ObterMarcacaoAsync(EmailVetBruno, "Pendente");
                    var vetBruno = await ObterUtilizadorIdAsync(EmailVetBruno);
                    return new Cenario(
                        operacao, id, HttpMethod.Put, $"/consultas/{id}",
                        CorpoAlteracao(vetBruno, dez, "Primeiro pedido"),
                        CorpoAlteracao(vetBruno, onze, "Segundo pedido"),
                        HttpStatusCode.OK, dez, onze, "Pendente", "Pendente");
                }

            case AtualizarEstado:
                {
                    var id = await ObterMarcacaoAsync(EmailVetBruno, "Pendente");
                    var estadoSegundo = mesmaAlteracao ? "Confirmada" : "Cancelada";
                    return new Cenario(
                        operacao, id, HttpMethod.Patch, $"/consultas/{id}/estado",
                        CorpoEstado("Confirmada"),
                        CorpoEstado(estadoSegundo),
                        HttpStatusCode.NoContent, dez, onze, "Confirmada", estadoSegundo);
                }

            case RegistarChegada:
                {
                    var id = await ObterMarcacaoAsync(EmailVetAna, "Confirmada");
                    return new Cenario(
                        operacao, id, HttpMethod.Patch, $"/consultas/{id}/chegada",
                        null, null, HttpStatusCode.OK, dez, onze, "Confirmada", "Confirmada");
                }

            default:
                throw new ArgumentOutOfRangeException(nameof(operacao), operacao, "Operação desconhecida.");
        }
    }

    /// <summary>
    /// A base fica com o efeito do pedido aceite, e só com esse: a versão gravada é a do ETag
    /// devolvido ao pedido aceite.
    /// </summary>
    private async Task AssertBaseCoerenteAsync(Cenario cenario, HttpResponseMessage aceite, bool primeiroAceite)
    {
        var final = await LerMarcacaoAsync(cenario.MarcacaoId);

        var etagAceite = Assert.IsType<EntityTagHeaderValue>(aceite.Headers.ETag).Tag;
        Assert.Equal(VersaoMarcacao.ParaETag(final.RowVersion), etagAceite);
        Assert.Equal(primeiroAceite ? cenario.EstadoPrimeiro : cenario.EstadoSegundo, final.Estado);

        switch (cenario.Operacao)
        {
            case AlterarMarcacao:
                Assert.Equal(primeiroAceite ? cenario.DataPrimeiro : cenario.DataSegundo, final.DataHoraInicio);
                Assert.Equal(primeiroAceite ? "Primeiro pedido" : "Segundo pedido", final.Motivo);
                break;

            case AtualizarEstado:
                // O estado gravado já foi comparado com o do pedido aceite.
                break;

            case RegistarChegada:
                Assert.Equal(await LerChegadaDaRespostaAsync(aceite), final.DataHoraChegada);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(cenario), cenario.Operacao, "Operação desconhecida.");
        }
    }

    // ----- Auxiliares -----
    private static Dictionary<string, object?> CorpoAlteracao(int veterinarioId, DateTime dataHoraUtc, string motivo) =>
        new()
        {
            ["veterinarioId"] = veterinarioId,
            ["dataHora"] = Texto(dataHoraUtc),
            ["motivo"] = motivo,
        };

    private static Dictionary<string, object?> CorpoEstado(string estado) =>
        new()
        {
            ["estado"] = estado,
            ["observacao"] = null,
        };

    private static async Task<HttpResponseMessage> EnviarAsync(
        HttpClient cliente,
        HttpMethod metodo,
        string url,
        object? corpo,
        string? ifMatch)
    {
        using var pedido = new HttpRequestMessage(metodo, url)
        {
            Content = corpo is null ? null : JsonContent.Create(corpo),
        };

        if (ifMatch is not null)
        {
            pedido.Headers.TryAddWithoutValidation("If-Match", ifMatch);
        }

        return await cliente.SendAsync(pedido);
    }

    private static async Task<string> LerETagAsync(HttpClient cliente, int marcacaoId)
    {
        using var resposta = await cliente.GetAsync($"/consultas/{marcacaoId}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        return Assert.IsType<EntityTagHeaderValue>(resposta.Headers.ETag).Tag;
    }

    private static async Task<DateTime> LerChegadaDaRespostaAsync(HttpResponseMessage resposta)
    {
        using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
        return documento.RootElement.GetProperty("chegadaEm").GetDateTime();
    }

    /// <summary>O código HTTP esperado, no formato de erro da I058 (<c>code</c>, <c>message</c>, <c>detail</c>).</summary>
    private static async Task AssertErroAsync(HttpResponseMessage resposta, HttpStatusCode esperado)
    {
        Assert.Equal(esperado, resposta.StatusCode);

        using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
        Assert.Equal((int)esperado, documento.RootElement.GetProperty("code").GetInt32());
        Assert.False(string.IsNullOrWhiteSpace(documento.RootElement.GetProperty("message").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(documento.RootElement.GetProperty("detail").GetString()));
    }

    /// <summary>Quarta-feira pelo menos 28 dias depois de hoje (hora de Lisboa), como nos testes da I073 e da I074.</summary>
    private static DateOnly QuartaFeiraFutura()
    {
        var hoje = DateOnly.FromDateTime(
            FusoHorarioClinica.ParaLocal(DateTime.UtcNow, FusoHorarioClinica.Obter()));
        var dia = hoje.AddDays(28);

        while (dia.DayOfWeek != DayOfWeek.Wednesday)
        {
            dia = dia.AddDays(1);
        }

        return dia;
    }

    /// <summary>Hora de Lisboa nesse dia, convertida para UTC (regra R-FUSO).</summary>
    private static DateTime InicioUtc(DateOnly dia, int hora, int minuto) =>
        FusoHorarioClinica.ParaUtc(dia, new TimeOnly(hora, minuto), FusoHorarioClinica.Obter())!.Value;

    private static string Texto(DateTime utc) =>
        utc.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture);

    private async Task<HttpClient> ClienteDeAsync(string email)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var jwtTokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();

        var utilizador = await authService.ObterAutenticadoAsync(await ObterUtilizadorIdAsync(email));
        var token = jwtTokenService.GerarToken(utilizador);

        var cliente = Fixture.Factory.CreateClient();
        cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        return cliente;
    }

    private async Task<int> ObterUtilizadorIdAsync(string email)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Utilizadores
            .AsNoTracking()
            .Where(u => u.Email == email)
            .Select(u => u.Id)
            .SingleAsync();
    }

    /// <summary>Marcação de demonstração do Veterinário no estado indicado (uma só em cada caso usado).</summary>
    private async Task<int> ObterMarcacaoAsync(string emailVeterinario, string estado)
    {
        var veterinarioId = await ObterUtilizadorIdAsync(emailVeterinario);

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Marcacoes
            .AsNoTracking()
            .Where(m => m.VeterinarioId == veterinarioId && m.Estado == estado)
            .Select(m => m.Id)
            .SingleAsync();
    }

    private async Task<MarcacaoBd> LerMarcacaoAsync(int marcacaoId)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Marcacoes
            .AsNoTracking()
            .Where(m => m.Id == marcacaoId)
            .Select(m => new MarcacaoBd(m.Estado, m.DataHoraInicio, m.Motivo, m.DataHoraChegada, m.RowVersion))
            .SingleAsync();
    }
}
