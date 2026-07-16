using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Billing;

/// <summary>
/// Mapeamento da entidade SubscriptionPlanTranslation.
/// Tabela: dbo.SubscriptionPlanTranslations — FK composta (SubscriptionPlanId, LanguageCode).
/// </summary>
public class SubscriptionPlanTranslationMapping : IEntityTypeConfiguration<SubscriptionPlanTranslationEntity>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlanTranslationEntity> builder)
    {
        builder.ToTable("SubscriptionPlanTranslations", "dbo");

        // Chave Primária composta
        builder.HasKey(x => new { x.SubscriptionPlanId, x.LanguageCode })
            .HasName("PK_SubscriptionPlanTranslations");

        // Propriedades
        builder.Property(x => x.SubscriptionPlanId)
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

        // Constraint único composto (SubscriptionPlanId + LanguageCode) — já garantido pela PK,
        // mas a constraint nomeada é requerida pelo schema SQL.
        builder.HasIndex(x => new { x.SubscriptionPlanId, x.LanguageCode })
            .IsUnique()
            .HasDatabaseName("UQ_SubscriptionPlanTranslations_Plan_Language");

        // Relacionamento com SubscriptionPlan
        builder.HasOne(x => x.SubscriptionPlan)
            .WithMany()
            .HasForeignKey(x => x.SubscriptionPlanId)
            .HasConstraintName("FK_SubscriptionPlanTranslations_SubscriptionPlan")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
