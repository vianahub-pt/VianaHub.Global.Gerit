using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Billing;

/// <summary>
/// Mapeamento da entidade TenantContact
/// Contatos do tenant com suporte a Row Level Security
/// </summary>
public class TenantContactPersonsMapping : IEntityTypeConfiguration<TenantContactPersonsEntity>
{
    public void Configure(EntityTypeBuilder<TenantContactPersonsEntity> builder)
    {
        builder.ToTable("TenantContactPersons", "dbo");

        // Chave Prim�ria
        builder.HasKey(x => x.Id)
            .HasName("PK_TenantContactPersons");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
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
            .HasColumnName("PhoneNumber")
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(x => x.JobTitle)
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(x => x.Department)
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(x => x.CellPhoneNumber)
            .HasColumnType("NVARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(x => x.IsCellPhoneWhatsapp)
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.IsPrimary)
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnType("BIT")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();
        builder.Property(x => x.CreatedBy)
              .HasColumnType("INT")
              .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("DATETIME2(7)")
            .HasDefaultValueSql("SYSUTCDATETIME()")
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(x => x.ModifiedAt)
            .HasColumnType("DATETIME2(7)")
            .IsRequired(false);

        // Indices
        // Email unico por tenant (apenas ativos e nao deletados)
        builder.HasIndex(x => new { x.TenantId, x.Email })
            .IsUnique()
            .HasDatabaseName("UX_TenantContactPersons_Email_Active")
            .HasFilter("[Email] IS NOT NULL AND [IsActive] = 1 AND [IsDeleted] = 0");

        // Apenas um contato primario por tenant
        builder.HasIndex(x => x.TenantId)
            .IsUnique()
            .HasDatabaseName("UX_TenantContactPersons_Primary")
            .HasFilter("[IsPrimary] = 1 AND [IsActive] = 1 AND [IsDeleted] = 0");

        // Indice para consultas por tenant
        builder.HasIndex(x => x.TenantId)
            .HasDatabaseName("IX_TenantContactPersons_TenantId")
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos configurados no TenantMapping
    }
}
