using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.RegistosClinicos;

/// <summary>
/// I094 — persistência e associação do registo clínico através da API real e do SQL Server dos
/// testes de integração (I091): o registo criado e atualizado fica na consulta e no paciente
/// certos e é visto assim em EP-033 e EP-034 (critério 3); os perfis sem permissão de escrita
/// recebem 403 e nada é gravado (critério 4).
/// </summary>
/// <remarks>
/// Dados da I061 usados: M1 (Bobi, vet.ana, Realizada, com registo clínico) e M3 (Kiko, vet.ana,
/// Confirmada, sem registo). O Kiko não tem histórico clínico; o Bobi tem o registo da M1. A base
/// é reposta antes de cada teste.
/// </remarks>
public sealed class RegistoClinicoPersistenciaEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailVetAna = "vet.ana@clinica-g08.test";
    private const string NomeVetAna = "Dra. Ana Veterinária (demonstração)";

    private static readonly DateTime InicioM1 = new(2026, 10, 14, 9, 30, 0);
    private static readonly DateTime InicioM3 = new(2026, 11, 3, 9, 0, 0);

    public RegistoClinicoPersistenciaEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    [Fact(DisplayName = "I094 (critério 3) — o registo criado na M3 fica na consulta e no paciente dela: EP-033 e o histórico do Kiko mostram-no, o do Bobi não")]
    public async Task RegistoCriadoFicaNaConsultaENoPaciente()
    {
        var m3 = await ObterMarcacaoIdAsync(InicioM3);
        var kiko = await ObterAnimalIdAsync("Kiko");
        var bobi = await ObterAnimalIdAsync("Bobi");
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var criar = await cliente.PostAsJsonAsync("/registos-clinicos", CorpoCriacao(m3));
        Assert.Equal(HttpStatusCode.Created, criar.StatusCode);
        int registoId;
        using (var documento = await LerAsync(criar))
        {
            registoId = documento.RootElement.GetProperty("id").GetInt32();
        }

        // EP-033: o registo pertence à M3 e ao paciente dela.
        using var registo = await cliente.GetAsync($"/registos-clinicos/{registoId}");
        Assert.Equal(HttpStatusCode.OK, registo.StatusCode);
        using (var documento = await LerAsync(registo))
        {
            var raiz = documento.RootElement;
            Assert.Equal(m3, raiz.GetProperty("consultaId").GetInt32());
            Assert.Equal(kiko, raiz.GetProperty("animalId").GetInt32());
            Assert.Equal(NomeVetAna, raiz.GetProperty("veterinario").GetString());
            Assert.Equal("Kiko com prurido auricular; sem febre.", raiz.GetProperty("notasClinicas").GetString());
            Assert.Equal("Otite externa", raiz.GetProperty("diagnostico").GetString());
            Assert.Equal("Limpeza auricular", Assert.Single(raiz.GetProperty("tratamentos").EnumerateArray().ToList()).GetProperty("descricao").GetString());
            Assert.Equal("Gotas auriculares X", Assert.Single(raiz.GetProperty("prescricoes").EnumerateArray().ToList()).GetProperty("medicamento").GetString());
        }

        // EP-034: aparece no histórico do Kiko, com a data da consulta e as contagens certas.
        using var historicoKiko = await cliente.GetAsync($"/animais/{kiko}/historico-clinico");
        Assert.Equal(HttpStatusCode.OK, historicoKiko.StatusCode);
        using (var documento = await LerAsync(historicoKiko))
        {
            var resumo = Assert.Single(documento.RootElement.GetProperty("registos").EnumerateArray().ToList());
            Assert.Equal(registoId, resumo.GetProperty("id").GetInt32());
            Assert.Equal(m3, resumo.GetProperty("consultaId").GetInt32());
            Assert.Equal("2026-11-03", resumo.GetProperty("data").GetString());
            Assert.Equal(1, resumo.GetProperty("numeroTratamentos").GetInt32());
            Assert.Equal(1, resumo.GetProperty("numeroPrescricoes").GetInt32());
        }

        // EP-034: não aparece no histórico de outro paciente.
        using var historicoBobi = await cliente.GetAsync($"/animais/{bobi}/historico-clinico");
        Assert.Equal(HttpStatusCode.OK, historicoBobi.StatusCode);
        using (var documento = await LerAsync(historicoBobi))
        {
            Assert.DoesNotContain(
                documento.RootElement.GetProperty("registos").EnumerateArray().ToList(),
                r => r.GetProperty("id").GetInt32() == registoId);
        }

        // Base de dados: registo, tratamento e prescrição ligados entre si, à M3 e ao Kiko.
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
        var gravado = await db.RegistosClinicos.AsNoTracking().SingleAsync(r => r.Id == registoId);
        Assert.Equal(m3, gravado.MarcacaoId);
        Assert.Equal(kiko, await db.Marcacoes.AsNoTracking().Where(m => m.Id == gravado.MarcacaoId).Select(m => m.PacienteId).SingleAsync());
        Assert.Equal("Limpeza auricular", (await db.Tratamentos.AsNoTracking().SingleAsync(t => t.RegistoClinicoId == registoId)).Descricao);
        Assert.Equal("3 gotas", (await db.Prescricoes.AsNoTracking().SingleAsync(p => p.RegistoClinicoId == registoId)).Dosagem);
    }

    [Fact(DisplayName = "I094 (critério 3) — a atualização grava notas, diagnóstico, tratamentos e prescrições e mantém a consulta e o paciente")]
    public async Task AtualizacaoGravaElementosEMantemAssociacoes()
    {
        var m3 = await ObterMarcacaoIdAsync(InicioM3);
        var kiko = await ObterAnimalIdAsync("Kiko");
        var vetAna = await ObterUtilizadorIdAsync(EmailVetAna);
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var criar = await cliente.PostAsJsonAsync("/registos-clinicos", CorpoCriacao(m3));
        Assert.Equal(HttpStatusCode.Created, criar.StatusCode);
        int registoId;
        int tratamentoId;
        int prescricaoId;
        using (var documento = await LerAsync(criar))
        {
            var raiz = documento.RootElement;
            registoId = raiz.GetProperty("id").GetInt32();
            tratamentoId = Assert.Single(raiz.GetProperty("tratamentos").EnumerateArray().ToList()).GetProperty("id").GetInt32();
            prescricaoId = Assert.Single(raiz.GetProperty("prescricoes").EnumerateArray().ToList()).GetProperty("id").GetInt32();
        }

        // Mantém o tratamento e a prescrição (pelo id, com dados novos) e acrescenta um tratamento.
        using var atualizar = await cliente.PutAsJsonAsync(
            $"/registos-clinicos/{registoId}",
            new
            {
                notasClinicas = "Reavaliação: canal auditivo menos inflamado.",
                diagnostico = "Otite externa (em resolução)",
                tratamentos = new object[]
                {
                    new { id = tratamentoId, descricao = "Limpeza auricular (repetida)", dataAplicacao = "2026-11-03" },
                    new { descricao = "Aplicação de pomada", dataAplicacao = "2026-11-03" }
                },
                prescricoes = new[]
                {
                    new { id = prescricaoId, medicamento = "Gotas auriculares X", dosagem = "2 gotas", frequencia = "2 vezes por dia", duracaoDias = 5 }
                }
            });

        Assert.Equal(HttpStatusCode.OK, atualizar.StatusCode);

        // EP-033: os elementos novos, na mesma consulta e no mesmo paciente.
        using var registo = await cliente.GetAsync($"/registos-clinicos/{registoId}");
        Assert.Equal(HttpStatusCode.OK, registo.StatusCode);
        using (var documento = await LerAsync(registo))
        {
            var raiz = documento.RootElement;
            Assert.Equal(m3, raiz.GetProperty("consultaId").GetInt32());
            Assert.Equal(kiko, raiz.GetProperty("animalId").GetInt32());
            Assert.Equal("Reavaliação: canal auditivo menos inflamado.", raiz.GetProperty("notasClinicas").GetString());
            Assert.Equal("Otite externa (em resolução)", raiz.GetProperty("diagnostico").GetString());
            Assert.Equal(NomeVetAna, raiz.GetProperty("atualizadoPor").GetString());
            Assert.Equal(2, raiz.GetProperty("tratamentos").GetArrayLength());
            var prescricao = Assert.Single(raiz.GetProperty("prescricoes").EnumerateArray().ToList());
            Assert.Equal(prescricaoId, prescricao.GetProperty("id").GetInt32());
            Assert.Equal("2 gotas", prescricao.GetProperty("dosagem").GetString());
        }

        // EP-034: o resumo do Kiko reflete a atualização.
        using var historico = await cliente.GetAsync($"/animais/{kiko}/historico-clinico");
        Assert.Equal(HttpStatusCode.OK, historico.StatusCode);
        using (var documento = await LerAsync(historico))
        {
            var resumo = Assert.Single(documento.RootElement.GetProperty("registos").EnumerateArray().ToList());
            Assert.Equal(registoId, resumo.GetProperty("id").GetInt32());
            Assert.Equal("Otite externa (em resolução)", resumo.GetProperty("diagnostico").GetString());
            Assert.Equal(2, resumo.GetProperty("numeroTratamentos").GetInt32());
            Assert.Equal(1, resumo.GetProperty("numeroPrescricoes").GetInt32());
        }

        // Base de dados: tudo continua ligado ao mesmo registo, à M3 e ao Kiko.
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
        var gravado = await db.RegistosClinicos.AsNoTracking().SingleAsync(r => r.Id == registoId);
        Assert.Equal(m3, gravado.MarcacaoId);
        Assert.Equal(vetAna, gravado.AtualizadoPorId);
        Assert.Equal(kiko, await db.Marcacoes.AsNoTracking().Where(m => m.Id == gravado.MarcacaoId).Select(m => m.PacienteId).SingleAsync());

        var tratamentos = await db.Tratamentos.AsNoTracking().Where(t => t.RegistoClinicoId == registoId).OrderBy(t => t.Id).ToListAsync();
        Assert.Equal(2, tratamentos.Count);
        Assert.Equal(tratamentoId, tratamentos[0].Id);
        Assert.Equal("Limpeza auricular (repetida)", tratamentos[0].Descricao);
        Assert.Equal("Aplicação de pomada", tratamentos[1].Descricao);

        var prescricaoGravada = await db.Prescricoes.AsNoTracking().SingleAsync(p => p.RegistoClinicoId == registoId);
        Assert.Equal(prescricaoId, prescricaoGravada.Id);
        Assert.Equal("2 gotas", prescricaoGravada.Dosagem);
    }

    [Theory(DisplayName = "I094 (critério 4) — Rececionista, Administrador e Cliente recebem 403 ao criar ou alterar e nada é gravado")]
    [InlineData("rececao@clinica-g08.test")]
    [InlineData("admin@clinica-g08.test")]
    [InlineData("cliente.joao@clinica-g08.test")]
    public async Task PerfisSemPermissaoNaoGravamNada(string email)
    {
        var m3 = await ObterMarcacaoIdAsync(InicioM3);
        var registoM1 = await ObterRegistoDaMarcacaoAsync(await ObterMarcacaoIdAsync(InicioM1));
        var antes = await ContarAsync();
        var diagnosticoAntes = await LerDiagnosticoAsync(registoM1);
        using var cliente = await ClienteDeAsync(email);

        using var criar = await cliente.PostAsJsonAsync("/registos-clinicos", CorpoCriacao(m3));
        using var atualizar = await cliente.PutAsJsonAsync(
            $"/registos-clinicos/{registoM1}",
            new { diagnostico = "Alterado por um perfil sem permissão" });

        await AssertErroAsync(criar, HttpStatusCode.Forbidden);
        await AssertErroAsync(atualizar, HttpStatusCode.Forbidden);
        Assert.Equal(antes, await ContarAsync());
        Assert.Equal(diagnosticoAntes, await LerDiagnosticoAsync(registoM1));
    }

    // ----- Auxiliares -----

    private static object CorpoCriacao(int consultaId) => new
    {
        consultaId,
        notasClinicas = "Kiko com prurido auricular; sem febre.",
        diagnostico = "Otite externa",
        tratamentos = new[] { new { descricao = "Limpeza auricular", dataAplicacao = "2026-11-03" } },
        prescricoes = new[] { new { medicamento = "Gotas auriculares X", dosagem = "3 gotas", frequencia = "2 vezes por dia", duracaoDias = 7 } }
    };

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

    /// <summary>Marcação da vet.ana que começa a essa hora (M1 ou M3 dos dados da I061).</summary>
    private async Task<int> ObterMarcacaoIdAsync(DateTime inicioUtc)
    {
        var veterinarioId = await ObterUtilizadorIdAsync(EmailVetAna);

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

    private async Task<string?> LerDiagnosticoAsync(int registoId)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.RegistosClinicos.AsNoTracking().Where(r => r.Id == registoId).Select(r => r.Diagnostico).SingleAsync();
    }

    private async Task<(int Registos, int Tratamentos, int Prescricoes)> ContarAsync()
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return (await db.RegistosClinicos.CountAsync(), await db.Tratamentos.CountAsync(), await db.Prescricoes.CountAsync());
    }
}
