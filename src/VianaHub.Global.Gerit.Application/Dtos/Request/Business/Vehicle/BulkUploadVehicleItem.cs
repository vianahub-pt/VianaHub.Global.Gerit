namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Vehicle;

public class BulkUploadVehicleItem
{
    public int StatusDefinitionId { get; set; }
    public int StatusDomainId { get; set; }
    public string? Plate { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int Year { get; set; }
    public string? Color { get; set; }
    public string? FuelType { get; set; }
}
