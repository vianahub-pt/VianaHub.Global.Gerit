namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientIndividual;

public class CreateClientIndividualRequest
{
    public int ClientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string CellPhoneNumber { get; set; }
    public bool IsWhatsapp { get; set; }
    public string Email { get; set; }
    public DateTime? BirthDate { get; set; }
    public string Gender { get; set; }
    public string DocumentType { get; set; }
    public string DocumentNumber { get; set; }
    public string Nationality { get; set; }
}
