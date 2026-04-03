using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Infra.Data.Context;

public class GeritDbContext : DbContext
{
    public GeritDbContext(DbContextOptions<GeritDbContext> options)
        : base(options)
    {
    }

    #region DbSets - Core Multi-Tenant Tables
    public DbSet<PlanEntity> Plans { get; set; }
    public DbSet<SubscriptionEntity> Subscriptions { get; set; }
    public DbSet<TenantEntity> Tenants { get; set; }
    public DbSet<TenantContact> TenantContacts { get; set; }
    public DbSet<TenantAddress> TenantAddresses { get; set; }
    public DbSet<TenantFiscalDataEntity> TenantFiscalData { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    #endregion

    #region DbSets - RBAC Structure
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<ResourceEntity> Resources { get; set; }
    public DbSet<ActionEntity> Actions { get; set; }
    public DbSet<RolePermissionEntity> RolePermissions { get; set; }
    public DbSet<UserRoleEntity> UserRoles { get; set; }
    public DbSet<JwtKeyEntity> JwtKeys { get; set; }
    #endregion

    #region DbSets - Domain Tables
    public DbSet<AddressTypeEntity> AddressTypes { get; set; }
    public DbSet<ClientTypeEntity> ClientTypes { get; set; }
    public DbSet<ClientEntity> Clients { get; set; }
    public DbSet<ClientContactEntity> ClientContacts { get; set; }
    public DbSet<ClientAddressEntity> ClientAddresses { get; set; }
    public DbSet<ClientFiscalDataEntity> ClientFiscalData { get; set; }
    public DbSet<TeamEntity> Teams { get; set; }
    public DbSet<TeamMemberEntity> TeamMembers { get; set; }
    public DbSet<TeamMemberContactEntity> TeamMemberContacts { get; set; }
    public DbSet<TeamMemberAddressEntity> TeamMemberAddresses { get; set; }
    public DbSet<FunctionEntity> Functions { get; set; }
    public DbSet<EquipmentEntity> Equipments { get; set; }
    public DbSet<EquipmentTypeEntity> EquipmentTypes { get; set; }
    public DbSet<VehicleEntity> Vehicles { get; set; }
    public DbSet<StatusTypeEntity> StatusTypes { get; set; }
    public DbSet<StatusEntity> Status { get; set; }
    public DbSet<InterventionEntity> Interventions { get; set; }
    public DbSet<InterventionTeamEntity> InterventionTeams { get; set; }
    public DbSet<InterventionContactEntity> InterventionContacts { get; set; }
    public DbSet<InterventionAddressEntity> InterventionAddresses { get; set; }
    public DbSet<InterventionTeamVehicleEntity> InterventionTeamVehicles { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica todas as configurações de mapeamento do assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GeritDbContext).Assembly);

        // Configura o schema padrão
        modelBuilder.HasDefaultSchema("dbo");
    }
}
