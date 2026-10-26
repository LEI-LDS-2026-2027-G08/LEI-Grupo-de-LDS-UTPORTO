namespace ClinicaVeterinaria.Application.Seguranca;

/// <summary>
/// Estados de uma conta, conforme a enumeração <c>EstadoUtilizador</c>
/// e a restrição CK_Utilizador_Estado.
/// </summary>
public static class EstadosUtilizador
{
    public const string Ativo = "Ativo";
    public const string Bloqueado = "Bloqueado";
}
