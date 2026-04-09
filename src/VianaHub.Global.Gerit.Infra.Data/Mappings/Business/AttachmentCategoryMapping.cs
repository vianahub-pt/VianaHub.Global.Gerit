using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class AttachmentCategoryMapping : IEntityTypeConfiguration<AttachmentCategoryEntity>
{
    public void Configure(EntityTypeBuilder<AttachmentCategoryEntity> builder)
    {
        builder.ToTable("AttachmentCategories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .IsRequired();

        // Chave alternativa para suportar FKs compostas com TenantId
        builder.HasAlternateKey(x => new { x.Id, x.TenantId })
            .HasName("UQ_AttachmentCategories_Id_Tenant");

        builder.Property(x => x.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("Description")
            .HasColumnType("NVARCHAR(300)")
            .HasMaxLength(300);

        builder.Property(x => x.DisplayOrder)
            .HasColumnName("DisplayOrder")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.IsSystem)
            .HasColumnName("IsSystem")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("IsActive")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasColumnName("CreatedBy")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2(7)")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnName("ModifiedBy");

        builder.Property(x => x.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2(7)");

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(x => new { x.TenantId, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_AttachmentCategories_Name_Tenant");

        builder.HasIndex(x => new { x.TenantId, x.DisplayOrder })
            .HasFilter("[IsDeleted] = 0")
            .HasDatabaseName("IX_AttachmentCategories_Tenant_Display");

        builder.HasIndex(x => x.TenantId)
            .HasFilter("[IsDeleted] = 0 AND [IsActive] = 1")
            .HasDatabaseName("IX_AttachmentCategories_Tenant_Active");

        // Constraints
        builder.HasCheckConstraint("CK_AttachmentCategories_Active_Deleted",
            "NOT ([IsActive] = 1 AND [IsDeleted] = 1)");
    }
}
