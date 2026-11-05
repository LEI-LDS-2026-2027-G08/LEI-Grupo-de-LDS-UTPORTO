using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.DTOs.Utilizadores;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Application.Validacao;

namespace ClinicaVeterinaria.UnitTests.Validacao;

/// <summary>
/// Regra R-PWD (I065): casos UT-F001-050 a UT-F001-059 da Especificação de Testes Unitários
/// (I028, secção 7.1.5), identificados na I086. A regra está em <see cref="PalavraPasseForteAttribute"/>;
/// a obrigatoriedade (caso 059) vem do <c>[Required]</c> do <see cref="CreateUtilizadorDto"/>.
/// </summary>
public class PalavraPasseForteAttributeTests
{
    [Fact(DisplayName = "UT-F001-050 — \"Temp#2026\" (exemplo da API) é uma palavra-passe válida")]
    public void ExemploDaApiEValido()
    {
        Assert.True(PalavraPasseForteAttribute.EForte("Temp#2026"));
    }

    [Fact(DisplayName = "UT-F001-051 — Uma palavra-passe com 8 caracteres e as quatro classes é válida")]
    public void OitoCaracteresEValido()
    {
        Assert.True(PalavraPasseForteAttribute.EForte("Abcde#1x"));
    }

    [Fact(DisplayName = "UT-F001-052 — Uma palavra-passe com 7 caracteres é inválida")]
    public void SeteCaracteresEInvalido()
    {
        Assert.False(PalavraPasseForteAttribute.EForte("Abcd#1x"));
    }

    [Fact(DisplayName = "UT-F001-053 — Uma palavra-passe com 128 caracteres e as quatro classes é válida")]
    public void CentoEVinteEOitoCaracteresEValido()
    {
        Assert.True(PalavraPasseForteAttribute.EForte(ComComprimento(128)));
    }

    [Fact(DisplayName = "UT-F001-054 — Uma palavra-passe com 129 caracteres é inválida (máximo 128)")]
    public void CentoEVinteENoveCaracteresEInvalido()
    {
        Assert.False(PalavraPasseForteAttribute.EForte(ComComprimento(129)));
    }

    [Fact(DisplayName = "UT-F001-055 — Sem letra maiúscula, a palavra-passe é inválida")]
    public void SemMaiusculaEInvalido()
    {
        Assert.False(PalavraPasseForteAttribute.EForte("temp#2026"));
    }

    [Fact(DisplayName = "UT-F001-056 — Sem letra minúscula, a palavra-passe é inválida")]
    public void SemMinusculaEInvalido()
    {
        Assert.False(PalavraPasseForteAttribute.EForte("TEMP#2026"));
    }

    [Fact(DisplayName = "UT-F001-057 — Sem algarismo, a palavra-passe é inválida")]
    public void SemAlgarismoEInvalido()
    {
        Assert.False(PalavraPasseForteAttribute.EForte("Temp#abcd"));
    }

    [Fact(DisplayName = "UT-F001-058 — Sem símbolo, a palavra-passe é inválida")]
    public void SemSimboloEInvalido()
    {
        Assert.False(PalavraPasseForteAttribute.EForte("Temp2026a"));
    }

    [Theory(DisplayName = "UT-F001-059 — Sem palavra-passe inicial, o pedido de EP-006 é inválido (obrigatória)")]
    [InlineData(null)]
    [InlineData("")]
    public void PalavraPasseEmFaltaEInvalida(string? palavraPasse)
    {
        var pedido = new CreateUtilizadorDto
        {
            Nome = "Rui Lopes",
            Email = "rui.lopes@clinica-g08.test",
            Telefone = "934567890",
            Perfil = Perfis.Rececionista,
            PasswordInicial = palavraPasse!
        };
        var erros = new List<ValidationResult>();

        Validator.TryValidateObject(pedido, new ValidationContext(pedido), erros, validateAllProperties: true);

        Assert.Contains(erros, e => e.MemberNames.Contains(nameof(CreateUtilizadorDto.PasswordInicial)));
    }

    [Fact(DisplayName = "I065 — O espaço não conta como símbolo")]
    public void EspacoNaoContaComoSimbolo()
    {
        Assert.False(PalavraPasseForteAttribute.EForte("Temp 2026"));
    }

    [Fact(DisplayName = "I065 — Uma palavra-passe forte pode ter espaços")]
    public void PalavraPasseForteComEspacosEValida()
    {
        Assert.True(PalavraPasseForteAttribute.EForte("Segura#2026 com espaços"));
    }

    [Fact(DisplayName = "I065 — No atributo, um valor vazio fica para o [Required]")]
    public void ValorVazioFicaParaORequired()
    {
        Assert.True(new PalavraPasseForteAttribute().IsValid(string.Empty));
    }

    /// <summary>Palavra-passe com as quatro classes de caracteres e o comprimento pedido.</summary>
    private static string ComComprimento(int comprimento) => "Aa1#" + new string('x', comprimento - 4);
}
