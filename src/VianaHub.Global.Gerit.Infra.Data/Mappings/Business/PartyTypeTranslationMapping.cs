using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade PartyTypeTranslation.
/// Tabela: dbo.PartyTypeTranslations — FK composta (PartyTypeId, LanguageCode).
/// </summary>
public class PartyTypeTranslationMapping : IEntityTypeConfiguration<PartyTypeTranslationEntity>
{
    public void Configure(EntityTypeBuilder<PartyTypeTranslationEntity> builder)
    {
        builder.ToTable("PartyTypeTranslations", "dbo");

        // Chave Primária composta
        builder.HasKey(x => new { x.PartyTypeId, x.LanguageCode })
            .HasName("PK_PartyTypeTranslations");

        // Propriedades
        builder.Property(x => x.PartyTypeId)
            .HasColumnType("TINYINT")
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

        // Constraint único composto (PartyTypeId + LanguageCode) — já garantido pela PK,
        // mas a constraint nomeada é requerida pelo schema SQL.
        builder.HasIndex(x => new { x.PartyTypeId, x.LanguageCode })
            .IsUnique()
            .HasDatabaseName("UQ_PartyTypeTranslations_PartyType_Language");

        // Constraint único composto (LanguageCode + Name) — nomes de tradução são únicos por idioma
        builder.HasIndex(x => new { x.LanguageCode, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_PartyTypeTranslations_Language_Name");
    }
}
