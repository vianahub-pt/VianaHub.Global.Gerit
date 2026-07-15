using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade AcquisitionSourceTypeTranslation.
/// Tabela: dbo.AcquisitionSourceTypeTranslations — FK composta (AcquisitionSourceTypeId, LanguageCode).
/// </summary>
public class AcquisitionSourceTypeTranslationMapping : IEntityTypeConfiguration<AcquisitionSourceTypeTranslationEntity>
{
    public void Configure(EntityTypeBuilder<AcquisitionSourceTypeTranslationEntity> builder)
    {
        builder.ToTable("AcquisitionSourceTypeTranslations", "dbo");

        // Chave Primária composta
        builder.HasKey(x => new { x.AcquisitionSourceTypeId, x.LanguageCode })
            .HasName("PK_AcquisitionSourceTypeTranslations");

        // Propriedades
        builder.Property(x => x.AcquisitionSourceTypeId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.LanguageCode)
            .HasColumnType("NVARCHAR(10)")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnType("NVARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("NVARCHAR(500)")
            .HasMaxLength(500)
            .IsRequired();

        // Constraint único composto (AcquisitionSourceTypeId + LanguageCode) — já garantido pela PK,
        // mas a constraint nomeada é requerida pelo schema SQL.
        builder.HasIndex(x => new { x.AcquisitionSourceTypeId, x.LanguageCode })
            .IsUnique()
            .HasDatabaseName("UQ_AcquisitionSourceTypeTranslations_AcquisitionSourceType_Language");

        // Constraint único composto (LanguageCode + Name) — nomes de tradução são únicos por idioma
        builder.HasIndex(x => new { x.LanguageCode, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_AcquisitionSourceTypeTranslations_Language_Name");
    }
}
