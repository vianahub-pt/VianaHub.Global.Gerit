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
            .HasColumnType("DATETIME2")
            .IsRequired();

        builder.Property(x => x.CurrentPeriodEnd)
            .HasColumnName("CurrentPeriodEnd")
            .HasColumnType("DATETIME2")
            .IsRequired();

        builder.Property(x => x.TrialStart)
            .HasColumnName("TrialStart")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        builder.Property(x => x.TrialEnd)
            .HasColumnName("TrialEnd")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        builder.Property(x => x.CancelAtPeriodEnd)
            .HasColumnName("CancelAtPeriodEnd")
            .HasColumnType("BIT")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.CanceledAt)
            .HasColumnName("CanceledAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        builder.Property(x => x.CancellationReason)
            .HasColumnName("CancellationReason")
            .HasColumnType("NVARCHAR(500)")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.StripeCustomerId)
            .HasColumnName("StripeCustomerId")
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
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
            .HasConstraintName("FK_Subscriptions_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        // Navegação - Relacionamento com Plan
        builder.HasOne(x => x.Plan)
            .WithMany(p => p.Subscriptions)
            .HasForeignKey(x => x.PlanId)
            .HasConstraintName("FK_Subscriptions_Plan")
            .OnDelete(DeleteBehavior.Restrict);

        // Chave alternativa (TenantId, Id)
        builder.HasIndex(x => new { x.TenantId, x.Id })
            .IsUnique()
            .HasDatabaseName("UQ_Subscriptions_TenantId_Id");

        // Constraint Única: Garantir que só pode haver um registro ativo por tenant
        builder.HasIndex(x => new { x.TenantId, x.IsActive })
            .IsUnique()
            .HasDatabaseName("UQ_Subscriptions_Tenant_Active");

        // Índice para performance em consultas por PlanId
        builder.HasIndex(x => x.PlanId)
            .HasDatabaseName("IX_Subscriptions_PlanId");
    }
}
