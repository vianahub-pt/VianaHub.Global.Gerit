using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade FileTypeTranslation.
/// Tabela: dbo.FileTypeTranslations — PK surrogate (Id).
/// </summary>
public class FileTypeTranslationsMapping : IEntityTypeConfiguration<FileTypeTranslationsEntity>
{
    public void Configure(EntityTypeBuilder<FileTypeTranslationsEntity> builder)
    {
        builder.ToTable("FileTypeTranslations", "dbo");

        // Chave Primária surrogate (Id)
        builder.HasKey(x => x.Id)
            .HasName("PK_FileTypeTranslations");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.FileTypeId)
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
        builder.HasOne(x => x.FileType)
            .WithMany()
            .HasForeignKey(x => x.FileTypeId)
            .HasConstraintName("FK_FileTypeTranslations_FileType")
            .OnDelete(DeleteBehavior.Restrict);

        // Constraint único composto (FileTypeId + LanguageCode)
        builder.HasIndex(x => new { x.FileTypeId, x.LanguageCode })
            .IsUnique()
            .HasDatabaseName("UQ_FileTypeTranslations_FileType_Language");

        // Constraint único composto (LanguageCode + Name)
        builder.HasIndex(x => new { x.LanguageCode, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_FileTypeTranslations_Language_Name");
    }
}
