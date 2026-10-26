using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.DTOs.Clinica;

namespace ClinicaVeterinaria.UnitTests.Validacao;

public class TelefoneClinicaTests
{
    private static bool TelefoneValido(string telefone)
    {
        var pedido = new UpdateClinicaDto
        {
            Nome = "Clínica",
            Nif = "501234560",
            Morada = "Rua do Exemplo, 100",
            Telefone = telefone,
            Email = "geral@clinica.test",
            DuracaoConsultaMinutos = 30,
            HorarioFuncionamento = []
        };
        return Validator.TryValidateObject(pedido, new ValidationContext(pedido), null, validateAllProperties: true);
    }

    [Theory]
    [InlineData("229000000")] // 9 caracteres
    [InlineData("+351 229 000 000")]
    [InlineData("12345678901234567890")] // 20 dígitos
    [InlineData("+1234567890123456789")] // + e 19 dígitos = 20 caracteres
    public void TelefoneAte20CaracteresEAceite(string telefone)
    {
        Assert.True(TelefoneValido(telefone));
    }

    [Theory]
    [InlineData("+12345678901234567890")] // + e 20 dígitos = 21 caracteres (coluna tem 20)
    [InlineData("123456789012345678901")] // 21 dígitos
    [InlineData("22900000")] // 8 caracteres
    [InlineData("22900000a")]
    [InlineData("22+9000000")] // + fora do início
    public void TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(string telefone)
    {
        Assert.False(TelefoneValido(telefone));
    }
}
