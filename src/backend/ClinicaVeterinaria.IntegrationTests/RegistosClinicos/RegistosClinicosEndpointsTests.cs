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
/// I077 — EP-031 (POST /registos-clinicos) e EP-032 (PUT /registos-clinicos/{id}) com a API real e
/// o SQL Server dos testes de integração (I091), sobre os dados de demonstração da I061: casos
/// IT-EP031-01 a 09 e IT-EP032-01 a 05 da Especificação de Testes de Integração (I030, secção 12),
/// a matriz de perfis (401 e 403), o Veterinário de outra consulta (403) e a criação simultânea.
/// </summary>
/// <remarks>
/// Marcações da I061 usadas: M1 (Bobi, vet.ana, Realizada, já com registo clínico), M2 (Mia,
/// vet.bruno, Cancelada), M3 (Kiko, vet.ana, Confirmada, sem registo) e M5 (Bobi, vet.ana, Pendente).
/// </remarks>
public sealed class RegistosClinicosEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailVetAna = "vet.ana@clinica-g08.test";
    private const string EmailVetBruno = "vet.bruno@clinica-g08.test";

    private static readonly DateTime InicioM1 = new(2026, 10, 14, 9, 30, 0);
    private static readonly DateTime InicioM2 = new(2026, 10, 20, 10, 0, 0);
    private static readonly DateTime InicioM3 = new(2026, 11, 3, 9, 0, 0);
    private static readonly DateTime InicioM5 = new(2026, 11, 5, 11, 0, 0);

    public RegistosClinicosEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    // ----- EP-031 — Criar registo clínico -----

    [Fact(DisplayName = "IT-EP031-01 — cria o registo com tratamento e prescrição persistidos (201 e Location)")]
    public async Task CriaRegisto()
    {
        var m3 = await ObterMarcacaoIdAsync(EmailVetAna, InicioM3);
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PostAsJsonAsync("/registos-clinicos", CorpoExemplo(m3));

        Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        var raiz = documento.RootElement;
        var id = raiz.GetProperty("id").GetInt32();
        Assert.Equal($"/registos-clinicos/{id}", resposta.Headers.Location!.OriginalString);
        Assert.Equal(m3, raiz.GetProperty("consultaId").GetInt32());
        Assert.Equal("Dra. Ana Veterinária (demonstração)", raiz.GetProperty("veterinario").GetString());
        Assert.Equal("Otite externa", raiz.GetProperty("diagnostico").GetString());
        Assert.Equal("2026-11-03", Assert.Single(raiz.GetProperty("tratamentos").EnumerateArray().ToList()).GetProperty("dataAplicacao").GetString());
        Assert.Equal(7, Assert.Single(raiz.GetProperty("prescricoes").EnumerateArray().ToList()).GetProperty("duracaoDias").GetInt32());
        Assert.Equal(JsonValueKind.Null, raiz.GetProperty("atualizadoEm").ValueKind);

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
        Assert.Equal(m3, (await db.RegistosClinicos.AsNoTracking().SingleAsync(r => r.Id == id)).MarcacaoId);
        Assert.Equal(1, await db.Tratamentos.CountAsync(t => t.RegistoClinicoId == id));
        Assert.Equal(1, await db.Prescricoes.CountAsync(p => p.RegistoClinicoId == id));
        Assert.Equal("Confirmada", (await db.Marcacoes.AsNoTracking().SingleAsync(m => m.Id == m3)).Estado);
    }

    [Fact(DisplayName = "IT-EP031-02 — consultaId em falta devolve 400")]
    public async Task ConsultaIdEmFaltaDevolve400()
    {
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PostAsJsonAsync("/registos-clinicos", new { diagnostico = "Otite externa" });

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "IT-EP031-03 — nenhum elemento clínico preenchido devolve 400")]
    public async Task SemElementoClinicoDevolve400()
    {
        var m3 = await ObterMarcacaoIdAsync(EmailVetAna, InicioM3);
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PostAsJsonAsync(
            "/registos-clinicos",
            new { consultaId = m3, notasClinicas = "   ", diagnostico = "", tratamentos = Array.Empty<object>(), prescricoes = Array.Empty<object>() });

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "IT-EP031-04 — prescrição incompleta (sem dosagem) devolve 400")]
    public async Task PrescricaoIncompletaDevolve400()
    {
        var m3 = await ObterMarcacaoIdAsync(EmailVetAna, InicioM3);
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PostAsJsonAsync(
            "/registos-clinicos",
            new { consultaId = m3, prescricoes = new[] { new { medicamento = "Gotas X", frequencia = "2 vezes por dia" } } });

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "IT-EP031-05 — consulta inexistente devolve 404")]
    public async Task ConsultaInexistenteDevolve404()
    {
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PostAsJsonAsync("/registos-clinicos", CorpoExemplo(999999));

        await AssertErroAsync(resposta, HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "IT-EP031-06 — consulta que já tem registo (M1) devolve 409")]
    public async Task ConsultaComRegistoDevolve409()
    {
        var m1 = await ObterMarcacaoIdAsync(EmailVetAna, InicioM1);
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PostAsJsonAsync("/registos-clinicos", CorpoExemplo(m1));

        await AssertErroAsync(resposta, HttpStatusCode.Conflict);
    }

    [Fact(DisplayName = "IT-EP031-07 — consulta Pendente (M5) devolve 422")]
    public async Task ConsultaPendenteDevolve422()
    {
        var m5 = await ObterMarcacaoIdAsync(EmailVetAna, InicioM5);
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PostAsJsonAsync("/registos-clinicos", CorpoExemplo(m5));

        await AssertErroAsync(resposta, HttpStatusCode.UnprocessableEntity);
    }

    [Fact(DisplayName = "IT-EP031-08 — consulta Cancelada (M2, do vet.bruno) devolve 422")]
    public async Task ConsultaCanceladaDevolve422()
    {
        var m2 = await ObterMarcacaoIdAsync(EmailVetBruno, InicioM2);
        using var cliente = await ClienteDeAsync(EmailVetBruno);

        using var resposta = await cliente.PostAsJsonAsync("/registos-clinicos", CorpoExemplo(m2));

        await AssertErroAsync(resposta, HttpStatusCode.UnprocessableEntity);
    }

    [Fact(DisplayName = "IT-EP031-09 — pedidos rejeitados não deixam registos, tratamentos nem prescrições gravados")]
    public async Task PedidosRejeitadosNaoGravamNada()
    {
        var antes = await ContarAsync();
        var m1 = await ObterMarcacaoIdAsync(EmailVetAna, InicioM1);
        var m3 = await ObterMarcacaoIdAsync(EmailVetAna, InicioM3);
        var m5 = await ObterMarcacaoIdAsync(EmailVetAna, InicioM5);
        using var cliente = await ClienteDeAsync(EmailVetAna);

        // 400 (prescrição incompleta, mas com tratamento válido), 409, 422 e 404.
        using var r400 = await cliente.PostAsJsonAsync(
            "/registos-clinicos",
            new
            {
                consultaId = m3,
                tratamentos = new[] { new { descricao = "Limpeza auricular", dataAplicacao = "2026-11-03" } },
                prescricoes = new[] { new { medicamento = "Gotas X", dosagem = "3 gotas" } }
            });
        using var r409 = await cliente.PostAsJsonAsync("/registos-clinicos", CorpoExemplo(m1));
        using var r422 = await cliente.PostAsJsonAsync("/registos-clinicos", CorpoExemplo(m5));
        using var r404 = await cliente.PostAsJsonAsync("/registos-clinicos", CorpoExemplo(999999));

        Assert.Equal(HttpStatusCode.BadRequest, r400.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, r409.StatusCode);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, r422.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, r404.StatusCode);
        Assert.Equal(antes, await ContarAsync());
    }

    [Fact(DisplayName = "I077 — consulta de outro Veterinário (M3 da vet.ana, pedido do vet.bruno) devolve 403")]
    public async Task ConsultaDeOutroVeterinarioDevolve403()
    {
        var m3 = await ObterMarcacaoIdAsync(EmailVetAna, InicioM3);
        using var cliente = await ClienteDeAsync(EmailVetBruno);

        using var resposta = await cliente.PostAsJsonAsync("/registos-clinicos", CorpoExemplo(m3));

        await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
    }

    [Fact(DisplayName = "I077 — pedidos simultâneos para a mesma consulta: um 201 e os outros 409, um só registo")]
    public async Task PedidosSimultaneosParaAMesmaConsulta()
    {
        const int Pedidos = 5;
        var m3 = await ObterMarcacaoIdAsync(EmailVetAna, InicioM3);
        var clientes = new List<HttpClient>();

        for (var i = 0; i < Pedidos; i++)
        {
            clientes.Add(await ClienteDeAsync(EmailVetAna));
        }

        try
        {
            var respostas = await Task.WhenAll(clientes.Select(c => c.PostAsJsonAsync("/registos-clinicos", CorpoExemplo(m3))));

            try
            {
                Assert.Single(respostas, r => r.StatusCode == HttpStatusCode.Created);
                Assert.Equal(Pedidos - 1, respostas.Count(r => r.StatusCode == HttpStatusCode.Conflict));

                using var scope = Fixture.Factory.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
                var registo = await db.RegistosClinicos.AsNoTracking().SingleAsync(r => r.MarcacaoId == m3);
                Assert.Equal(1, await db.Tratamentos.CountAsync(t => t.RegistoClinicoId == registo.Id));
            }
            finally
            {
                foreach (var resposta in respostas)
                {
                    resposta.Dispose();
                }
            }
        }
        finally
        {
            foreach (var cliente in clientes)
            {
                cliente.Dispose();
            }
        }
    }

    [Theory(DisplayName = "I077 — só o Veterinário: Rececionista, Auxiliar, Administrador e Cliente recebem 403 em EP-031 e EP-032")]
    [InlineData("rececao@clinica-g08.test")]
    [InlineData("aux.carla@clinica-g08.test")]
    [InlineData("admin@clinica-g08.test")]
    [InlineData("cliente.joao@clinica-g08.test")]
    public async Task OutrosPerfisRecebem403(string email)
    {
        var m3 = await ObterMarcacaoIdAsync(EmailVetAna, InicioM3);
        var registoM1 = await ObterRegistoDaMarcacaoAsync(EmailVetAna, InicioM1);
        using var cliente = await ClienteDeAsync(email);

        using var criar = await cliente.PostAsJsonAsync("/registos-clinicos", CorpoExemplo(m3));
        using var atualizar = await cliente.PutAsJsonAsync($"/registos-clinicos/{registoM1}", new { diagnostico = "Outro" });

        await AssertErroAsync(criar, HttpStatusCode.Forbidden);
        await AssertErroAsync(atualizar, HttpStatusCode.Forbidden);
    }

    [Fact(DisplayName = "I077 — EP-031 e EP-032 sem token devolvem 401")]
    public async Task SemTokenDevolve401()
    {
        using var cliente = Fixture.Factory.CreateClient();

        using var criar = await cliente.PostAsJsonAsync("/registos-clinicos", CorpoExemplo(1));
        using var atualizar = await cliente.PutAsJsonAsync("/registos-clinicos/1", new { diagnostico = "Outro" });

        await AssertErroAsync(criar, HttpStatusCode.Unauthorized);
        await AssertErroAsync(atualizar, HttpStatusCode.Unauthorized);
    }

    // ----- EP-032 — Atualizar registo clínico -----

    [Fact(DisplayName = "IT-EP032-01 e 04 — atualiza o registo da M1 e regista atualizadoEm e o autor")]
    public async Task AtualizaRegisto()
    {
        var m1 = await ObterMarcacaoIdAsync(EmailVetAna, InicioM1);
        var registoId = await ObterRegistoDaMarcacaoAsync(EmailVetAna, InicioM1);
        var vetAna = await ObterUtilizadorIdAsync(EmailVetAna);
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PutAsJsonAsync(
            $"/registos-clinicos/{registoId}",
            new { notasClinicas = "Reavaliação: melhoria evidente.", diagnostico = "Otite externa (em resolução)", consultaId = 999999 });

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        using var documento = await LerAsync(resposta);
        var raiz = documento.RootElement;
        Assert.Equal("Otite externa (em resolução)", raiz.GetProperty("diagnostico").GetString());
        Assert.Equal("Dra. Ana Veterinária (demonstração)", raiz.GetProperty("atualizadoPor").GetString());
        Assert.Equal(JsonValueKind.String, raiz.GetProperty("atualizadoEm").ValueKind);

        // A consulta de origem não muda, mesmo que o pedido traga outro consultaId (critério 5).
        Assert.Equal(m1, raiz.GetProperty("consultaId").GetInt32());

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
        var gravado = await db.RegistosClinicos.AsNoTracking().SingleAsync(r => r.Id == registoId);
        Assert.Equal("Otite externa (em resolução)", gravado.Diagnostico);
        Assert.Equal(m1, gravado.MarcacaoId);
        Assert.Equal(vetAna, gravado.AtualizadoPorId);
        Assert.NotNull(gravado.AtualizadoEm);
    }

    [Fact(DisplayName = "IT-EP032-02 — dados clínicos inválidos devolvem 400 e o registo fica como estava")]
    public async Task DadosInvalidosDevolvem400()
    {
        var registoId = await ObterRegistoDaMarcacaoAsync(EmailVetAna, InicioM1);
        var antes = await LerRegistoAsync(registoId);
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PutAsJsonAsync($"/registos-clinicos/{registoId}", new { notasClinicas = "" });

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
        var depois = await LerRegistoAsync(registoId);
        Assert.Equal(antes.Diagnostico, depois.Diagnostico);
        Assert.Equal(antes.AtualizadoEm, depois.AtualizadoEm);
    }

    [Fact(DisplayName = "IT-EP032-03 — registo inexistente devolve 404")]
    public async Task RegistoInexistenteDevolve404()
    {
        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PutAsJsonAsync("/registos-clinicos/999999", new { diagnostico = "Otite externa" });

        await AssertErroAsync(resposta, HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "IT-EP032-05 — mantém o tratamento indicado, acrescenta um novo e remove a prescrição")]
    public async Task ListasFicamCoerentes()
    {
        var registoId = await ObterRegistoDaMarcacaoAsync(EmailVetAna, InicioM1);
        int tratamentoExistente;

        using (var scope = Fixture.Factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
            tratamentoExistente = await db.Tratamentos.AsNoTracking()
                .Where(t => t.RegistoClinicoId == registoId)
                .Select(t => t.Id)
                .FirstAsync();
        }

        using var cliente = await ClienteDeAsync(EmailVetAna);

        using var resposta = await cliente.PutAsJsonAsync(
            $"/registos-clinicos/{registoId}",
            new
            {
                diagnostico = "Otite externa",
                tratamentos = new object[]
                {
                    new { id = tratamentoExistente, descricao = "Limpeza auricular (repetida)", dataAplicacao = "2026-10-14" },
                    new { descricao = "Aplicação de pomada", dataAplicacao = "2026-10-15" }
                },
                prescricoes = Array.Empty<object>()
            });

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        using var scopeDepois = Fixture.Factory.Services.CreateScope();
        var dbDepois = scopeDepois.ServiceProvider.GetRequiredService<ClinicaDbContext>();
        var tratamentos = await dbDepois.Tratamentos.AsNoTracking()
            .Where(t => t.RegistoClinicoId == registoId)
            .OrderBy(t => t.Id)
            .ToListAsync();
        Assert.Equal(2, tratamentos.Count);
        Assert.Equal(tratamentoExistente, tratamentos[0].Id);
        Assert.Equal("Limpeza auricular (repetida)", tratamentos[0].Descricao);
        Assert.Equal("Aplicação de pomada", tratamentos[1].Descricao);
        Assert.Equal(0, await dbDepois.Prescricoes.CountAsync(p => p.RegistoClinicoId == registoId));
    }

    [Fact(DisplayName = "I077 — registo de outro Veterinário (M1 da vet.ana, pedido do vet.bruno) devolve 403")]
    public async Task RegistoDeOutroVeterinarioDevolve403()
    {
        var registoId = await ObterRegistoDaMarcacaoAsync(EmailVetAna, InicioM1);
        using var cliente = await ClienteDeAsync(EmailVetBruno);

        using var resposta = await cliente.PutAsJsonAsync($"/registos-clinicos/{registoId}", new { diagnostico = "Outro" });

        await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
    }

    // ----- Auxiliares -----

    /// <summary>O pedido de exemplo da Documentação de API (EP-031), com datas de novembro.</summary>
    private static object CorpoExemplo(int consultaId) => new
    {
        consultaId,
        notasClinicas = "Animal ativo; ligeira inflamação no ouvido esquerdo.",
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

    private async Task<int> ObterRegistoDaMarcacaoAsync(string emailVeterinario, DateTime inicioUtc)
    {
        var marcacaoId = await ObterMarcacaoIdAsync(emailVeterinario, inicioUtc);

        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.RegistosClinicos.AsNoTracking().Where(r => r.MarcacaoId == marcacaoId).Select(r => r.Id).SingleAsync();
    }

    private async Task<(string? Diagnostico, DateTime? AtualizadoEm)> LerRegistoAsync(int registoId)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
        var registo = await db.RegistosClinicos.AsNoTracking().SingleAsync(r => r.Id == registoId);

        return (registo.Diagnostico, registo.AtualizadoEm);
    }

    private async Task<(int Registos, int Tratamentos, int Prescricoes)> ContarAsync()
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return (await db.RegistosClinicos.CountAsync(), await db.Tratamentos.CountAsync(), await db.Prescricoes.CountAsync());
    }
}
