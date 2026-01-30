using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade InterventionContact
/// Contatos da intervenção com suporte a Row Level Security
/// </summary>
public class InterventionContactMapping : IEntityTypeConfiguration<InterventionContactEntity>
{
    public void Configure(EntityTypeBuilder<InterventionContactEntity> builder)
    {
        builder.ToTable("InterventionContacts", "dbo");

        // Chave Primária
        builder.HasKey(ic => ic.Id)
            .HasName("PK_InterventionContacts");

        builder.Property(ic => ic.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(ic => ic.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(ic => ic.InterventionId)
            .HasColumnName("InterventionId")
            .IsRequired();

        builder.Property(ic => ic.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(ic => ic.Email)
            .HasColumnName("Email")
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(ic => ic.Phone)
            .HasColumnName("Phone")
            .HasColumnType("NVARCHAR(30)")
            .HasMaxLength(30)
            .IsRequired(false);

        builder.Property(ic => ic.IsPrimary)
            .HasColumnName("IsPrimary")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(ic => ic.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(ic => ic.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(ic => ic.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ic => ic.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(ic => ic.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(ic => ic.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Índices
        builder.HasIndex(ic => ic.InterventionId)
            .HasDatabaseName("IX_InterventionContacts_InterventionId")
            .IncludeProperties(ic => ic.TenantId)
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos
        builder.HasOne(ic => ic.Tenant)
            .WithMany()
            .HasForeignKey(ic => ic.TenantId)
            .HasConstraintName("FK_InterventionContacts_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com Intervention configurado no InterventionMapping
    }
}
