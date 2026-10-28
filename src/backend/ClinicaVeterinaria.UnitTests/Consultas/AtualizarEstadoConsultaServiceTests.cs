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
/// I074 — <see cref="ConsultasService.AtualizarEstadoAsync"/> (EP-027) com o
/// <c>ClinicaDbContext</c> em memória. Inclui o caso UT-F003-044 e a validação
/// da persistência das transições de estado.
/// </summary>
public class AtualizarEstadoConsultaServiceTests
{
    private static readonly DateTimeOffset Relogio = new(2026, 10, 14, 8, 0, 0, TimeSpan.Zero);

    [Fact(DisplayName = "UT-F003-044 — mudar estado com If-Match desatualizado devolve 412")]
    public async Task AtualizarEstadoComIfMatchDesatualizadoDevolve412()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var versaoAtual = new byte[]
        {
        0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x07, 0xD2
        };

        var id = await AdicionarMarcacaoAsync(
            db,
            d.AnimalA1,
            d.VetA,
            "2026-10-15T09:00:00Z",
            30,
            EstadosConsulta.Pendente,
            versaoAtual);

        await Assert.ThrowsAsync<PreconditionFailedException>(
            () => CriarServico(db).AtualizarEstadoAsync(
                id,
                new UpdateEstadoConsultaDto
                {
                    Estado = EstadosConsulta.Confirmada
                },
                "\"AAAAAAAAB9E=\""));

        var gravada = await db.Marcacoes.AsNoTracking()
            .SingleAsync(m => m.Id == id);

        Assert.Equal(
            EstadosConsulta.Pendente,
            gravada.Estado);
    }

    // ----- I074 / EP-027 — Atualizar estado -----

    [Fact(DisplayName = "I074 — EP-027 altera Pendente para Confirmada")]
    public async Task AtualizarEstadoGravaTransicaoValida()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var id = await AdicionarMarcacaoAsync(
            db,
            d.AnimalA1,
            d.VetA,
            "2026-10-15T09:00:00Z",
            30,
            EstadosConsulta.Pendente);

        await CriarServico(db).AtualizarEstadoAsync(
            id,
            new UpdateEstadoConsultaDto
            {
                Estado = EstadosConsulta.Confirmada
            },
            null);

        var gravada = await db.Marcacoes.AsNoTracking()
            .SingleAsync(m => m.Id == id);

        Assert.Equal(
            EstadosConsulta.Confirmada,
            gravada.Estado);

        Assert.Equal(
            Relogio.UtcDateTime,
            gravada.DataUltimaAlteracao);
    }

    [Fact(DisplayName = "I074 — EP-027 não altera a marcação quando a transição é inválida")]
    public async Task TransicaoInvalidaNaoAlteraMarcacao()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var id = await AdicionarMarcacaoAsync(
            db,
            d.AnimalA1,
            d.VetA,
            "2026-10-15T09:00:00Z",
            30,
            EstadosConsulta.Pendente);

        await Assert.ThrowsAsync<UnprocessableEntityException>(
            () => CriarServico(db).AtualizarEstadoAsync(
                id,
                new UpdateEstadoConsultaDto
                {
                    Estado = EstadosConsulta.Realizada
                },
                null));

        var gravada = await db.Marcacoes.AsNoTracking()
            .SingleAsync(m => m.Id == id);

        Assert.Equal(
            EstadosConsulta.Pendente,
            gravada.Estado);

        Assert.Null(gravada.DataUltimaAlteracao);
    }

    [Theory(DisplayName = "I074 — EP-027 rejeita Pendente e estado inexistente com 400")]
    [InlineData(EstadosConsulta.Pendente)]
    [InlineData("Faltou")]
    public async Task DestinoInvalidoDevolve400(string estado)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var id = await AdicionarMarcacaoAsync(
            db,
            d.AnimalA1,
            d.VetA,
            "2026-10-15T09:00:00Z",
            30,
            EstadosConsulta.Pendente);

        await Assert.ThrowsAsync<ArgumentException>(
            () => CriarServico(db).AtualizarEstadoAsync(
                id,
                new UpdateEstadoConsultaDto
                {
                    Estado = estado
                },
                null));
    }

    [Fact(DisplayName = "I074 — EP-027 marcação inexistente devolve 404")]
    public async Task AtualizarEstadoMarcacaoInexistenteDevolve404()
    {
        using var db = AjudaBaseDados.CriarContexto();

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => CriarServico(db).AtualizarEstadoAsync(
                999,
                new UpdateEstadoConsultaDto
                {
                    Estado = EstadosConsulta.Confirmada
                },
                null));
    }

    // ----- Auxiliares -----

    private static ConsultasService CriarServico(ClinicaDbContext db) => new(db, new RelogioFixo(Relogio));

    private static DateTime Utc(string texto) => DateTime.Parse(
        texto, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);

    private static async Task<DadosBase> CriarBaseAsync(ClinicaDbContext db)
    {
        var vetA = await AdicionarFuncionarioAsync(db, "Ana Vet", Perfis.Veterinario);
        var vetB = await AdicionarFuncionarioAsync(db, "Bruno Vet", Perfis.Veterinario);
        var recA = await AdicionarFuncionarioAsync(db, "Rita Rececao", Perfis.Rececionista);

        // Segunda a sexta 09:00–13:00 e 14:00–19:00; sábado 09:00–13:00; domingo fechado.
        for (byte dia = 1; dia <= 6; dia++)
        {
            db.HorariosFuncionamento.Add(Periodo(dia, "09:00", "13:00"));

            if (dia <= 5)
            {
                db.HorariosFuncionamento.Add(Periodo(dia, "14:00", "19:00"));
            }
        }

        var especie = new Especie { Nome = "Cão" };
        db.Especies.Add(especie);
        await db.SaveChangesAsync();

        var utilizadorCliente = new Utilizador
        {
            Email = "cliente.a@clinica-g08.test",
            PasswordHash = "hash-nao-usado-nestes-testes",
            Perfil = Perfis.Cliente,
            Estado = EstadosUtilizador.Ativo,
            DataCriacao = Relogio.UtcDateTime
        };
        db.Utilizadores.Add(utilizadorCliente);
        await db.SaveChangesAsync();

        var cliente = new Cliente
        {
            UtilizadorId = utilizadorCliente.Id,
            Nome = "Joana Cliente",
            Telefone = "960000001",
            DataRegisto = Relogio.UtcDateTime
        };
        db.Clientes.Add(cliente);
        await db.SaveChangesAsync();

        var animalA1 = new Paciente
        {
            ClienteId = cliente.Id,
            Nome = "Bobi",
            EspecieId = especie.Id,
            DataRegisto = Relogio.UtcDateTime
        };
        var animalA2 = new Paciente
        {
            ClienteId = cliente.Id,
            Nome = "Mia",
            EspecieId = especie.Id,
            DataRegisto = Relogio.UtcDateTime
        };
        db.Pacientes.AddRange(animalA1, animalA2);
        await db.SaveChangesAsync();

        return new DadosBase(vetA, vetB, recA, animalA1.Id, animalA2.Id);
    }

    private static HorarioFuncionamento Periodo(byte dia, string abertura, string fecho) => new()
    {
        DiaSemana = dia,
        HoraAbertura = TimeOnly.Parse(abertura, CultureInfo.InvariantCulture),
        HoraFecho = TimeOnly.Parse(fecho, CultureInfo.InvariantCulture),
        Ativo = true
    };

    private static async Task<int> AdicionarFuncionarioAsync(ClinicaDbContext db, string nome, string perfil)
    {
        var utilizador = new Utilizador
        {
            Email = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "@clinica-g08.test",
            PasswordHash = "hash-nao-usado-nestes-testes",
            Perfil = perfil,
            Estado = EstadosUtilizador.Ativo,
            DataCriacao = Relogio.UtcDateTime
        };
        db.Utilizadores.Add(utilizador);
        await db.SaveChangesAsync();

        db.Funcionarios.Add(new Funcionario
        {
            UtilizadorId = utilizador.Id,
            Nome = nome
        });
        await db.SaveChangesAsync();

        return utilizador.Id;
    }

    private static async Task<int> AdicionarMarcacaoAsync(
        ClinicaDbContext db,
        int pacienteId,
        int veterinarioId,
        string inicioUtc,
        short duracaoMinutos,
        string estado,
        byte[]? rowVersion = null)
    {
        var marcacao = new Marcacao
        {
            PacienteId = pacienteId,
            VeterinarioId = veterinarioId,
            DataHoraInicio = DateTime.SpecifyKind(Utc(inicioUtc), DateTimeKind.Unspecified),
            DuracaoMinutos = duracaoMinutos,
            Estado = estado,
            CriadaPorId = 1,
            DataCriacao = Relogio.UtcDateTime,
            RowVersion = rowVersion ?? new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }
        };

        db.Marcacoes.Add(marcacao);
        await db.SaveChangesAsync();

        return marcacao.Id;
    }

    private sealed record DadosBase(int VetA, int VetB, int RecA, int AnimalA1, int AnimalA2);
}
