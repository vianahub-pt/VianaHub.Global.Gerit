using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Identity;

public class JwtKeyMapping : IEntityTypeConfiguration<JwtKeyEntity>
{
    public void Configure(EntityTypeBuilder<JwtKeyEntity> builder)
    {
        builder.ToTable("JwtKeys", "dbo");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TenantId)
            .IsRequired();
        
        builder.Property(x => x.KeyId)
            .IsRequired();
        
        builder.Property(x => x.PublicKey)
            .IsRequired();
        
        builder.Property(x => x.PrivateKeyEncrypted)
            .IsRequired();
        
        builder.Property(x => x.Algorithm)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.KeySize)
            .IsRequired();
        
        builder.Property(x => x.KeyType)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.RevokedReason)
            .HasMaxLength(500);
        
        builder.Property(x => x.UsageCount)
            .IsRequired();
        
        builder.Property(x => x.ActivatedAt);
        
        builder.Property(x => x.ExpiresAt)
            .IsRequired();
        
        builder.Property(x => x.LastUsedAt);
        
        builder.Property(x => x.NextRotationAt)
            .IsRequired();
        
        builder.Property(x => x.RevokedAt);
        
        builder.Property(x => x.LastValidatedAt);
        
        builder.Property(x => x.ValidationCount)
            .IsRequired();
        
        builder.Property(x => x.RotationPolicyDays)
            .IsRequired();
        
        builder.Property(x => x.OverlapPeriodDays)
            .IsRequired();
        
        builder.Property(x => x.MaxTokenLifetimeMinutes)
            .IsRequired();

        // PlainPrivateKey não deve ser persistido no banco - é apenas um campo temporário
        builder.Ignore(x => x.PlainPrivateKey);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("DATETIME2(7)")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.ModifiedAt)
            .HasColumnType("DATETIME2(7)")
            .IsRequired(false);

        // Relacionamentos
        builder.HasOne<TenantEntity>()
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(x => x.KeyId).IsUnique().HasDatabaseName("UQ_JwtKeys_KeyId");

        builder.HasIndex(x => new { x.IsActive, x.IsDeleted })
            .HasFilter("IsDeleted = 0")
            .HasDatabaseName("IX_JwtKeys_IsActive_IsDeleted");

        builder.HasIndex(x => x.NextRotationAt)
            .HasFilter("IsActive = 1 AND IsDeleted = 0 AND RevokedAt IS NULL")
            .HasDatabaseName("IX_JwtKeys_NextRotation");

        builder.HasIndex(x => x.ExpiresAt)
            .HasFilter("IsActive = 1 AND IsDeleted = 0 AND RevokedAt IS NULL")
            .HasDatabaseName("IX_JwtKeys_Expiration");

        builder.HasIndex(x => x.KeyId)
            .IncludeProperties(x => new { x.Algorithm, x.PublicKey })
            .HasFilter("IsDeleted = 0 AND RevokedAt IS NULL")
            .HasDatabaseName("IX_JwtKeys_KeyId_Lookup");
    }
}
