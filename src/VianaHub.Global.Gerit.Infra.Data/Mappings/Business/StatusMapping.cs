using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade Status
/// Status das intervenções com suporte a Row Level Security
/// </summary>
public class StatusMapping : IEntityTypeConfiguration<StatusEntity>
{
    public void Configure(EntityTypeBuilder<StatusEntity> builder)
    {
        builder.ToTable("Status", "dbo");

        // Chave Primária
        builder.HasKey(x => x.Id)
            .HasName("PK_Status");

        builder.Property(x => x.Id)
            .HasColumnType("INT")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Chave alternativa para suportar FKs compostas com TenantId
        builder.HasAlternateKey(x => new { x.Id, x.TenantId })
            .HasName("UQ_Status_Id_Tenant");

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.StatusTypeId)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnType("NVARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("NVARCHAR(500)")
            .HasMaxLength(500)
            .IsRequired();

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

        // Índice único por TenantId e Name
        builder.HasIndex(x => new { x.TenantId, x.Name })
            .IsUnique()
            .HasDatabaseName("UQ_Status_Tenant_Name");

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_Status_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.StatusTypeId)
            .HasConstraintName("FK_Status_StatusType")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
