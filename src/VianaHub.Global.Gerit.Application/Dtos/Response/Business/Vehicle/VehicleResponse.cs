namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Vehicle;

public class VehicleResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string Plate { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public string FuelType { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
