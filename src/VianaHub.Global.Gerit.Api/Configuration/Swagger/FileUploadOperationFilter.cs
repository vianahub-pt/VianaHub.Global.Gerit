#nullable enable

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VianaHub.Global.Gerit.Api.Configuration.Swagger;

/// <summary>
/// Operation filter para suportar upload de arquivos no Swagger
/// </summary>
public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParameters = context.ApiDescription.ParameterDescriptions
            .Where(p => p.ModelMetadata?.ModelType == typeof(IFormFile) || 
                       p.ModelMetadata?.ModelType == typeof(IFormFile[]))
            .ToList();

        if (!fileParameters.Any())
            return;

        operation.RequestBody = new OpenApiRequestBody
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = fileParameters.ToDictionary(
                            p => p.Name,
                            p => new OpenApiSchema
                            {
                                Type = "string",
                                Format = "binary"
                            }
                        ),
                        Required = fileParameters
                            .Where(p => p.IsRequired)
                            .Select(p => p.Name)
                            .ToHashSet()
                    }
                }
            }
        };

        // Remove os parâmetros de arquivo da lista de parâmetros normais
        foreach (var param in fileParameters)
        {
            var paramToRemove = operation.Parameters
                .FirstOrDefault(p => p.Name == param.Name);
            
            if (paramToRemove != null)
            {
                operation.Parameters.Remove(paramToRemove);
            }
        }
    }
}
