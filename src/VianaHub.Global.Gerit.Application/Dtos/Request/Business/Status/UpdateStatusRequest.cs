namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Status;

/// <summary>
/// Request para atualizar um Status de Intervenção
/// </summary>
public class UpdateStatusRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}
