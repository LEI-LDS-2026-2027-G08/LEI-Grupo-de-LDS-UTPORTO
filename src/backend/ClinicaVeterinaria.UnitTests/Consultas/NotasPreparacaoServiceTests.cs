using System.Globalization;
using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Application.DTOs.Consultas;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.UnitTests.Seguranca;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.UnitTests.Consultas;

/// <summary>
/// I075 — <see cref="ConsultasService.CriarNotaPreparacaoAsync"/> (EP-029) com o
/// <c>ClinicaDbContext</c> em memória. Casos UT-F003-076 a UT-F003-084.
/// </summary>
public class NotasPreparacaoServiceTests
{
    private static readonly DateTimeOffset Relogio = new(2026, 10, 14, 9, 25, 0, TimeSpan.Zero);

    [Fact(DisplayName = "UT-F003-076 — Auxiliar com apoio cria a nota com o autor e a hora do servidor")]
    public async Task AuxiliarComApoioCriaNota()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var aux = await AdicionarAuxiliarAsync(db, "Carla Auxiliar", podeRegistarApoio: true);
        var id = await AdicionarMarcacaoAsync(db, EstadosConsulta.Confirmada);

        var nota = await CriarServico(db).CriarNotaPreparacaoAsync(
            id, new CreateNotaPreparacaoDto { Texto = "Sala 2 preparada." }, aux);

        Assert.Equal(id, nota.ConsultaId);
        Assert.Equal("Sala 2 preparada.", nota.Texto);
        Assert.Equal("Carla Auxiliar", nota.Autor);
        Assert.Equal(Relogio.UtcDateTime, nota.CriadaEm);

        var gravada = await db.InformacoesApoio.AsNoTracking().SingleAsync();
        Assert.Equal(aux, gravada.AuxiliarId);
        Assert.Equal(id, gravada.MarcacaoId);
    }

    [Fact(DisplayName = "UT-F003-077 — Auxiliar sem autorização de apoio recebe 403 e nada é gravado")]
    public async Task AuxiliarSemApoioDevolve403()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var aux = await AdicionarAuxiliarAsync(db, "Diogo Auxiliar", podeRegistarApoio: false);
        var id = await AdicionarMarcacaoAsync(db, EstadosConsulta.Confirmada);

        await Assert.ThrowsAsync<ForbiddenException>(
            () => CriarServico(db).CriarNotaPreparacaoAsync(
                id, new CreateNotaPreparacaoDto { Texto = "Sala 2 preparada." }, aux));

        Assert.Empty(db.InformacoesApoio);
    }

    [Theory(DisplayName = "UT-F003-078, 079 e 081 — texto vazio, só espaços ou com 1001 caracteres devolve 400")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task TextoVazioDevolve400(string? texto)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var aux = await AdicionarAuxiliarAsync(db, "Carla Auxiliar", podeRegistarApoio: true);
        var id = await AdicionarMarcacaoAsync(db, EstadosConsulta.Confirmada);

        await Assert.ThrowsAsync<ArgumentException>(
            () => CriarServico(db).CriarNotaPreparacaoAsync(
                id, new CreateNotaPreparacaoDto { Texto = texto }, aux));

        Assert.Empty(db.InformacoesApoio);
    }

    [Theory(DisplayName = "UT-F003-080 e 081 — 1000 caracteres é aceite e 1001 devolve 400")]
    [InlineData(1000, true)]
    [InlineData(1001, false)]
    public async Task LimiteDeMilCaracteres(int comprimento, bool aceite)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var aux = await AdicionarAuxiliarAsync(db, "Carla Auxiliar", podeRegistarApoio: true);
        var id = await AdicionarMarcacaoAsync(db, EstadosConsulta.Confirmada);
        var pedido = new CreateNotaPreparacaoDto { Texto = new string('a', comprimento) };

        if (aceite)
        {
            var nota = await CriarServico(db).CriarNotaPreparacaoAsync(id, pedido, aux);
            Assert.Equal(comprimento, nota.Texto.Length);
        }
        else
        {
            await Assert.ThrowsAsync<ArgumentException>(
                () => CriarServico(db).CriarNotaPreparacaoAsync(id, pedido, aux));
            Assert.Empty(db.InformacoesApoio);
        }
    }

    [Fact(DisplayName = "UT-F003-082 — marcação Cancelada devolve 422")]
    public async Task MarcacaoCanceladaDevolve422()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var aux = await AdicionarAuxiliarAsync(db, "Carla Auxiliar", podeRegistarApoio: true);
        var id = await AdicionarMarcacaoAsync(db, EstadosConsulta.Cancelada);

        await Assert.ThrowsAsync<UnprocessableEntityException>(
            () => CriarServico(db).CriarNotaPreparacaoAsync(
                id, new CreateNotaPreparacaoDto { Texto = "Sala 2 preparada." }, aux));

        Assert.Empty(db.InformacoesApoio);
    }

    [Fact(DisplayName = "UT-F003-083 — marcação Realizada é aceite: só Cancelada impede o registo")]
    public async Task MarcacaoRealizadaAceite()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var aux = await AdicionarAuxiliarAsync(db, "Carla Auxiliar", podeRegistarApoio: true);
        var id = await AdicionarMarcacaoAsync(db, EstadosConsulta.Realizada);

        await CriarServico(db).CriarNotaPreparacaoAsync(
            id, new CreateNotaPreparacaoDto { Texto = "Tutor pediu corte de unhas." }, aux);

        Assert.Single(db.InformacoesApoio);
    }

    [Fact(DisplayName = "UT-F003-084 — marcação inexistente devolve 404")]
    public async Task MarcacaoInexistenteDevolve404()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var aux = await AdicionarAuxiliarAsync(db, "Carla Auxiliar", podeRegistarApoio: true);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => CriarServico(db).CriarNotaPreparacaoAsync(
                999, new CreateNotaPreparacaoDto { Texto = "Sala 2 preparada." }, aux));
    }

    [Fact(DisplayName = "I075 — EP-029 não altera a marcação")]
    public async Task NotaNaoAlteraMarcacao()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var aux = await AdicionarAuxiliarAsync(db, "Carla Auxiliar", podeRegistarApoio: true);
        var id = await AdicionarMarcacaoAsync(db, EstadosConsulta.Confirmada);
        var antes = await db.Marcacoes.AsNoTracking().SingleAsync(m => m.Id == id);

        await CriarServico(db).CriarNotaPreparacaoAsync(
            id, new CreateNotaPreparacaoDto { Texto = "Sala 2 preparada." }, aux);

        var depois = await db.Marcacoes.AsNoTracking().SingleAsync(m => m.Id == id);
        Assert.Equal(antes.Estado, depois.Estado);
        Assert.Equal(antes.DataHoraChegada, depois.DataHoraChegada);
        Assert.Equal(antes.DataUltimaAlteracao, depois.DataUltimaAlteracao);
        Assert.Equal(antes.RowVersion, depois.RowVersion);
    }

    // ----- Auxiliares -----

    private static ConsultasService CriarServico(ClinicaDbContext db) => new(db, new RelogioFixo(Relogio));

    private static async Task<int> AdicionarAuxiliarAsync(ClinicaDbContext db, string nome, bool podeRegistarApoio)
    {
        var utilizador = new Utilizador
        {
            Email = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "@clinica-g08.test",
            PasswordHash = "hash-nao-usado-nestes-testes",
            Perfil = Perfis.AuxiliarVeterinario,
            Estado = EstadosUtilizador.Ativo,
            DataCriacao = Relogio.UtcDateTime
        };
        db.Utilizadores.Add(utilizador);
        await db.SaveChangesAsync();

        db.Funcionarios.Add(new Funcionario
        {
            UtilizadorId = utilizador.Id,
            Nome = nome,
            PodeRegistarApoio = podeRegistarApoio
        });
        await db.SaveChangesAsync();

        return utilizador.Id;
    }

    private static async Task<int> AdicionarMarcacaoAsync(ClinicaDbContext db, string estado)
    {
        var marcacao = new Marcacao
        {
            PacienteId = 1,
            VeterinarioId = 1,
            DataHoraInicio = new DateTime(2026, 10, 14, 9, 30, 0, DateTimeKind.Unspecified),
            DuracaoMinutos = 30,
            Estado = estado,
            CriadaPorId = 1,
            DataCriacao = Relogio.UtcDateTime,
            RowVersion = [0, 0, 0, 0, 0, 0, 0, 1]
        };

        db.Marcacoes.Add(marcacao);
        await db.SaveChangesAsync();

        return marcacao.Id;
    }
}
