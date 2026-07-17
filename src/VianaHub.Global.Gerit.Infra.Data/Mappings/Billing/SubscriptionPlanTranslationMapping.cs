using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Billing;

/// <summary>
/// Mapeamento da entidade SubscriptionPlanTranslation.
/// Tabela: dbo.SubscriptionPlanTranslations — PK surrogate (Id).
/// </summary>
public class SubscriptionPlanTranslationMapping : IEntityTypeConfiguration<SubscriptionPlanTranslationEntity>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlanTranslationEntity> builder)
    {
        builder.ToTable("SubscriptionPlanTranslations", "dbo");

        // Chave Primária surrogate (Id)
        builder.HasKey(x => x.Id)
            .HasName("PK_SubscriptionPlanTranslations");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.SubscriptionPlanId)
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
            .HasColumnType("NVARCHAR(500)")
            .HasMaxLength(500)
            .IsRequired(false);

        // Relacionamento FK com SubscriptionPlan
        builder.HasOne(x => x.SubscriptionPlan)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.SubscriptionPlanId)
            .HasConstraintName("FK_SubscriptionPlanTranslations_SubscriptionPlan")
            .OnDelete(DeleteBehavior.Cascade);

        // Constraint único composto (SubscriptionPlanId + LanguageCode)
        builder.HasIndex(x => new { x.SubscriptionPlanId, x.LanguageCode })
            .IsUnique()
            .HasDatabaseName("UQ_SubscriptionPlanTranslations_SubscriptionPlan_Language");

        // Constraint único composto (LanguageCode + Name)
        builder.HasIndex(x => new { x.LanguageCode, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_SubscriptionPlanTranslations_Language_Name");
    }
}
