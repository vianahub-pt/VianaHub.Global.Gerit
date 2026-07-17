using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade PartyTypeTranslation.
/// Tabela: dbo.PartyTypeTranslations — PK surrogate (Id).
/// </summary>
public class PartyTypeTranslationsMapping : IEntityTypeConfiguration<PartyTypeTranslationsEntity>
{
    public void Configure(EntityTypeBuilder<PartyTypeTranslationsEntity> builder)
    {
        builder.ToTable("PartyTypeTranslations", "dbo");

        // Chave Primária surrogate (Id)
        builder.HasKey(x => x.Id)
            .HasName("PK_PartyTypeTranslations");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.PartyTypeId)
            .HasColumnType("TINYINT")
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
        builder.HasOne(x => x.PartyType)
            .WithMany()
            .HasForeignKey(x => x.PartyTypeId)
            .HasConstraintName("FK_PartyTypeTranslations_PartyType")
            .OnDelete(DeleteBehavior.Restrict);

        // Constraint único composto (PartyTypeId + LanguageCode)
        builder.HasIndex(x => new { x.PartyTypeId, x.LanguageCode })
            .IsUnique()
            .HasDatabaseName("UQ_PartyTypeTranslations_PartyType_Language");

        // Constraint único composto (LanguageCode + Name) — nomes de tradução são únicos por idioma
        builder.HasIndex(x => new { x.LanguageCode, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_PartyTypeTranslations_Language_Name");
    }
}
