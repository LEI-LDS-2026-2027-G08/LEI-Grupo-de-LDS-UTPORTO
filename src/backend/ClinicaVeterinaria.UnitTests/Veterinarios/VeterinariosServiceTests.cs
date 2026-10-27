using System.Globalization;
using ClinicaVeterinaria.Application.DTOs.Veterinarios;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Application.Validacao;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.UnitTests.Seguranca;

namespace ClinicaVeterinaria.UnitTests.Veterinarios;

/// <summary>
/// I071 — <see cref="VeterinariosService"/> (EP-021, EP-022) com o <c>ClinicaDbContext</c> em memória
/// (I028, secção 4.2). Casos UT-F003-010 e UT-F003-013 a UT-F003-018 da Especificação de Testes
/// Unitários (I028, secção 7.3.1); os restantes são próprios desta tarefa. As regras do cálculo estão
/// testadas em <c>CalculadoraDisponibilidadeTests</c>.
/// </summary>
public class VeterinariosServiceTests
{
    // RELOGIO da I028: quarta-feira, 2026-10-14, 09:00 em Lisboa.
    private static readonly DateTimeOffset Relogio = new(2026, 10, 14, 8, 0, 0, TimeSpan.Zero);

    private static readonly DateOnly Quinta = new(2026, 10, 15);

    // ----- EP-021 — Listar veterinários -----

    [Fact(DisplayName = "I071 — EP-021 lista só os Veterinários com conta ativa, por nome")]
    public async Task ListaSoVeterinariosAtivosPorNome()
    {
        using var db = CriarContexto();
        var dados = await CriarBaseAsync(db);

        var veterinarios = await CriarServico(db).ListarAsync();

        Assert.Equal("Ana Vet|Bruno Vet", string.Join('|', veterinarios.Select(v => v.Nome)));
        Assert.Equal(dados.VetA, veterinarios[0].Id);
        Assert.Equal(dados.VetB, veterinarios[1].Id);
    }

    [Fact(DisplayName = "I071 — EP-021 com Veterinários com o mesmo nome ordena pelo identificador")]
    public async Task MesmoNomeOrdenadoPorIdentificador()
    {
        using var db = CriarContexto();
        var primeiro = await AdicionarFuncionarioAsync(db, "Carla Vet", Perfis.Veterinario);
        var segundo = await AdicionarFuncionarioAsync(db, "Carla Vet", Perfis.Veterinario);

        var veterinarios = await CriarServico(db).ListarAsync();

        Assert.Equal(primeiro, veterinarios[0].Id);
        Assert.Equal(segundo, veterinarios[1].Id);
    }

    [Fact(DisplayName = "I071 — EP-021 sem Veterinários devolve uma lista vazia")]
    public async Task SemVeterinariosListaVazia()
    {
        using var db = CriarContexto();
        await AdicionarFuncionarioAsync(db, "Rita Rececao", Perfis.Rececionista);

        Assert.Empty(await CriarServico(db).ListarAsync());
    }

    // ----- EP-022 — Consultar disponibilidade -----

    [Fact(DisplayName = "I071 — EP-022 devolve os horários livres em HH:mm, com a duração e o Veterinário")]
    public async Task DisponibilidadeDevolveHorariosEmTexto()
    {
        using var db = CriarContexto();
        var dados = await CriarBaseAsync(db);

        var disponibilidade = await CriarServico(db).ObterDisponibilidadeAsync(dados.VetA, Periodo(Quinta));

        Assert.Equal(dados.VetA, disponibilidade.VeterinarioId);
        Assert.Equal(30, disponibilidade.DuracaoConsultaMinutos);
        var dia = Assert.Single(disponibilidade.Dias);
        Assert.Equal(Quinta, dia.Data);
        Assert.Equal(18, dia.HorariosLivres.Count);
        Assert.Equal("09:00", dia.HorariosLivres[0]);
        Assert.Equal("18:30", dia.HorariosLivres[^1]);
    }

    [Fact(DisplayName = "I071 — EP-022 retira os horários das marcações guardadas, mas não os das canceladas")]
    public async Task MarcacoesGuardadasOcupamHorario()
    {
        using var db = CriarContexto();
        var dados = await CriarBaseAsync(db);
        await AdicionarMarcacaoAsync(db, dados.VetA, "2026-10-15T09:00:00Z", 30, "Confirmada"); // 10:00 local
        await AdicionarMarcacaoAsync(db, dados.VetA, "2026-10-15T13:00:00Z", 30, "Pendente");   // 14:00 local
        await AdicionarMarcacaoAsync(db, dados.VetA, "2026-10-15T14:00:00Z", 30, "Cancelada");  // 15:00 local

        var livres = (await CriarServico(db).ObterDisponibilidadeAsync(dados.VetA, Periodo(Quinta))).Dias[0].HorariosLivres;

        Assert.DoesNotContain("10:00", livres);
        Assert.DoesNotContain("14:00", livres);
        Assert.Contains("15:00", livres);
        Assert.Equal(16, livres.Count);
    }

    [Fact(DisplayName = "UT-F003-010 — Uma marcação de outro Veterinário não ocupa o horário")]
    public async Task MarcacaoDeOutroVeterinarioNaoOcupa()
    {
        using var db = CriarContexto();
        var dados = await CriarBaseAsync(db);
        await AdicionarMarcacaoAsync(db, dados.VetB, "2026-10-15T09:00:00Z", 30, "Confirmada"); // 10:00 local

        var servico = CriarServico(db);
        var livresVetA = (await servico.ObterDisponibilidadeAsync(dados.VetA, Periodo(Quinta))).Dias[0].HorariosLivres;
        var livresVetB = (await servico.ObterDisponibilidadeAsync(dados.VetB, Periodo(Quinta))).Dias[0].HorariosLivres;

        Assert.Contains("10:00", livresVetA);
        Assert.DoesNotContain("10:00", livresVetB);
    }

    [Fact(DisplayName = "I071 — EP-022 usa a duração configurada em EP-010 e ignora os períodos inativos")]
    public async Task UsaDuracaoConfiguradaEPeriodosAtivos()
    {
        using var db = CriarContexto();
        var dados = await CriarBaseAsync(db);
        db.ConfiguracaoSistema.Add(new ConfiguracaoSistema
        {
            Chave = "DuracaoPadraoConsultaMinutos",
            Valor = "45",
            DataUltimaAlteracao = Relogio.UtcDateTime
        });
        // Período inativo ao domingo: não pode criar horários.
        db.HorariosFuncionamento.Add(new HorarioFuncionamento
        {
            DiaSemana = 7,
            HoraAbertura = new TimeOnly(9, 0),
            HoraFecho = new TimeOnly(13, 0),
            Ativo = false
        });
        await db.SaveChangesAsync();
        var servico = CriarServico(db);

        var quinta = await servico.ObterDisponibilidadeAsync(dados.VetA, Periodo(Quinta));
        var domingo = await servico.ObterDisponibilidadeAsync(dados.VetA, Periodo(new DateOnly(2026, 10, 18)));

        Assert.Equal(45, quinta.DuracaoConsultaMinutos);
        Assert.Equal("09:00|09:45|10:30|11:15|12:00", string.Join('|', quinta.Dias[0].HorariosLivres.Take(5)));
        Assert.Empty(domingo.Dias[0].HorariosLivres);
    }

    [Fact(DisplayName = "UT-F003-013 — Um período de 31 dias é aceite e devolve 31 dias")]
    public async Task PeriodoDe31DiasDevolve31Dias()
    {
        using var db = CriarContexto();
        var dados = await CriarBaseAsync(db);

        var disponibilidade = await CriarServico(db).ObterDisponibilidadeAsync(
            dados.VetA, Periodo(new DateOnly(2026, 10, 1), new DateOnly(2026, 10, 31)));

        Assert.Equal(31, disponibilidade.Dias.Count);
        Assert.Equal(new DateOnly(2026, 10, 1), disponibilidade.Dias[0].Data);
        Assert.Equal(new DateOnly(2026, 10, 31), disponibilidade.Dias[^1].Data);
    }

    [Theory(DisplayName = "UT-F003-014, UT-F003-015 e UT-F003-016 — Período de 32 dias, invertido ou sem data de início dá 400")]
    [InlineData("2026-10-01", "2026-11-01", ValidadorPeriodoDisponibilidade.MensagemPeriodoDemasiadoLongo)]
    [InlineData("2026-10-15", "2026-10-14", ValidadorPeriodoDisponibilidade.MensagemDataFimAnterior)]
    [InlineData(null, "2026-10-15", ValidadorPeriodoDisponibilidade.MensagemDataInicioObrigatoria)]
    public async Task PeriodoInvalidoERecusado(string? dataInicio, string dataFim, string mensagem)
    {
        using var db = CriarContexto();
        var dados = await CriarBaseAsync(db);
        var query = new DisponibilidadeQuery
        {
            DataInicio = dataInicio is null ? null : Data(dataInicio),
            DataFim = Data(dataFim)
        };

        var excecao = await Assert.ThrowsAsync<ArgumentException>(
            () => CriarServico(db).ObterDisponibilidadeAsync(dados.VetA, query));

        Assert.Equal(mensagem, excecao.Message);
    }

    [Fact(DisplayName = "UT-F003-017 — Sem dataFim, a disponibilidade tem um único dia")]
    public async Task SemDataFimTemUmDia()
    {
        using var db = CriarContexto();
        var dados = await CriarBaseAsync(db);

        var disponibilidade = await CriarServico(db).ObterDisponibilidadeAsync(
            dados.VetA, new DisponibilidadeQuery { DataInicio = Quinta });

        Assert.Equal(Quinta, Assert.Single(disponibilidade.Dias).Data);
    }

    [Theory(DisplayName = "UT-F003-018 — O id de quem não é Veterinário, ou inexistente, dá 404")]
    [InlineData("REC_A")]
    [InlineData("999")]
    public async Task NaoVeterinarioOuInexistenteDaNaoEncontrado(string conta)
    {
        using var db = CriarContexto();
        var dados = await CriarBaseAsync(db);
        var id = conta == "REC_A" ? dados.RecA : 999;

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => CriarServico(db).ObterDisponibilidadeAsync(id, Periodo(Quinta)));
    }

    [Fact(DisplayName = "I071 — Um Veterinário com a conta bloqueada dá 404 (não está disponível para marcação)")]
    public async Task VeterinarioBloqueadoDaNaoEncontrado()
    {
        using var db = CriarContexto();
        var dados = await CriarBaseAsync(db);

        var excecao = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => CriarServico(db).ObterDisponibilidadeAsync(dados.VetBloqueado, Periodo(Quinta)));

        Assert.Equal("O veterinário indicado não existe ou não está ativo.", excecao.Message);
    }

    [Fact(DisplayName = "I071 — Um período inválido dá 400 antes de se procurar o Veterinário")]
    public async Task PeriodoInvalidoAntesDoVeterinario()
    {
        using var db = CriarContexto();

        await Assert.ThrowsAsync<ArgumentException>(() => CriarServico(db).ObterDisponibilidadeAsync(
            999, Periodo(new DateOnly(2026, 10, 15), new DateOnly(2026, 10, 14))));
    }

    // ----- Preparação dos dados -----

    private sealed record DadosBase(int VetA, int VetB, int VetBloqueado, int RecA);

    // Base em memória da I064 (I028, secção 4.2); o serviço não abre transações.
    private static ClinicaDbContext CriarContexto() => AjudaBaseDados.CriarContexto();

    private static VeterinariosService CriarServico(ClinicaDbContext db) =>
        new(db, new RelogioFixo(Relogio));

    private static DisponibilidadeQuery Periodo(DateOnly inicio, DateOnly? fim = null) =>
        new() { DataInicio = inicio, DataFim = fim };

    private static DateOnly Data(string texto) => DateOnly.Parse(texto, CultureInfo.InvariantCulture);

    /// <summary>
    /// HORARIO_PADRAO da I028 e as contas VET_A, VET_B, um Veterinário bloqueado, REC_A,
    /// um Administrador e um Cliente.
    /// </summary>
    private static async Task<DadosBase> CriarBaseAsync(ClinicaDbContext db)
    {
        for (byte dia = 1; dia <= 5; dia++)
        {
            db.HorariosFuncionamento.Add(Horario(dia, 9, 13));
            db.HorariosFuncionamento.Add(Horario(dia, 14, 19));
        }

        db.HorariosFuncionamento.Add(Horario(6, 9, 13));
        await db.SaveChangesAsync();

        // Criados fora da ordem alfabética, para a ordenação ser verificada.
        var vetB = await AdicionarFuncionarioAsync(db, "Bruno Vet", Perfis.Veterinario);
        var vetA = await AdicionarFuncionarioAsync(db, "Ana Vet", Perfis.Veterinario);
        var vetBloqueado = await AdicionarFuncionarioAsync(db, "Abel Vet", Perfis.Veterinario, EstadosUtilizador.Bloqueado);
        var recA = await AdicionarFuncionarioAsync(db, "Rita Rececao", Perfis.Rececionista);
        await AdicionarFuncionarioAsync(db, "Alice Admin", Perfis.Administrador);
        await AdicionarClienteAsync(db);

        return new DadosBase(vetA, vetB, vetBloqueado, recA);
    }

    private static HorarioFuncionamento Horario(byte diaSemana, int abertura, int fecho) => new()
    {
        DiaSemana = diaSemana,
        HoraAbertura = new TimeOnly(abertura, 0),
        HoraFecho = new TimeOnly(fecho, 0),
        Ativo = true
    };

    private static async Task<int> AdicionarFuncionarioAsync(
        ClinicaDbContext db,
        string nome,
        string perfil,
        string estado = EstadosUtilizador.Ativo)
    {
        var utilizador = new Utilizador
        {
            Email = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "@clinica-g08.test",
            PasswordHash = "hash-nao-usado-nestes-testes",
            Perfil = perfil,
            Estado = estado,
            DataCriacao = Relogio.UtcDateTime
        };
        db.Utilizadores.Add(utilizador);
        await db.SaveChangesAsync();

        db.Funcionarios.Add(new Funcionario { UtilizadorId = utilizador.Id, Nome = nome });
        await db.SaveChangesAsync();

        return utilizador.Id;
    }

    private static async Task AdicionarClienteAsync(ClinicaDbContext db)
    {
        var utilizador = new Utilizador
        {
            Email = "cliente@clinica-g08.test",
            PasswordHash = "hash-nao-usado-nestes-testes",
            Perfil = Perfis.Cliente,
            Estado = EstadosUtilizador.Ativo,
            DataCriacao = Relogio.UtcDateTime
        };
        db.Utilizadores.Add(utilizador);
        await db.SaveChangesAsync();

        db.Clientes.Add(new Cliente
        {
            UtilizadorId = utilizador.Id,
            Nome = "Aaron Cliente",
            Telefone = "960000001",
            DataRegisto = Relogio.UtcDateTime
        });
        await db.SaveChangesAsync();
    }

    private static async Task AdicionarMarcacaoAsync(
        ClinicaDbContext db,
        int veterinarioId,
        string inicioUtc,
        short duracaoMinutos,
        string estado)
    {
        db.Marcacoes.Add(new Marcacao
        {
            PacienteId = 1,
            VeterinarioId = veterinarioId,
            DataHoraInicio = DateTime.Parse(
                inicioUtc, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal),
            DuracaoMinutos = duracaoMinutos,
            Estado = estado,
            CriadaPorId = 1,
            DataCriacao = Relogio.UtcDateTime
        });
        await db.SaveChangesAsync();
    }
}
