using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.DTOs.Veterinarios;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.UnitTests.Validacao;

/// <summary>
/// I071 — período de EP-022. Casos UT-F003-013 a UT-F003-016 da Especificação de Testes Unitários
/// (I028, secção 7.3.1) aplicados à regra; no serviço, os mesmos casos estão em <c>VeterinariosServiceTests</c>.
/// </summary>
public class ValidadorPeriodoDisponibilidadeTests
{
    [Fact(DisplayName = "UT-F003-013 — Um período de 31 dias é aceite")]
    public void PeriodoDe31DiasEAceite()
    {
        Assert.Null(ValidadorPeriodoDisponibilidade.ObterErro(new DateOnly(2026, 10, 1), new DateOnly(2026, 10, 31)));
    }

    [Fact(DisplayName = "UT-F003-014 — Um período de 32 dias é recusado")]
    public void PeriodoDe32DiasERecusado()
    {
        Assert.Equal(
            ValidadorPeriodoDisponibilidade.MensagemPeriodoDemasiadoLongo,
            ValidadorPeriodoDisponibilidade.ObterErro(new DateOnly(2026, 10, 1), new DateOnly(2026, 11, 1)));
    }

    [Fact(DisplayName = "UT-F003-015 — Uma data de fim anterior à de início é recusada")]
    public void DataFimAnteriorERecusada()
    {
        Assert.Equal(
            ValidadorPeriodoDisponibilidade.MensagemDataFimAnterior,
            ValidadorPeriodoDisponibilidade.ObterErro(new DateOnly(2026, 10, 15), new DateOnly(2026, 10, 14)));
    }

    [Fact(DisplayName = "UT-F003-016 — Sem data de início o pedido é recusado (obrigatória)")]
    public void SemDataInicioERecusado()
    {
        Assert.Equal(
            ValidadorPeriodoDisponibilidade.MensagemDataInicioObrigatoria,
            ValidadorPeriodoDisponibilidade.ObterErro(null, new DateOnly(2026, 10, 15)));
    }

    [Fact(DisplayName = "I071 — Sem data de fim, o período é só o dia de início")]
    public void SemDataFimOPeriodoEUmDia()
    {
        var inicio = new DateOnly(2026, 10, 15);

        Assert.Null(ValidadorPeriodoDisponibilidade.ObterErro(inicio, null));
        Assert.Equal(inicio, ValidadorPeriodoDisponibilidade.DataFimEfetiva(inicio, null));
    }

    [Theory(DisplayName = "I071 — Datas fora de 2000-01-01 a 9998-12-31 são recusadas")]
    [InlineData(1999, 12, 31)]
    [InlineData(9999, 1, 1)]
    [InlineData(1, 1, 1)]
    public void DatasForaDosLimitesSaoRecusadas(int ano, int mes, int dia)
    {
        var data = new DateOnly(ano, mes, dia);

        Assert.Equal(ValidadorPeriodoDisponibilidade.MensagemForaDosLimites, ValidadorPeriodoDisponibilidade.ObterErro(data, null));
    }

    [Fact(DisplayName = "I071 — No pedido, sem dataInicio o [ApiController] responde 400 com a mensagem em português")]
    public void PedidoSemDataInicioTemErro()
    {
        var erro = Assert.Single(Validar(new DisponibilidadeQuery()));

        Assert.Equal(ValidadorPeriodoDisponibilidade.MensagemDataInicioObrigatoria, erro.ErrorMessage);
    }

    [Fact(DisplayName = "I071 — No pedido, um período de 32 dias tem erro no campo dataFim")]
    public void PedidoComPeriodoLongoTemErro()
    {
        var erro = Assert.Single(Validar(new DisponibilidadeQuery
        {
            DataInicio = new DateOnly(2026, 10, 1),
            DataFim = new DateOnly(2026, 11, 1)
        }));

        Assert.Equal(ValidadorPeriodoDisponibilidade.MensagemPeriodoDemasiadoLongo, erro.ErrorMessage);
        Assert.Contains(nameof(DisponibilidadeQuery.DataFim), erro.MemberNames);
    }

    [Fact(DisplayName = "I071 — No pedido, um período válido não tem erros")]
    public void PedidoValidoNaoTemErros()
    {
        Assert.Empty(Validar(new DisponibilidadeQuery { DataInicio = new DateOnly(2026, 10, 14) }));
    }

    private static List<ValidationResult> Validar(DisponibilidadeQuery query)
    {
        var resultados = new List<ValidationResult>();
        _ = Validator.TryValidateObject(query, new ValidationContext(query), resultados, validateAllProperties: true);
        return resultados;
    }
}
