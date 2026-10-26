using System.Globalization;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace ClinicaVeterinaria.UnitTests.Seguranca;

/// <summary>
/// I064 — fim de sessão (EP-002) e limpeza das revogações expiradas no <see cref="AuthService"/>.
/// Inclui os casos UT-F001-016, UT-F001-021 e UT-F001-022 da Especificação de Testes
/// Unitários (I028, secção 7.1.1); os restantes casos são próprios desta tarefa.
/// </summary>
public class AuthServiceFimSessaoTests
{
    // RELOGIO da Especificação de Testes Unitários (I028, secção 5).
    private static readonly DateTimeOffset Relogio = new(2026, 10, 14, 8, 0, 0, TimeSpan.Zero);

    // exp de um token emitido em RELOGIO (validade de 8 horas).
    private static readonly DateTime ExpiracaoToken = new(2026, 10, 14, 16, 0, 0, DateTimeKind.Utc);

    // REC_A da Especificação de Testes Unitários (I028, secção 5).
    private const int RecA = 2;

    [Fact(DisplayName = "UT-F001-021 — O fim de sessão regista o jti como revogado até à expiração do token")]
    public async Task FimDeSessaoRegistaRevogacaoAteExpiracao()
    {
        using var db = CriarBaseComRececionista();
        var servico = CriarServico(db, Relogio);

        await servico.TerminarSessaoAsync(RecA, "j2", ExpiracaoToken);

        var revogado = Assert.Single(await db.TokensRevogados.AsNoTracking().ToListAsync());
        Assert.Equal("j2", revogado.Jti);
        Assert.Equal(RecA, revogado.UtilizadorId);
        Assert.Equal(ExpiracaoToken, revogado.DataExpiracao);
        Assert.Equal(Relogio.UtcDateTime, revogado.DataRevogacao);
    }

    [Fact(DisplayName = "UT-F001-022 — Terminar a sessão duas vezes com o mesmo jti não falha nem repete a revogação")]
    public async Task FimDeSessaoRepetidoEIdempotente()
    {
        using var db = CriarBaseComRececionista();
        var servico = CriarServico(db, Relogio);
        await servico.TerminarSessaoAsync(RecA, "j2", ExpiracaoToken);

        await servico.TerminarSessaoAsync(RecA, "j2", ExpiracaoToken);

        Assert.Single(await db.TokensRevogados.AsNoTracking().ToListAsync());
    }

    [Fact(DisplayName = "UT-F001-016 — Um token com o jti revogado deixa de ser uma sessão válida")]
    public async Task TokenComJtiRevogadoNaoESessaoValida()
    {
        using var db = CriarBaseComRececionista();
        db.TokensRevogados.Add(Revogacao("j1", ExpiracaoToken));
        await db.SaveChangesAsync();
        var servico = CriarServico(db, Relogio);

        var valida = await servico.SessaoValidaAsync(RecA, Perfis.Rececionista, "j1");

        Assert.False(valida);
    }

    [Fact(DisplayName = "I064 — Depois do fim de sessão, o mesmo token deixa de ser uma sessão válida")]
    public async Task DepoisDoFimDeSessaoOTokenDeixaDeSerValido()
    {
        using var db = CriarBaseComRececionista();
        var servico = CriarServico(db, Relogio);

        await servico.TerminarSessaoAsync(RecA, "j1", ExpiracaoToken);

        Assert.False(await servico.SessaoValidaAsync(RecA, Perfis.Rececionista, "j1"));
    }

    [Fact(DisplayName = "I064 — O fim de sessão não afeta os outros tokens do mesmo utilizador")]
    public async Task FimDeSessaoNaoAfetaOutrosTokens()
    {
        using var db = CriarBaseComRececionista();
        var servico = CriarServico(db, Relogio);

        await servico.TerminarSessaoAsync(RecA, "j1", ExpiracaoToken);

        Assert.True(await servico.SessaoValidaAsync(RecA, Perfis.Rececionista, "j3"));
    }

    [Fact(DisplayName = "I064 — A revogação fica na base de dados e continua a valer num novo arranque da API")]
    public async Task RevogacaoContinuaValidaDepoisDeReiniciar()
    {
        var nomeBase = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

        using (var antesDeReiniciar = CriarBaseComRececionista(nomeBase))
        {
            await CriarServico(antesDeReiniciar, Relogio).TerminarSessaoAsync(RecA, "j1", ExpiracaoToken);
        }

        // Contexto e serviço novos sobre a mesma base: nada vem da memória do processo anterior.
        using var depoisDeReiniciar = AjudaBaseDados.CriarContexto(nomeBase);
        var servico = CriarServico(depoisDeReiniciar, Relogio.AddMinutes(5));

        Assert.False(await servico.SessaoValidaAsync(RecA, Perfis.Rececionista, "j1"));
    }

    [Fact(DisplayName = "I064 — A data de revogação é gravada sem frações de segundo (DATETIME2(0))")]
    public async Task DataDeRevogacaoSemFracoesDeSegundo()
    {
        using var db = CriarBaseComRececionista();
        var servico = CriarServico(db, Relogio.AddMilliseconds(567));

        await servico.TerminarSessaoAsync(RecA, "j2", ExpiracaoToken);

        var revogado = Assert.Single(await db.TokensRevogados.AsNoTracking().ToListAsync());
        Assert.Equal(Relogio.UtcDateTime, revogado.DataRevogacao);
    }

    [Theory(DisplayName = "I064 — Um jti vazio é recusado (400) e nada é gravado")]
    [InlineData("")]
    [InlineData("   ")]
    public async Task JtiVazioERecusado(string jti)
    {
        using var db = CriarBaseComRececionista();
        var servico = CriarServico(db, Relogio);

        await Assert.ThrowsAsync<ArgumentException>(() => servico.TerminarSessaoAsync(RecA, jti, ExpiracaoToken));

        Assert.Empty(await db.TokensRevogados.AsNoTracking().ToListAsync());
    }

    [Fact(DisplayName = "I064 — A limpeza apaga só as revogações de tokens já expirados")]
    public async Task LimpezaApagaSoRevogacoesExpiradas()
    {
        using var db = CriarBaseComRececionista();
        db.TokensRevogados.AddRange(
            Revogacao("expirou-ontem", Relogio.UtcDateTime.AddDays(-1)),
            Revogacao("expirou-ha-um-segundo", Relogio.UtcDateTime.AddSeconds(-1)),
            Revogacao("ainda-valido", ExpiracaoToken));
        await db.SaveChangesAsync();
        var servico = CriarServico(db, Relogio);

        var removidas = await servico.RemoverRevogacoesExpiradasAsync();

        Assert.Equal(2, removidas);
        var restante = Assert.Single(await db.TokensRevogados.AsNoTracking().ToListAsync());
        Assert.Equal("ainda-valido", restante.Jti);
    }

    [Fact(DisplayName = "I064 — A limpeza mantém a revogação de um token que expira no próprio instante")]
    public async Task LimpezaMantemRevogacaoQueExpiraAgora()
    {
        using var db = CriarBaseComRececionista();
        db.TokensRevogados.Add(Revogacao("expira-agora", Relogio.UtcDateTime));
        await db.SaveChangesAsync();
        var servico = CriarServico(db, Relogio);

        var removidas = await servico.RemoverRevogacoesExpiradasAsync();

        // Com ClockSkew zero, o token ainda é aceite no instante exp: a revogação tem de ficar.
        Assert.Equal(0, removidas);
        Assert.Single(await db.TokensRevogados.AsNoTracking().ToListAsync());
    }

    [Fact(DisplayName = "I064 — Sem revogações expiradas, a limpeza não apaga nada")]
    public async Task LimpezaSemRevogacoesExpiradasNaoApagaNada()
    {
        using var db = CriarBaseComRececionista();
        var servico = CriarServico(db, Relogio);

        var removidas = await servico.RemoverRevogacoesExpiradasAsync();

        Assert.Equal(0, removidas);
    }

    private static ClinicaDbContext CriarBaseComRececionista() =>
        CriarBaseComRececionista(Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

    private static ClinicaDbContext CriarBaseComRececionista(string nomeBase)
    {
        var db = AjudaBaseDados.CriarContexto(nomeBase);
        db.Utilizadores.Add(new Utilizador
        {
            Id = RecA,
            Email = "rececao@clinica-g08.test",
            PasswordHash = "hash-nao-usado-nestes-testes",
            Perfil = Perfis.Rececionista,
            Estado = EstadosUtilizador.Ativo
        });
        db.SaveChanges();

        return db;
    }

    private static TokenRevogado Revogacao(string jti, DateTime dataExpiracao) => new()
    {
        Jti = jti,
        UtilizadorId = RecA,
        DataExpiracao = dataExpiracao,
        DataRevogacao = Relogio.UtcDateTime.AddHours(-1)
    };

    private static AuthService CriarServico(ClinicaDbContext db, DateTimeOffset agora) =>
        new(
            db,
            new PasswordHasher<Utilizador>(),
            AjudaSeguranca.CriarTokenService(AjudaSeguranca.NovaChave(), new RelogioFixo(agora)),
            NullLogger<AuthService>.Instance,
            new RelogioFixo(agora));
}
