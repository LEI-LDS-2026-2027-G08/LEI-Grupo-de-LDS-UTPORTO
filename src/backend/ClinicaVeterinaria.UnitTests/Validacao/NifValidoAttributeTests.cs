using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.UnitTests.Validacao;

/// <summary>
/// Regra R-NIF (I067, I068): casos UT-F001-060 a UT-F001-066 da Especificação de Testes Unitários
/// (I028, secção 7.1.6), identificados na I086, e casos próprios do atributo no pedido.
/// </summary>
public class NifValidoAttributeTests
{
    [Fact(DisplayName = "UT-F001-060 — \"501234560\" (resto 1, dígito de controlo 0) é um NIF válido")]
    public void RestoUmDaControloZero()
    {
        Assert.True(NifValidoAttribute.EValido("501234560"));
    }

    [Fact(DisplayName = "UT-F001-061 — \"234567899\" (resto 2, dígito de controlo 9) é um NIF válido")]
    public void RestoDoisDaControloNove()
    {
        Assert.True(NifValidoAttribute.EValido("234567899"));
    }

    [Fact(DisplayName = "UT-F001-062 — \"501234567\" tem o dígito de controlo errado e é inválido")]
    public void DigitoDeControloErradoEInvalido()
    {
        Assert.False(NifValidoAttribute.EValido("501234567"));
    }

    [Fact(DisplayName = "UT-F001-063 — Um NIF com 8 algarismos é inválido")]
    public void OitoAlgarismosEInvalido()
    {
        Assert.False(NifValidoAttribute.EValido("50123456"));
    }

    [Fact(DisplayName = "UT-F001-064 — Um NIF com 10 algarismos é inválido")]
    public void DezAlgarismosEInvalido()
    {
        Assert.False(NifValidoAttribute.EValido("5012345600"));
    }

    [Fact(DisplayName = "UT-F001-065 — Um NIF com caracteres não numéricos é inválido")]
    public void CaracteresNaoNumericosEInvalido()
    {
        Assert.False(NifValidoAttribute.EValido("50123456A"));
    }

    [Fact(DisplayName = "UT-F001-066 — Um NIF com espaços no meio é inválido: não há normalização")]
    public void EspacosNoMeioEInvalido()
    {
        Assert.False(new NifValidoAttribute().IsValid("501 234 560"));
    }

    [Theory(DisplayName = "I067 — Outros NIF com o dígito de controlo correto são válidos")]
    [InlineData("123456789")]
    [InlineData("999999990")]
    public void OutrosNifValidos(string nif)
    {
        Assert.True(NifValidoAttribute.EValido(nif));
    }

    [Theory(DisplayName = "I067 — Sem valor, o atributo aceita: a obrigatoriedade fica para o [Required]")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void NifVazioEAceiteNoPedido(string? valor)
    {
        Assert.True(new NifValidoAttribute().IsValid(valor));
    }

    [Fact(DisplayName = "I067 — No pedido, os espaços à volta do NIF são ignorados")]
    public void EspacosAVoltaSaoIgnorados()
    {
        Assert.True(new NifValidoAttribute().IsValid(" 123456789 "));
    }
}
