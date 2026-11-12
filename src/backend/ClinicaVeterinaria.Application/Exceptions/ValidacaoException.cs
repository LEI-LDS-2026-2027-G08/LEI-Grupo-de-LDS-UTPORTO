namespace ClinicaVeterinaria.Application.Exceptions;

public sealed class ValidacaoException : Exception
{
    public ValidacaoException(string message)
        : base(message)
    {
    }
}
