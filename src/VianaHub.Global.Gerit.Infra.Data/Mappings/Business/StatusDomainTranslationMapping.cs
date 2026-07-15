using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade StatusDomainTranslation.
/// Tabela: dbo.StatusDomainTranslations — FK composta (StatusDomainId, LanguageCode).
/// </summary>
public class StatusDomainTranslationMapping : IEntityTypeConfiguration<StatusDomainTranslationEntity>
{
    public void Configure(EntityTypeBuilder<StatusDomainTranslationEntity> builder)
    {
        builder.ToTable("StatusDomainTranslations", "dbo");

        // Chave Primária composta
        builder.HasKey(x => new { x.StatusDomainId, x.LanguageCode })
            .HasName("PK_StatusDomainTranslations");

        // Propriedades
        builder.Property(x => x.StatusDomainId)
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

        // Constraint único composto (StatusDomainId + LanguageCode) — já garantido pela PK,
        // mas a constraint nomeada é requerida pelo schema SQL.
        builder.HasIndex(x => new { x.StatusDomainId, x.LanguageCode })
            .IsUnique()
            .HasDatabaseName("UQ_StatusDomainTranslations_StatusDomain_Language");

        // Constraint único composto (LanguageCode + Name) — nomes de tradução são únicos por idioma
        builder.HasIndex(x => new { x.LanguageCode, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_StatusDomainTranslations_Language_Name");
    }
}
