using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.DTOs.Clientes;

namespace ClinicaVeterinaria.UnitTests.Validacao;

public class TelefoneClienteTests
{
    private static bool TelefoneValido(string telefone)
    {
        var pedido = new UpdateClienteDto { Nome = "Cliente", Telefone = telefone, Email = "cliente@mail.test" };
        return Validator.TryValidateObject(pedido, new ValidationContext(pedido), null, validateAllProperties: true);
    }

    [Theory]
    [InlineData("912345678")] // 9 caracteres
    [InlineData("+351 912 345 678")]
    [InlineData("12345678901234567890")] // 20 dígitos
    [InlineData("+1234567890123456789")] // + e 19 dígitos = 20 caracteres
    public void TelefoneAte20CaracteresEAceite(string telefone)
    {
        Assert.True(TelefoneValido(telefone));
    }

    [Theory]
    [InlineData("+12345678901234567890")] // + e 20 dígitos = 21 caracteres (coluna tem 20)
    [InlineData("123456789012345678901")] // 21 dígitos
    [InlineData("12345678")] // 8 caracteres
    [InlineData("91234567a")]
    [InlineData("91+2345678")] // + fora do início
    public void TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(string telefone)
    {
        Assert.False(TelefoneValido(telefone));
    }
}
