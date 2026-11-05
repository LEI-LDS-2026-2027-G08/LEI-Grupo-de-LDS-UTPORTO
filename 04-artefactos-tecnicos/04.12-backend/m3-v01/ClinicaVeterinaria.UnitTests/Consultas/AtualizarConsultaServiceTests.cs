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
/// I074 — <see cref="ConsultasService.AtualizarConsultaAsync"/> (EP-026) com o
/// <c>ClinicaDbContext</c> em memória. Casos UT-F003-033 a UT-F003-043 da
/// Especificação de Testes Unitários.
/// </summary>
public class AtualizarConsultaServiceTests
{
    private static readonly DateTimeOffset Relogio = new(2026, 10, 14, 8, 0, 0, TimeSpan.Zero);

    [Fact(DisplayName = "UT-F003-033 — altera Veterinário e horário de uma marcação Pendente")]
    public async Task AlteraMarcacaoPendente()
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

        var resultado = await CriarServico(db).AtualizarConsultaAsync(
            id,
            new UpdateConsultaDto
            {
                VeterinarioId = d.VetB,
                DataHora = Utc("2026-10-15T10:00:00Z"),
                Motivo = "Nova hora"
            },
            null);

        Assert.Equal(d.VetB, resultado.Consulta.Veterinario.Id);
        Assert.Equal(
            Utc("2026-10-15T10:00:00Z"),
            resultado.Consulta.DataHora);
        Assert.Equal("Nova hora", resultado.Consulta.Motivo);
        Assert.Equal(
            EstadosConsulta.Pendente,
            resultado.Consulta.Estado);
    }

    [Fact(DisplayName = "UT-F003-034 — uma marcação Confirmada também pode ser reagendada")]
    public async Task AlteraMarcacaoConfirmada()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var id = await AdicionarMarcacaoAsync(
            db,
            d.AnimalA1,
            d.VetA,
            "2026-10-15T09:00:00Z",
            30,
            EstadosConsulta.Confirmada);

        var resultado = await CriarServico(db).AtualizarConsultaAsync(
            id,
            new UpdateConsultaDto
            {
                VeterinarioId = d.VetA,
                DataHora = Utc("2026-10-15T10:00:00Z")
            },
            null);

        Assert.Equal(
            EstadosConsulta.Confirmada,
            resultado.Consulta.Estado);
    }

    [Theory(DisplayName = "UT-F003-035/036 — Realizada ou Cancelada não podem ser alteradas")]
    [InlineData(EstadosConsulta.Realizada)]
    [InlineData(EstadosConsulta.Cancelada)]
    public async Task EstadoFinalNaoPodeSerAlterado(string estado)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var id = await AdicionarMarcacaoAsync(
            db,
            d.AnimalA1,
            d.VetA,
            "2026-10-15T09:00:00Z",
            30,
            estado);

        await Assert.ThrowsAsync<UnprocessableEntityException>(
            () => CriarServico(db).AtualizarConsultaAsync(
                id,
                new UpdateConsultaDto
                {
                    VeterinarioId = d.VetA,
                    DataHora = Utc("2026-10-15T10:00:00Z")
                },
                null));
    }

    [Fact(DisplayName = "UT-F003-037 — reagendar para horário ocupado devolve 422")]
    public async Task ReagendarParaHorarioOcupadoDevolve422()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var id = await AdicionarMarcacaoAsync(
            db,
            d.AnimalA1,
            d.VetA,
            "2026-10-15T08:00:00Z",
            30,
            EstadosConsulta.Pendente);

        await AdicionarMarcacaoAsync(
            db,
            d.AnimalA2,
            d.VetA,
            "2026-10-15T10:00:00Z",
            30,
            EstadosConsulta.Confirmada);

        var erro = await Assert.ThrowsAsync<UnprocessableEntityException>(
            () => CriarServico(db).AtualizarConsultaAsync(
                id,
                new UpdateConsultaDto
                {
                    VeterinarioId = d.VetA,
                    DataHora = Utc("2026-10-15T10:00:00Z")
                },
                null));

        Assert.Equal(
            ConsultasService.MensagemVeterinarioIndisponivel,
            erro.Message);
    }

    [Fact(DisplayName = "UT-F003-038 — a própria marcação não conta como ocupação")]
    public async Task PropriaMarcacaoNaoContaComoOcupacao()
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

        var resultado = await CriarServico(db).AtualizarConsultaAsync(
            id,
            new UpdateConsultaDto
            {
                VeterinarioId = d.VetA,
                DataHora = Utc("2026-10-15T09:15:00Z")
            },
            null);

        Assert.Equal(
            Utc("2026-10-15T09:15:00Z"),
            resultado.Consulta.DataHora);
    }

    [Fact(DisplayName = "UT-F003-039 — reagendar para data passada devolve 400")]
    public async Task ReagendarParaPassadoDevolve400()
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
            () => CriarServico(db).AtualizarConsultaAsync(
                id,
                new UpdateConsultaDto
                {
                    VeterinarioId = d.VetA,
                    DataHora = Relogio.UtcDateTime.AddHours(-1)
                },
                null));
    }

    [Fact(DisplayName = "UT-F003-040 — marcação inexistente devolve 404")]
    public async Task AtualizarMarcacaoInexistenteDevolve404()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => CriarServico(db).AtualizarConsultaAsync(
                999,
                new UpdateConsultaDto
                {
                    VeterinarioId = d.VetA,
                    DataHora = Utc("2026-10-15T09:00:00Z")
                },
                null));
    }

    [Fact(DisplayName = "UT-F003-041 — If-Match desatualizado devolve 412 e não altera a marcação")]
    public async Task AtualizarComIfMatchDesatualizadoDevolve412()
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
            () => CriarServico(db).AtualizarConsultaAsync(
                id,
                new UpdateConsultaDto
                {
                    VeterinarioId = d.VetB,
                    DataHora = Utc("2026-10-15T10:00:00Z")
                },
                "\"AAAAAAAAB9E=\""));

        var gravada = await db.Marcacoes.AsNoTracking()
            .SingleAsync(m => m.Id == id);

        Assert.Equal(d.VetA, gravada.VeterinarioId);
        Assert.Equal(
            Utc("2026-10-15T09:00:00Z"),
            DateTime.SpecifyKind(
                gravada.DataHoraInicio,
                DateTimeKind.Utc));
    }

    [Fact(DisplayName = "UT-F003-042 — If-Match com a versão atual permite alterar a marcação")]
    public async Task AtualizarComIfMatchAtualEAceite()
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

        var resultado = await CriarServico(db).AtualizarConsultaAsync(
            id,
            new UpdateConsultaDto
            {
                VeterinarioId = d.VetB,
                DataHora = Utc("2026-10-15T10:00:00Z")
            },
            "\"AAAAAAAAB9I=\"");

        Assert.Equal(
            d.VetB,
            resultado.Consulta.Veterinario.Id);

        Assert.Equal(
            Utc("2026-10-15T10:00:00Z"),
            resultado.Consulta.DataHora);
    }

    [Fact(DisplayName = "UT-F003-043 — sem If-Match a alteração é aceite")]
    public async Task AtualizarSemIfMatchEAceite()
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

        var resultado = await CriarServico(db).AtualizarConsultaAsync(
            id,
            new UpdateConsultaDto
            {
                VeterinarioId = d.VetA,
                DataHora = Utc("2026-10-15T10:00:00Z")
            },
            null);

        Assert.Equal(
            Utc("2026-10-15T10:00:00Z"),
            resultado.Consulta.DataHora);
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
