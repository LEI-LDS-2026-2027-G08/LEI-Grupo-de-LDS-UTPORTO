using Microsoft.Extensions.Options;

namespace ClinicaVeterinaria.Api.Seguranca;

/// <summary>
/// Validação da secção <c>Jwt</c> no arranque da API (ValidateOnStart).
/// Não corre durante o <c>dotnet ef</c>, que constrói a aplicação sem a iniciar.
/// </summary>
public sealed class JwtOptionsValidator : IValidateOptions<JwtOptions>
{
    public ValidateOptionsResult Validate(string? name, JwtOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var erros = options.ValidarConfiguracao();

        return erros.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(erros);
    }
}
