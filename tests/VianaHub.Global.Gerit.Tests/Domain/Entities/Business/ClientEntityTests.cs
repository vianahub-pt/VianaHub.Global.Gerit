using System;
using System.Linq;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Enums;
using Xunit;

namespace VianaHub.Global.Gerit.Tests.Domain.Entities.Business;

public class ClientEntityTests
{
    [Fact(DisplayName = "ClientEntity - ClientContact SetPrimary/RemovePrimary funciona")]
    [Trait("Domain", "")]
    public void ClientContact_SetPrimaryAndRemovePrimary_WorksAsExpected()
    {
        var first = new ClientContactEntity(1, 0, "Contato 1", "111", "", true, "c1@test.com", false, 7);
        var second = new ClientContactEntity(1, 0, "Contato 2", "222", "", true, "c2@test.com", false, 7);

        // Set primary on first
        first.SetPrimary(7);
        Assert.True(first.IsPrimary);

        // Set primary on second
        second.SetPrimary(7);
        Assert.True(second.IsPrimary);

        // Remove primary
        second.RemovePrimary(7);
        Assert.False(second.IsPrimary);
    }

    [Fact(DisplayName = "ClientEntity - ClientAddress SetPrimary/RemovePrimary funciona")]
    [Trait("Domain", "")]
    public void ClientAddress_SetPrimaryAndRemovePrimary_WorksAsExpected()
    {
        var first = new ClientAddressEntity(1, 0, 1, "PT", "Rua 1", "10", "", "Centro", "Porto", "Porto", "4000-001", null, null, "", false, 7);
        var second = new ClientAddressEntity(1, 0, 2, "PT", "Rua 2", "20", "", "Centro", "Lisboa", "Lisboa", "1000-001", null, null, "", false, 7);

        first.SetPrimary(7);
        Assert.True(first.IsPrimary);

        second.SetPrimary(7);
        Assert.True(second.IsPrimary);

        second.RemovePrimary(7);
        Assert.False(second.IsPrimary);
    }

    private static ClientEntity CreateClient()
    {
        return new ClientEntity(1, ClientType.PessoaSingular, OriginType.Outros, urlImage: null, note: null, createdBy: 7);
    }
}
