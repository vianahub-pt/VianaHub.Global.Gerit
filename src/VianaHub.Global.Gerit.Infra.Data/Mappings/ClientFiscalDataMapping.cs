using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings;

/// <summary>
/// Mapeamento da entidade ClientFiscalData
/// Dados fiscais do cliente com suporte a Row Level Security
/// </summary>
public class ClientFiscalDataMapping : IEntityTypeConfiguration<ClientFiscalDataEntity>
{
    public void Configure(EntityTypeBuilder<ClientFiscalDataEntity> builder)
    {
        builder.ToTable("ClientFiscalData", "dbo");

        // Chave Primária
        builder.HasKey(cfd => cfd.Id)
            .HasName("PK_ClientFiscalData");

        builder.Property(cfd => cfd.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(cfd => cfd.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(cfd => cfd.ClientId)
            .HasColumnName("ClientId")
            .IsRequired();

        builder.Property(cfd => cfd.NIF)
            .HasColumnName("NIF")
            .HasColumnType("CHAR(9)")
            .HasMaxLength(9)
            .IsRequired();

        builder.Property(cfd => cfd.VATNumber)
            .HasColumnName("VATNumber")
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(cfd => cfd.CAE)
            .HasColumnName("CAE")
            .HasColumnType("NVARCHAR(10)")
            .HasMaxLength(10)
            .IsRequired(false);

        builder.Property(cfd => cfd.FiscalCountry)
            .HasColumnName("FiscalCountry")
            .HasColumnType("CHAR(2)")
            .HasMaxLength(2)
            .HasDefaultValue("PT")
            .IsRequired();

        builder.Property(cfd => cfd.IsVATRegistered)
            .HasColumnName("IsVATRegistered")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(cfd => cfd.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(cfd => cfd.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(cfd => cfd.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(cfd => cfd.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(cfd => cfd.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(cfd => cfd.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Constraints únicos
        builder.HasIndex(cfd => cfd.NIF)
            .IsUnique()
            .HasDatabaseName("UQ_ClientFiscalData_NIF");

        // Índices
        builder.HasIndex(cfd => cfd.ClientId)
            .HasDatabaseName("IX_ClientFiscalData_ClientId")
            .IncludeProperties(cfd => cfd.TenantId)
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos
        builder.HasOne(cfd => cfd.Tenant)
            .WithMany()
            .HasForeignKey(cfd => cfd.TenantId)
            .HasConstraintName("FK_ClientFiscalData_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento Many-to-One com Client (ClientFiscalData.Client -> Client.FiscalData)
        builder.HasOne(cfd => cfd.Client)
            .WithMany(c => c.FiscalData)
            .HasForeignKey(cfd => cfd.ClientId)
            .HasConstraintName("FK_ClientFiscalData_Client")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
