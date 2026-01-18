using Microsoft.EntityFrameworkCore;
using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Infra.Data.Context;

public class GeritDbContext : DbContext
{
    private readonly int? _tenantId;

    public GeritDbContext(DbContextOptions<GeritDbContext> options, int? tenantId = null) 
        : base(options)
    {
        _tenantId = tenantId;
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
    public DbSet<Domain.Entities.ActionEntity> Actions { get; set; }
    public DbSet<RolePermissionEntity> RolePermissions { get; set; }
    public DbSet<UserRoleEntity> UserRoles { get; set; }
    public DbSet<JwtKeyEntity> JwtKeys { get; set; }
    #endregion

    #region DbSets - Domain Tables
    public DbSet<ClientEntity> Clients { get; set; }
    public DbSet<ClientContactEntity> ClientContacts { get; set; }
    public DbSet<ClientAddressEntity> ClientAddresses { get; set; }
    public DbSet<ClientFiscalDataEntity> ClientFiscalData { get; set; }
    public DbSet<TeamMemberEntity> TeamMembers { get; set; }
    public DbSet<TeamMemberContactEntity> TeamMemberContacts { get; set; }
    public DbSet<TeamMemberAddressEntity> TeamMemberAddresses { get; set; }
    public DbSet<EquipmentEntity> Equipments { get; set; }
    public DbSet<VehicleEntity> Vehicles { get; set; }
    public DbSet<InterventionEntity> Interventions { get; set; }
    public DbSet<InterventionContactEntity> InterventionContacts { get; set; }
    public DbSet<InterventionAddressEntity> InterventionAddresses { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica todas as configurações de mapeamento do assembly
        // Os mapeamentos estão na pasta Mappings e seguem exatamente as especificações
        // do arquivo docs/sql/Create-Tables.sql incluindo:
        // - Todos os índices (clusterizados, não-clusterizados, únicos, filtrados)
        // - Todas as constraints (PK, FK, UQ, CHECK)
        // - Configurações de Row Level Security (aplicadas via migrations)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GeritDbContext).Assembly);

        // Configura o schema padrão
        modelBuilder.HasDefaultSchema("dbo");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Define o TenantId no SESSION_CONTEXT antes de executar operações
        // Isso é necessário para o Row Level Security funcionar corretamente
        if (_tenantId.HasValue)
        {
            await Database.ExecuteSqlRawAsync(
                $"EXEC sp_set_session_context @key = N'TenantId', @value = {_tenantId.Value}", 
                cancellationToken);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        // Define o TenantId no SESSION_CONTEXT antes de executar operações
        // Isso é necessário para o Row Level Security funcionar corretamente
        if (_tenantId.HasValue)
        {
            Database.ExecuteSqlRaw(
                $"EXEC sp_set_session_context @key = N'TenantId', @value = {_tenantId.Value}");
        }

        return base.SaveChanges();
    }

    /// <summary>
    /// Define o contexto do Tenant para Row Level Security
    /// DEVE ser chamado antes de qualquer operação que envolva tabelas com RLS
    /// </summary>
    public async Task SetTenantContextAsync(int tenantId, CancellationToken cancellationToken = default)
    {
        await Database.ExecuteSqlRawAsync(
            $"EXEC sp_set_session_context @key = N'TenantId', @value = {tenantId}", 
            cancellationToken);
    }

    /// <summary>
    /// Limpa o contexto do Tenant
    /// Deve ser chamado ao finalizar operações ou trocar de tenant
    /// </summary>
    public async Task ClearTenantContextAsync(CancellationToken cancellationToken = default)
    {
        await Database.ExecuteSqlRawAsync(
            "EXEC sp_set_session_context @key = N'TenantId', @value = NULL", 
            cancellationToken);
    }
}
