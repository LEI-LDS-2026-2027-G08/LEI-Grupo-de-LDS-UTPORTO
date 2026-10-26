using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.UnitTests.Validacao;

public class PalavraPasseForteAttributeTests
{
    [Theory]
    [InlineData("Temp#2026")] // exemplo da Documentação de API (EP-006)
    [InlineData("Ab1!abcd")] // 8 caracteres
    [InlineData("Segura#2026 com espaços")]
    public void PalavraPasseComTodosOsTiposDeCaracteresEForte(string palavraPasse)
    {
        Assert.True(PalavraPasseForteAttribute.EForte(palavraPasse));
    }

    [Theory]
    [InlineData("Ab1!abc")] // 7 caracteres
    [InlineData("temp#2026")] // sem maiúscula
    [InlineData("TEMP#2026")] // sem minúscula
    [InlineData("Temp#abcd")] // sem algarismo
    [InlineData("Temp2026a")] // sem símbolo
    [InlineData("Temp 2026")] // o espaço não conta como símbolo
    [InlineData(null)]
    public void PalavraPasseSemAPoliticaEFraca(string? palavraPasse)
    {
        Assert.False(PalavraPasseForteAttribute.EForte(palavraPasse));
    }

    [Fact]
    public void PalavraPasseComMaisDe128CaracteresERejeitada()
    {
        Assert.False(PalavraPasseForteAttribute.EForte("Aa1!" + new string('x', 125)));
    }

    [Fact]
    public void ValorVazioFicaParaORequired()
    {
        Assert.True(new PalavraPasseForteAttribute().IsValid(string.Empty));
    }
}
