using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class ClientCompanyMapping : IEntityTypeConfiguration<ClientCompanyEntity>
{
    public void Configure(EntityTypeBuilder<ClientCompanyEntity> builder)
    {
        builder.ToTable("ClientCompanies");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("Id").IsRequired();

        builder.HasAlternateKey(x => new { x.Id, x.TenantId })
            .HasName("UQ_ClientCompanies_Id_Tenant");

        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.ClientId)
            .IsRequired();

        builder.Property(x => x.LegalName)
            .HasColumnType("NVARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.TradeName)
            .HasColumnType("NVARCHAR(200)")
            .HasMaxLength(200);

        builder.Property(x => x.Site)
            .HasColumnType("NVARCHAR(500)")
            .HasMaxLength(500);

        builder.Property(x => x.CompanyRegistration)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50);

        builder.Property(x => x.CAE)
            .HasColumnType("NVARCHAR(10)")
            .HasMaxLength(10);

        builder.Property(x => x.NumberOfEmployee);

        builder.Property(x => x.LegalRepresentative)
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150);

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
            .WithOne(c => c.Company)
            .HasForeignKey<ClientCompanyEntity>(nameof(ClientCompanyEntity.ClientId), nameof(ClientCompanyEntity.TenantId))
            .HasPrincipalKey<ClientEntity>(nameof(ClientEntity.Id), nameof(ClientEntity.TenantId))
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasIndex(x => new { x.TenantId, x.ClientId })
            .HasFilter("[IsDeleted] = 0")
            .IsUnique()
            .HasDatabaseName("UX_ClientCompanies_Client");

        builder.HasCheckConstraint("CK_ClientCompanies_Active_Deleted",
            "NOT ([IsActive] = 1 AND [IsDeleted] = 1)");
    }
}

