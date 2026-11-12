namespace ClinicaVeterinaria.Application.Exceptions;

public sealed class NaoAutenticadoException : Exception
{
    public NaoAutenticadoException(string message)
        : base(message)
    {
    }
}
