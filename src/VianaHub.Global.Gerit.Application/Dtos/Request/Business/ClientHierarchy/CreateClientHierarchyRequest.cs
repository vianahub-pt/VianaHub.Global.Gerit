namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientHierarchy;

public class CreateClientHierarchyRequest
{
    public int ParentClientId { get; set; }
    public int ChildClientId { get; set; }
    public int RelationshipType { get; set; }
}
