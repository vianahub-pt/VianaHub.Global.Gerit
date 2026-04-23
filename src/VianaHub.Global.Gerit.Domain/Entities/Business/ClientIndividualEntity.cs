using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

public class ClientIndividualEntity : Entity
{
    public int TenantId { get; private set; }
    public int ClientId { get; private set; }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PhoneNumber { get; private set; }
    public string CellPhoneNumber { get; private set; }
    public bool IsWhatsapp { get; private set; }
    public string Email { get; private set; }
    public DateTime BirthDate { get; private set; }
    public string Gender { get; private set; }
    public string DocumentType { get; private set; }
    public string DocumentNumber { get; private set; }
    public string Nationality { get; private set; }

    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation
    public ClientEntity Client { get; private set; } = null!;

    // EF
    protected ClientIndividualEntity() { }

    public ClientIndividualEntity(int tenantId, string firstName, string lastName, string phoneNumber, string cellPhoneNumber, bool isWhatsapp, string email, DateTime birthDate, string gender, string documentType, string documentNumber, string nationality, int createdBy)
    {
        TenantId = tenantId;
        ClientId = Id;

        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        CellPhoneNumber = cellPhoneNumber;
        IsWhatsapp = isWhatsapp;
        Email = email;
        BirthDate = birthDate;
        Gender = gender;
        DocumentType = documentType;
        DocumentNumber = documentNumber;
        Nationality = nationality;

        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public string DisplayName => $"{FirstName} {LastName}".Trim();

    public void Update(string firstName, string lastName, string phoneNumber, string cellPhoneNumber, bool isWhatsapp, string email, DateTime birthDate, string gender, string documentType, string documentNumber, string nationality, int modifiedBy)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        CellPhoneNumber = cellPhoneNumber;
        IsWhatsapp = isWhatsapp;
        Email = email;
        BirthDate = birthDate;
        Gender = gender;
        DocumentType = documentType;
        DocumentNumber = documentNumber;
        Nationality = nationality;

        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
    public void Activate(int modifiedBy)
    {
        IsActive = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Deactivate(int modifiedBy)
    {
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Delete(int modifiedBy)
    {
        IsActive = false;
        IsDeleted = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}