namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientHierarchy;

public class CreateClientHierarchyRequest
{
    public int ParentId { get; set; }
    public int ChildId { get; set; }
    public int RelationshipType { get; set; }
}
