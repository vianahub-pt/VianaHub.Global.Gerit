namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionStatus;

/// <summary>
/// Request para criar um novo Status de Intervenção
/// </summary>
public class CreateInterventionStatusRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}
