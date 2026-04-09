namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientHierarchy;

public class ClientHierarchyResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int ParentClientId { get; set; }
    public int ChildClientId { get; set; }
    public int RelationshipType { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
