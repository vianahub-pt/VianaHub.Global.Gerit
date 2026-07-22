namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.AddressType;

/// <summary>
/// Resposta detalhada de um Tipo de Endereço (inclui traduções e campos de auditoria).
/// Classe independente — não herda de AddressTypeResponse.
/// </summary>
public class AddressTypeDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? LanguageCode { get; set; }
    public bool IsActive { get; set; }

    public List<AddressTypeTranslationResponse>? Translations { get; set; }

    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
