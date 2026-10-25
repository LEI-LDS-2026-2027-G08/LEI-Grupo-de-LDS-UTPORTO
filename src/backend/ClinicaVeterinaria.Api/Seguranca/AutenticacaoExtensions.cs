using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ClinicaVeterinaria.Api.Seguranca;

/// <summary>
/// Autenticação JWT e políticas de autorização por perfil (I063).
/// </summary>
/// <remarks>
/// Todos os endpoints exigem autenticação por omissão (política de fallback).
/// Um endpoint público tem de ser marcado com <c>[AllowAnonymous]</c>.
/// </remarks>
public static class AutenticacaoExtensions
{
    public static IServiceCollection AddAutenticacaoJwt(
        this IServiceCollection services,
        IConfiguration configuracao)
    {
        ArgumentNullException.ThrowIfNull(configuracao);

        // A configuração só é validada quando a API arranca (ValidateOnStart),
        // para não impedir o dotnet ef de construir a aplicação sem a chave JWT.
        services
            .AddOptions<JwtOptions>()
            .Bind(configuracao.GetSection(JwtOptions.Seccao))
            .ValidateOnStart();
        services.AddSingleton<IValidateOptions<JwtOptions>, JwtOptionsValidator>();

        services.TryAddSingleton(TimeProvider.System);
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services
            .AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtOptions>>((options, jwt) => ConfigurarJwtBearer(options, jwt.Value));

        services.AddAuthorizationBuilder()
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build())
            .AddPolicy(PoliticasAutorizacao.Administrador, p => p.RequireRole(Perfis.Administrador))
            .AddPolicy(PoliticasAutorizacao.Rececionista, p => p.RequireRole(Perfis.Rececionista))
            .AddPolicy(PoliticasAutorizacao.Veterinario, p => p.RequireRole(Perfis.Veterinario))
            .AddPolicy(PoliticasAutorizacao.AuxiliarVeterinario, p => p.RequireRole(Perfis.AuxiliarVeterinario))
            .AddPolicy(PoliticasAutorizacao.Cliente, p => p.RequireRole(Perfis.Cliente))
            .AddPolicy(PoliticasAutorizacao.Funcionario, p => p.RequireRole(Perfis.ListaFuncionarios))
            .AddPolicy(PoliticasAutorizacao.ClienteProprietario, p => p
                .RequireRole(Perfis.Cliente)
                .RequireAssertion(contexto => contexto.User.ObterClienteId() is not null));

        return services;
    }

    private static void ConfigurarJwtBearer(JwtBearerOptions options, JwtOptions jwt)
    {
        // Mantém os nomes curtos das claims (sub, role, jti...) em vez de
        // os converter para os URIs do ClaimTypes.
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,

            ValidateAudience = true,
            ValidAudience = jwt.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = jwt.CriarChaveAssinatura(),
            ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
            RequireSignedTokens = true,

            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero,

            NameClaimType = ClaimsClinica.UtilizadorId,
            RoleClaimType = ClaimsClinica.Perfil
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = ValidarSessaoAsync,
            OnChallenge = ResponderNaoAutenticadoAsync,
            OnForbidden = ResponderSemPermissaoAsync
        };
    }

    /// <summary>
    /// Depois de validada a assinatura e a validade, confirma na base de dados
    /// que a conta continua ativa, que o perfil não mudou e que o token não foi revogado.
    /// </summary>
    private static async Task ValidarSessaoAsync(TokenValidatedContext contexto)
    {
        var principal = contexto.Principal;
        var utilizadorId = principal?.ObterUtilizadorId();
        var perfil = principal?.ObterPerfil();
        var jti = principal?.ObterJti();

        if (utilizadorId is null || string.IsNullOrEmpty(perfil) || string.IsNullOrEmpty(jti))
        {
            contexto.Fail("O token não contém as claims obrigatórias.");
            return;
        }

        var authService = contexto.HttpContext.RequestServices.GetRequiredService<IAuthService>();

        if (!await authService.SessaoValidaAsync(utilizadorId.Value, perfil, jti))
        {
            contexto.Fail("A sessão já não é válida.");
        }
    }

    private static Task ResponderNaoAutenticadoAsync(JwtBearerChallengeContext contexto)
    {
        contexto.HandleResponse();

        var detalhe = contexto.AuthenticateFailure switch
        {
            null => "É necessário apresentar um token JWT válido.",
            SecurityTokenExpiredException => "O token expirou. Inicie sessão novamente.",
            _ => "O token apresentado é inválido ou a sessão já não é válida."
        };

        return EscreverErroAsync(
            contexto.Response,
            StatusCodes.Status401Unauthorized,
            "Autenticação necessária.",
            detalhe);
    }

    private static Task ResponderSemPermissaoAsync(ForbiddenContext contexto) =>
        EscreverErroAsync(
            contexto.Response,
            StatusCodes.Status403Forbidden,
            "Não tem permissão para executar esta operação.",
            "O perfil do utilizador autenticado não tem acesso a esta operação.");

    // Mesmo formato do ExceptionHandlingMiddleware (I058).
    private static Task EscreverErroAsync(
        HttpResponse resposta,
        int codigo,
        string mensagem,
        string detalhe)
    {
        resposta.StatusCode = codigo;

        return resposta.WriteAsJsonAsync(new
        {
            code = codigo,
            message = mensagem,
            detail = detalhe
        });
    }
}
