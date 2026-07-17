using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade AddressTypeTranslation.
/// Tabela: dbo.AddressTypeTranslations — PK surrogate (Id).
/// Name NVARCHAR(200), Description NVARCHAR(500) conforme SQL.
/// </summary>
public class AddressTypeTranslationsMapping : IEntityTypeConfiguration<AddressTypeTranslationsEntity>
{
    public void Configure(EntityTypeBuilder<AddressTypeTranslationsEntity> builder)
    {
        builder.ToTable("AddressTypeTranslations", "dbo");

        // Chave Primária surrogate (Id)
        builder.HasKey(x => x.Id)
            .HasName("PK_AddressTypeTranslations");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

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
            .IsRequired(false);

        // Relacionamento FK
        builder.HasOne(x => x.AddressType)
            .WithMany()
            .HasForeignKey(x => x.AddressTypeId)
            .HasConstraintName("FK_AddressTypeTranslations_AddressType")
            .OnDelete(DeleteBehavior.Restrict);

        // Constraint único composto (AddressTypeId + LanguageCode)
        builder.HasIndex(x => new { x.AddressTypeId, x.LanguageCode })
            .IsUnique()
            .HasDatabaseName("UQ_AddressTypeTranslations_AddressType_Language");

        // Constraint único composto (LanguageCode + Name)
        builder.HasIndex(x => new { x.LanguageCode, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_AddressTypeTranslations_Language_Name");
    }
}
