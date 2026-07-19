namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.StatusDefinition;

/// <summary>
/// Resposta detalhada de uma Definição de Status (inclui traduções).
/// </summary>
public class StatusDefinitionDetailResponse : StatusDefinitionResponse
{
    public List<StatusDefinitionTranslationResponse>? Translations { get; set; }
}
