namespace ClinicaVeterinaria.Application.Exceptions;

public sealed class NaoEncontradoException : Exception
{
    public NaoEncontradoException(string message)
        : base(message)
    {
    }
}
