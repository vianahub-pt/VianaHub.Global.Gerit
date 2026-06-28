using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Enums;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Infra.Data.Context;
using VianaHub.Global.Gerit.Infra.Data.Repository.Business;

namespace VianaHub.Global.Gerit.Tests.Infra.Data.Repository.Business
{
    public class ClientRepositoryTests
    {
        [Fact(DisplayName = "GetPagedAsync: does not return deleted records")]
        [Trait("Infra.Data", "")]
        public async Task GetPagedAsync_ShouldNotReturnDeletedRecords()
        {
            var options = new DbContextOptionsBuilder<GeritDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using (var context = new GeritDbContext(options))
            {
                var active = new ClientEntity(1, ClientType.PessoaSingular, (int)OriginType.Outros, urlImage: null, note: null, createdBy: 1);
                active.AddIndividual(new ClientIndividualEntity(1, "Teste Active User", "Active", "User", "", "", false, "active@test.local", null, null, null, null, null, 1));

                var deleted = new ClientEntity(1, ClientType.PessoaSingular, (int)OriginType.Outros, urlImage: null, note: null, createdBy: 1);
                deleted.AddIndividual(new ClientIndividualEntity(1, "Teste Deleted User", "Deleted", "User", "", "", false, "deleted@test.local", null, null, null, null, null, 1));
                deleted.Delete(ClientType.PessoaSingular, 1);

                await context.Clients.AddRangeAsync(active, deleted);
                await context.SaveChangesAsync();

                var repo = new ClientRepository(context);

                var filter = new PagedFilter(null, null, 1, 10, null, null);
                var result = await repo.GetPagedAsync(filter, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(1, result.TotalItems);
                Assert.Single(result.Items);
                Assert.DoesNotContain(result.Items, i => i.Individual != null && i.Individual.Email == "deleted@test.local");
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
                var active = new ClientEntity(1, ClientType.PessoaSingular, (int)OriginType.Outros, urlImage: null, note: null, createdBy: 1);
                active.AddIndividual(new ClientIndividualEntity(1, "Teste Alice Active", "Alice", "Active", "", "", false, "alice.active@test.local", null, null, null, null, null, 1));

                var deleted = new ClientEntity(1, ClientType.PessoaSingular, (int)OriginType.Outros, urlImage: null, note: null, createdBy: 1);
                deleted.AddIndividual(new ClientIndividualEntity(1, "Teste Alice Deleted", "Alice", "Deleted", "", "", false, "alice.deleted@test.local", null, null, null, null, null, 1));
                deleted.Delete(ClientType.PessoaSingular, 1);

                await context.Clients.AddRangeAsync(active, deleted);
                await context.SaveChangesAsync();

                var repo = new ClientRepository(context);

                var filter = new PagedFilter("Alice", null, 1, 10, null, null);
                var result = await repo.GetPagedAsync(filter, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(1, result.TotalItems);
                Assert.Single(result.Items);
                Assert.DoesNotContain(result.Items, i => i.Individual != null && i.Individual.Email == "alice.deleted@test.local");
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
                var deleted1 = new ClientEntity(1, ClientType.PessoaSingular, (int)OriginType.Outros, urlImage: null, note: null, createdBy: 1);
                deleted1.AddIndividual(new ClientIndividualEntity(1, "Del1 User", "Del1", "User", "", "", false, "del1@test.local", null, null, null, null, null, 1));
                deleted1.Delete(ClientType.PessoaSingular, 1);

                var deleted2 = new ClientEntity(1, ClientType.PessoaSingular, (int)OriginType.Outros, urlImage: null, note: null, createdBy: 1);
                deleted2.AddIndividual(new ClientIndividualEntity(1, "Del2 User", "Del2", "User", "", "", false, "del2@test.local", null, null, null, null, null, 1));
                deleted2.Delete(ClientType.PessoaSingular, 1);

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
