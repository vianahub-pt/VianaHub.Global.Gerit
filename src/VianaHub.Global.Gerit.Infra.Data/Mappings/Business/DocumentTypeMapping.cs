using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade DocumentType (Tipos de Documento: BI, Passaporte, NIF, etc.).
/// Tabela: dbo.DocumentTypes
/// Não é Aggregate Root.
/// </summary>
public class DocumentTypeMapping : IEntityTypeConfiguration<DocumentTypeEntity>
{
    public void Configure(EntityTypeBuilder<DocumentTypeEntity> builder)
    {
        builder.ToTable("DocumentTypes", "dbo");

        // Chave Primária
        builder.HasKey(x => x.Id)
            .HasName("PK_DocumentTypes");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.Code)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        // FK para a tabela de traduções DocumentTypeTranslations
        builder.HasMany(x => x.Translations)
            .WithOne(x => x.DocumentType)
            .HasForeignKey(x => x.DocumentTypeId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_DocumentTypeTranslations_DocumentTypes_DocumentTypeId");

        builder.Property(x => x.IsActive)
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.ModifiedAt)
            .HasColumnType("DATETIME2")
            .IsRequired(false);
    }
}
