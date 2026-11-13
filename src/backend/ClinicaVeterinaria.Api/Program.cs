using ClinicaVeterinaria.Api.Middleware;
using ClinicaVeterinaria.Api.Seguranca;
using ClinicaVeterinaria.Application;
using ClinicaVeterinaria.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var allowedOrigins =
    builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

// Add services to the container.
builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
        options.InvalidModelStateResponseFactory = RespostaValidacaoModelo.Criar);
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

// JWT (HS256, 8 horas) e políticas de autorização por perfil — I063.
// Falha no arranque se a secção Jwt estiver incompleta.
builder.Services.AddAutenticacaoJwt(builder.Configuration);

// Fim de sessão (EP-002) — I064: apaga periodicamente as revogações de "tokens" expirados.
builder.Services.AddLimpezaTokensRevogados(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        // Só as origens configuradas em Cors:AllowedOrigins; sem configuração.
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders(HeaderNames.ETag);
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduza o token JWT.",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
            },
            Array.Empty<string>()
        },
    });
});

builder.Services.AddDbContext<ClinicaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ClinicaDb")));

var app = builder.Build();

if (app.Configuration.GetValue<bool>("BaseDados:AplicarMigracoesNoArranque"))
{
    var avisoBaseIndisponivel = LoggerMessage.Define<int>(
        LogLevel.Warning,
        new EventId(1, "BaseDadosIndisponivel"),
        "Base de dados indisponível ao aplicar as migrações (tentativa {Tentativa}); nova tentativa em 15 s.");
    const int maxTentativas = 12;
    for (var tentativa = 1; tentativa <= maxTentativas; tentativa++)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            await scope.ServiceProvider.GetRequiredService<ClinicaDbContext>().Database.MigrateAsync();
            break;
        }
        catch (Exception ex) when (tentativa < maxTentativas && (ex is SqlException || ex.InnerException is SqlException))
        {
            avisoBaseIndisponivel(app.Logger, tentativa, ex);
            await Task.Delay(TimeSpan.FromSeconds(15));
        }
    }
}

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
// O Swagger fica ativo em Development e, com Swagger: Ativo = true, no ambiente
// público de avaliação (I104, critério 1: iniciar sessão pelo Swagger).
if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("Swagger:Ativo"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("FrontendPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
