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
    /// Construtor para criação de novos dados fiscais do Cliente
    /// </summary>
    public ClientFiscalDataEntity(int tenantId, int clientId, string nif, string vatNumber, 
        string cae = null, string fiscalCountry = "PT", bool isVATRegistered = true)
    {
        TenantId = tenantId;
        ClientId = clientId;
        SetNIF(nif);
        SetVATNumber(vatNumber);
        CAE = cae;
        SetFiscalCountry(fiscalCountry);
        IsVATRegistered = isVATRegistered;
        IsActive = true;
        IsDeleted = false;
    }

    public void SetNIF(string nif)
    {
        if (string.IsNullOrWhiteSpace(nif))
            throw new ArgumentException("NIF não pode ser vazio.", nameof(nif));

        if (nif.Length != 9)
            throw new ArgumentException("NIF deve ter exatamente 9 caracteres.", nameof(nif));

        if (!nif.All(char.IsDigit))
            throw new ArgumentException("NIF deve conter apenas dígitos.", nameof(nif));

        NIF = nif;
    }

    public void SetVATNumber(string vatNumber)
    {
        if (string.IsNullOrWhiteSpace(vatNumber))
            throw new ArgumentException("Número VAT não pode ser vazio.", nameof(vatNumber));

        if (vatNumber.Length > 20)
            throw new ArgumentException("Número VAT não pode ter mais de 20 caracteres.", nameof(vatNumber));

        VATNumber = vatNumber;
    }

    public void SetCAE(string cae)
    {
        if (cae?.Length > 10)
            throw new ArgumentException("CAE não pode ter mais de 10 caracteres.", nameof(cae));

        CAE = cae;
    }

    public void SetFiscalCountry(string fiscalCountry)
    {
        if (string.IsNullOrWhiteSpace(fiscalCountry))
            throw new ArgumentException("País fiscal não pode ser vazio.", nameof(fiscalCountry));

        if (fiscalCountry.Length != 2)
            throw new ArgumentException("País fiscal deve ter exatamente 2 caracteres.", nameof(fiscalCountry));

        FiscalCountry = fiscalCountry.ToUpper();
    }

    public void RegisterForVAT()
    {
        IsVATRegistered = true;
    }

    public void UnregisterFromVAT()
    {
        IsVATRegistered = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Delete()
    {
        IsDeleted = true;
        IsActive = false;
    }
}
