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
        
        builder.Property(x => x.Id)
            .IsRequired();

        builder.HasAlternateKey(x => new { x.Id, x.TenantId })
            .HasName("UQ_ClientHierarchies_Id_Tenant");

        builder.Property(x => x.TenantId)
            .IsRequired();
        
        builder.Property(x => x.ParentId)
            .IsRequired();
        
        builder.Property(x => x.ChildId)
            .IsRequired();
        
        builder.Property(x => x.RelationshipType)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("DATETIME2(7)")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.ModifiedAt)
            .HasColumnType("DATETIME2(7)")
            .IsRequired(false);

        builder.HasOne(x => x.ParentClient)
            .WithMany(c => c.ChildHierarchies)
            .HasForeignKey(x => new { x.ParentId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ChildClient)
            .WithMany(c => c.ParentHierarchies)
            .HasForeignKey(x => new { x.ChildId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ParentId, x.ChildId, x.TenantId })
            .HasDatabaseName("IX_ClientHierarchies_ParentChild_Tenant")
            .IsUnique();

        builder.HasIndex(x => x.TenantId)
            .HasDatabaseName("IX_ClientHierarchies_TenantId");
    }
}

