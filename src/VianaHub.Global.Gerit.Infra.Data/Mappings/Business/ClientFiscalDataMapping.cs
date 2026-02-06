using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

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
        builder.HasKey(x => x.Id)
            .HasName("PK_ClientFiscalData");

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(x => x.ClientId)
            .HasColumnName("ClientId")
            .IsRequired();

        builder.Property(x => x.NIF)
            .HasColumnName("NIF")
            .HasColumnType("CHAR(9)")
            .HasMaxLength(9)
            .IsRequired();

        builder.Property(x => x.VATNumber)
            .HasColumnName("VATNumber")
            .HasColumnType("NVARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.CAE)
            .HasColumnName("CAE")
            .HasColumnType("NVARCHAR(10)")
            .HasMaxLength(10)
            .IsRequired(false);

        builder.Property(x => x.FiscalCountry)
            .HasColumnName("FiscalCountry")
            .HasColumnType("CHAR(2)")
            .HasMaxLength(2)
            .HasDefaultValue("PT")
            .IsRequired();

        builder.Property(x => x.IsVATRegistered)
            .HasColumnName("IsVATRegistered")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("IsActive")
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnName("IsDeleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
              .HasColumnName("CreatedBy")
              .HasColumnType("INT")
              .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME2")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        // Constraints únicos
        builder.HasIndex(x => x.NIF)
            .IsUnique()
            .HasDatabaseName("UQ_ClientFiscalData_NIF");

        // Índices
        builder.HasIndex(x => x.ClientId)
            .HasDatabaseName("IX_ClientFiscalData_ClientId")
            .IncludeProperties(x => x.TenantId)
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .HasConstraintName("FK_ClientFiscalData_Tenant")
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento Many-to-One com Client (ClientFiscalData.Client -> Client.FiscalData)
        builder.HasOne(x => x.Client)
            .WithMany(c => c.FiscalData)
            .HasForeignKey(x => x.ClientId)
            .HasConstraintName("FK_ClientFiscalData_Client")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
