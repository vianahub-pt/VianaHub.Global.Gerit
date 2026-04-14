using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Enums;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

public class ClientEntity : Entity, IAggregateRoot
{
    private readonly List<ClientContactEntity> _contacts = [];
    private readonly List<ClientAddressEntity> _addresses = [];
    private readonly List<ClientConsentsEntity> _consents = [];
    private readonly List<ClientHierarchyEntity> _parentHierarchies = [];
    private readonly List<ClientHierarchyEntity> _childHierarchies = [];

    public int TenantId { get; private set; }
    public ClientTypeEntity ClientType { get; private set; }
    public Origin Origin { get; private set; }
    public string UrlImage { get; private set; }
    public string Notes { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Especializações do agregado
    public ClientIndividualEntity Individual { get; private set; }
    public ClientCompanyEntity Company { get; private set; }

    // Partes internas do agregado
    public IReadOnlyCollection<ClientContactEntity> Contacts => _contacts.AsReadOnly();
    public IReadOnlyCollection<ClientAddressEntity> Addresses => _addresses.AsReadOnly();
    public IReadOnlyCollection<ClientConsentsEntity> Consents => _consents.AsReadOnly();
    public IReadOnlyCollection<ClientHierarchyEntity> ParentHierarchies => _parentHierarchies.AsReadOnly();
    public IReadOnlyCollection<ClientHierarchyEntity> ChildHierarchies => _childHierarchies.AsReadOnly();

    // Navigation Properties
    public TenantEntity Tenant { get; private set; } = null!;

    // Construtor protegido para o EF Core
    protected ClientEntity() { }

    public ClientEntity(int tenantId, ClientTypeEntity clientType, Origin origin, string urlImage, string notes, int createdBy)
    {
        TenantId = tenantId;
        ClientType = clientType;
        Origin = origin;
        UrlImage = urlImage;
        Notes = notes;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddIndividual(int tenantId, int clientId, string firstName, string lastName, DateTime birthDate, string gender, string documentType, string documentNumber, string nationality, int createdBy)
    {
        Individual = new ClientIndividualEntity(tenantId, clientId, firstName, lastName, birthDate, gender, documentType, documentNumber, nationality, createdBy);
    }
    public void AddCompany(int tenantId, int clientId, string legalName, string tradeName, string site, string companyRegistration, string cae, int numberOfEmployee, string legalRepresentative, int createdBy)
    {
        Company = new ClientCompanyEntity(tenantId, clientId, legalName, tradeName, site, companyRegistration, cae, numberOfEmployee, legalRepresentative, createdBy);
    }
    public void AddContact(int tenantId, int clientId, string name, string phoneNumber, string cellPhoneNumber, bool isWhatsapp, string email, bool isPrimary, int createdBy)
    {
        if (_contacts.Any(x => x.TenantId == tenantId && x.ClientId == clientId && x.Name == name && x.Email == email))
            _contacts.Add(new ClientContactEntity(tenantId, clientId, name, phoneNumber, cellPhoneNumber, isWhatsapp, email, isPrimary, createdBy));
    }
    public void AddAddress(int tenantId, int clientId, int addressTypeId, string countryCode, string street, string streetNumber, string complement, string neighborhood, string city, string district, string postalCode, decimal? latitude, decimal? longitude, string notes, bool isPrimary, int createdBy)
    {
        if (_addresses.Any(x => x.TenantId == tenantId && x.ClientId == clientId && x.CountryCode == countryCode && x.Street == street && x.StreetNumber == streetNumber && x.Neighborhood == neighborhood && x.City == city && x.District == district))
            _addresses.Add(new ClientAddressEntity(tenantId, clientId, addressTypeId, countryCode, street, streetNumber, complement, neighborhood, city, district, postalCode, latitude, longitude, notes, isPrimary, createdBy));
    }
    public void AddConsent(int tenantId, int clientId, int consentTypeId, bool granted, DateTime grantedDate, string origin, string ipAddress, string userAgent, int createdBy)
    {
        if (_consents.Any(x => x.TenantId == tenantId && x.ClientId == clientId && x.ConsentTypeId == consentTypeId && x.Origin == origin && x.IpAddress == ipAddress))
            _consents.Add(new ClientConsentsEntity(tenantId, clientId, consentTypeId, granted, grantedDate, origin, ipAddress, userAgent, createdBy));
    }
    public void AddParentHierarchy(int tenantId, int parentClientId, int childClientId, int relationshipType, int createdBy)
    {
        if (_parentHierarchies.Any(x => x.TenantId == tenantId && x.ParentClientId == parentClientId && x.ChildClientId == childClientId && x.RelationshipType == relationshipType))
            _parentHierarchies.Add(new ClientHierarchyEntity(tenantId, parentClientId, childClientId, relationshipType, createdBy));
    }
    public void AddChildHierarchy(int tenantId, int parentClientId, int childClientId, int relationshipType, int createdBy)
    {
        if (_childHierarchies.Any(x => x.TenantId == tenantId && x.ParentClientId == parentClientId && x.ChildClientId == childClientId && x.RelationshipType == relationshipType))
            _childHierarchies.Add(new ClientHierarchyEntity(tenantId, parentClientId, childClientId, relationshipType, createdBy));
    }

    public void UpdateIndividual(string firstName, string lastName, DateTime birthDate, string gender, string documentType, string documentNumber, string nationality, int modifiedBy)
    {
        Individual.Update(firstName, lastName, birthDate, gender, documentType, documentNumber, nationality, modifiedBy);
    }
    public void UpdateCompany(int companyId, string legalName, string tradeName, string site, string companyRegistration, string cae, int numberOfEmployee, string legalRepresentative, int modifiedBy)
    {
        Company.Update(legalName, tradeName, site, companyRegistration, cae, numberOfEmployee, legalRepresentative, modifiedBy);
    }
    public void UpdateContact(int contactId, string name, string phoneNumber, string cellPhoneNumber, bool isWhatsapp, string email, bool isPrimary, int modifiedBy)
    {
        var contact = _contacts.FirstOrDefault(x => x.Id == contactId && !x.IsDeleted);
        contact.Update(name, phoneNumber, cellPhoneNumber, isWhatsapp, email, isPrimary, modifiedBy);
    }
    public void UpdateAddress(int addressId, int addressTypeId, string countryCode, string street, string streetNumber, string complement, string neighborhood, string city, string district, string postalCode, decimal? latitude, decimal? longitude, string notes, bool isPrimary, int modifiedBy)
    {
        var address = _addresses.FirstOrDefault(x => x.Id == addressId && !x.IsDeleted);
        address.Update(addressTypeId, countryCode, street, streetNumber, complement, neighborhood, city, district, postalCode, latitude, longitude, notes, isPrimary, modifiedBy); 
    }
    public void UpdateConsent(int consentId, bool granted, string origin, string ipAddress, string userAgent, int modifiedBy)
    {
        var consent = _consents.FirstOrDefault(x => x.Id == consentId && !x.IsDeleted);
        consent.Update(origin, ipAddress, userAgent, modifiedBy);
    }
    public void UpdateParentHierarchy(int hierarchyId, int relationshipType, int modifiedBy)
    {
        var hierarchy = _parentHierarchies.FirstOrDefault(x => x.Id == hierarchyId && !x.IsDeleted);
        hierarchy.UpdateRelationshipType(relationshipType, modifiedBy);
    }
    public void UpdateChildHierarchy(int hierarchyId, int relationshipType, int modifiedBy)
    {
        var hierarchy = _childHierarchies.FirstOrDefault(x => x.Id == hierarchyId && !x.IsDeleted);
        hierarchy.UpdateRelationshipType(relationshipType, modifiedBy);
    }
    public void RevokeConsent(int consentId, DateTime revokedDate, int modifiedBy)
    {
        var consent = _consents.FirstOrDefault(x => x.Id == consentId && !x.IsDeleted);
        consent.Revoke(revokedDate, modifiedBy);
    }
    
    public void DeleteIndividual(int modifiedBy)
    {
        Individual.Delete(modifiedBy);
    }
    public void DeleteCompany(int modifiedBy)
    {
        Company.Delete(modifiedBy);
    }
    public void DeleteContact(int contactId, int modifiedBy)
    {
        var contact = _contacts.FirstOrDefault(x => x.Id == contactId && !x.IsDeleted);
        contact.Delete(modifiedBy);
    }
    public void DeleteAddress(int addressId, int modifiedBy)
    {
        var address = _addresses.FirstOrDefault(x => x.Id == addressId && !x.IsDeleted);
        address.Delete(modifiedBy);
    }
    public void DeleteConsent(int consentId, int modifiedBy)
    {
        var consent = _consents.FirstOrDefault(x => x.Id == consentId && !x.IsDeleted);
        consent.Delete(modifiedBy);
    }
    public void DeleteParentHierarchy(int hierarchyId, int modifiedBy)
    {
        var hierarchy = _parentHierarchies.FirstOrDefault(x => x.Id == hierarchyId && !x.IsDeleted);
        hierarchy.Delete(modifiedBy);
    }
    public void DeleteChildHierarchy(int hierarchyId, int modifiedBy)
    {
        var hierarchy = _childHierarchies.FirstOrDefault(x => x.Id == hierarchyId && !x.IsDeleted);
        hierarchy.Delete(modifiedBy);
    }
    public void SetPrimaryAddress(int addressId, int modifiedBy)
    {
        var address = _addresses.FirstOrDefault(x => x.Id == addressId && !x.IsDeleted);
        address.SetPrimary(modifiedBy);
    }
    public void SetPrimaryContact(int contactId, int modifiedBy)
    {
        var contact = _contacts.FirstOrDefault(x => x.Id == contactId && !x.IsDeleted);
        contact.SetPrimary(modifiedBy);
    }

    public void Update(Origin origin, string urlImage, string notes, int modifiedBy)
    {
        Origin = origin;
        UrlImage = urlImage;
        Notes = notes;
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