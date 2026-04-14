using System;
using System.Linq;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Enums;
using Xunit;

namespace VianaHub.Global.Gerit.Tests.Domain.Entities.Business;

public class ClientEntityTests
{
    [Fact]
    public void AddContact_ShouldKeepOnlyOnePrimaryContact()
    {
        var client = CreateClient();
        var firstPrimary = new ClientContactEntity(1, 0, "Contato 1", "c1@test.com", "111", true, 7);
        var secondPrimary = new ClientContactEntity(1, 0, "Contato 2", "c2@test.com", "222", true, 7);

        client.AddContact(firstPrimary, 7);
        client.AddContact(secondPrimary, 7);

        Assert.Single(client.Contacts.Where(x => x.IsPrimary));
        Assert.True(client.Contacts.Single(x => x.IsPrimary).Email == "c2@test.com");
    }

    [Fact]
    public void AddAddress_ShouldKeepOnlyOnePrimaryAddress()
    {
        var client = CreateClient();
        var firstPrimary = new ClientAddressEntity(1, 0, 1, "PT", "Rua 1", "Centro", "Porto", "Porto", "4000-001", "10", null, null, null, null, true, 7);
        var secondPrimary = new ClientAddressEntity(1, 0, 2, "PT", "Rua 2", "Centro", "Lisboa", "Lisboa", "1000-001", "20", null, null, null, null, true, 7);

        client.AddAddress(firstPrimary, 7);
        client.AddAddress(secondPrimary, 7);

        Assert.Single(client.Addresses.Where(x => x.IsPrimary));
        Assert.True(client.Addresses.Single(x => x.IsPrimary).Street == "Rua 2");
    }

    private static ClientEntity CreateClient()
    {
        return new ClientEntity(
            tenantId: 1,
            clientType: ClientType.Individual,
            origin: Origin.Manual,
            name: "Cliente Teste",
            phone: "999999999",
            email: "cliente@test.com",
            website: null,
            urlImage: null,
            score: null,
            consent: true,
            consentDate: DateTime.UtcNow,
            remarks: null,
            createdBy: 7);
    }
}
