namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade de tradução de um DocumentType por idioma (pt-PT, en-US, es-ES).
/// Compõe a chave estrangeira composta (DocumentTypeId + LanguageCode).
/// </summary>
public class DocumentTypeTranslationEntity
{
    public int DocumentTypeId { get; private set; }
    public string? LanguageCode { get; private set; }
    public string? Name { get; private set; }
    public string? Description { get; private set; }

    // Navigation Property
    public DocumentTypeEntity DocumentType { get; private set; }

    // Construtor protegido para o EF Core
    protected DocumentTypeTranslationEntity() { }

    /// <summary>
    /// Construtor para criação de uma nova tradução de DocumentType
    /// </summary>
    public DocumentTypeTranslationEntity(int documentTypeId, string languageCode, string name, string description)
    {
        DocumentTypeId = documentTypeId;
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
