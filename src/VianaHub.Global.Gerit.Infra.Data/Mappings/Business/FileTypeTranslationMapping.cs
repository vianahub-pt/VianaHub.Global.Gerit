using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade FileTypeTranslation.
/// Tabela: dbo.FileTypeTranslations — FK composta (FileTypeId, LanguageCode).
/// </summary>
public class FileTypeTranslationMapping : IEntityTypeConfiguration<FileTypeTranslationEntity>
{
    public void Configure(EntityTypeBuilder<FileTypeTranslationEntity> builder)
    {
        builder.ToTable("FileTypeTranslations", "dbo");

        // Chave Primária composta
        builder.HasKey(x => new { x.FileTypeId, x.LanguageCode })
            .HasName("PK_FileTypeTranslations");

        // Propriedades
        builder.Property(x => x.FileTypeId)
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

        // Constraint único composto (FileTypeId + LanguageCode) — já garantido pela PK,
        // mas a constraint nomeada é requerida pelo schema SQL.
        builder.HasIndex(x => new { x.FileTypeId, x.LanguageCode })
            .IsUnique()
            .HasDatabaseName("UQ_FileTypeTranslations_FileType_Language");

        // Constraint único composto (LanguageCode + Name) — nomes de tradução são únicos por idioma
        builder.HasIndex(x => new { x.LanguageCode, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_FileTypeTranslations_Language_Name");
    }
}
