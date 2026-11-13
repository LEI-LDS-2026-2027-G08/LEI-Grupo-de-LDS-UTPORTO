using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.DTOs.Clinica;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Domain.Entities;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.UnitTests.Seguranca;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.UnitTests.DadosClinica;

/// <summary>
/// I086 — dados gerais, configurações e horário da clínica (EP-009, EP-010) no
/// <see cref="ClinicaService"/>: casos UT-F001-100 a UT-F001-111 da Especificação de Testes
/// Unitários (I028, secção 7.1.9). As regras de formato do pedido (duração e NIF) estão no
/// <see cref="UpdateClinicaDto"/> e dão 400 na validação automática do [ApiController].
/// </summary>
public class ClinicaServiceTests
{
    // RELOGIO da Especificação de Testes Unitários (I028, secção 5).
    private static readonly DateTimeOffset Relogio = new(2026, 10, 14, 8, 0, 0, TimeSpan.Zero);

    private const string ChaveDuracaoConsulta = "DuracaoPadraoConsultaMinutos";

    // ----- EP-009 — Consultar clínica -----
    [Fact(DisplayName = "UT-F001-100 — A clínica é devolvida com a duração de 30 minutos e os 11 períodos, com o dia 1 como Segunda")]
    public async Task ObterDevolveDuracaoEHorarioPadrao()
    {
        using var db = CriarBaseComClinica(HorarioPadrao());

        var clinica = await CriarServico(db).ObterAsync();

        Assert.Equal(30, clinica.DuracaoConsultaMinutos);
        Assert.Equal(11, clinica.HorarioFuncionamento.Count);
        Assert.Equal("Segunda", clinica.HorarioFuncionamento[0].DiaSemana);
    }

    [Fact(DisplayName = "UT-F001-101 — Um período com DiaSemana = 7 é devolvido como Domingo")]
    public async Task DiaSeteEDomingo()
    {
        using var db = CriarBaseComClinica([Periodo(7, 10, 12)]);

        var clinica = await CriarServico(db).ObterAsync();

        Assert.Equal("Domingo", Assert.Single(clinica.HorarioFuncionamento).DiaSemana);
    }

    // ----- EP-010 — Atualizar clínica -----
    [Fact(DisplayName = "UT-F001-102 — Um pedido válido substitui os dados, a duração e o horário e devolve o ClinicaDto")]
    public async Task AtualizarSubstituiDadosDuracaoEHorario()
    {
        using var db = CriarBaseComClinica([Periodo(7, 10, 12)]);
        var pedido = PedidoValido(HorarioPadraoDto());
        pedido.Nome = "Clínica Veterinária G08 — Maia";
        pedido.DuracaoConsultaMinutos = 20;

        var clinica = await CriarServico(db).AtualizarAsync(pedido);

        Assert.Equal("Clínica Veterinária G08 — Maia", (await db.Clinica.AsNoTracking().SingleAsync()).Nome);
        Assert.Equal("20", (await db.ConfiguracaoSistema.AsNoTracking().SingleAsync(c => c.Chave == ChaveDuracaoConsulta)).Valor);
        Assert.Equal(11, await db.HorariosFuncionamento.CountAsync());
        Assert.Equal(11, clinica.HorarioFuncionamento.Count);
    }

    [Fact(DisplayName = "UT-F001-103 — Um período Segunda 09:00–09:00 devolve 400 e o horário mantém-se")]
    public async Task FechoIgualAAberturaDevolve400()
    {
        using var db = CriarBaseComClinica(HorarioPadrao());

        await Assert.ThrowsAsync<ValidacaoException>(
            () => CriarServico(db).AtualizarAsync(PedidoValido([PeriodoDto("Segunda", "09:00", "09:00")])));

        Assert.Equal(11, await db.HorariosFuncionamento.CountAsync());
    }

    [Fact(DisplayName = "UT-F001-104 — Um período Segunda 14:00–13:00 devolve 400")]
    public async Task FechoAntesDaAberturaDevolve400()
    {
        using var db = CriarBaseComClinica(HorarioPadrao());

        await Assert.ThrowsAsync<ValidacaoException>(
            () => CriarServico(db).AtualizarAsync(PedidoValido([PeriodoDto("Segunda", "14:00", "13:00")])));
    }

    [Fact(DisplayName = "UT-F001-105 — Períodos sobrepostos no mesmo dia (09:00–13:00 e 12:00–15:00) devolvem 400")]
    public async Task PeriodosSobrepostosDevolvem400()
    {
        using var db = CriarBaseComClinica(HorarioPadrao());
        var pedido = PedidoValido([PeriodoDto("Segunda", "09:00", "13:00"), PeriodoDto("Segunda", "12:00", "15:00")]);

        await Assert.ThrowsAsync<ValidacaoException>(() => CriarServico(db).AtualizarAsync(pedido));
    }

    [Fact(DisplayName = "UT-F001-106 — Um período que acaba às 13:00 e outro que começa às 13:00 não se sobrepõem")]
    public async Task PeriodosContiguosSaoAceites()
    {
        using var db = CriarBaseComClinica(HorarioPadrao());
        var pedido = PedidoValido([PeriodoDto("Segunda", "09:00", "13:00"), PeriodoDto("Segunda", "13:00", "15:00")]);

        var clinica = await CriarServico(db).AtualizarAsync(pedido);

        Assert.Equal(2, clinica.HorarioFuncionamento.Count);
    }

    [Fact(DisplayName = "UT-F001-107 — As mesmas horas em dias diferentes (Segunda e Terca) são aceites")]
    public async Task MesmasHorasEmDiasDiferentesSaoAceites()
    {
        using var db = CriarBaseComClinica(HorarioPadrao());
        var pedido = PedidoValido([PeriodoDto("Segunda", "09:00", "13:00"), PeriodoDto("Terca", "09:00", "13:00")]);

        var clinica = await CriarServico(db).AtualizarAsync(pedido);

        Assert.Equal(2, clinica.HorarioFuncionamento.Count);
    }

    [Theory(DisplayName = "UT-F001-108 — Horas fora do formato HH:mm devolvem 400")]
    [InlineData("9h00", "13:00")]
    [InlineData("09:00", "25:00")]
    public async Task HoraForaDoFormatoDevolve400(string abertura, string fecho)
    {
        using var db = CriarBaseComClinica(HorarioPadrao());

        await Assert.ThrowsAsync<ValidacaoException>(
            () => CriarServico(db).AtualizarAsync(PedidoValido([PeriodoDto("Segunda", abertura, fecho)])));
    }

    [Fact(DisplayName = "UT-F001-109 — duracaoConsultaMinutos = 0 é recusada na validação do pedido (400)")]
    public void DuracaoZeroEInvalida()
    {
        var pedido = PedidoValido(HorarioPadraoDto());
        pedido.DuracaoConsultaMinutos = 0;

        Assert.Contains(nameof(UpdateClinicaDto.DuracaoConsultaMinutos), CamposComErro(pedido));
    }

    [Fact(DisplayName = "UT-F001-110 — Horário vazio: aceite como clínica fechada (divergência registada; a I028 previa 400)")]
    public async Task HorarioVazioFechaAClinica()
    {
        using var db = CriarBaseComClinica(HorarioPadrao());

        var clinica = await CriarServico(db).AtualizarAsync(PedidoValido([]));

        Assert.Empty(clinica.HorarioFuncionamento);
    }

    [Fact(DisplayName = "UT-F001-111 — Um NIF com o dígito de controlo errado é recusado na validação do pedido (400)")]
    public void NifInvalidoEInvalido()
    {
        var pedido = PedidoValido(HorarioPadraoDto());
        pedido.Nif = "501234567";

        Assert.Contains(nameof(UpdateClinicaDto.Nif), CamposComErro(pedido));
    }

    [Fact(DisplayName = "I067 — Sem a lista do horário (null), o pedido é recusado (400)")]
    public void HorarioEmFaltaEInvalido()
    {
        var pedido = PedidoValido(HorarioPadraoDto());
        pedido.HorarioFuncionamento = null;

        Assert.Contains(nameof(UpdateClinicaDto.HorarioFuncionamento), CamposComErro(pedido));
    }

    // ----- Apoio -----
    private static ClinicaService CriarServico(ClinicaDbContext db) => new(db, new RelogioFixo(Relogio));

    private static ClinicaDbContext CriarBaseComClinica(IEnumerable<HorarioFuncionamento> horario)
    {
        var db = AjudaBaseDados.CriarContexto();

        db.Clinica.Add(new Clinica
        {
            Id = 1,
            Nome = "Clínica Veterinária G08",
            NIF = "501234560",
            Morada = "Rua do Exemplo, 100, 4470-000 Maia",
            Telefone = "229000000",
            Email = "geral@clinica-g08.test",
            DataUltimaAlteracao = Relogio.UtcDateTime.AddDays(-30),
        });
        db.ConfiguracaoSistema.Add(new ConfiguracaoSistema
        {
            Chave = ChaveDuracaoConsulta,
            Valor = "30",
            DataUltimaAlteracao = Relogio.UtcDateTime.AddDays(-30),
        });
        db.HorariosFuncionamento.AddRange(horario);
        db.SaveChanges();
        db.ChangeTracker.Clear();

        return db;
    }

    /// <summary>HORARIO_PADRAO da I028: segunda a sexta 09:00–13:00 e 14:00–19:00; sábado 09:00–13:00.</summary>
    private static List<HorarioFuncionamento> HorarioPadrao()
    {
        var periodos = new List<HorarioFuncionamento>();

        for (byte dia = 1; dia <= 5; dia++)
        {
            periodos.Add(Periodo(dia, 9, 13));
            periodos.Add(Periodo(dia, 14, 19));
        }

        periodos.Add(Periodo(6, 9, 13));

        return periodos;
    }

    private static HorarioFuncionamento Periodo(byte dia, int abertura, int fecho) => new()
    {
        DiaSemana = dia,
        HoraAbertura = new TimeOnly(abertura, 0),
        HoraFecho = new TimeOnly(fecho, 0),
        Ativo = true,
    };

    private static List<PeriodoHorarioDto> HorarioPadraoDto()
    {
        string[] dias = ["Segunda", "Terca", "Quarta", "Quinta", "Sexta"];
        var periodos = new List<PeriodoHorarioDto>();

        foreach (var dia in dias)
        {
            periodos.Add(PeriodoDto(dia, "09:00", "13:00"));
            periodos.Add(PeriodoDto(dia, "14:00", "19:00"));
        }

        periodos.Add(PeriodoDto("Sabado", "09:00", "13:00"));

        return periodos;
    }

    private static PeriodoHorarioDto PeriodoDto(string dia, string abertura, string fecho) =>
        new() { DiaSemana = dia, Abertura = abertura, Fecho = fecho };

    private static UpdateClinicaDto PedidoValido(List<PeriodoHorarioDto> horario) => new()
    {
        Nome = "Clínica Veterinária G08",
        Nif = "501234560",
        Morada = "Rua do Exemplo, 100, 4470-000 Maia",
        Telefone = "229000000",
        Email = "geral@clinica-g08.test",
        DuracaoConsultaMinutos = 30,
        HorarioFuncionamento = horario,
    };

    private static List<string> CamposComErro(object pedido)
    {
        var erros = new List<ValidationResult>();
        Validator.TryValidateObject(pedido, new ValidationContext(pedido), erros, validateAllProperties: true);
        return erros.SelectMany(e => e.MemberNames).ToList();
    }
}
