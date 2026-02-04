namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Function;

public class FunctionResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}
