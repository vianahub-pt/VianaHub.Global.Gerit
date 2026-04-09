using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class ClientHierarchyMapping : IEntityTypeConfiguration<ClientHierarchyEntity>
{
    public void Configure(EntityTypeBuilder<ClientHierarchyEntity> builder)
    {
        builder.ToTable("ClientHierarchies");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("Id").IsRequired();

        builder.HasAlternateKey(x => new { x.Id, x.TenantId })
            .HasName("UQ_ClientHierarchies_Id_Tenant");

        builder.Property(x => x.TenantId).HasColumnName("TenantId").IsRequired();
        builder.Property(x => x.ParentClientId).HasColumnName("ParentClientId").IsRequired();
        builder.Property(x => x.ChildClientId).HasColumnName("ChildClientId").IsRequired();
        builder.Property(x => x.RelationshipType).HasColumnName("RelationshipType").IsRequired();

        builder.Property(x => x.IsActive).HasColumnName("IsActive").HasDefaultValue(true);
        builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(false);
        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("DATETIME2(7)").IsRequired();
        builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
        builder.Property(x => x.ModifiedAt).HasColumnName("ModifiedAt").HasColumnType("DATETIME2(7)");

        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ParentClient)
            .WithMany()
            .HasForeignKey(x => new { x.ParentClientId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ChildClient)
            .WithMany()
            .HasForeignKey(x => new { x.ChildClientId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ParentClientId, x.ChildClientId, x.TenantId })
            .HasDatabaseName("IX_ClientHierarchies_ParentChild_Tenant")
            .IsUnique();

        builder.HasIndex(x => x.TenantId)
            .HasDatabaseName("IX_ClientHierarchies_TenantId");
    }
}
