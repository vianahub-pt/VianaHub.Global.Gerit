using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade DocumentTypeTranslation.
/// Tabela: dbo.DocumentTypeTranslations — PK surrogate (Id).
/// </summary>
public class DocumentTypeTranslationsMapping : IEntityTypeConfiguration<DocumentTypeTranslationsEntity>
{
    public void Configure(EntityTypeBuilder<DocumentTypeTranslationsEntity> builder)
    {
        builder.ToTable("DocumentTypeTranslations", "dbo");

        // Chave Primária surrogate (Id)
        builder.HasKey(x => x.Id)
            .HasName("PK_DocumentTypeTranslations");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.DocumentTypeId)
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

        // Relacionamento FK configurado exclusivamente em DocumentTypeMapping
        // para evitar shadow FK DocumentTypeEntityId gerada por convenção do EF Core.

        // Constraint único composto (DocumentTypeId + LanguageCode)
        builder.HasIndex(x => new { x.DocumentTypeId, x.LanguageCode })
            .IsUnique()
            .HasDatabaseName("UQ_DocumentTypeTranslations_DocumentType_Language");

        // Constraint único composto (LanguageCode + Name)
        builder.HasIndex(x => new { x.LanguageCode, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_DocumentTypeTranslations_Language_Name");
    }
}
