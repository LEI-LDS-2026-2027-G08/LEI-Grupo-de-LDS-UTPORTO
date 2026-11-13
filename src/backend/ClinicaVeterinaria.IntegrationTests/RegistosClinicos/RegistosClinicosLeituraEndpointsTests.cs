using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.RegistosClinicos;

/// <summary>
/// I078 — EP-033 (GET /registos-clinicos/{id}) e EP-034 (GET /animais/{id}/historico-clinico) com
/// a API real e o SQL Server dos testes de integração (I091), sobre os dados de demonstração da
/// I061: casos IT-EP033-01 a 03 e IT-EP034-01 a 05 da Especificação de Testes de Integração (I030,
/// secção 12), a matriz de perfis (401 e 403) e a ausência de operações de escrita para o
/// Auxiliar Veterinário.
/// </summary>
/// <remarks>
/// Dados da I061 usados: o registo clínico da M1 (Bobi, vet.ana, 2026-10-14, com um tratamento e
/// uma prescrição) e o Kiko, sem registos. Para o histórico com vários registos, o teste acrescenta
/// ao Bobi uma consulta Realizada do vet.bruno a 2026-09-02 com registo (a base é reposta antes de
/// cada teste).
/// </remarks>
public sealed class RegistosClinicosLeituraEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailVetAna = "vet.ana@clinica-g08.test";
    private const string EmailVetBruno = "vet.bruno@clinica-g08.test";
    private const string EmailAuxCarla = "aux.carla@clinica-g08.test";
    private const string EmailAuxDiogo = "aux.diogo@clinica-g08.test";
    private const string EmailRececao = "rececao@clinica-g08.test";

    private static readonly DateTime InicioM1 = new(2026, 10, 14, 9, 30, 0);

    // 2026-09-02 09:00 em Lisboa (hora de verão, UTC+1).
    private static readonly DateTime InicioSetembro = new(2026, 9, 2, 8, 0, 0);

    public RegistosClinicosLeituraEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    // ----- EP-033 — Consultar registo clínico -----
    [Fact(DisplayName = "IT-EP033-01 — o Veterinário consulta o registo da M1: 200 com notas, diagnóstico, tratamentos e prescrições")]
    public async Task VeterinarioConsultaRegisto()
    {
        var m1 = await ObterMarcacaoIdAsync(EmailVetAna, InicioM1);
        var registoId = await ObterRegistoDaMarcacaoAsync(m1);
        var bobi = await ObterAnimalIdAsync("Bobi");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.GetAsync($"/registos-clinicos/{registoId}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        var raiz = documento.RootElement;
        Assert.Equal(registoId, raiz.GetProperty("id").GetInt32());
        Assert.Equal(m1, raiz.GetProperty("consultaId").GetInt32());
        Assert.Equal(bobi, raiz.GetProperty("animalId").GetInt32());
        Assert.Equal("Dra. Ana Veterinária (demonstração)", raiz.GetProperty("veterinario").GetString());
        Assert.Equal("Otite externa", raiz.GetProperty("diagnostico").GetString());
        Assert.False(string.IsNullOrWhiteSpace(raiz.GetProperty("notasClinicas").GetString()));
        Assert.Equal(
            "Limpeza auricular com solução fisiológica",
            Assert.Single(raiz.GetProperty("tratamentos").EnumerateArray().ToList()).GetProperty("descricao").GetString());
        Assert.Equal(
            "Gotas auriculares (fictício)",
            Assert.Single(raiz.GetProperty("prescricoes").EnumerateArray().ToList()).GetProperty("medicamento").GetString());
    }

    [Theory(DisplayName = "IT-EP033-02 — o Auxiliar Veterinário consulta o mesmo registo (200), com ou sem autorização de apoio")]
    [InlineData(EmailAuxCarla)]
    [InlineData(EmailAuxDiogo)]
    public async Task AuxiliarConsultaRegisto(string emailAuxiliar)
    {
        var registoId = await ObterRegistoDaMarcacaoAsync(await ObterMarcacaoIdAsync(EmailVetAna, InicioM1));
        using var veterinario = await ClienteDeAsync(EmailVetAna);
        using var auxiliar = await ClienteDeAsync(emailAuxiliar);

        using var respostaVeterinario = await veterinario.GetAsync($"/registos-clinicos/{registoId}");
        using var respostaAuxiliar = await auxiliar.GetAsync($"/registos-clinicos/{registoId}");

        // Contrato v02 (PA-08 da I028): o Auxiliar recebe o mesmo RegistoClinicoDto do Veterinário.
        Assert.Equal(HttpStatusCode.OK, respostaAuxiliar.StatusCode);
        Assert.Equal(
            await respostaVeterinario.Content.ReadAsStringAsync(),
            await respostaAuxiliar.Content.ReadAsStringAsync());
    }

    [Fact(DisplayName = "IT-EP033-02 — somente leitura: o Auxiliar recebe 403 ao criar ou alterar e o registo fica como estava")]
    public async Task AuxiliarNaoAlteraRegisto()
    {
        var m1 = await ObterMarcacaoIdAsync(EmailVetAna, InicioM1);
        var m3 = await ObterMarcacaoIdAsync(EmailVetAna, new DateTime(2026, 11, 3, 9, 0, 0));
        var registoId = await ObterRegistoDaMarcacaoAsync(m1);
        var antes = await ContarAsync();
        using var cliente = await ClienteDeAsync(EmailAuxCarla);

        using var criar = await cliente.PostAsJsonAsync("/registos-clinicos", new { consultaId = m3, diagnostico = "Otite externa" });
        using var atualizar = await cliente.PutAsJsonAsync($"/registos-clinicos/{registoId}", new { diagnostico = "Alterado pelo Auxiliar" });

        await AssertErroAsync(criar, HttpStatusCode.Forbidden);
        await AssertErroAsync(atualizar, HttpStatusCode.Forbidden);
        Assert.Equal(antes, await ContarAsync());

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
        var registo = await db.RegistosClinicos.AsNoTracking().SingleAsync(r => r.Id == registoId);
        Assert.Equal("Otite externa", registo.Diagnostico);
        Assert.Null(registo.AtualizadoEm);
    }

    [Fact(DisplayName = "IT-EP033-03 — registo inexistente devolve 404")]
    public async Task RegistoInexistenteDevolve404()
    {
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.GetAsync("/registos-clinicos/999999");

        await AssertErroAsync(resposta, HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "I078 — um Veterinário consulta o registo feito por outro Veterinário do mesmo paciente (200)")]
    public async Task VeterinarioConsultaRegistoDeColega()
    {
        var registoId = await ObterRegistoDaMarcacaoAsync(await ObterMarcacaoIdAsync(EmailVetAna, InicioM1));
        using var cliente = await ClienteDeAsync(EmailVetBruno);

        using var resposta = await cliente.GetAsync($"/registos-clinicos/{registoId}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
    }

    // ----- EP-034 — Consultar histórico clínico do paciente -----
    [Fact(DisplayName = "IT-EP034-01 — animal com vários registos: 200, do mais recente para o mais antigo")]
    public async Task HistoricoPorOrdemDescendente()
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        var registoM1 = await ObterRegistoDaMarcacaoAsync(await ObterMarcacaoIdAsync(EmailVetAna, InicioM1));
        var registoSetembro = await AcrescentarRegistoSetembroAsync(bobi);
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.GetAsync($"/animais/{bobi}/historico-clinico");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        var raiz = documento.RootElement;
        var animal = raiz.GetProperty("animal");
        Assert.Equal(bobi, animal.GetProperty("id").GetInt32());
        Assert.Equal("Bobi", animal.GetProperty("nome").GetString());
        Assert.Equal("Cão", animal.GetProperty("especie").GetString());

        var registos = raiz.GetProperty("registos").EnumerateArray().ToList();
        Assert.Equal(2, registos.Count);
        Assert.Equal(registoM1, registos[0].GetProperty("id").GetInt32());
        Assert.Equal("2026-10-14", registos[0].GetProperty("data").GetString());
        Assert.Equal("Dra. Ana Veterinária (demonstração)", registos[0].GetProperty("veterinario").GetString());
        Assert.Equal("Otite externa", registos[0].GetProperty("diagnostico").GetString());
        Assert.Equal(1, registos[0].GetProperty("numeroTratamentos").GetInt32());
        Assert.Equal(1, registos[0].GetProperty("numeroPrescricoes").GetInt32());
        Assert.Equal(registoSetembro, registos[1].GetProperty("id").GetInt32());
        Assert.Equal("2026-09-02", registos[1].GetProperty("data").GetString());
        Assert.Equal("Dr. Bruno Veterinário (demonstração)", registos[1].GetProperty("veterinario").GetString());
        Assert.Equal(0, registos[1].GetProperty("numeroTratamentos").GetInt32());
    }

    [Fact(DisplayName = "IT-EP034-02 — intervalo válido: só os registos do intervalo")]
    public async Task HistoricoFiltradoPorIntervalo()
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        var registoSetembro = await AcrescentarRegistoSetembroAsync(bobi);
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.GetAsync($"/animais/{bobi}/historico-clinico?dataInicio=2026-09-01&dataFim=2026-09-30");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        var registo = Assert.Single(documento.RootElement.GetProperty("registos").EnumerateArray().ToList());
        Assert.Equal(registoSetembro, registo.GetProperty("id").GetInt32());
    }

    [Theory(DisplayName = "IT-EP034-03 — intervalo de datas inválido devolve 400")]
    [InlineData("dataInicio=2026-10-14&dataFim=2026-09-01")]
    [InlineData("dataInicio=2026-13-40")]
    [InlineData("dataFim=1999-12-31")]
    public async Task IntervaloInvalidoDevolve400(string parametros)
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.GetAsync($"/animais/{bobi}/historico-clinico?{parametros}");

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "IT-EP034-04 — animal inexistente devolve 404")]
    public async Task AnimalInexistenteDevolve404()
    {
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.GetAsync("/animais/999999/historico-clinico");

        await AssertErroAsync(resposta, HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "IT-EP034-05 — animal sem histórico (Kiko): 200 com a coleção vazia")]
    public async Task AnimalSemHistorico()
    {
        var kiko = await ObterAnimalIdAsync("Kiko");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.GetAsync($"/animais/{kiko}/historico-clinico");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        Assert.Equal("Kiko", documento.RootElement.GetProperty("animal").GetProperty("nome").GetString());
        Assert.Empty(documento.RootElement.GetProperty("registos").EnumerateArray());
    }

    [Fact(DisplayName = "I078 — o Auxiliar Veterinário consulta o histórico do paciente (200), o mesmo do Veterinário")]
    public async Task AuxiliarConsultaHistorico()
    {
        var bobi = await ObterAnimalIdAsync("Bobi");
        using var veterinario = await ClienteDeAsync(EmailVetAna);
        using var auxiliar = await ClienteDeAsync(EmailAuxDiogo);

        using var respostaVeterinario = await veterinario.GetAsync($"/animais/{bobi}/historico-clinico");
        using var respostaAuxiliar = await auxiliar.GetAsync($"/animais/{bobi}/historico-clinico");

        Assert.Equal(HttpStatusCode.OK, respostaAuxiliar.StatusCode);
        Assert.Equal(
            await respostaVeterinario.Content.ReadAsStringAsync(),
            await respostaAuxiliar.Content.ReadAsStringAsync());
    }

    // ----- Perfis sem acesso e pedidos sem token (I078, critério 5) -----
    [Theory(DisplayName = "I078 — Rececionista, Administrador e Cliente recebem 403 em EP-033 e EP-034")]
    [InlineData(EmailRececao)]
    [InlineData("admin@clinica-g08.test")]
    [InlineData("cliente.joao@clinica-g08.test")]
    public async Task OutrosPerfisRecebem403(string email)
    {
        var registoId = await ObterRegistoDaMarcacaoAsync(await ObterMarcacaoIdAsync(EmailVetAna, InicioM1));
        var bobi = await ObterAnimalIdAsync("Bobi");
        using var cliente = await ClienteDeAsync(email);

        using var registo = await cliente.GetAsync($"/registos-clinicos/{registoId}");
        using var historico = await cliente.GetAsync($"/animais/{bobi}/historico-clinico");

        await AssertErroAsync(registo, HttpStatusCode.Forbidden);
        await AssertErroAsync(historico, HttpStatusCode.Forbidden);
    }

    [Fact(DisplayName = "I078 — EP-033 e EP-034 sem token devolvem 401")]
    public async Task SemTokenDevolve401()
    {
        using var cliente = Fixture.Factory.CreateClient();

        using var registo = await cliente.GetAsync("/registos-clinicos/1");
        using var historico = await cliente.GetAsync("/animais/1/historico-clinico");

        await AssertErroAsync(registo, HttpStatusCode.Unauthorized);
        await AssertErroAsync(historico, HttpStatusCode.Unauthorized);
    }

    // ----- Auxiliares -----

    /// <summary>
    /// Consulta Realizada do Bobi com o vet.bruno a 2026-09-02, com registo clínico sem tratamentos
    /// nem prescrições. Devolve o identificador do registo.
    /// </summary>
    private async Task<int> AcrescentarRegistoSetembroAsync(int bobi)
    {
        var vetBruno = await ObterUtilizadorIdAsync(EmailVetBruno);
        var rececao = await ObterUtilizadorIdAsync(EmailRececao);

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        var marcacao = new Marcacao
        {
            PacienteId = bobi,
            VeterinarioId = vetBruno,
            DataHoraInicio = InicioSetembro,
            DuracaoMinutos = 30,
            Motivo = "Check-up anual",
            Estado = "Realizada",
            CriadaPorId = rececao,
            DataCriacao = InicioSetembro.AddDays(-7),
        };
        db.Marcacoes.Add(marcacao);
        await db.SaveChangesAsync();

        var registo = new RegistoClinico
        {
            MarcacaoId = marcacao.Id,
            VeterinarioId = vetBruno,
            Diagnostico = "Check-up sem alterações",
            CriadoEm = InicioSetembro.AddMinutes(30),
        };
        db.RegistosClinicos.Add(registo);
        await db.SaveChangesAsync();

        return registo.Id;
    }

    private static async Task<JsonDocument> LerAsync(HttpResponseMessage resposta) =>
        JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());

    /// <summary>O código HTTP esperado, no formato de erro da I058 (<c>code</c>, <c>message</c>, <c>detail</c>).</summary>
    private static async Task AssertErroAsync(HttpResponseMessage resposta, HttpStatusCode esperado)
    {
        Assert.Equal(esperado, resposta.StatusCode);

        using var documento = await LerAsync(resposta);
        Assert.Equal((int)esperado, documento.RootElement.GetProperty("code").GetInt32());
        Assert.False(string.IsNullOrWhiteSpace(documento.RootElement.GetProperty("message").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(documento.RootElement.GetProperty("detail").GetString()));
    }

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

        return await db.Utilizadores.AsNoTracking().Where(u => u.Email == email).Select(u => u.Id).SingleAsync();
    }

    private async Task<int> ObterAnimalIdAsync(string nome)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Pacientes.AsNoTracking().Where(p => p.Nome == nome).Select(p => p.Id).SingleAsync();
    }

    private async Task<int> ObterMarcacaoIdAsync(string emailVeterinario, DateTime inicioUtc)
    {
        var veterinarioId = await ObterUtilizadorIdAsync(emailVeterinario);

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Marcacoes.AsNoTracking()
            .Where(m => m.VeterinarioId == veterinarioId && m.DataHoraInicio == inicioUtc)
            .Select(m => m.Id)
            .SingleAsync();
    }

    private async Task<int> ObterRegistoDaMarcacaoAsync(int marcacaoId)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.RegistosClinicos.AsNoTracking().Where(r => r.MarcacaoId == marcacaoId).Select(r => r.Id).SingleAsync();
    }

    private async Task<(int Registos, int Tratamentos, int Prescricoes)> ContarAsync()
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return (await db.RegistosClinicos.CountAsync(), await db.Tratamentos.CountAsync(), await db.Prescricoes.CountAsync());
    }
}
