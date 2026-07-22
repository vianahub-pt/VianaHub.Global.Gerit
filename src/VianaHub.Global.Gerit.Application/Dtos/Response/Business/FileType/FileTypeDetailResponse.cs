namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.FileType;

/// <summary>
/// Resposta detalhada de um FileType (inclui traduções e campos de auditoria).
/// Classe independente — não herda de FileTypeResponse.
/// </summary>
public class FileTypeDetailResponse
{
    public int Id { get; set; }
    public string? MimeType { get; set; }
    public string? Extension { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? LanguageCode { get; set; }
    public bool IsActive { get; set; }

    public List<FileTypeTranslationResponse>? Translations { get; set; }

    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
