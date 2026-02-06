using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Identity;

/// <summary>
/// Mapeamento da entidade UserRole
/// Relação usuário x role com suporte a Row Level Security
/// </summary>
public class UserRoleMapping : IEntityTypeConfiguration<UserRoleEntity>
{
    public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        builder.ToTable("UserRoles", "dbo");

        // Chave Primária
        builder.HasKey(x => x.Id)
            .HasName("PK_UserRoles");

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .IsRequired();

        builder.Property(x => x.RoleId)
            .HasColumnName("RoleId")
            .IsRequired();

        // Constraints únicos
        builder.HasIndex(x => new { x.TenantId, x.UserId, x.RoleId })
            .IsUnique()
            .HasDatabaseName("UQ_UserRoles");

        // Índices
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

        // NOTA: Relacionamentos com User e Role já estão configurados em:
        // - UserMapping: HasMany(u => u.UserRoles).WithOne(x => x.User).HasForeignKey(x => x.UserId)
        // - RoleMapping: HasMany(r => r.UserRoles).WithOne(x => x.Role).HasForeignKey(x => x.RoleId)
        // Não configurar novamente aqui para evitar propriedades sombra (shadow properties)
    }
}
