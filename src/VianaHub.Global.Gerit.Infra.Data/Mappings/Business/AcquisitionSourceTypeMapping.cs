using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade AcquisitionSourceType (catálogo global).
/// Tabela: dbo.AcquisitionSourceTypes — Id INT (PK identity), Code NVARCHAR(50) UK.
/// Não é multi-tenant: dados globais compartilhados entre tenants.
/// </summary>
public class AcquisitionSourceTypeMapping : IEntityTypeConfiguration<AcquisitionSourceTypeEntity>
{
    public void Configure(EntityTypeBuilder<AcquisitionSourceTypeEntity> builder)
    {
        builder.ToTable("AcquisitionSourceTypes", "dbo");

        builder.HasKey(x => x.Id)
            .HasName("PK_AcquisitionSourceTypes");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .HasDatabaseName("UQ_AcquisitionSourceTypes_Code")
            .IsUnique();

        builder.Property(x => x.Name)
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("UQ_AcquisitionSourceTypes_Name")
            .IsUnique();

        builder.Property(x => x.Description)
            .HasColumnType("NVARCHAR(300)")
            .HasMaxLength(300)
            .IsRequired(false);

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

        // Relacionamento com Translations (1:N)
        builder.HasMany(x => x.Translations)
            .WithOne(x => x.AcquisitionSourceType)
            .HasForeignKey(x => x.AcquisitionSourceTypeId)
            .HasConstraintName("FK_AcquisitionSourceTypeTranslations_AcquisitionSourceTypes")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
