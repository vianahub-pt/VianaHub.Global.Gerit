using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Enums;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

public class ClientEntity : Entity, IAggregateRoot
{
    private readonly List<ClientAddressEntity> _addresses = [];
    private readonly List<ClientContactEntity> _contacts = [];
    private readonly List<ClientConsentsEntity> _consents = [];
    private readonly List<ClientHierarchyEntity> _childHierarchies = [];
    private readonly List<ClientHierarchyEntity> _parentHierarchies = [];

    public int TenantId { get; private set; }
    public ClientType ClientType { get; private set; }
    public OriginType OriginType { get; private set; }
    public string UrlImage { get; private set; }
    public string Note { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Especializações do agregado
    public ClientIndividualEntity Individual { get; private set; }
    public ClientCompanyEntity Company { get; private set; }


    // Navigation Properties
    public TenantEntity Tenant { get; private set; } = null!;


    public IReadOnlyCollection<ClientAddressEntity> Addresses => _addresses.AsReadOnly();
    public IReadOnlyCollection<ClientContactEntity> Contacts => _contacts.AsReadOnly();
    public IReadOnlyCollection<ClientConsentsEntity> Consents => _consents.AsReadOnly();
    public IReadOnlyCollection<ClientHierarchyEntity> ChildHierarchies => _childHierarchies.AsReadOnly();
    public IReadOnlyCollection<ClientHierarchyEntity> ParentHierarchies => _parentHierarchies.AsReadOnly();


    // Construtor protegido para o EF Core
    protected ClientEntity() { }

    public ClientEntity(int tenantId, ClientType clientType, OriginType originType, string urlImage, string note, int createdBy)
    {
        TenantId = tenantId;
        ClientType = clientType;
        OriginType = originType;
        UrlImage = urlImage;
        Note = note;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddIndividual(ClientIndividualEntity clientIndividual)
    {
        Individual = clientIndividual;
    }

    public void AddCompany(ClientCompanyEntity clientCompany)
    {
        Company = clientCompany;
    }

    public void Update(ClientType clientType, OriginType originType, string urlImage, string note, int modifiedBy)
    {
        ClientType = clientType;
        OriginType = originType;
        UrlImage = urlImage;
        Note = note;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateIndividual(string firstName, string lastName, string phoneNumber, string cellPhoneNumber, bool isWhatsapp, string email, DateTime birthDate, string gender, string documentType, string documentNumber, string nationality, int modifiedBy)
    {
        Individual.Update(firstName, lastName, phoneNumber, cellPhoneNumber, isWhatsapp, email, birthDate, gender, documentType, documentNumber, nationality, modifiedBy);
    }
    public void UpdateCompany(string legalName, string tradeName, string phoneMumber, string cellPhoneNumber, bool isWhatsapp, string email, string site, string companyRegistration, string cae, int? numberOfEmployee, string legalRepresentative, int modifiedBy)
    {
        Company.Update(legalName, tradeName, phoneMumber, cellPhoneNumber, isWhatsapp, email, site, companyRegistration, cae, numberOfEmployee, legalRepresentative, modifiedBy);
    }

    public void Activate(ClientType clientType, int modifiedBy)
    {
        IsActive = true;

        switch (clientType)
        {
            case ClientType.PessoaSingular:
            case ClientType.RecibosVerdes:
            case ClientType.Freelancer:
                Individual.Activate(modifiedBy);
                break;
            case ClientType.PessoaJuridica:
            case ClientType.SociedadeUnipessoalQuotas:
                Company.Activate(modifiedBy);
                break;
            default:
                break;
        }

        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
    public void Deactivate(ClientType clientType, int modifiedBy)
    {
        IsActive = false;

        switch (clientType)
        {
            case ClientType.PessoaSingular:
            case ClientType.RecibosVerdes:
            case ClientType.Freelancer:
                Individual.Deactivate(modifiedBy);
                break;
            case ClientType.PessoaJuridica:
            case ClientType.SociedadeUnipessoalQuotas:
                Company.Deactivate(modifiedBy);
                break;
            default:
                break;
        }

        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
    public void Delete(ClientType clientType, int modifiedBy)
    {
        IsActive = false;
        IsDeleted = true;

        switch (clientType)
        {
            case ClientType.PessoaSingular:
            case ClientType.RecibosVerdes:
            case ClientType.Freelancer:
                Individual.Delete(modifiedBy);
                break;
            case ClientType.PessoaJuridica:
            case ClientType.SociedadeUnipessoalQuotas:
                Company.Delete(modifiedBy);
                break;
            default:
                break;
        }

        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}