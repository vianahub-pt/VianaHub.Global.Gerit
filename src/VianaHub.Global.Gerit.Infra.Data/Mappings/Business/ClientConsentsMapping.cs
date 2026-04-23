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

        builder.Property(x => x.TenantId)
            .IsRequired();
        
        builder.Property(x => x.ClientId)
            .IsRequired();
        
        builder.Property(x => x.ConsentTypeId)
            .IsRequired();
        
        builder.Property(x => x.Granted)
            .HasDefaultValue(false);
        
        builder.Property(x => x.GrantedDate)
            .HasColumnType("DATETIME2(7)")
            .IsRequired();
        
        builder.Property(x => x.RevokedDate)
            .HasColumnType("DATETIME2(7)");
        
        builder.Property(x => x.Origin)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.IpAddress)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50);
        
        builder.Property(x => x.UserAgent)
            .HasColumnType("NVARCHAR(500)")
            .HasMaxLength(500);

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

