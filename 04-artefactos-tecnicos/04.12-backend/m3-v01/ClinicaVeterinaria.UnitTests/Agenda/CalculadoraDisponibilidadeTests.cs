using System.Globalization;
using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.UnitTests.Agenda;

/// <summary>
/// I071 — cálculo da disponibilidade, sem base de dados (critério 6). Casos UT-F003-001 a
/// UT-F003-009, UT-F003-011 e UT-F003-012 da Especificação de Testes Unitários (I028, secção 7.3.1);
/// os restantes casos são próprios desta tarefa. Os casos UT-F003-010 e UT-F003-013 a UT-F003-018
/// dependem da leitura dos dados ou dos parâmetros e estão em <c>VeterinariosServiceTests</c> e
/// <c>ValidadorPeriodoDisponibilidadeTests</c>.
/// </summary>
public class CalculadoraDisponibilidadeTests
{
    // RELOGIO da I028: quarta-feira, 2026-10-14, 09:00 em Lisboa (hora de verão, UTC+1).
    private static readonly DateTime Relogio = new(2026, 10, 14, 8, 0, 0, DateTimeKind.Utc);

    // Quinta-feira, ainda na hora de verão.
    private static readonly DateOnly Quinta = new(2026, 10, 15);

    private const string DiaCompleto =
        "09:00|09:30|10:00|10:30|11:00|11:30|12:00|12:30|14:00|14:30|15:00|15:30|16:00|16:30|17:00|17:30|18:00|18:30";

    // HORARIO_PADRAO da I028: segunda a sexta 09:00–13:00 e 14:00–19:00; sábado 09:00–13:00; domingo fechado.
    private static readonly IReadOnlyList<PeriodoValidado> HorarioPadrao =
    [
        Periodo(1, "09:00", "13:00"), Periodo(1, "14:00", "19:00"),
        Periodo(2, "09:00", "13:00"), Periodo(2, "14:00", "19:00"),
        Periodo(3, "09:00", "13:00"), Periodo(3, "14:00", "19:00"),
        Periodo(4, "09:00", "13:00"), Periodo(4, "14:00", "19:00"),
        Periodo(5, "09:00", "13:00"), Periodo(5, "14:00", "19:00"),
        Periodo(6, "09:00", "13:00")
    ];

    private static TimeZoneInfo Fuso => FusoHorarioClinica.Obter();

    [Fact(DisplayName = "UT-F003-001 — Sem marcações, um dia útil tem 18 horários de 30 em 30 minutos")]
    public void DiaUtilSemMarcacoesTemDezoitoHorarios()
    {
        var livres = Livres(Quinta);

        Assert.Equal(DiaCompleto, livres);
        Assert.Equal(18, livres.Split('|').Length);
    }

    [Fact(DisplayName = "UT-F003-002 — Uma marcação Confirmada às 10:00 locais ocupa só esse horário")]
    public void MarcacaoConfirmadaOcupaOHorario()
    {
        var livres = Livres(Quinta, marcacoes: [Marcacao("2026-10-15T09:00:00Z", 30, "Confirmada")]).Split('|');

        Assert.DoesNotContain("10:00", livres);
        Assert.Contains("09:30", livres);
        Assert.Contains("10:30", livres);
    }

    [Fact(DisplayName = "UT-F003-003 — Duas marcações seguidas ocupam os dois horários e libertam os vizinhos")]
    public void MarcacoesSeguidasOcupamOsDoisHorarios()
    {
        var livres = Livres(
            Quinta,
            marcacoes:
            [
                Marcacao("2026-10-15T09:00:00Z", 30, "Pendente"),
                Marcacao("2026-10-15T09:30:00Z", 30, "Confirmada")
            ]).Split('|');

        Assert.DoesNotContain("10:00", livres);
        Assert.DoesNotContain("10:30", livres);
        Assert.Contains("09:30", livres);
        Assert.Contains("11:00", livres);
    }

    [Fact(DisplayName = "UT-F003-004 — O último horário de cada período acaba no fecho")]
    public void UltimoHorarioAcabaNoFecho()
    {
        var livres = Livres(Quinta).Split('|');

        Assert.Contains("12:30", livres);
        Assert.DoesNotContain("13:00", livres);
        Assert.Contains("18:30", livres);
        Assert.DoesNotContain("19:00", livres);
    }

    [Fact(DisplayName = "UT-F003-005 — Uma marcação Cancelada não ocupa horário")]
    public void MarcacaoCanceladaNaoOcupaHorario()
    {
        var livres = Livres(Quinta, marcacoes: [Marcacao("2026-10-15T09:00:00Z", 30, "Cancelada")]);

        Assert.Equal(DiaCompleto, livres);
    }

    [Fact(DisplayName = "UT-F003-006 — Um domingo sem períodos é devolvido com a lista vazia")]
    public void DomingoSemPeriodosTemListaVazia()
    {
        var domingo = new DateOnly(2026, 10, 18);

        var dia = Assert.Single(Calcular(domingo, domingo));

        Assert.Equal(domingo, dia.Data);
        Assert.Empty(dia.HorariosLivres);
    }

    [Fact(DisplayName = "UT-F003-007 — Entre os dois períodos do dia (13:00–14:00) não há horários")]
    public void IntervaloEntrePeriodosNaoTemHorarios()
    {
        var dia = Assert.Single(Calcular(Quinta, Quinta));

        Assert.DoesNotContain(dia.HorariosLivres, hora => hora.Hour == 13);
    }

    [Fact(DisplayName = "UT-F003-008 — Com duração de 45 minutos a grelha é de 45 em 45 minutos e só se a consulta couber")]
    public void DuracaoDe45MinutosMudaAGrelha()
    {
        var livres = Livres(Quinta, duracaoMinutos: 45);

        Assert.StartsWith("09:00|09:45|10:30|11:15|12:00|14:00", livres, StringComparison.Ordinal);
        Assert.DoesNotContain("12:45", livres.Split('|'));
        Assert.EndsWith("17:00|17:45", livres, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "UT-F003-009 — Uma marcação de 60 minutos às 10:15 ocupa todos os horários que se sobrepõem")]
    public void MarcacaoDe60MinutosForaDaGrelha()
    {
        var livres = Livres(Quinta, marcacoes: [Marcacao("2026-10-15T09:15:00Z", 60, "Pendente")]).Split('|');

        Assert.DoesNotContain("10:00", livres);
        Assert.DoesNotContain("10:30", livres);
        Assert.DoesNotContain("11:00", livres);
        Assert.Contains("09:30", livres);
        Assert.Contains("11:30", livres);
    }

    [Fact(DisplayName = "UT-F003-011 — Na hora de inverno, 09:00Z é 09:00 em Lisboa")]
    public void HoraDeInvernoConverteParaHoraLocal()
    {
        var segunda = new DateOnly(2026, 10, 26);

        var livres = Livres(segunda, marcacoes: [Marcacao("2026-10-26T09:00:00Z", 30, "Confirmada")]).Split('|');

        Assert.DoesNotContain("09:00", livres);
        Assert.Contains("10:00", livres);
    }

    [Fact(DisplayName = "UT-F003-012 — Os horários que já começaram não são devolvidos")]
    public void HorariosQueJaComecaramNaoSaoDevolvidos()
    {
        var quarta = new DateOnly(2026, 10, 14);

        var livres = Livres(quarta, agoraUtc: new DateTime(2026, 10, 14, 8, 10, 0, DateTimeKind.Utc));

        Assert.StartsWith("09:30|", livres, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "I071 — Um horário que começa no próprio instante atual ainda é devolvido")]
    public void HorarioQueComecaAgoraEDevolvido()
    {
        var quarta = new DateOnly(2026, 10, 14);

        var livres = Livres(quarta, agoraUtc: new DateTime(2026, 10, 14, 8, 30, 0, DateTimeKind.Utc));

        Assert.StartsWith("09:30|", livres, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "I071 — Um dia inteiramente no passado não tem horários livres")]
    public void DiaNoPassadoNaoTemHorarios()
    {
        var ontem = new DateOnly(2026, 10, 13);

        Assert.Empty(Assert.Single(Calcular(ontem, ontem)).HorariosLivres);
    }

    [Fact(DisplayName = "I071 — Uma marcação que acaba quando o horário começa não o ocupa (R-HORARIO)")]
    public void MarcacaoQueSoTocaNaoOcupa()
    {
        // 09:00–10:00 locais: ocupa as 09:00 e as 09:30; o horário das 10:00 só toca no fim da marcação.
        var livres = Livres(Quinta, marcacoes: [Marcacao("2026-10-15T08:00:00Z", 60, "Pendente")]).Split('|');

        Assert.DoesNotContain("09:00", livres);
        Assert.DoesNotContain("09:30", livres);
        Assert.Contains("10:00", livres);
    }

    [Fact(DisplayName = "I071 — Vários dias: um elemento por dia, por ordem, incluindo sábado e domingo")]
    public void VariosDiasPorOrdem()
    {
        var dias = Calcular(new DateOnly(2026, 10, 15), new DateOnly(2026, 10, 18));

        Assert.Equal(
            "2026-10-15|2026-10-16|2026-10-17|2026-10-18",
            string.Join('|', dias.Select(dia => dia.Data.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))));
        Assert.Equal(18, dias[1].HorariosLivres.Count);
        Assert.Equal(8, dias[2].HorariosLivres.Count);
        Assert.Empty(dias[3].HorariosLivres);
    }

    [Fact(DisplayName = "I071 — Uma consulta mais longa do que o período não tem horários")]
    public void DuracaoMaiorDoQueOPeriodoNaoTemHorarios()
    {
        Assert.Equal(string.Empty, Livres(new DateOnly(2026, 10, 17), duracaoMinutos: 240 + 30));
    }

    [Fact(DisplayName = "I071 — No dia da mudança para a hora de verão, a hora que não existe não é devolvida")]
    public void HoraInexistenteNaMudancaDeHoraNaoEDevolvida()
    {
        // Em 2027-03-28 (domingo), às 01:00 de Lisboa os relógios passam para as 02:00.
        var domingo = new DateOnly(2027, 3, 28);
        IReadOnlyList<PeriodoValidado> horarioNoturno = [Periodo(7, "00:00", "04:00")];

        var dia = Assert.Single(CalculadoraDisponibilidade.Calcular(
            domingo, domingo, horarioNoturno, 60, [], Relogio, Fuso));

        Assert.Equal("00:00|02:00|03:00", Texto(dia.HorariosLivres));
    }

    [Fact(DisplayName = "I071 — Uma duração não positiva ou um período invertido são recusados")]
    public void ArgumentosInvalidosSaoRecusados()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CalculadoraDisponibilidade.Calcular(
            Quinta, Quinta, HorarioPadrao, 0, [], Relogio, Fuso));
        Assert.Throws<ArgumentException>(() => CalculadoraDisponibilidade.Calcular(
            Quinta, Quinta.AddDays(-1), HorarioPadrao, 30, [], Relogio, Fuso));
    }

    [Theory(DisplayName = "I071 — O dia da semana segue a base de dados: 1 = segunda … 7 = domingo")]
    [InlineData("2026-10-12", 1)]
    [InlineData("2026-10-15", 4)]
    [InlineData("2026-10-17", 6)]
    [InlineData("2026-10-18", 7)]
    public void DiaSemanaComoNaBaseDeDados(string data, int esperado)
    {
        Assert.Equal(esperado, CalculadoraDisponibilidade.DiaSemana(DateOnly.Parse(data, CultureInfo.InvariantCulture)));
    }

    // --- Verificação de um intervalo qualquer (reutilizável por EP-025 e EP-026) ---

    [Theory(DisplayName = "I071 — Um intervalo livre e dentro de um período está disponível, mesmo fora da grelha")]
    [InlineData("2026-10-15T08:00:00Z", 30)] // 09:00–09:30
    [InlineData("2026-10-15T09:15:00Z", 30)] // 10:15–10:45 (fora da grelha de 30 minutos, PA-12)
    [InlineData("2026-10-15T11:30:00Z", 30)] // 12:30–13:00: acaba no fecho
    [InlineData("2026-10-26T09:00:00Z", 30)] // 09:00 em Lisboa, já na hora de inverno
    public void IntervaloLivreEstaDisponivel(string inicio, int duracao)
    {
        Assert.True(CalculadoraDisponibilidade.IntervaloDisponivel(
            Instante(inicio), duracao, HorarioPadrao, [], Fuso));
    }

    [Theory(DisplayName = "I071 — Um intervalo fora do horário não está disponível")]
    [InlineData("2026-10-15T11:45:00Z", 30)] // 12:45–13:15: passa o fecho
    [InlineData("2026-10-15T07:30:00Z", 60)] // 08:30–09:30: começa antes da abertura
    [InlineData("2026-10-18T09:00:00Z", 30)] // domingo
    [InlineData("2026-10-26T08:30:00Z", 30)] // 08:30 em Lisboa, na hora de inverno
    public void IntervaloForaDoHorarioNaoEstaDisponivel(string inicio, int duracao)
    {
        Assert.False(CalculadoraDisponibilidade.IntervaloDisponivel(
            Instante(inicio), duracao, HorarioPadrao, [], Fuso));
    }

    [Fact(DisplayName = "I071 — Um intervalo que se sobrepõe a uma marcação não cancelada não está disponível")]
    public void IntervaloSobrepostoNaoEstaDisponivel()
    {
        MarcacaoAgendada[] marcacoes = [Marcacao("2026-10-15T09:00:00Z", 30, "Confirmada")];

        Assert.False(CalculadoraDisponibilidade.IntervaloDisponivel(
            Instante("2026-10-15T08:45:00Z"), 30, HorarioPadrao, marcacoes, Fuso));
        Assert.True(CalculadoraDisponibilidade.IntervaloDisponivel(
            Instante("2026-10-15T09:30:00Z"), 30, HorarioPadrao, marcacoes, Fuso));
    }

    [Fact(DisplayName = "I071 — Uma marcação cancelada não impede um intervalo")]
    public void MarcacaoCanceladaNaoImpedeIntervalo()
    {
        Assert.True(CalculadoraDisponibilidade.IntervaloDisponivel(
            Instante("2026-10-15T09:00:00Z"), 30, HorarioPadrao,
            [Marcacao("2026-10-15T09:00:00Z", 30, "Cancelada")], Fuso));
    }

    // --- Fuso horário da clínica (R-FUSO) ---

    [Theory(DisplayName = "I071 — A hora local de Lisboa é convertida para UTC conforme a hora de verão")]
    [InlineData("2026-10-15", "10:00", "2026-10-15T09:00:00Z")] // verão: UTC+1
    [InlineData("2026-10-26", "09:00", "2026-10-26T09:00:00Z")] // inverno: UTC
    [InlineData("2026-10-25", "01:30", "2026-10-25T00:30:00Z")] // hora repetida: primeira ocorrência
    public void HoraLocalConvertidaParaUtc(string data, string hora, string esperado)
    {
        var utc = FusoHorarioClinica.ParaUtc(
            DateOnly.Parse(data, CultureInfo.InvariantCulture),
            TimeOnly.Parse(hora, CultureInfo.InvariantCulture),
            Fuso);

        Assert.Equal(Instante(esperado), utc);
        Assert.Equal(DateTimeKind.Utc, utc!.Value.Kind);
    }

    [Fact(DisplayName = "I071 — Uma hora que não existe (mudança para a hora de verão) não tem UTC")]
    public void HoraInexistenteNaoTemUtc()
    {
        Assert.Null(FusoHorarioClinica.ParaUtc(new DateOnly(2027, 3, 28), new TimeOnly(1, 30), Fuso));
    }

    // --- Preparação ---

    private static IReadOnlyList<DiaDisponivel> Calcular(DateOnly inicio, DateOnly fim) =>
        CalculadoraDisponibilidade.Calcular(inicio, fim, HorarioPadrao, 30, [], Relogio, Fuso);

    /// <summary>Horários livres de um único dia, como texto "HH:mm|HH:mm|…".</summary>
    private static string Livres(
        DateOnly data,
        int duracaoMinutos = 30,
        IReadOnlyList<MarcacaoAgendada>? marcacoes = null,
        DateTime? agoraUtc = null)
    {
        var dia = Assert.Single(CalculadoraDisponibilidade.Calcular(
            data, data, HorarioPadrao, duracaoMinutos, marcacoes ?? [], agoraUtc ?? Relogio, Fuso));

        return Texto(dia.HorariosLivres);
    }

    private static string Texto(IEnumerable<TimeOnly> horas) =>
        string.Join('|', horas.Select(hora => hora.ToString("HH:mm", CultureInfo.InvariantCulture)));

    private static PeriodoValidado Periodo(byte diaSemana, string abertura, string fecho) =>
        new(diaSemana, TimeOnly.Parse(abertura, CultureInfo.InvariantCulture), TimeOnly.Parse(fecho, CultureInfo.InvariantCulture));

    private static MarcacaoAgendada Marcacao(string inicioUtc, int duracaoMinutos, string estado) =>
        new(Instante(inicioUtc), duracaoMinutos, estado);

    private static DateTime Instante(string utc) =>
        DateTime.Parse(utc, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
}
