namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade de tradução de um AddressType por idioma (pt-PT, en-US, es-ES).
/// Compõe a chave estrangeira composta (AddressTypeId + LanguageCode).
/// Description usa NVARCHAR(500) — maior que os 300 das demais traduções do domínio.
/// </summary>
public class AddressTypeTranslationEntity
{
    public int AddressTypeId { get; private set; }
    public string? LanguageCode { get; private set; }
    public string? Name { get; private set; }
    public string? Description { get; private set; }

    // Navigation Property
    public AddressTypeEntity AddressType { get; private set; }

    // Construtor protegido para o EF Core
    protected AddressTypeTranslationEntity() { }

    /// <summary>
    /// Construtor para criação de uma nova tradução de AddressType
    /// </summary>
    public AddressTypeTranslationEntity(int addressTypeId, string languageCode, string name, string description)
    {
        AddressTypeId = addressTypeId;
        LanguageCode = languageCode;
        Name = name;
        Description = description;
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
