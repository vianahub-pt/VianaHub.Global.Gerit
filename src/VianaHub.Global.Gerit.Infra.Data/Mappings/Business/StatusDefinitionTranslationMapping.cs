using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade StatusDefinitionTranslation.
/// Tabela: dbo.StatusDefinitionTranslations — FK composta (StatusDefinitionId, TenantId, StatusDomainId).
/// </summary>
public class StatusDefinitionTranslationMapping : IEntityTypeConfiguration<StatusDefinitionTranslationEntity>
{
    public void Configure(EntityTypeBuilder<StatusDefinitionTranslationEntity> builder)
    {
        builder.ToTable("StatusDefinitionTranslations", "dbo");

        // Chave Primária
        builder.HasKey(x => x.Id)
            .HasName("PK_StatusDefinitionTranslations");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.StatusDomainId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.StatusDefinitionId)
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
            .IsRequired(false);

        // Constraint único composto (TenantId, StatusDefinitionId, LanguageCode) — uma tradução por idioma por definição
        builder.HasIndex(x => new { x.TenantId, x.StatusDefinitionId, x.LanguageCode })
            .IsUnique()
            .HasDatabaseName("UQ_StatusDefinitionTranslations_Status_Language");

        // Constraint único composto (TenantId, StatusDomainId, LanguageCode, Name) — nomes de tradução são únicos por tenant, domínio e idioma
        builder.HasIndex(x => new { x.TenantId, x.StatusDomainId, x.LanguageCode, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_StatusDefinitionTranslations_Tenant_Domain_Language_Name");
    }
}
