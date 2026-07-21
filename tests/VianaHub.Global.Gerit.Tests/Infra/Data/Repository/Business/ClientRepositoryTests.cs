using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Infra.Data.Context;
using VianaHub.Global.Gerit.Infra.Data.Repository.Business;

namespace VianaHub.Global.Gerit.Tests.Infra.Data.Repository.Business
{
    public class ClientRepositoryTests
    {
        private static ClientEntity CreateActiveClient(string name, string email)
        {
            return new ClientEntity(1, 1, 1, imageUrl: null, note: null,
                name: name, phoneNumber: null, cellPhoneNumber: null,
                isCellPhoneWhatsapp: false, email: email, websiteUrl: null,
                birthDate: null, gender: null, nationality: null,
                companyRegistrationNumber: null, economicActivityCode: null,
                numberOfEmployees: null, statusDefinitionId: 0, statusDomainId: 0,
                createdBy: 1);
        }

        private static ClientEntity CreateDeletedClient(string name, string email)
        {
            var client = CreateActiveClient(name, email);
            client.Delete(1);
            return client;
        }

        [Fact(DisplayName = "GetPagedAsync: does not return deleted records")]
        [Trait("Infra.Data", "")]
        public async Task GetPagedAsync_ShouldNotReturnDeletedRecords()
        {
            var options = new DbContextOptionsBuilder<GeritDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using (var context = new GeritDbContext(options))
            {
                // Seed required related entities (PartyType, AcquisitionSourceType, Tenant)
                // necessários porque o repositório faz Include com navegações obrigatórias,
                // que o InMemory provider trata como INNER JOIN.
                var partyType = new PartyTypeEntity("PF", 1);
                context.PartyTypes.Add(partyType);
                // PartyTypeEntity.Id é TINYINT sem identity — force-set Id=1
                context.Entry(partyType).Property("Id").CurrentValue = (byte)1;

                var acquisitionSourceType = new AcquisitionSourceTypeEntity("DIRECT", 1);
                context.AcquisitionSourceTypes.Add(acquisitionSourceType);
                // AcquisitionSourceTypeEntity.Id é INT com identity — auto-gera Id=1

                var tenant = new TenantEntity(1, 1, "Test Tenant", "tenant@test.local", null, null, null, 1);
                context.Tenants.Add(tenant);
                // TenantEntity.Id é INT com identity — auto-gera Id=1

                await context.SaveChangesAsync();

                // Agora criar os clients de teste
                var active = CreateActiveClient("Teste Active User", "active@test.local");
                var deleted = CreateDeletedClient("Teste Deleted User", "deleted@test.local");

                await context.Clients.AddRangeAsync(active, deleted);
                await context.SaveChangesAsync();

                var repo = new ClientRepository(context);

                var filter = new PagedFilter(null, null, 1, 10, null, null);
                var result = await repo.GetPagedAsync(filter, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(1, result.TotalItems);
                Assert.Single(result.Items);
                Assert.DoesNotContain(result.Items, i => i.Email == "deleted@test.local");
            }
        }

        [Fact(DisplayName = "GetPagedAsync: search excludes deleted records")]
        [Trait("Layer", "Infra.Data")]
        public async Task GetPagedAsync_SearchShouldExcludeDeletedRecords()
        {
            var options = new DbContextOptionsBuilder<GeritDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using (var context = new GeritDbContext(options))
            {
                // Seed required related entities
                var partyType = new PartyTypeEntity("PF", 1);
                context.PartyTypes.Add(partyType);
                context.Entry(partyType).Property("Id").CurrentValue = (byte)1;

                var acquisitionSourceType = new AcquisitionSourceTypeEntity("DIRECT", 1);
                context.AcquisitionSourceTypes.Add(acquisitionSourceType);

                var tenant = new TenantEntity(1, 1, "Test Tenant", "tenant@test.local", null, null, null, 1);
                context.Tenants.Add(tenant);

                await context.SaveChangesAsync();

                var active = CreateActiveClient("Alice Active", "alice.active@test.local");
                var deleted = CreateDeletedClient("Alice Deleted", "alice.deleted@test.local");

                await context.Clients.AddRangeAsync(active, deleted);
                await context.SaveChangesAsync();

                var repo = new ClientRepository(context);

                var filter = new PagedFilter("Alice", null, 1, 10, null, null);
                var result = await repo.GetPagedAsync(filter, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(1, result.TotalItems);
                Assert.Single(result.Items);
                Assert.DoesNotContain(result.Items, i => i.Email == "alice.deleted@test.local");
            }
        }

        [Fact(DisplayName = "GetPagedAsync: quando todos os registros estao deletados retorna vazio")]
        [Trait("Layer", "Infra.Data")]
        public async Task GetPagedAsync_WhenAllRecordsDeleted_ReturnsEmpty()
        {
            var options = new DbContextOptionsBuilder<GeritDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using (var context = new GeritDbContext(options))
            {
                // Seed required related entities
                var partyType = new PartyTypeEntity("PF", 1);
                context.PartyTypes.Add(partyType);
                context.Entry(partyType).Property("Id").CurrentValue = (byte)1;

                var acquisitionSourceType = new AcquisitionSourceTypeEntity("DIRECT", 1);
                context.AcquisitionSourceTypes.Add(acquisitionSourceType);

                var tenant = new TenantEntity(1, 1, "Test Tenant", "tenant@test.local", null, null, null, 1);
                context.Tenants.Add(tenant);

                await context.SaveChangesAsync();

                var deleted1 = CreateDeletedClient("Del1 User", "del1@test.local");
                var deleted2 = CreateDeletedClient("Del2 User", "del2@test.local");

                await context.Clients.AddRangeAsync(deleted1, deleted2);
                await context.SaveChangesAsync();

                var repo = new ClientRepository(context);

                var filter = new PagedFilter(null, null, 1, 10, null, null);
                var result = await repo.GetPagedAsync(filter, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(0, result.TotalItems);
                Assert.Empty(result.Items);
            }
        }
    }
}
