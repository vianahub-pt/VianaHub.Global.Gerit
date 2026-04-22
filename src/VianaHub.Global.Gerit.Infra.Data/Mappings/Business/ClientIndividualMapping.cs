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
        
        builder.Property(x => x.Id)
            .IsRequired();

        builder.HasAlternateKey(x => new { x.Id, x.TenantId })
            .HasName("UQ_ClientIndividuals_Id_Tenant");

        builder.Property(x => x.TenantId)
            .IsRequired();
        
        builder.Property(x => x.ClientId)
            .IsRequired();

        builder.Property(x => x.FirstName)
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.LastName)
            .HasColumnType("NVARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.CellPhoneNumber)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50);

        builder.Property(x => x.IsWhatsapp)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnType("NVARCHAR(500)")
            .HasMaxLength(100);

        builder.Property(x => x.BirthDate)
            .HasColumnType("DATE");
        
        builder.Property(x => x.Gender)
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20);
        
        builder.Property(x => x.DocumentType)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50);
        
        builder.Property(x => x.DocumentNumber)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50);
        
        builder.Property(x => x.Nationality)
            .HasColumnType("CHAR(2)")
            .HasMaxLength(2);

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
        builder.HasOne(x => x.Client)
            .WithOne(c => c.Individual)
            .HasForeignKey<ClientIndividualEntity>(nameof(ClientIndividualEntity.ClientId), nameof(ClientIndividualEntity.TenantId))
            .HasPrincipalKey<ClientEntity>(nameof(ClientEntity.Id), nameof(ClientEntity.TenantId))
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

