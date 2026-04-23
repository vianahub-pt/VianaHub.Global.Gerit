namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientConsents;

public class UpdateClientConsentsRequest
{
    public bool Granted { get; set; }
    public DateTime? RevokedDate { get; set; }
}
