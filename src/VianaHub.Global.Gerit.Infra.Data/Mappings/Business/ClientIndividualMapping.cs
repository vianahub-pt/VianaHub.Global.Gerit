using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

public class ClientIndividualMapping : IEntityTypeConfiguration<ClientIndividualEntity>
{
    public void Configure(EntityTypeBuilder<ClientIndividualEntity> builder)
    {
        builder.ToTable("ClientIndividuals");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("Id").IsRequired();

        builder.HasAlternateKey(x => new { x.Id, x.TenantId })
            .HasName("UQ_ClientIndividuals_Id_Tenant");

        builder.Property(x => x.TenantId).HasColumnName("TenantId").IsRequired();
        builder.Property(x => x.ClientId).HasColumnName("ClientId").IsRequired();

        builder.Property(x => x.FirstName).HasColumnName("FirstName").HasColumnType("NVARCHAR(100)").HasMaxLength(100).IsRequired();
        builder.Property(x => x.LastName).HasColumnName("LastName").HasColumnType("NVARCHAR(100)").HasMaxLength(100).IsRequired();
        builder.Property(x => x.PhoneNumber).HasColumnName("PhoneNumber").HasColumnType("NVARCHAR(50)").HasMaxLength(50);
        builder.Property(x => x.CellPhoneNumber).HasColumnName("CellPhoneNumber").HasColumnType("NVARCHAR(50)").HasMaxLength(50);
        builder.Property(x => x.IsWhatsapp).HasColumnName("IsWhatsapp").HasDefaultValue(false);
        builder.Property(x => x.Email).HasColumnName("Email").HasColumnType("NVARCHAR(500)").HasMaxLength(500);
        builder.Property(x => x.BirthDate).HasColumnName("BirthDate").HasColumnType("DATE");
        builder.Property(x => x.Gender).HasColumnName("Gender").HasColumnType("NVARCHAR(20)").HasMaxLength(20);
        builder.Property(x => x.DocumentType).HasColumnName("DocumentType").HasColumnType("NVARCHAR(50)").HasMaxLength(50);
        builder.Property(x => x.DocumentNumber).HasColumnName("DocumentNumber").HasColumnType("NVARCHAR(50)").HasMaxLength(50);
        builder.Property(x => x.Nationality).HasColumnName("Nationality").HasColumnType("CHAR(2)").HasMaxLength(2);

        builder.Property(x => x.IsActive).HasColumnName("IsActive").HasDefaultValue(true);
        builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(false);
        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("DATETIME2(7)").IsRequired();
        builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
        builder.Property(x => x.ModifiedAt).HasColumnName("ModifiedAt").HasColumnType("DATETIME2(7)");

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey(x => new { x.ClientId, x.TenantId })
            .HasPrincipalKey(x => new { x.Id, x.TenantId })
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(x => new { x.TenantId, x.ClientId })
            .HasFilter("[IsDeleted] = 0")
            .IsUnique()
            .HasDatabaseName("UX_ClientIndividuals_Client");

        builder.HasIndex(x => new { x.TenantId, x.DocumentType, x.DocumentNumber })
            .HasFilter("[DocumentNumber] IS NOT NULL")
            .IsUnique()
            .HasDatabaseName("UX_ClientIndividuals_Document");

        // Constraints
        builder.HasCheckConstraint("CK_ClientIndividuals_Active_Deleted",
            "NOT ([IsActive] = 1 AND [IsDeleted] = 1)");
    }
}
