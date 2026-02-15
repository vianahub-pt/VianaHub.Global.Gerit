namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Status;

/// <summary>
/// Request para criar um novo Status de Intervenção
/// </summary>
public class CreateStatusRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}
