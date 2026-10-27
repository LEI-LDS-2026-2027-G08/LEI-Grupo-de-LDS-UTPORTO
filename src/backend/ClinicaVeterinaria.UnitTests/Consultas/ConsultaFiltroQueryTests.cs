using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.DTOs.Consultas;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.UnitTests.Consultas;

/// <summary>
/// I072 — validação dos parâmetros de EP-023 (<see cref="ConsultaFiltroQuery"/>), tal como a faz o
/// <c>[ApiController]</c> antes de chamar o controlador: um erro dá 400. Inclui UT-TRV-001 da
/// Especificação de Testes Unitários (I028, secção 7.7).
/// </summary>
public class ConsultaFiltroQueryTests
{
    [Fact(DisplayName = "UT-TRV-001 — sem pagina nem tamanho, pagina = 1 e tamanho = 20")]
    public void PaginacaoPorOmissao()
    {
        var filtro = new ConsultaFiltroQuery();

        Assert.Equal(1, filtro.Pagina);
        Assert.Equal(20, filtro.Tamanho);
        Assert.Empty(Validar(filtro));
    }

    [Fact(DisplayName = "I072 — todos os filtros são opcionais e um só dia (dataInicio = dataFim) é válido")]
    public void FiltrosOpcionaisEUmSoDia()
    {
        var dia = new DateOnly(2026, 10, 15);

        Assert.Empty(Validar(new ConsultaFiltroQuery { DataInicio = dia }));
        Assert.Empty(Validar(new ConsultaFiltroQuery { DataFim = dia }));
        Assert.Empty(Validar(new ConsultaFiltroQuery { DataInicio = dia, DataFim = dia }));
        Assert.Empty(Validar(new ConsultaFiltroQuery { DataInicio = dia, DataFim = dia.AddDays(400) }));
    }

    [Fact(DisplayName = "UT-F003-052 (validação do pedido) — dataFim anterior a dataInicio é inválida")]
    public void DataFimAnteriorInvalida()
    {
        var erros = Validar(new ConsultaFiltroQuery
        {
            DataInicio = new DateOnly(2026, 10, 15),
            DataFim = new DateOnly(2026, 10, 14)
        });

        Assert.Equal(ValidadorPeriodoDisponibilidade.MensagemDataFimAnterior, Assert.Single(erros));
    }

    [Theory(DisplayName = "I072 — datas fora de 2000-01-01 a 9998-12-31 são inválidas; os limites são aceites")]
    [InlineData(1999, 12, 31, false)]
    [InlineData(2000, 1, 1, true)]
    [InlineData(9998, 12, 31, true)]
    [InlineData(9999, 1, 1, false)]
    public void LimitesDasDatas(int ano, int mes, int dia, bool valida)
    {
        var data = new DateOnly(ano, mes, dia);

        var erros = Validar(new ConsultaFiltroQuery { DataInicio = data })
            .Concat(Validar(new ConsultaFiltroQuery { DataFim = data }))
            .ToList();

        if (valida)
        {
            Assert.Empty(erros);
        }
        else
        {
            Assert.Equal(2, erros.Count);
            Assert.All(erros, e => Assert.Equal(ValidadorPeriodoDisponibilidade.MensagemForaDosLimites, e));
        }
    }

    [Theory(DisplayName = "I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas")]
    [InlineData("Pendente", true)]
    [InlineData("confirmada", true)]
    [InlineData(" Realizada ", true)]
    [InlineData("CANCELADA", true)]
    [InlineData("", true)]
    [InlineData("Faturada", false)]
    [InlineData("Confirmadas", false)]
    public void Estado(string estado, bool valido)
    {
        var erros = Validar(new ConsultaFiltroQuery { Estado = estado });

        if (valido)
        {
            Assert.Empty(erros);
        }
        else
        {
            Assert.Equal(ValidadorFiltroConsultas.MensagemEstadoInvalido(estado.Trim()), Assert.Single(erros));
        }
    }

    [Theory(DisplayName = "UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1")]
    [InlineData(1, 1, true)]
    [InlineData(1, 100, true)]
    [InlineData(1, 101, false)]
    [InlineData(1, 0, false)]
    [InlineData(0, 20, false)]
    [InlineData(int.MaxValue, 20, true)]
    public void LimitesDaPaginacao(int pagina, int tamanho, bool valido)
    {
        var erros = Validar(new ConsultaFiltroQuery { Pagina = pagina, Tamanho = tamanho });

        Assert.Equal(valido, erros.Count == 0);
    }

    [Theory(DisplayName = "I072 — veterinarioId e animalId, quando indicados, têm de ser superiores a 0")]
    [InlineData(0, null)]
    [InlineData(-1, null)]
    [InlineData(null, 0)]
    public void IdentificadoresPositivos(int? veterinarioId, int? animalId)
    {
        var erros = Validar(new ConsultaFiltroQuery { VeterinarioId = veterinarioId, AnimalId = animalId });

        Assert.Single(erros);
    }

    private static List<string?> Validar(ConsultaFiltroQuery filtro)
    {
        var resultados = new List<ValidationResult>();
        _ = Validator.TryValidateObject(filtro, new ValidationContext(filtro), resultados, validateAllProperties: true);

        return resultados.Select(r => r.ErrorMessage).ToList();
    }
}
