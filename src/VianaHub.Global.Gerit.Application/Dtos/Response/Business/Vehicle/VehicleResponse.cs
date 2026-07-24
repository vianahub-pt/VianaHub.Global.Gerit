namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Vehicle;

public class VehicleResponse
{
    public int Id { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? Plate { get; set; }
    public int Year { get; set; }
    public bool IsActive { get; set; }
}
