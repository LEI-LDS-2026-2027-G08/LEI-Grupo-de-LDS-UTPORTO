using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Api.DTOs.Clientes;
using ClinicaVeterinaria.Application.DTOs.Clinica;

namespace ClinicaVeterinaria.UnitTests.Validacao;

/// <summary>
/// Regra única de e-mail: os pedidos de Clientes (EP-013 e EP-014) e da clínica (EP-010)
/// usam a mesma regra R-EMAIL (<c>[EmailValido]</c>) das contas de funcionário (I028).
/// </summary>
public class EmailClienteEClinicaTests
{
    private static bool EmailClienteValido(string email)
    {
        var pedido = new UpdateClienteDto
        {
            Nome = "Ana Pereira",
            Telefone = "912345678",
            Email = email,
        };
        return Validator.TryValidateObject(pedido, new ValidationContext(pedido), null, validateAllProperties: true);
    }

    private static bool EmailClinicaValido(string email)
    {
        var pedido = new UpdateClinicaDto
        {
            Nome = "Clínica",
            Nif = "501234560",
            Morada = "Rua do Exemplo, 100",
            Telefone = "229000000",
            Email = email,
            DuracaoConsultaMinutos = 30,
            HorarioFuncionamento = [],
        };
        return Validator.TryValidateObject(pedido, new ValidationContext(pedido), null, validateAllProperties: true);
    }

    [Theory(DisplayName = "R-EMAIL — Cliente e clínica: um e-mail com a forma nome@dominio.pt é aceite")]
    [InlineData("joao.costa@mail.pt")]
    [InlineData("geral@clinica-g08.test")]
    public void EmailValidoEAceite(string email)
    {
        Assert.True(EmailClienteValido(email));
        Assert.True(EmailClinicaValido(email));
    }

    [Theory(DisplayName = "R-EMAIL — Cliente e clínica: um e-mail fora da regra é recusado, como nas contas de funcionário")]
    [InlineData("joao@mail")] // sem ponto no domínio: o [EmailAddress] do .NET aceitava-o
    [InlineData("joao.costa.mail.pt")]
    [InlineData("joao@@mail.pt")]
    [InlineData("joao costa@mail.pt")]
    [InlineData("joao@mail..pt")]
    public void EmailForaDaRegraERecusado(string email)
    {
        Assert.False(EmailClienteValido(email));
        Assert.False(EmailClinicaValido(email));
    }
}
