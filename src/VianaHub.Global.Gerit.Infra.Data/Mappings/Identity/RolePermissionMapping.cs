using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Identity;

/// <summary>
/// Mapeamento da entidade RolePermission
/// Permissões por role com suporte a Row Level Security
/// </summary>
public class RolePermissionMapping : IEntityTypeConfiguration<RolePermissionEntity>
{
    public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
    {
        builder.ToTable("RolePermissions", "dbo");

        // Chave Primária
        builder.HasKey(rp => rp.Id)
            .HasName("PK_RolePermissions");

        builder.Property(rp => rp.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(rp => rp.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(rp => rp.RoleId)
            .HasColumnName("RoleId")
            .IsRequired();

        builder.Property(rp => rp.ResourceId)
            .HasColumnName("ResourceId")
            .IsRequired();

        builder.Property(rp => rp.ActionId)
            .HasColumnName("ActionId")
            .IsRequired();

        // Constraints únicos
        builder.HasIndex(rp => new { rp.TenantId, rp.RoleId, rp.ResourceId, rp.ActionId })
            .IsUnique()
            .HasDatabaseName("UQ_RolePermissions");

        // Índices
        builder.HasIndex(rp => rp.RoleId)
            .HasDatabaseName("IX_RolePermissions_RoleId")
            .IncludeProperties(rp => new { rp.TenantId, rp.ResourceId, rp.ActionId });

        builder.HasIndex(rp => rp.ResourceId)
            .HasDatabaseName("IX_RolePermissions_ResourceId")
            .IncludeProperties(rp => new { rp.TenantId, rp.RoleId, rp.ActionId });

        builder.HasIndex(rp => rp.ActionId)
            .HasDatabaseName("IX_RolePermissions_ActionId")
            .IncludeProperties(rp => new { rp.TenantId, rp.RoleId, rp.ResourceId });

        // Relacionamentos
        builder.HasOne(rp => rp.Tenant)
            .WithMany()
            .HasForeignKey(rp => rp.TenantId)
            .HasConstraintName("FK_RolePermissions_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamentos com Role, Resource e ActionEntity configurados nos respectivos mappings
    }
}
