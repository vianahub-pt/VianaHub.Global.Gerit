using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade AcquisitionSourceTypeTranslation.
/// Tabela: dbo.AcquisitionSourceTypeTranslations — PK surrogate (Id).
/// </summary>
public class AcquisitionSourceTypeTranslationMapping : IEntityTypeConfiguration<AcquisitionSourceTypeTranslationEntity>
{
    public void Configure(EntityTypeBuilder<AcquisitionSourceTypeTranslationEntity> builder)
    {
        builder.ToTable("AcquisitionSourceTypeTranslations", "dbo");

        // Chave Primária surrogate (Id)
        builder.HasKey(x => x.Id)
            .HasName("PK_AcquisitionSourceTypeTranslations");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.AcquisitionSourceTypeId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.LanguageCode)
            .HasColumnType("NVARCHAR(10)")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("NVARCHAR(300)")
            .HasMaxLength(300)
            .IsRequired(false);

        // Relacionamento FK
        builder.HasOne(x => x.AcquisitionSourceType)
            .WithMany()
            .HasForeignKey(x => x.AcquisitionSourceTypeId)
            .HasConstraintName("FK_AcquisitionSourceTypeTranslations_AcquisitionSourceType")
            .OnDelete(DeleteBehavior.Restrict);

        // Constraint único composto (AcquisitionSourceTypeId + LanguageCode)
        builder.HasIndex(x => new { x.AcquisitionSourceTypeId, x.LanguageCode })
            .IsUnique()
            .HasDatabaseName("UQ_AcquisitionSourceTypeTranslations_AcquisitionSourceType_Language");

        // Constraint único composto (LanguageCode + Name)
        builder.HasIndex(x => new { x.LanguageCode, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_AcquisitionSourceTypeTranslations_Language_Name");
    }
}
