namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.StatusDefinition;

/// <summary>
/// Resposta de uma tradução de StatusDefinition.
/// </summary>
public class StatusDefinitionTranslationResponse
{
    public int Id { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
