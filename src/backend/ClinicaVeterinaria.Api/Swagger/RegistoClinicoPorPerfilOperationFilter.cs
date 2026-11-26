using ClinicaVeterinaria.Api.Controllers;
using ClinicaVeterinaria.Api.DTOs.RegistosClinicos;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ClinicaVeterinaria.Api.Swagger;

/// <summary>Documenta as duas respostas de leitura, escolhidas pelo perfil autenticado.</summary>
public sealed class RegistoClinicoPorPerfilOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.DeclaringType != typeof(RegistosClinicosController))
        {
            return;
        }

        var tipos = context.MethodInfo.Name switch
        {
            nameof(RegistosClinicosController.Obter) => new[] { typeof(RegistoClinicoDto), typeof(RegistoClinicoAuxiliarDto) },
            nameof(RegistosClinicosController.ObterHistorico) => new[] { typeof(HistoricoClinicoDto), typeof(HistoricoClinicoAuxiliarDto) },
            _ => [],
        };
        if (tipos.Length == 0 || !operation.Responses.TryGetValue("200", out var resposta))
        {
            return;
        }

        resposta.Description = "Veterinário: resposta completa. Auxiliar: resposta reduzida, sem notas clínicas nem diagnóstico.";
        foreach (var conteudo in resposta.Content.Values)
        {
            conteudo.Schema = new OpenApiSchema
            {
                AnyOf = tipos.Select(tipo => context.SchemaGenerator.GenerateSchema(tipo, context.SchemaRepository)).ToList(),
            };
        }
    }
}
