using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Infra.Data.Mappings.Business;

/// <summary>
/// Mapeamento da entidade ClientContact
/// Contatos do cliente com suporte a Row Level Security
/// </summary>
public class ClientContactMapping : IEntityTypeConfiguration<ClientContactEntity>
{
    public void Configure(EntityTypeBuilder<ClientContactEntity> builder)
    {
        builder.ToTable("ClientContacts", "dbo");

        // Chave Prim�ria
        builder.HasKey(x => x.Id)
            .HasName("PK_ClientContacts");

        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .IsRequired();

        // Propriedades
        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.ClientId)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnType("NVARCHAR(150)")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnType("NVARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired();

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
        builder.HasIndex(x => x.ClientId)
            .HasDatabaseName("IX_ClientContacts_ClientId")
            .IncludeProperties(x => x.TenantId)
            .HasFilter("[IsDeleted] = 0");

        // Relacionamentos


        // Relacionamento com Client configurado no ClientMapping
    }
}

