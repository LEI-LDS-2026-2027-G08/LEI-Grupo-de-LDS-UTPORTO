using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.DTOs.Vacinacoes;

namespace ClinicaVeterinaria.UnitTests.Vacinacao;

/// <summary>
/// I082 — validação dos pedidos de EP-042 e EP-043 (e da atualização do RF056) tal como a faz o
/// <c>[ApiController]</c> antes de chamar o controlador: um erro dá 400. A data futura depende do
/// relógio e é verificada no serviço (<c>VacinacoesServiceTests</c>).
/// </summary>
public class AdministracaoDtoValidacaoTests
{
    [Fact(DisplayName = "I082 — o pedido de exemplo da API (EP-043) é válido")]
    public void PedidoDeExemploEValido()
    {
        Assert.Empty(Validar(Pedido()));
    }

    [Theory(DisplayName = "UT-F006-010 — tipo, produto, lote e data de administração são obrigatórios")]
    [InlineData("tipo")]
    [InlineData("produto")]
    [InlineData("lote")]
    [InlineData("dataAdministracao")]
    public void CamposObrigatorios(string campo)
    {
        var pedido = Pedido();

        switch (campo)
        {
            case "tipo":
                pedido.Tipo = "";
                break;
            case "produto":
                pedido.Produto = " ";
                break;
            case "lote":
                pedido.Lote = "";
                break;
            default:
                pedido.DataAdministracao = null;
                break;
        }

        Assert.Single(Validar(pedido));
    }

    [Theory(DisplayName = "I082 — tipo só aceita Vacina ou Desparasitacao")]
    [InlineData("Vacina", true)]
    [InlineData("Desparasitacao", true)]
    [InlineData("Vacinas", false)]
    [InlineData("vacina", false)]
    [InlineData("Desparasitação", false)]
    public void TipoValido(string tipo, bool valido)
    {
        var pedido = Pedido();
        pedido.Tipo = tipo;

        Assert.Equal(valido, Validar(pedido).Count == 0);
    }

    [Theory(DisplayName = "UT-F006-013 a 016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres")]
    [InlineData(null, 50, 150, true)]
    [InlineData(1, 50, 150, true)]
    [InlineData(32767, 50, 150, true)]
    [InlineData(0, 50, 150, false)]
    [InlineData(-30, 50, 150, false)]
    [InlineData(32768, 50, 150, false)]
    [InlineData(90, 51, 150, false)]
    [InlineData(90, 50, 151, false)]
    public void LimitesDosCampos(int? intervaloDias, int tamanhoLote, int tamanhoProduto, bool valido)
    {
        var pedido = Pedido();
        pedido.IntervaloDias = intervaloDias;
        pedido.Lote = new string('L', tamanhoLote);
        pedido.Produto = new string('P', tamanhoProduto);

        Assert.Equal(valido, Validar(pedido).Count == 0);
    }

    [Theory(DisplayName = "UT-F006-020 — BoletimQuery: tipo opcional, só Vacina ou Desparasitacao")]
    [InlineData(null, true)]
    [InlineData("Vacina", true)]
    [InlineData("Desparasitacao", true)]
    [InlineData("Vacinas", false)]
    public void TipoDoBoletim(string? tipo, bool valido)
    {
        var query = new BoletimQuery { Tipo = tipo };

        Assert.Equal(valido, Validar(query).Count == 0);
    }

    private static CreateAdministracaoDto Pedido() => new()
    {
        Tipo = "Desparasitacao",
        Produto = "Desparasitante interno Y",
        Lote = "DP-0042",
        DataAdministracao = new DateOnly(2026, 10, 14),
        IntervaloDias = 90
    };

    private static List<ValidationResult> Validar(object objeto)
    {
        var resultados = new List<ValidationResult>();
        _ = Validator.TryValidateObject(objeto, new ValidationContext(objeto), resultados, validateAllProperties: true);

        return resultados;
    }
}
