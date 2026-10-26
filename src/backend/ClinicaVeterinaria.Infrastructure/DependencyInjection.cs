using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ClinicaVeterinaria.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        // Formato PasswordHasher v3 (PBKDF2-HMAC-SHA512, 100 000 iterações),
        // o mesmo dos hashes dos dados de demonstração (I061).
        services.AddScoped<IPasswordHasher<Utilizador>, PasswordHasher<Utilizador>>();
        services.TryAddSingleton(TimeProvider.System);
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IClinicaService, ClinicaService>();
        return services;
    }
}
