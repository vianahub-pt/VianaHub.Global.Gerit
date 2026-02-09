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
        builder.HasKey(x => new { x.TenantId, x.RoleId, x.ResourceId, x.ActionId })
            .HasName("PK_RolePermissions");

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(x => x.RoleId)
            .HasColumnName("RoleId")
            .IsRequired();

        builder.Property(x => x.ResourceId)
            .HasColumnName("ResourceId")
            .IsRequired();

        builder.Property(x => x.ActionId)
            .HasColumnName("ActionId")
            .IsRequired();

        // Constraints únicos
        builder.HasIndex(x => new { x.TenantId, x.RoleId, x.ResourceId, x.ActionId })
            .IsUnique()
            .HasDatabaseName("UQ_RolePermissions");

        // Índices
        builder.HasIndex(x => x.RoleId)
            .HasDatabaseName("IX_RolePermissions_RoleId")
            .IncludeProperties(x => new { x.TenantId, x.ResourceId, x.ActionId });

        builder.HasIndex(x => x.ResourceId)
            .HasDatabaseName("IX_RolePermissions_ResourceId")
            .IncludeProperties(x => new { x.TenantId, x.RoleId, x.ActionId });

        builder.HasIndex(x => x.ActionId)
            .HasDatabaseName("IX_RolePermissions_ActionId")
            .IncludeProperties(x => new { x.TenantId, x.RoleId, x.ResourceId });

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_RolePermissions_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamentos com Role, Resource e ActionEntity configurados nos respectivos mappings
    }
}
