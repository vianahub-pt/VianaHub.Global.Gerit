using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa os dados fiscais do Cliente
/// </summary>
public class ClientFiscalDataEntity : Entity
{
    public int TenantId { get; private set; }
    public int ClientId { get; private set; }
    public string NIF { get; private set; }
    public string VATNumber { get; private set; }
    public string CAE { get; private set; }
    public string FiscalCountry { get; private set; }
    public bool IsVATRegistered { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }
    public ClientEntity Client { get; private set; }

    // Construtor protegido para o EF Core
    protected ClientFiscalDataEntity() { }

    /// <summary>
    /// Construtor para criaþÒo de novos dados fiscais do Cliente
    /// </summary>
    public ClientFiscalDataEntity(int tenantId, int clientId, string nif, string vatNumber, string cae, string fiscalCountry, bool isVATRegistered, int createdBy)
    {
        TenantId = tenantId;
        ClientId = clientId;
        NIF = nif;
        VATNumber = vatNumber;
        CAE = cae;
        FiscalCountry = fiscalCountry;
        IsVATRegistered = isVATRegistered;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateFiscalData(string nif, string vatNumber, string cae, string fiscalCountry, bool isVATRegistered, int modifiedBy)
    {
        NIF = nif;
        VATNumber = vatNumber;
        CAE = cae;
        FiscalCountry = fiscalCountry;
        IsVATRegistered = isVATRegistered;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void RegisterForVAT(int modifiedBy)
    {
        IsVATRegistered = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UnregisterFromVAT(int modifiedBy)
    {
        IsVATRegistered = false;
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
        IsDeleted = true;
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}
