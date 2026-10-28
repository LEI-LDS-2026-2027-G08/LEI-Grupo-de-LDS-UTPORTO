using ClinicaVeterinaria.Application.Agenda;

namespace ClinicaVeterinaria.UnitTests.Agenda;

/// <summary>
/// I072/I074 — versão da marcação como ETag e comparação forte de If-Match.
/// Casos UT-F003-085 a UT-F003-090 da Especificação de Testes Unitários.
/// </summary>
public class VersaoMarcacaoTests
{
    [Fact(DisplayName = "UT-F003-085 — RowVersion 00 00 00 00 00 00 07 D1 dá o ETag \"AAAAAAAAB9E=\"")]
    public void RowVersionEmBase64EntreAspas()
    {
        var rowVersion = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0xD1 };

        Assert.Equal("\"AAAAAAAAB9E=\"", VersaoMarcacao.ParaETag(rowVersion));
    }
    [Fact(DisplayName = "UT-F003-086 — If-Match com a versão atual corresponde")]
    public void IfMatchComVersaoAtualCorresponde()
    {
        var rowVersion = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0xD1 };

        Assert.True(
            VersaoMarcacao.CorrespondeIfMatch(
                rowVersion,
                "\"AAAAAAAAB9E=\""));
    }

    [Fact(DisplayName = "UT-F003-087 — If-Match com versão diferente não corresponde")]
    public void IfMatchComVersaoDiferenteNaoCorresponde()
    {
        var rowVersion = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0xD1 };

        Assert.False(
            VersaoMarcacao.CorrespondeIfMatch(
                rowVersion,
                "\"AAAAAAAAB9I=\""));
    }

    [Fact(DisplayName = "UT-F003-088 — If-Match fraco não corresponde")]
    public void IfMatchFracoNaoCorresponde()
    {
        var rowVersion = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0xD1 };

        Assert.False(
            VersaoMarcacao.CorrespondeIfMatch(
                rowVersion,
                "W/\"AAAAAAAAB9E=\""));
    }

    [Fact(DisplayName = "UT-F003-089 — If-Match asterisco corresponde a recurso existente")]
    public void IfMatchAsteriscoCorresponde()
    {
        var rowVersion = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0xD1 };

        Assert.True(
            VersaoMarcacao.CorrespondeIfMatch(
                rowVersion,
                "*"));
    }

    [Fact(DisplayName = "UT-F003-090 — If-Match sem aspas não corresponde")]
    public void IfMatchSemAspasNaoCorresponde()
    {
        var rowVersion = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0xD1 };

        Assert.False(
            VersaoMarcacao.CorrespondeIfMatch(
                rowVersion,
                "AAAAAAAAB9E="));
    }

    [Fact(DisplayName = "I074 — Sem If-Match a operação não fica condicionada à versão")]
    public void SemIfMatchCorresponde()
    {
        var rowVersion = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0xD1 };

        Assert.True(
            VersaoMarcacao.CorrespondeIfMatch(
                rowVersion,
                null));
    }

    [Fact(DisplayName = "I072 — versões diferentes dão ETags diferentes")]
    public void VersoesDiferentesDaoETagsDiferentes()
    {
        var v1 = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0xD1 };
        var v2 = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0xD2 };

        Assert.NotEqual(VersaoMarcacao.ParaETag(v1), VersaoMarcacao.ParaETag(v2));
    }

    [Fact(DisplayName = "I072 — sem versão (null) é um erro de programação")]
    public void SemVersaoLancaExcecao()
    {
        Assert.Throws<ArgumentNullException>(() => VersaoMarcacao.ParaETag(null!));
    }
}
