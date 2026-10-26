using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.UnitTests.Validacao;

public class NifValidoAttributeTests
{
    [Theory]
    [InlineData("501234560")] // NIF da clínica nos dados de referência
    [InlineData("123456789")]
    [InlineData("234567899")]
    [InlineData("999999990")]
    public void NifComDigitoDeControloCorretoEValido(string nif)
    {
        Assert.True(NifValidoAttribute.EValido(nif));
    }

    [Theory]
    [InlineData("123456780")] // dígito de controlo errado
    [InlineData("12345678")] // 8 dígitos
    [InlineData("1234567890")] // 10 dígitos
    [InlineData("12345678A")]
    [InlineData("")]
    [InlineData(null)]
    public void NifComFormatoOuControloErradoEInvalido(string? nif)
    {
        Assert.False(NifValidoAttribute.EValido(nif));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(" 123456789 ")]
    public void NifVazioOuComEspacosEAceiteNoPedido(string? valor)
    {
        Assert.True(new NifValidoAttribute().IsValid(valor));
    }

    [Fact]
    public void NifInvalidoERejeitadoNoPedido()
    {
        Assert.False(new NifValidoAttribute().IsValid("123456780"));
    }
}
