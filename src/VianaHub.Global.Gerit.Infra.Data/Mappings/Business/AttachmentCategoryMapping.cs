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
            .IsRequired();

        // Chave alternativa para suportar FKs compostas com TenantId
        builder.HasAlternateKey(x => new { x.Id, x.TenantId })
            .HasName("UQ_AttachmentCategories_Id_Tenant");

        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("NVARCHAR(300)")
            .HasMaxLength(300);

        builder.Property(x => x.DisplayOrder)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.IsSystem)
            .HasDefaultValue(false)
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
