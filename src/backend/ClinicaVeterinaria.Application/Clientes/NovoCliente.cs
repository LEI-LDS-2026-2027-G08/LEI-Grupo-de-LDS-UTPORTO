namespace ClinicaVeterinaria.Application.Clientes;

public sealed class NovoCliente(AlteracaoCliente dados, string passwordInicial)
{
    public AlteracaoCliente Dados { get; } = dados;

    public string PasswordInicial { get; } = passwordInicial;
}
