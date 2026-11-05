using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.UnitTests.Validacao;

/// <summary>
/// I065 — regra R-EMAIL. Casos UT-F001-067 a UT-F001-072 da Especificação de Testes
/// Unitários (I028, secção 7.1.7); os restantes são próprios desta tarefa.
/// </summary>
public class EmailValidoAttributeTests
{
    private const string Dominio = "@clinica-g08.pt";

    [Fact(DisplayName = "UT-F001-067 — \"joao.costa@mail.pt\" é um e-mail válido")]
    public void EmailComPontoNoDominioEValido()
    {
        Assert.True(EmailValidoAttribute.EValido("joao.costa@mail.pt"));
    }

    [Fact(DisplayName = "UT-F001-068 — Um e-mail sem @ é inválido")]
    public void EmailSemArrobaEInvalido()
    {
        Assert.False(EmailValidoAttribute.EValido("joao.costa.mail.pt"));
    }

    [Fact(DisplayName = "UT-F001-069 — Um e-mail sem ponto no domínio é inválido")]
    public void EmailSemPontoNoDominioEInvalido()
    {
        Assert.False(EmailValidoAttribute.EValido("joao@mail"));
    }

    [Fact(DisplayName = "UT-F001-070 — Um e-mail com espaços é inválido")]
    public void EmailComEspacosEInvalido()
    {
        Assert.False(EmailValidoAttribute.EValido("joao costa@mail.pt"));
    }

    [Fact(DisplayName = "UT-F001-071 — Um e-mail válido com 256 caracteres é aceite")]
    public void EmailCom256CaracteresEValido()
    {
        var email = new string('a', 256 - Dominio.Length) + Dominio;

        Assert.Equal(256, email.Length);
        Assert.True(EmailValidoAttribute.EValido(email));
    }

    [Fact(DisplayName = "UT-F001-072 — Um e-mail com 257 caracteres é inválido")]
    public void EmailCom257CaracteresEInvalido()
    {
        var email = new string('a', 257 - Dominio.Length) + Dominio;

        Assert.Equal(257, email.Length);
        Assert.False(EmailValidoAttribute.EValido(email));
    }

    [Theory(DisplayName = "I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido")]
    [InlineData("@mail.pt")]
    [InlineData("joao@@mail.pt")]
    [InlineData("joao@costa@mail.pt")]
    [InlineData("joao@")]
    [InlineData("joao@.pt")]
    [InlineData("joao@mail.")]
    [InlineData("joao@mail..pt")]
    [InlineData(" joao@mail.pt")]
    [InlineData(null)]
    [InlineData("")]
    public void FormatoInvalidoERecusado(string? email)
    {
        Assert.False(EmailValidoAttribute.EValido(email));
    }

    [Theory(DisplayName = "I065 — No pedido, um valor vazio fica para o [Required]")]
    [InlineData(null)]
    [InlineData("")]
    public void ValorVazioFicaParaORequired(string? email)
    {
        Assert.True(new EmailValidoAttribute().IsValid(email));
    }
}
