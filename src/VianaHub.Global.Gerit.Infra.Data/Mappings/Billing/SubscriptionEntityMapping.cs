using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Billing;

public class SubscriptionEntityMapping : IEntityTypeConfiguration<SubscriptionEntity>
{
    public void Configure(EntityTypeBuilder<SubscriptionEntity> builder)
    {
        builder.ToTable("Subscriptions", "dbo");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(x => x.PlanId)
            .HasColumnName("PlanId")
            .IsRequired();

        builder.Property(x => x.StripeId)
            .HasColumnName("StripeId")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.CurrentPeriodStart)
            .HasColumnName("CurrentPeriodStart")
            .IsRequired();

        builder.Property(x => x.CurrentPeriodEnd)
            .HasColumnName("CurrentPeriodEnd")
            .IsRequired();

        builder.Property(x => x.TrialStart)
            .HasColumnName("TrialStart")
            .IsRequired(false);

        builder.Property(x => x.TrialEnd)
            .HasColumnName("TrialEnd")
            .IsRequired(false);

        builder.Property(x => x.CancelAtPeriodEnd)
            .HasColumnName("CancelAtPeriodEnd")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.CanceledAt)
            .HasColumnName("CanceledAt")
            .IsRequired(false);

        builder.Property(x => x.CancellationReason)
            .HasColumnName("CancellationReason")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.StripeCustomerId)
            .HasColumnName("StripeCustomerId")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.IsActive)
            .HasColumnName("IsActive")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.IsDeleted)
            .HasColumnName("IsDeleted")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedBy)
              .HasColumnName("CreatedBy")
              .HasColumnType("INT")
              .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Constraint: Se IsDeleted = 1, então IsActive = 0
        builder.HasCheckConstraint("CK_Subscriptions_DeletedImpliesInactive", "[IsDeleted] = 0 OR [IsActive] = 0");

        // Navegação - Relacionamento com Tenant
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Navegação - Relacionamento com Plan
        builder.HasOne(x => x.Plan)
            .WithMany(p => p.Subscriptions)
            .HasForeignKey(x => x.PlanId)
            .OnDelete(DeleteBehavior.Restrict);

        // Chave alternativa (TenantId, Id)
        builder.HasIndex(x => new { x.TenantId, x.Id })
            .IsUnique()
            .HasDatabaseName("AK_Subscriptions_TenantId_Id");
    }
}
