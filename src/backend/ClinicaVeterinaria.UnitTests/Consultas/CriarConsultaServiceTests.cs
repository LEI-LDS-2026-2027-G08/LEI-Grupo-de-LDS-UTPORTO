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
/// I073 — <see cref="ConsultasService.CriarConsultaAsync"/> (EP-025) com o <c>ClinicaDbContext</c>
/// em memória (I028, secção 4.2). Casos UT-F003-019 a UT-F003-032 da Especificação de Testes
/// Unitários (I028, secção 7.3.1); os restantes são próprios desta tarefa.
/// </summary>
/// <remarks>
/// <c>RELOGIO</c> é 2026-10-14T08:00:00Z (quarta-feira, 09:00 em Lisboa, hora de verão, UTC+1) e
/// <c>HORARIO_PADRAO</c> é o horário dos dados da I061. A 2026-10-15 (quinta-feira), as 10:00 de
/// Lisboa são as 09:00 UTC. A base em memória não tem transações nem bloqueios: a criação
/// simultânea (critério 2) é verificada nos testes de integração, com o SQL Server.
/// </remarks>
public class CriarConsultaServiceTests
{
    private static readonly DateTimeOffset Relogio = new(2026, 10, 14, 8, 0, 0, TimeSpan.Zero);

    // ----- Caso válido -----

    [Fact(DisplayName = "UT-F003-019 — cria a marcação Pendente, com a duração configurada e a Rececionista como autora")]
    public async Task CriaMarcacaoPendente()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var criada = await CriarServico(db).CriarConsultaAsync(
            Pedido(d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z", "Consulta de rotina"), d.RecA);

        var consulta = criada.Consulta;
        Assert.Equal(EstadosConsulta.Pendente, consulta.Estado);
        Assert.Equal(30, consulta.DuracaoMinutos);
        Assert.Equal(Utc("2026-10-15T09:00:00Z"), consulta.DataHora);
        Assert.Equal(DateTimeKind.Utc, consulta.DataHora.Kind);
        Assert.Equal("Consulta de rotina", consulta.Motivo);
        Assert.Equal(d.AnimalA1, consulta.Animal.Id);
        Assert.Equal(d.VetA, consulta.Veterinario.Id);
        Assert.Null(consulta.ChegadaEm);
        Assert.Empty(consulta.NotasPreparacao);

        var gravada = await db.Marcacoes.AsNoTracking().SingleAsync();
        Assert.Equal(consulta.Id, gravada.Id);
        Assert.Equal(d.RecA, gravada.CriadaPorId);
        Assert.Equal(EstadosConsulta.Pendente, gravada.Estado);
        Assert.Equal((short)30, gravada.DuracaoMinutos);
        Assert.Equal(Relogio.UtcDateTime, gravada.DataCriacao);
    }

    // ----- 400 — data e campos -----

    [Fact(DisplayName = "UT-F003-020 — data no passado (RELOGIO − 1 min) devolve 400")]
    public async Task DataNoPassadoDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<ArgumentException>(() => CriarServico(db).CriarConsultaAsync(
            Pedido(d.AnimalA1, d.VetA, "2026-10-14T07:59:00Z"), d.RecA));

        Assert.Equal(ConsultasService.MensagemDataNaoFutura, erro.Message);
        Assert.Equal(0, await db.Marcacoes.CountAsync());
    }

    [Fact(DisplayName = "UT-F003-021 — data igual a RELOGIO devolve 400: tem de ser estritamente futura")]
    public async Task DataIgualAoRelogioDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<ArgumentException>(() => CriarServico(db).CriarConsultaAsync(
            Pedido(d.AnimalA1, d.VetA, "2026-10-14T08:00:00Z"), d.RecA));

        Assert.Equal(ConsultasService.MensagemDataNaoFutura, erro.Message);
    }

    [Theory(DisplayName = "UT-F003-022 — animalId = 0 ou veterinarioId = 0 devolve 400")]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public async Task IdentificadorZeroDevolve400(int animalId, int veterinarioId)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var pedido = new CreateConsultaDto
        {
            AnimalId = animalId,
            VeterinarioId = veterinarioId,
            DataHora = Utc("2026-10-15T09:00:00Z")
        };

        await Assert.ThrowsAsync<ArgumentException>(() => CriarServico(db).CriarConsultaAsync(pedido, 1));
    }

    [Fact(DisplayName = "I073 — animalId, veterinarioId e dataHora em falta devolvem 400 com as três mensagens")]
    public async Task CamposEmFaltaDevolvem400()
    {
        using var db = AjudaBaseDados.CriarContexto();

        var erro = await Assert.ThrowsAsync<ArgumentException>(
            () => CriarServico(db).CriarConsultaAsync(new CreateConsultaDto(), 1));

        Assert.Contains("animalId", erro.Message, StringComparison.Ordinal);
        Assert.Contains("veterinarioId", erro.Message, StringComparison.Ordinal);
        Assert.Contains("dataHora", erro.Message, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "I073 — motivo com mais de 250 caracteres devolve 400")]
    public async Task MotivoDemasiadoLongoDevolve400()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        await Assert.ThrowsAsync<ArgumentException>(() => CriarServico(db).CriarConsultaAsync(
            Pedido(d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z", new string('x', 251)), d.RecA));
    }

    // ----- 422 — horário de funcionamento -----

    [Fact(DisplayName = "UT-F003-023 — domingo (clínica fechada) devolve 422")]
    public async Task DomingoDevolve422()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<UnprocessableEntityException>(() => CriarServico(db).CriarConsultaAsync(
            Pedido(d.AnimalA1, d.VetA, "2026-10-18T10:00:00Z"), d.RecA));

        Assert.Equal(ConsultasService.MensagemForaDoHorario, erro.Message);
        Assert.Equal(0, await db.Marcacoes.CountAsync());
    }

    [Fact(DisplayName = "UT-F003-024 — início às 12:45 locais (acaba às 13:15) devolve 422")]
    public async Task ConsultaQueUltrapassaOFechoDevolve422()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<UnprocessableEntityException>(() => CriarServico(db).CriarConsultaAsync(
            Pedido(d.AnimalA1, d.VetA, "2026-10-15T11:45:00Z"), d.RecA));

        Assert.Equal(ConsultasService.MensagemForaDoHorario, erro.Message);
    }

    [Fact(DisplayName = "UT-F003-025 — início às 13:00 locais (hora de fecho) devolve 422")]
    public async Task InicioNoFechoDevolve422()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<UnprocessableEntityException>(() => CriarServico(db).CriarConsultaAsync(
            Pedido(d.AnimalA1, d.VetA, "2026-10-15T12:00:00Z"), d.RecA));

        Assert.Equal(ConsultasService.MensagemForaDoHorario, erro.Message);
    }

    [Fact(DisplayName = "UT-F003-026 — início às 12:30 locais (acaba às 13:00) é aceite")]
    public async Task ConsultaQueAcabaNoFechoEAceite()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var criada = await CriarServico(db).CriarConsultaAsync(
            Pedido(d.AnimalA1, d.VetA, "2026-10-15T11:30:00Z"), d.RecA);

        Assert.Equal(Utc("2026-10-15T11:30:00Z"), criada.Consulta.DataHora);
    }

    [Fact(DisplayName = "I073 — hora de inverno: 09:00Z de 2026-10-26 são 09:00 em Lisboa (aceite) e 08:30Z são 08:30 (422)")]
    public async Task HoraDeInverno()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var servico = CriarServico(db);

        var criada = await servico.CriarConsultaAsync(Pedido(d.AnimalA1, d.VetA, "2026-10-26T09:00:00Z"), d.RecA);
        Assert.Equal(Utc("2026-10-26T09:00:00Z"), criada.Consulta.DataHora);

        await Assert.ThrowsAsync<UnprocessableEntityException>(
            () => servico.CriarConsultaAsync(Pedido(d.AnimalA2, d.VetB, "2026-10-26T08:30:00Z"), d.RecA));
    }

    [Fact(DisplayName = "I073 — a duração vem da configuração: com 45 minutos, 12:30 locais já não cabe (422)")]
    public async Task DuracaoConfigurada()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        db.ConfiguracaoSistema.Add(new ConfiguracaoSistema
        {
            Chave = "DuracaoPadraoConsultaMinutos",
            Valor = "45",
            DataUltimaAlteracao = Relogio.UtcDateTime
        });
        await db.SaveChangesAsync();
        var servico = CriarServico(db);

        await Assert.ThrowsAsync<UnprocessableEntityException>(
            () => servico.CriarConsultaAsync(Pedido(d.AnimalA1, d.VetA, "2026-10-15T11:30:00Z"), d.RecA));

        var criada = await servico.CriarConsultaAsync(Pedido(d.AnimalA1, d.VetA, "2026-10-15T11:15:00Z"), d.RecA);
        Assert.Equal(45, criada.Consulta.DuracaoMinutos);
    }

    // ----- 422 — Veterinário indisponível -----

    [Fact(DisplayName = "UT-F003-027 — marcação Confirmada do mesmo Veterinário às 10:00 locais devolve 422")]
    public async Task MesmaHoraOcupadaDevolve422()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        await AdicionarMarcacaoAsync(db, d.AnimalA2, d.VetA, "2026-10-15T09:00:00Z", 30, EstadosConsulta.Confirmada);

        var erro = await Assert.ThrowsAsync<UnprocessableEntityException>(() => CriarServico(db).CriarConsultaAsync(
            Pedido(d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z"), d.RecA));

        Assert.Equal(ConsultasService.MensagemVeterinarioIndisponivel, erro.Message);
        Assert.Equal(1, await db.Marcacoes.CountAsync());
    }

    [Fact(DisplayName = "UT-F003-028 — marcação das 10:15 às 10:45 locais: nova às 10:30 devolve 422")]
    public async Task SobreposicaoParcialDevolve422()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        await AdicionarMarcacaoAsync(db, d.AnimalA2, d.VetA, "2026-10-15T09:15:00Z", 30, EstadosConsulta.Pendente);

        var erro = await Assert.ThrowsAsync<UnprocessableEntityException>(() => CriarServico(db).CriarConsultaAsync(
            Pedido(d.AnimalA1, d.VetA, "2026-10-15T09:30:00Z"), d.RecA));

        Assert.Equal(ConsultasService.MensagemVeterinarioIndisponivel, erro.Message);
    }

    [Fact(DisplayName = "UT-F003-029 — marcação das 10:00 às 10:30 locais: nova às 10:30 é aceite (só se tocam)")]
    public async Task MarcacoesQueSeTocamSaoAceites()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        await AdicionarMarcacaoAsync(db, d.AnimalA2, d.VetA, "2026-10-15T09:00:00Z", 30, EstadosConsulta.Confirmada);

        var criada = await CriarServico(db).CriarConsultaAsync(
            Pedido(d.AnimalA1, d.VetA, "2026-10-15T09:30:00Z"), d.RecA);

        Assert.Equal(EstadosConsulta.Pendente, criada.Consulta.Estado);
        Assert.Equal(2, await db.Marcacoes.CountAsync());
    }

    [Fact(DisplayName = "UT-F003-030 — marcação Cancelada às 10:00 locais não ocupa o horário")]
    public async Task MarcacaoCanceladaNaoOcupa()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        await AdicionarMarcacaoAsync(db, d.AnimalA2, d.VetA, "2026-10-15T09:00:00Z", 30, EstadosConsulta.Cancelada);

        var criada = await CriarServico(db).CriarConsultaAsync(
            Pedido(d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z"), d.RecA);

        Assert.Equal(EstadosConsulta.Pendente, criada.Consulta.Estado);
    }

    [Fact(DisplayName = "I073 — marcação de outro Veterinário à mesma hora não ocupa o horário")]
    public async Task OutroVeterinarioNaoOcupa()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        await AdicionarMarcacaoAsync(db, d.AnimalA2, d.VetB, "2026-10-15T09:00:00Z", 30, EstadosConsulta.Confirmada);

        var criada = await CriarServico(db).CriarConsultaAsync(
            Pedido(d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z"), d.RecA);

        Assert.Equal(d.VetA, criada.Consulta.Veterinario.Id);
    }

    [Fact(DisplayName = "I073 — marcação mais longa que começou antes ocupa o horário (60 min às 09:45 locais)")]
    public async Task MarcacaoLongaAnteriorOcupa()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        await AdicionarMarcacaoAsync(db, d.AnimalA2, d.VetA, "2026-10-15T08:45:00Z", 60, EstadosConsulta.Confirmada);

        await Assert.ThrowsAsync<UnprocessableEntityException>(() => CriarServico(db).CriarConsultaAsync(
            Pedido(d.AnimalA1, d.VetA, "2026-10-15T09:30:00Z"), d.RecA));
    }

    // ----- 404 -----

    [Fact(DisplayName = "UT-F003-031 — animal inexistente devolve 404")]
    public async Task AnimalInexistenteDevolve404()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var erro = await Assert.ThrowsAsync<KeyNotFoundException>(() => CriarServico(db).CriarConsultaAsync(
            Pedido(999, d.VetA, "2026-10-15T09:00:00Z"), d.RecA));

        Assert.Equal(ConsultasService.MensagemAnimalNaoEncontrado, erro.Message);
    }

    [Fact(DisplayName = "UT-F003-032 — veterinarioId inexistente ou de uma conta que não é de Veterinário devolve 404")]
    public async Task VeterinarioInexistenteDevolve404()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var servico = CriarServico(db);

        foreach (var veterinarioId in new[] { 999, d.RecA })
        {
            var erro = await Assert.ThrowsAsync<KeyNotFoundException>(() => servico.CriarConsultaAsync(
                Pedido(d.AnimalA1, veterinarioId, "2026-10-15T09:00:00Z"), d.RecA));

            Assert.Equal(ConsultasService.MensagemVeterinarioNaoEncontrado, erro.Message);
        }
    }

    [Fact(DisplayName = "I073 — Veterinário com a conta bloqueada devolve 404, como em EP-021 e EP-022")]
    public async Task VeterinarioBloqueadoDevolve404()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var vet = await db.Utilizadores.SingleAsync(u => u.Id == d.VetB);
        vet.Estado = "Bloqueado";
        await db.SaveChangesAsync();

        await Assert.ThrowsAsync<KeyNotFoundException>(() => CriarServico(db).CriarConsultaAsync(
            Pedido(d.AnimalA1, d.VetB, "2026-10-15T09:00:00Z"), d.RecA));
    }

    // ----- Dados do pedido -----

    [Fact(DisplayName = "I073 — dataHora sem indicação de fuso é lida como UTC e os segundos fracionários são retirados")]
    public async Task DataSemFusoELidaComoUtc()
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);
        var pedido = new CreateConsultaDto
        {
            AnimalId = d.AnimalA1,
            VeterinarioId = d.VetA,
            DataHora = new DateTime(2026, 10, 15, 9, 0, 0, 500, DateTimeKind.Unspecified)
        };

        var criada = await CriarServico(db).CriarConsultaAsync(pedido, d.RecA);

        Assert.Equal(Utc("2026-10-15T09:00:00Z"), criada.Consulta.DataHora);
    }

    [Theory(DisplayName = "I073 — motivo com espaços é aparado e motivo vazio fica nulo")]
    [InlineData("  Vacinação  ", "Vacinação")]
    [InlineData("   ", null)]
    [InlineData(null, null)]
    public async Task MotivoNormalizado(string? motivo, string? esperado)
    {
        using var db = AjudaBaseDados.CriarContexto();
        var d = await CriarBaseAsync(db);

        var criada = await CriarServico(db).CriarConsultaAsync(
            Pedido(d.AnimalA1, d.VetA, "2026-10-15T09:00:00Z", motivo), d.RecA);

        Assert.Equal(esperado, criada.Consulta.Motivo);
    }

    // ----- Auxiliares -----

    private static ConsultasService CriarServico(ClinicaDbContext db) => new(db, new RelogioFixo(Relogio));

    private static CreateConsultaDto Pedido(int animalId, int veterinarioId, string dataHoraUtc, string? motivo = null) =>
        new()
        {
            AnimalId = animalId,
            VeterinarioId = veterinarioId,
            DataHora = Utc(dataHoraUtc),
            Motivo = motivo
        };

    private static DateTime Utc(string texto) => DateTime.Parse(
        texto, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);

    /// <summary>
    /// Dados de referência da I028 (secção 5): <c>HORARIO_PADRAO</c>, VET_A, VET_B, REC_A e
    /// CLIENTE_A com ANIMAL_A1 e ANIMAL_A2. Sem configuração de duração: 30 minutos.
    /// </summary>
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

        var animalA1 = new Paciente { ClienteId = cliente.Id, Nome = "Bobi", EspecieId = especie.Id, DataRegisto = Relogio.UtcDateTime };
        var animalA2 = new Paciente { ClienteId = cliente.Id, Nome = "Mia", EspecieId = especie.Id, DataRegisto = Relogio.UtcDateTime };
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

        db.Funcionarios.Add(new Funcionario { UtilizadorId = utilizador.Id, Nome = nome });
        await db.SaveChangesAsync();

        return utilizador.Id;
    }

    private static async Task AdicionarMarcacaoAsync(
        ClinicaDbContext db,
        int pacienteId,
        int veterinarioId,
        string inicioUtc,
        short duracaoMinutos,
        string estado)
    {
        // Gravada como Unspecified, como o SQL Server a devolve, para verificar a leitura em UTC.
        db.Marcacoes.Add(new Marcacao
        {
            PacienteId = pacienteId,
            VeterinarioId = veterinarioId,
            DataHoraInicio = DateTime.SpecifyKind(Utc(inicioUtc), DateTimeKind.Unspecified),
            DuracaoMinutos = duracaoMinutos,
            Estado = estado,
            CriadaPorId = 1,
            DataCriacao = Relogio.UtcDateTime
        });
        await db.SaveChangesAsync();
    }

    private sealed record DadosBase(int VetA, int VetB, int RecA, int AnimalA1, int AnimalA2);
}
