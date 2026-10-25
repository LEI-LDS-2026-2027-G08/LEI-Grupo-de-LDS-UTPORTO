using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        // Formato PasswordHasher v3 (PBKDF2-HMAC-SHA512, 100 000 iterações),
        // o mesmo dos hashes dos dados de demonstração (I061).
        services.AddScoped<IPasswordHasher<Utilizador>, PasswordHasher<Utilizador>>();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}
