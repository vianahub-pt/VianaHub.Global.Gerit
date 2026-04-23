using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade Employee
/// Membros do time com suporte a Row Level Security
/// </summary>
public class EmployeeMapping : IEntityTypeConfiguration<EmployeeEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.ToTable("Employees", "dbo");

        // Chave Prim�ria
        builder.HasKey(tm => tm.Id)
            .HasName("PK_Employees");

        builder.Property(tm => tm.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Chave alternativa para suportar FKs compostas com TenantId
        builder.HasAlternateKey(tm => new { tm.Id, tm.TenantId })
            .HasName("UQ_Employees_Id_Tenant");

        // Propriedades
        builder.Property(tm => tm.TenantId)
            .IsRequired();

        builder.Property(tm => tm.Name)
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(tm => tm.TaxNumber)
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired(false);

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
        builder.HasOne(tm => tm.Tenant)
            .WithMany()
            .HasForeignKey(tm => tm.TenantId)
            .HasConstraintName("FK_Employees_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(tm => tm.Contacts)
            .WithOne(tmc => tmc.Employee)
            .HasForeignKey(tmc => tmc.EmployeeId)
            .HasConstraintName("FK_EmployeeContacts_Employee")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(tm => tm.Addresses)
            .WithOne(x => x.Employee)
            .HasForeignKey(x => x.EmployeeId)
            .HasConstraintName("FK_EmployeeAddresses_Employee")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
