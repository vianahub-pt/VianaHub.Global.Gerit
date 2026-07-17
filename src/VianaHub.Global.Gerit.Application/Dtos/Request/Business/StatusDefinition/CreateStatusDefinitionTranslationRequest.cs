namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.StatusDefinition;

/// <summary>
/// DTO para criação de uma tradução de StatusDefinition.
/// </summary>
public class CreateStatusDefinitionTranslationRequest
{
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
