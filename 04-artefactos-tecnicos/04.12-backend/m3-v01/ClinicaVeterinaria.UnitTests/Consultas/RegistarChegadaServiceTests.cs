using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.UnitTests.Seguranca;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.UnitTests.Consultas;

/// <summary>
/// I075 — <see cref="ConsultasService.RegistarChegadaAsync"/> (EP-028) com o
/// <c>ClinicaDbContext</c> em memória. Casos UT-F003-045 a UT-F003-049.
/// </summary>
public class RegistarChegadaServiceTests
{
    private static readonly DateTimeOffset Relogio = new(2026, 10, 14, 9, 22, 0, TimeSpan.Zero);

    private static readonly byte[] VersaoV2 = [0, 0, 0, 0, 0, 0, 0x07, 0xD2];

    // Versão V1 (0x07D1), anterior à gravada.
    private const string IfMatchV1 = "\"AAAAAAAAB9E=\"";

    [Fact(DisplayName = "UT-F003-045 — chegada numa marcação Confirmada grava a hora do servidor")]
    public async Task ChegadaEmConfirmadaGravaHoraDoServidor()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var id = await AdicionarMarcacaoAsync(db, EstadosConsulta.Confirmada);

        var resultado = await CriarServico(db).RegistarChegadaAsync(id, null);

        Assert.Equal(id, resultado.Chegada.Id);
        Assert.Equal(Relogio.UtcDateTime, resultado.Chegada.ChegadaEm);
        Assert.False(string.IsNullOrWhiteSpace(resultado.ETag));

        var gravada = await db.Marcacoes.AsNoTracking().SingleAsync(m => m.Id == id);
        Assert.Equal(Relogio.UtcDateTime, gravada.DataHoraChegada);
    }

    [Theory(DisplayName = "UT-F003-046 e 047 — chegada numa marcação não Confirmada devolve 422")]
    [InlineData(EstadosConsulta.Pendente)]
    [InlineData(EstadosConsulta.Cancelada)]
    public async Task ChegadaEmMarcacaoNaoConfirmadaDevolve422(string estado)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var id = await AdicionarMarcacaoAsync(db, estado);

        await Assert.ThrowsAsync<UnprocessableEntityException>(
            () => CriarServico(db).RegistarChegadaAsync(id, null));

        var gravada = await db.Marcacoes.AsNoTracking().SingleAsync(m => m.Id == id);
        Assert.Null(gravada.DataHoraChegada);
    }

    [Fact(DisplayName = "UT-F003-048 — segunda chegada devolve 422 e mantém a primeira hora")]
    public async Task SegundaChegadaDevolve422()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var id = await AdicionarMarcacaoAsync(db, EstadosConsulta.Confirmada);

        await CriarServico(db).RegistarChegadaAsync(id, null);

        var depois = new RelogioFixo(Relogio.AddMinutes(10));

        await Assert.ThrowsAsync<UnprocessableEntityException>(
            () => new ConsultasService(db, depois).RegistarChegadaAsync(id, null));

        var gravada = await db.Marcacoes.AsNoTracking().SingleAsync(m => m.Id == id);
        Assert.Equal(Relogio.UtcDateTime, gravada.DataHoraChegada);
    }

    [Fact(DisplayName = "UT-F003-049 — If-Match desatualizado devolve 412 e não regista a chegada")]
    public async Task IfMatchDesatualizadoDevolve412()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var id = await AdicionarMarcacaoAsync(db, EstadosConsulta.Confirmada, VersaoV2);

        await Assert.ThrowsAsync<PreconditionFailedException>(
            () => CriarServico(db).RegistarChegadaAsync(id, IfMatchV1));

        var gravada = await db.Marcacoes.AsNoTracking().SingleAsync(m => m.Id == id);
        Assert.Null(gravada.DataHoraChegada);
    }

    [Fact(DisplayName = "I075 — EP-028 marcação inexistente devolve 404")]
    public async Task MarcacaoInexistenteDevolve404()
    {
        using var db = AjudaBaseDados.CriarContexto();

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => CriarServico(db).RegistarChegadaAsync(999, null));
    }

    // ----- Auxiliares -----

    private static ConsultasService CriarServico(ClinicaDbContext db) => new(db, new RelogioFixo(Relogio));

    // A base em memória não verifica chaves estrangeiras: a chegada só lê a própria marcação.
    private static async Task<int> AdicionarMarcacaoAsync(
        ClinicaDbContext db,
        string estado,
        byte[]? rowVersion = null)
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
            RowVersion = rowVersion ?? [0, 0, 0, 0, 0, 0, 0, 1]
        };

        db.Marcacoes.Add(marcacao);
        await db.SaveChangesAsync();

        return marcacao.Id;
    }
}
