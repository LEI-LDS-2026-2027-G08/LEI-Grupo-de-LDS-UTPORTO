using ClinicaVeterinaria.Application.Agenda;

namespace ClinicaVeterinaria.UnitTests.Agenda;

/// <summary>
/// I072 — versão da marcação como <c>ETag</c> (Documentação de API v02, secção 5.1), devolvida por
/// EP-024. Caso UT-F003-085 da Especificação de Testes Unitários (I028, secção 7.3.5); a comparação
/// com <c>If-Match</c> (UT-F003-086 a 090) é da I074.
/// </summary>
public class VersaoMarcacaoTests
{
    [Fact(DisplayName = "UT-F003-085 — RowVersion 00 00 00 00 00 00 07 D1 dá o ETag \"AAAAAAAAB9E=\"")]
    public void RowVersionEmBase64EntreAspas()
    {
        Assert.True(false); // falha provocada (I102)
        var rowVersion = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0xD1 };

        Assert.Equal("\"AAAAAAAAB9E=\"", VersaoMarcacao.ParaETag(rowVersion));
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
