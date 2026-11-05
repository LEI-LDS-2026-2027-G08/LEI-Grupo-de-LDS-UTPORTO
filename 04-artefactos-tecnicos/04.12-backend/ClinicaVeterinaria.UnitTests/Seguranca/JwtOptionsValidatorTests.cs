using ClinicaVeterinaria.Api.Seguranca;
using Microsoft.Extensions.Options;

namespace ClinicaVeterinaria.UnitTests.Seguranca;

/// <summary>
/// I086 — validação da secção <c>Jwt</c> no arranque da API (<see cref="JwtOptionsValidator"/>):
/// casos UT-F001-035 a UT-F001-039 da Especificação de Testes Unitários (I028, secção 7.1.3).
/// </summary>
public class JwtOptionsValidatorTests
{
    [Fact(DisplayName = "UT-F001-035 — Sem chave, a validação falha e indica Jwt:Key e os User Secrets")]
    public void ChaveVaziaFalhaEIndicaOsUserSecrets()
    {
        var resultado = Validar(Opcoes(string.Empty));

        Assert.True(resultado.Failed);
        Assert.Contains("Jwt:Key", resultado.FailureMessage, StringComparison.Ordinal);
        Assert.Contains("User Secrets", resultado.FailureMessage, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "UT-F001-036 — Uma chave com 31 bytes falha: o mínimo é 32 bytes (256 bits)")]
    public void ChaveCom31BytesFalha()
    {
        var resultado = Validar(Opcoes(ChaveComBytes(31)));

        Assert.True(resultado.Failed);
        Assert.Contains("32 bytes", resultado.FailureMessage, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "UT-F001-037 — Chave com 32 bytes, Issuer, Audience e validade de 8 horas: a configuração é aceite")]
    public void ConfiguracaoCompletaEAceite()
    {
        var resultado = Validar(Opcoes(ChaveComBytes(32)));

        Assert.True(resultado.Succeeded);
    }

    [Fact(DisplayName = "UT-F001-038 — Uma chave que não está em Base64 falha com mensagem sobre Base64")]
    public void ChaveForaDeBase64Falha()
    {
        var resultado = Validar(Opcoes("isto não é base64"));

        Assert.True(resultado.Failed);
        Assert.Contains("Base64", resultado.FailureMessage, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "UT-F001-039 — Uma validade de 0 horas falha")]
    public void ValidadeZeroFalha()
    {
        var opcoes = Opcoes(ChaveComBytes(32));
        opcoes.ExpirationHours = 0;

        var resultado = Validar(opcoes);

        Assert.True(resultado.Failed);
    }

    private static ValidateOptionsResult Validar(JwtOptions opcoes) =>
        new JwtOptionsValidator().Validate(Options.DefaultName, opcoes);

    private static JwtOptions Opcoes(string chave) => AjudaSeguranca.CriarOpcoes(chave);

    private static string ChaveComBytes(int tamanho) => Convert.ToBase64String(new byte[tamanho]);
}
