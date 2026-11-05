using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.Consultas;

/// <summary>
/// I076 — GET /me/consultas (EP-030), casos IT-EP030-01 a 06 da Especificação de Testes de
/// Integração (I030), mais 401 e 403. Dados da I061: João tem Bobi (M1 Realizada, M5 Pendente) e
/// Mia (M2 Cancelada); Maria tem Kiko (M3 Confirmada). Como as datas da I061 são fixas, cada teste
/// acrescenta marcações relativas ao instante atual, para não depender do dia em que corre.
/// </summary>
public sealed class MinhasConsultasEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private const string EmailJoao = "cliente.joao@clinica-g08.test";
    private const string EmailVetAna = "vet.ana@clinica-g08.test";
    private const string EmailRececao = "rececao@clinica-g08.test";

    private static readonly string[] AnimaisDoJoao = ["Bobi", "Mia"];
    private static readonly string[] EstadosFuturos = ["Pendente", "Confirmada"];

    private static readonly string[] CamposPermitidos = ["animal", "dataHora", "estado", "id", "veterinario"];

    public MinhasConsultasEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    [Fact(DisplayName = "IT-EP030-01 — tipo=futuras devolve só as marcações futuras do Cliente, por data crescente")]
    public async Task FuturasSoDoCliente()
    {
        var m = await CriarMarcacoesDeTesteAsync();
        using var cliente = await CriarClienteAutenticadoPorEmailAsync(EmailJoao);

        using var resposta = await cliente.GetAsync("/me/consultas?tipo=futuras");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        var consultas = await LerConsultasAsync(resposta);

        Assert.Contains(consultas, c => c.Id == m.FuturaBobi);
        Assert.Contains(consultas, c => c.Id == m.FuturaMia);
        Assert.DoesNotContain(consultas, c => c.Id == m.CanceladaFuturaBobi);
        Assert.DoesNotContain(consultas, c => c.Id == m.FuturaKiko);
        Assert.All(consultas, c =>
        {
            Assert.Contains(c.Animal, AnimaisDoJoao);
            Assert.Contains(c.Estado, EstadosFuturos);
            Assert.True(c.DataHora >= m.Inicio);
        });
        Assert.Equal(consultas.OrderBy(c => c.DataHora).ThenBy(c => c.Id).Select(c => c.Id), consultas.Select(c => c.Id));
    }

    [Fact(DisplayName = "I076 — sem tipo devolve o mesmo que tipo=futuras")]
    public async Task SemTipoIgualAFuturas()
    {
        await CriarMarcacoesDeTesteAsync();
        using var cliente = await CriarClienteAutenticadoPorEmailAsync(EmailJoao);

        using var semTipo = await cliente.GetAsync("/me/consultas");
        using var futuras = await cliente.GetAsync("/me/consultas?tipo=futuras");

        Assert.Equal(HttpStatusCode.OK, semTipo.StatusCode);
        Assert.Equal(
            (await LerConsultasAsync(futuras)).Select(c => c.Id),
            (await LerConsultasAsync(semTipo)).Select(c => c.Id));
    }

    [Fact(DisplayName = "IT-EP030-02 — tipo=historico devolve passadas e canceladas (mesmo futuras) do Cliente, por data decrescente")]
    public async Task HistoricoSoDoCliente()
    {
        var m = await CriarMarcacoesDeTesteAsync();
        var m1 = await ObterMarcacaoIdAsync("Bobi", "Realizada");
        var m2 = await ObterMarcacaoIdAsync("Mia", "Cancelada");
        using var cliente = await CriarClienteAutenticadoPorEmailAsync(EmailJoao);

        using var resposta = await cliente.GetAsync("/me/consultas?tipo=historico");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        var consultas = await LerConsultasAsync(resposta);

        Assert.Contains(consultas, c => c.Id == m1);
        Assert.Contains(consultas, c => c.Id == m2);
        Assert.Contains(consultas, c => c.Id == m.CanceladaFuturaBobi);
        Assert.Contains(consultas, c => c.Id == m.PassadaBobi);
        Assert.DoesNotContain(consultas, c => c.Id == m.FuturaBobi);
        Assert.DoesNotContain(consultas, c => c.Id == m.PassadaKiko);
        Assert.All(consultas, c => Assert.Contains(c.Animal, AnimaisDoJoao));
        Assert.Equal(
            consultas.OrderByDescending(c => c.DataHora).ThenByDescending(c => c.Id).Select(c => c.Id),
            consultas.Select(c => c.Id));
    }

    [Theory(DisplayName = "IT-EP030-03 — tipo inválido devolve 400")]
    [InlineData("todas")]
    [InlineData("passadas")]
    public async Task TipoInvalidoDevolve400(string tipo)
    {
        using var cliente = await CriarClienteAutenticadoPorEmailAsync(EmailJoao);

        using var resposta = await cliente.GetAsync($"/me/consultas?tipo={tipo}");

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "I076 — animalId inferior a 1 devolve 400")]
    public async Task AnimalIdInvalidoDevolve400()
    {
        using var cliente = await CriarClienteAutenticadoPorEmailAsync(EmailJoao);

        using var resposta = await cliente.GetAsync("/me/consultas?animalId=0");

        await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "IT-EP030-04 — animalId próprio devolve só as marcações desse animal")]
    public async Task AnimalProprioFiltra()
    {
        var m = await CriarMarcacoesDeTesteAsync();
        var mia = await ObterAnimalIdAsync("Mia");
        using var cliente = await CriarClienteAutenticadoPorEmailAsync(EmailJoao);

        using var futuras = await cliente.GetAsync($"/me/consultas?tipo=futuras&animalId={mia}");
        using var historico = await cliente.GetAsync($"/me/consultas?tipo=historico&animalId={mia}");

        Assert.Equal(HttpStatusCode.OK, futuras.StatusCode);
        Assert.Equal(HttpStatusCode.OK, historico.StatusCode);
        var listaFuturas = await LerConsultasAsync(futuras);
        var listaHistorico = await LerConsultasAsync(historico);

        Assert.Contains(listaFuturas, c => c.Id == m.FuturaMia);
        Assert.Contains(listaHistorico, c => c.Id == m.PassadaMia);
        Assert.All(listaFuturas.Concat(listaHistorico), c => Assert.Equal("Mia", c.Animal));
    }

    [Fact(DisplayName = "IT-EP030-05 — animalId de outro Cliente devolve 403 sem dados")]
    public async Task AnimalDeOutroClienteDevolve403()
    {
        await CriarMarcacoesDeTesteAsync();
        var kiko = await ObterAnimalIdAsync("Kiko");
        using var cliente = await CriarClienteAutenticadoPorEmailAsync(EmailJoao);

        using var resposta = await cliente.GetAsync($"/me/consultas?animalId={kiko}");

        var corpo = await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
        Assert.DoesNotContain("Kiko", corpo, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "I076 — animalId inexistente devolve 403 (o contrato não prevê 404)")]
    public async Task AnimalInexistenteDevolve403()
    {
        var inexistente = await ObterAnimalIdInexistenteAsync();
        using var cliente = await CriarClienteAutenticadoPorEmailAsync(EmailJoao);

        using var resposta = await cliente.GetAsync($"/me/consultas?animalId={inexistente}");

        await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
    }

    [Fact(DisplayName = "IT-EP030-06 — a resposta não expõe informação clínica nem o motivo da marcação")]
    public async Task RespostaSemInformacaoClinica()
    {
        await CriarMarcacoesDeTesteAsync();
        using var cliente = await CriarClienteAutenticadoPorEmailAsync(EmailJoao);

        // A M1 do Bobi tem registo clínico (otite externa), tratamento e prescrição na I061.
        using var historico = await cliente.GetAsync("/me/consultas?tipo=historico");
        using var futuras = await cliente.GetAsync("/me/consultas?tipo=futuras");

        foreach (var resposta in new[] { historico, futuras })
        {
            Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
            var corpo = await resposta.Content.ReadAsStringAsync();

            Assert.DoesNotContain("Otite", corpo, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("diagnostico", corpo, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("motivo", corpo, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("Comichão", corpo, StringComparison.OrdinalIgnoreCase);

            using var documento = JsonDocument.Parse(corpo);
            Assert.All(documento.RootElement.EnumerateArray(), item =>
                Assert.Equal(CamposPermitidos, item.EnumerateObject().Select(p => p.Name).Order(StringComparer.Ordinal)));
        }
    }

    [Theory(DisplayName = "IT-EP030 — perfis internos recebem 403")]
    [InlineData("Administrador")]
    [InlineData("Rececionista")]
    [InlineData("Veterinario")]
    [InlineData("AuxiliarVeterinario")]
    public async Task PerfisInternosRecebem403(string perfil)
    {
        using var cliente = await Fixture.CriarClienteAutenticadoAsync(perfil);

        using var resposta = await cliente.GetAsync("/me/consultas");

        await AssertErroAsync(resposta, HttpStatusCode.Forbidden);
    }

    [Fact(DisplayName = "IT-EP030 — sem token devolve 401")]
    public async Task SemTokenDevolve401()
    {
        using var cliente = Fixture.Factory.CreateClient();

        using var resposta = await cliente.GetAsync("/me/consultas");

        await AssertErroAsync(resposta, HttpStatusCode.Unauthorized);
    }

    // ----- Apoio -----

    private sealed record MarcacoesDeTeste(
        DateTime Inicio,
        int FuturaBobi,
        int FuturaMia,
        int CanceladaFuturaBobi,
        int PassadaBobi,
        int PassadaMia,
        int FuturaKiko,
        int PassadaKiko);

    private sealed record Consulta(int Id, DateTime DataHora, string Estado, string Animal, string Veterinario);

    /// <summary>
    /// Marcações relativas ao instante atual (datas inteiras, às 10:00 UTC), para os testes não
    /// dependerem das datas fixas da I061.
    /// </summary>
    private async Task<MarcacoesDeTeste> CriarMarcacoesDeTesteAsync()
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        var agora = DateTime.UtcNow;
        var hoje = new DateTime(agora.Year, agora.Month, agora.Day, 10, 0, 0, DateTimeKind.Utc);

        var vetAna = await ObterUtilizadorIdAsync(db, EmailVetAna);
        var rececao = await ObterUtilizadorIdAsync(db, EmailRececao);
        var bobi = await ObterAnimalIdAsync(db, "Bobi");
        var mia = await ObterAnimalIdAsync(db, "Mia");
        var kiko = await ObterAnimalIdAsync(db, "Kiko");

        async Task<int> Marcar(int animal, int dias, string estado)
        {
            var marcacao = new Marcacao
            {
                PacienteId = animal,
                VeterinarioId = vetAna,
                DataHoraInicio = hoje.AddDays(dias),
                DuracaoMinutos = 30,
                Motivo = "Comichão no ouvido (teste I076)",
                Estado = estado,
                CriadaPorId = rececao,
                DataCriacao = agora.AddDays(-60)
            };
            db.Marcacoes.Add(marcacao);
            await db.SaveChangesAsync();
            return marcacao.Id;
        }

        return new MarcacoesDeTeste(
            Inicio: agora,
            FuturaBobi: await Marcar(bobi, 40, "Pendente"),
            FuturaMia: await Marcar(mia, 41, "Confirmada"),
            CanceladaFuturaBobi: await Marcar(bobi, 42, "Cancelada"),
            PassadaBobi: await Marcar(bobi, -40, "Realizada"),
            PassadaMia: await Marcar(mia, -41, "Realizada"),
            FuturaKiko: await Marcar(kiko, 43, "Confirmada"),
            PassadaKiko: await Marcar(kiko, -42, "Realizada"));
    }

    private static async Task<List<Consulta>> LerConsultasAsync(HttpResponseMessage resposta)
    {
        using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());

        return documento.RootElement
            .EnumerateArray()
            .Select(c => new Consulta(
                c.GetProperty("id").GetInt32(),
                DateTime.Parse(c.GetProperty("dataHora").GetString()!, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                c.GetProperty("estado").GetString()!,
                c.GetProperty("animal").GetProperty("nome").GetString()!,
                c.GetProperty("veterinario").GetString()!))
            .ToList();
    }

    /// <summary>Formato único de erro da infraestrutura (I058): <c>code</c> igual ao código HTTP.</summary>
    private static async Task<string> AssertErroAsync(HttpResponseMessage resposta, HttpStatusCode esperado)
    {
        Assert.Equal(esperado, resposta.StatusCode);

        var corpo = await resposta.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(corpo), $"A resposta {(int)esperado} não tem corpo.");

        using var documento = JsonDocument.Parse(corpo);
        Assert.Equal((int)esperado, documento.RootElement.GetProperty("code").GetInt32());

        return corpo;
    }

    private async Task<int> ObterMarcacaoIdAsync(string animal, string estado)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        // Marcações da I061 (criadas em 2026-10, antes das marcações de teste).
        return await (
            from m in db.Marcacoes.AsNoTracking()
            join p in db.Pacientes.AsNoTracking() on m.PacienteId equals p.Id
            where p.Nome == animal && m.Estado == estado && m.Motivo != "Comichão no ouvido (teste I076)"
            orderby m.Id
            select m.Id)
            .FirstAsync();
    }

    private async Task<int> ObterAnimalIdAsync(string nome)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        return await ObterAnimalIdAsync(scope.ServiceProvider.GetRequiredService<ClinicaDbContext>(), nome);
    }

    private static Task<int> ObterAnimalIdAsync(ClinicaDbContext db, string nome) =>
        db.Pacientes.AsNoTracking().Where(p => p.Nome == nome).Select(p => p.Id).SingleAsync();

    private static Task<int> ObterUtilizadorIdAsync(ClinicaDbContext db, string email) =>
        db.Utilizadores.AsNoTracking().Where(u => u.Email == email).Select(u => u.Id).SingleAsync();

    private async Task<int> ObterAnimalIdInexistenteAsync()
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        var maior = await db.Pacientes.AsNoTracking().MaxAsync(p => (int?)p.Id) ?? 0;
        return maior + 1000;
    }

    /// <summary>Token de uma conta concreta, para distinguir os Clientes da I061.</summary>
    private async Task<HttpClient> CriarClienteAutenticadoPorEmailAsync(string email)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        var utilizadorId = await ObterUtilizadorIdAsync(db, email);

        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var jwtTokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();

        var token = jwtTokenService.GerarToken(await authService.ObterAutenticadoAsync(utilizadorId));

        var cliente = Fixture.Factory.CreateClient();
        cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

        return cliente;
    }
}
