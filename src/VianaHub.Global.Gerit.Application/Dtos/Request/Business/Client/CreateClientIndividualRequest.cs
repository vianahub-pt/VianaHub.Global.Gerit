namespace VianaHub.Global.Gerit.Application.Dtos.Business.Client;

public class CreateClientIndividualRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public string? Gender { get; set; }
    public string? DocumentType { get; set; }
    public string? DocumentNumber { get; set; }
    public string? Nationality { get; set; }
}
