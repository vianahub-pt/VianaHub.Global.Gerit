using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade StatusDomainTranslation.
/// Tabela: dbo.StatusDomainTranslations — PK surrogate (Id).
/// </summary>
public class StatusDomainTranslationsMapping : IEntityTypeConfiguration<StatusDomainTranslationsEntity>
{
    public void Configure(EntityTypeBuilder<StatusDomainTranslationsEntity> builder)
    {
        builder.ToTable("StatusDomainTranslations", "dbo");

        // Chave Primária surrogate (Id)
        builder.HasKey(x => x.Id)
            .HasName("PK_StatusDomainTranslations");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.StatusDomainId)
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

        // Relacionamento FK configurado exclusivamente em StatusDomainMapping
        // para evitar shadow FK StatusDomainEntityId gerada por convenção do EF Core.

        // Constraint único composto (StatusDomainId + LanguageCode)
        builder.HasIndex(x => new { x.StatusDomainId, x.LanguageCode })
            .IsUnique()
            .HasDatabaseName("UQ_StatusDomainTranslations_StatusDomain_Language");

        // Constraint único composto (LanguageCode + Name)
        builder.HasIndex(x => new { x.LanguageCode, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_StatusDomainTranslations_Language_Name");
    }
}
