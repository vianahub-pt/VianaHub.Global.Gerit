using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class ClientConsentsMapping : IEntityTypeConfiguration<ClientConsentsEntity>
{
    public void Configure(EntityTypeBuilder<ClientConsentsEntity> builder)
    {
        builder.ToTable("ClientConsents");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("Id").IsRequired();

        builder.HasAlternateKey(x => new { x.Id, x.TenantId })
            .HasName("UQ_ClientConsents_Id_Tenant");

        builder.Property(x => x.TenantId).HasColumnName("TenantId").IsRequired();
        builder.Property(x => x.ClientId).HasColumnName("ClientId").IsRequired();
        builder.Property(x => x.ConsentTypeId).HasColumnName("ConsentTypeId").IsRequired();
        builder.Property(x => x.Granted).HasColumnName("Granted").HasDefaultValue(false);
        builder.Property(x => x.GrantedDate).HasColumnName("GrantedDate").HasColumnType("DATETIME2(7)").IsRequired();
        builder.Property(x => x.RevokedDate).HasColumnName("RevokedDate").HasColumnType("DATETIME2(7)");
        builder.Property(x => x.Origin).HasColumnName("Origin").HasColumnType("NVARCHAR(50)").HasMaxLength(50).IsRequired();
        builder.Property(x => x.IpAddress).HasColumnName("IpAddress").HasColumnType("NVARCHAR(50)").HasMaxLength(50);
        builder.Property(x => x.UserAgent).HasColumnName("UserAgent").HasColumnType("NVARCHAR(500)").HasMaxLength(500);

        builder.Property(x => x.IsActive).HasColumnName("IsActive").HasDefaultValue(true);
        builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(false);
        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("DATETIME2(7)").IsRequired();
        builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
        builder.Property(x => x.ModifiedAt).HasColumnName("ModifiedAt").HasColumnType("DATETIME2(7)");

        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Client)
            .WithMany(c => c.Consents)
            .HasForeignKey(x => new { x.ClientId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ConsentType)
            .WithMany()
            .HasForeignKey(x => x.ConsentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ClientId, x.ConsentTypeId, x.TenantId })
            .HasDatabaseName("IX_ClientConsents_Client_ConsentType_Tenant");

        builder.HasIndex(x => x.TenantId)
            .HasDatabaseName("IX_ClientConsents_TenantId");

        builder.HasIndex(x => new { x.Granted, x.TenantId })
            .HasDatabaseName("IX_ClientConsents_Granted_Tenant");
    }
}

