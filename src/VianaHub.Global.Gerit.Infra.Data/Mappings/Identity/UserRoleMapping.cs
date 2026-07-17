using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Identity;

/// <summary>
/// Mapeamento da entidade UserRole
/// Rela��o usu�rio x role com suporte a Row Level Security
/// </summary>
public class UserRoleMapping : IEntityTypeConfiguration<UserRoleEntity>
{
    public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        builder.ToTable("UserRoles", "dbo");

        // Chave prim�ria composta
        builder.HasKey(x => new { x.TenantId, x.UserId, x.RoleId })
            .HasName("PK_UserRoles");

        // Propriedades
        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.RoleId)
            .IsRequired();

        // Índices
        // Unique constraint: nao pode duplicar (Tenant, User, Role)
        builder.HasIndex(x => new { x.TenantId, x.UserId, x.RoleId })
            .IsUnique()
            .HasDatabaseName("UQ_UserRoles");

        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_UserRoles_UserId")
            .IncludeProperties(x => new { x.TenantId, x.RoleId });

        builder.HasIndex(x => x.RoleId)
            .HasDatabaseName("IX_UserRoles_RoleId")
            .IncludeProperties(x => new { x.TenantId, x.UserId });

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_UserRoles_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

    }
}
