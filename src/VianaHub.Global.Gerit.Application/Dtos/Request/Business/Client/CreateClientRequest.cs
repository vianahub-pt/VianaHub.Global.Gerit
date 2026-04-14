using VianaHub.Global.Gerit.Application.Dtos.Business.Client;
using VianaHub.Global.Gerit.Domain.Enums;

namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Client;

public class CreateClientRequest
{
    public CreateClientTypeRequest ClientType { get; set; }
    public Origin Origin { get; set; }
    public string UrlImage { get; set; }
    public string Notes { get; set; }

    public CreateClientIndividualRequest? Individual { get; set; }
    public CreateClientCompanyRequest? Company { get; set; }

    public List<CreateClientContactRequest> Contacts { get; set; } = [];
    public List<CreateClientAddressRequest> Addresses { get; set; } = [];
    public List<CreateClientConsentRequest> Consents { get; set; } = [];
    public List<CreateClientHierarchyRequest> ParentHierarchies { get; set; } = [];
    public List<CreateClientHierarchyRequest> ChildHierarchies { get; set; } = [];
}
