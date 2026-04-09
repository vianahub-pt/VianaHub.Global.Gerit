using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class VisitAttachmentMapping : IEntityTypeConfiguration<VisitAttachmentEntity>
{
    public void Configure(EntityTypeBuilder<VisitAttachmentEntity> builder)
    {
        builder.ToTable("VisitAttachments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .IsRequired();

        // Chave alternativa para suportar FKs compostas com TenantId
        builder.HasAlternateKey(x => new { x.Id, x.TenantId })
            .HasName("UQ_VisitAttachments_Id_Tenant");

        builder.Property(x => x.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(x => x.FileTypeId)
            .HasColumnName("FileTypeId")
            .IsRequired();

        builder.Property(x => x.VisitId)
            .HasColumnName("VisitId")
            .IsRequired();

        builder.Property(x => x.AttachmentCategoryId)
            .HasColumnName("AttachmentCategoryId")
            .IsRequired();

        builder.Property(x => x.PublicId)
            .HasColumnName("PublicId")
            .HasDefaultValueSql("NEWID()")
            .IsRequired();

        builder.Property(x => x.S3Key)
            .HasColumnName("S3Key")
            .HasColumnType("NVARCHAR(500)")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.FileName)
            .HasColumnName("FileName")
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.FileSizeBytes)
            .HasColumnName("FileSizeBytes")
            .IsRequired();

        builder.Property(x => x.DisplayOrder)
            .HasColumnName("DisplayOrder")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.IsPrimary)
            .HasColumnName("IsPrimary")
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

        builder.HasOne(x => x.FileType)
            .WithMany()
            .HasForeignKey(x => x.FileTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Visit)
            .WithMany()
            .HasForeignKey(x => new { x.VisitId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AttachmentCategory)
            .WithMany()
            .HasForeignKey(x => new { x.AttachmentCategoryId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(x => new { x.TenantId, x.VisitId })
            .HasFilter("[IsDeleted] = 0")
            .HasDatabaseName("IX_VisitAttachments_Tenant_Visit")
            .IncludeProperties(x => new { x.AttachmentCategoryId, x.DisplayOrder, x.IsPrimary, x.FileTypeId });

        builder.HasIndex(x => new { x.TenantId, x.FileTypeId })
            .HasFilter("[IsDeleted] = 0")
            .HasDatabaseName("IX_VisitAttachments_FileTypeId");

        builder.HasIndex(x => new { x.TenantId, x.PublicId })
            .IsUnique()
            .HasDatabaseName("UX_VisitAttachments_PublicId");

        builder.HasIndex(x => new { x.TenantId, x.S3Key })
            .HasFilter("[IsDeleted] = 0")
            .IsUnique()
            .HasDatabaseName("UX_VisitAttachments_S3Key");

        builder.HasIndex(x => new { x.TenantId, x.VisitId })
            .HasFilter("[IsPrimary] = 1 AND [IsDeleted] = 0 AND [IsActive] = 1")
            .IsUnique()
            .HasDatabaseName("UX_VisitAttachments_Primary");

        // Constraints
        builder.HasCheckConstraint("CK_VisitAttachments_Active_Deleted",
            "NOT ([IsActive] = 1 AND [IsDeleted] = 1)");

        builder.HasCheckConstraint("CK_VisitAttachments_FileSizeBytes",
            "[FileSizeBytes] > 0");
    }
}
