namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.StatusDefinition;

/// <summary>
/// DTO para atualização de uma tradução de StatusDefinition.
/// </summary>
public class UpdateStatusDefinitionTranslationRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
