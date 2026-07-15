using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade DocumentTypeTranslation.
/// Tabela: dbo.DocumentTypeTranslations — FK composta (DocumentTypeId, LanguageCode).
/// </summary>
public class DocumentTypeTranslationMapping : IEntityTypeConfiguration<DocumentTypeTranslationEntity>
{
    public void Configure(EntityTypeBuilder<DocumentTypeTranslationEntity> builder)
    {
        builder.ToTable("DocumentTypeTranslations", "dbo");

        // Chave Primária composta
        builder.HasKey(x => new { x.DocumentTypeId, x.LanguageCode })
            .HasName("PK_DocumentTypeTranslations");

        // Propriedades
        builder.Property(x => x.DocumentTypeId)
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

        // Constraint único composto (DocumentTypeId + LanguageCode) — já garantido pela PK,
        // mas a constraint nomeada é requerida pelo schema SQL.
        builder.HasIndex(x => new { x.DocumentTypeId, x.LanguageCode })
            .IsUnique()
            .HasDatabaseName("UQ_DocumentTypeTranslations_DocumentType_Language");

        // Constraint único composto (LanguageCode + Name) — nomes de tradução são únicos por idioma
        builder.HasIndex(x => new { x.LanguageCode, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_DocumentTypeTranslations_Language_Name");
    }
}
