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

        builder.Property(x => x.TenantId).HasColumnName("TenantId").IsRequired();
        builder.Property(x => x.ClientId).HasColumnName("ClientId").IsRequired();

        builder.Property(x => x.LegalName).HasColumnName("LegalName").HasColumnType("NVARCHAR(200)").HasMaxLength(200).IsRequired();
        builder.Property(x => x.TradeName).HasColumnName("TradeName").HasColumnType("NVARCHAR(200)").HasMaxLength(200);
        builder.Property(x => x.PhoneNumber).HasColumnName("PhoneNumber").HasColumnType("NVARCHAR(50)").HasMaxLength(50);
        builder.Property(x => x.CellPhoneNumber).HasColumnName("CellPhoneNumber").HasColumnType("NVARCHAR(50)").HasMaxLength(50);
        builder.Property(x => x.IsWhatsapp).HasColumnName("IsWhatsapp").HasDefaultValue(false);
        builder.Property(x => x.Email).HasColumnName("Email").HasColumnType("NVARCHAR(500)").HasMaxLength(500);
        builder.Property(x => x.Site).HasColumnName("Site").HasColumnType("NVARCHAR(500)").HasMaxLength(500);
        builder.Property(x => x.CompanyRegistration).HasColumnName("CompanyRegistration").HasColumnType("NVARCHAR(50)").HasMaxLength(50);
        builder.Property(x => x.CAE).HasColumnName("CAE").HasColumnType("NVARCHAR(10)").HasMaxLength(10);
        builder.Property(x => x.NumberOfEmployee).HasColumnName("NumberOfEmployee");
        builder.Property(x => x.LegalRepresentative).HasColumnName("LegalRepresentative").HasColumnType("NVARCHAR(150)").HasMaxLength(150);

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
            .WithMany()
            .HasForeignKey(x => new { x.ClientId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.TenantId, x.ClientId })
            .HasFilter("[IsDeleted] = 0")
            .IsUnique()
            .HasDatabaseName("UX_ClientCompanies_Client");

        builder.HasCheckConstraint("CK_ClientCompanies_Active_Deleted",
            "NOT ([IsActive] = 1 AND [IsDeleted] = 1)");
    }
}
