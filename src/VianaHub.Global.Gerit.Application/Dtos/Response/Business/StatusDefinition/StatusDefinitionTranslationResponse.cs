namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.StatusDefinition;

/// <summary>
/// Resposta de tradução de uma Definição de Status por idioma.
/// </summary>
public class StatusDefinitionTranslationResponse
{
    public int Id { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
