namespace ClinicaVeterinaria.Application.Exceptions;

/// <summary>
/// Exceção usada quando uma pré-condição HTTP falha,
/// nomeadamente quando o cabeçalho If-Match não corresponde
/// à versão atual da marcação.
/// </summary>
public sealed class PreconditionFailedException : Exception
{
    public PreconditionFailedException(string message)
        : base(message)
    {
    }

    public PreconditionFailedException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {
    }
}
