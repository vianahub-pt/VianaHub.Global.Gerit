using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade AddressTypeTranslation.
/// Tabela: dbo.AddressTypeTranslations — FK composta (AddressTypeId, LanguageCode).
/// Description usa NVARCHAR(500) — maior que os 300 das demais traduções do domínio.
/// </summary>
public class AddressTypeTranslationMapping : IEntityTypeConfiguration<AddressTypeTranslationEntity>
{
    public void Configure(EntityTypeBuilder<AddressTypeTranslationEntity> builder)
    {
        builder.ToTable("AddressTypeTranslations", "dbo");

        // Chave Primária composta
        builder.HasKey(x => new { x.AddressTypeId, x.LanguageCode })
            .HasName("PK_AddressTypeTranslations");

        // Propriedades
        builder.Property(x => x.AddressTypeId)
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

        // Constraint único composto (AddressTypeId + LanguageCode) — já garantido pela PK,
        // mas a constraint nomeada é requerida pelo schema SQL.
        builder.HasIndex(x => new { x.AddressTypeId, x.LanguageCode })
            .IsUnique()
            .HasDatabaseName("UQ_AddressTypeTranslations_AddressType_Language");

        // Constraint único composto (LanguageCode + Name) — nomes de tradução são únicos por idioma
        builder.HasIndex(x => new { x.LanguageCode, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_AddressTypeTranslations_Language_Name");
    }
}
