namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientIndividual;

public class ClientIndividualResponse
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string ClientName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string CellPhoneNumber { get; set; }
    public bool IsWhatsapp { get; set; }
    public string Email { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? Age { get; set; }
    public string Gender { get; set; }
    public string DocumentType { get; set; }
    public string DocumentNumber { get; set; }
    public string Nationality { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
