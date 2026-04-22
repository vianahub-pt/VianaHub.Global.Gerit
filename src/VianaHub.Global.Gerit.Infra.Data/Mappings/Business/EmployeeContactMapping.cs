using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade EmployeeContact
/// Contatos do membro do time com suporte a Row Level Security
/// </summary>
public class EmployeeContactMapping : IEntityTypeConfiguration<EmployeeContactEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeContactEntity> builder)
    {
        builder.ToTable("EmployeeContacts", "dbo");

        // Chave Prim�ria
        builder.HasKey(x => x.Id)
            .HasName("PK_EmployeeContacts");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.EmployeeId)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasColumnType("NVARCHAR(30)")
            .HasMaxLength(30)
            .IsRequired(false);

        builder.Property(x => x.IsPrimary)
            .HasColumnType("BIT")
            .HasDefaultValue(false)
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

        // �ndices
        builder.HasIndex(x => x.EmployeeId)
            .HasDatabaseName("IX_EmployeeContacts_EmployeeId")
            .IncludeProperties(x => x.TenantId)
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_EmployeeContacts_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com Employee configurado no EmployeeMapping
    }
}
