namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade de tradução de um AcquisitionSourceType por idioma (pt-PT, en-US, es-ES).
/// Compõe a chave estrangeira composta (AcquisitionSourceTypeId + LanguageCode).
/// </summary>
public class AcquisitionSourceTypeTranslationEntity
{
    public int AcquisitionSourceTypeId { get; private set; }
    public string? LanguageCode { get; private set; }
    public string? Name { get; private set; }
    public string? Description { get; private set; }

    // Navigation Property
    public AcquisitionSourceTypeEntity AcquisitionSourceType { get; private set; }

    // Construtor protegido para o EF Core
    protected AcquisitionSourceTypeTranslationEntity() { }

    /// <summary>
    /// Construtor para criação de uma nova tradução de AcquisitionSourceType
    /// </summary>
    public AcquisitionSourceTypeTranslationEntity(int acquisitionSourceTypeId, string languageCode, string name, string description)
    {
        AcquisitionSourceTypeId = acquisitionSourceTypeId;
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
