using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.DTOs.Animais;

namespace ClinicaVeterinaria.UnitTests.Validacao;

/// <summary>
/// I086 — limites da paginação: casos UT-TRV-003 e UT-TRV-004 da Especificação de Testes Unitários
/// (I028, secção 7.6.1), com os parâmetros de EP-015 (<see cref="AnimalFiltroQuery"/>). Não há uma
/// classe PaginacaoQuery comum: cada filtro tem os mesmos limites (UT-TRV-001 e 002 estão nos
/// testes de ConsultaFiltroQuery e de UtilizadoresService, e UT-TRV-005 nos de ClientesService).
/// </summary>
public class PaginacaoTests
{
    [Fact(DisplayName = "UT-TRV-003 — tamanho = 101 é recusado (400)")]
    public void TamanhoAcimaDoMaximoEInvalido()
    {
        Assert.Contains(nameof(AnimalFiltroQuery.Tamanho), CamposComErro(new AnimalFiltroQuery { Tamanho = 101 }));
    }

    [Theory(DisplayName = "UT-TRV-004 — tamanho = 0 ou pagina = 0 são recusados (400)")]
    [InlineData(1, 0, nameof(AnimalFiltroQuery.Tamanho))]
    [InlineData(0, 20, nameof(AnimalFiltroQuery.Pagina))]
    public void PaginaOuTamanhoZeroEInvalido(int pagina, int tamanho, string campo)
    {
        Assert.Contains(campo, CamposComErro(new AnimalFiltroQuery { Pagina = pagina, Tamanho = tamanho }));
    }

    private static List<string> CamposComErro(object pedido)
    {
        var erros = new List<ValidationResult>();
        Validator.TryValidateObject(pedido, new ValidationContext(pedido), erros, validateAllProperties: true);
        return erros.SelectMany(e => e.MemberNames).ToList();
    }
}
