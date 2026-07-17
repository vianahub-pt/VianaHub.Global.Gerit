using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa um Tipo de Origem de Aquisição (catálogo global).
/// Tabela: dbo.AcquisitionSourceTypes — Code NVARCHAR(50) UK + Translations 1:N.
/// </summary>
public class AcquisitionSourceTypeEntity : Entity
{
    public string? Code { get; private set; }
    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public IReadOnlyCollection<AcquisitionSourceTypeTranslationsEntity> Translations { get; private set; }

    protected AcquisitionSourceTypeEntity() { }

    /// <summary>
    /// Construtor para criação de um novo AcquisitionSourceType
    /// </summary>
    public AcquisitionSourceTypeEntity(string code, string name, string? description, int createdBy)
    {
        Code = code;
        Name = name;
        Description = description;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
        Translations = new List<AcquisitionSourceTypeTranslationsEntity>();
    }

    public void Update(string name, string? description, int modifiedBy)
    {
        Name = name;
        Description = description;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateCode(string code, int modifiedBy)
    {
        Code = code;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Activate(int modifiedBy)
    {
        IsActive = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Deactivate(int modifiedBy)
    {
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Delete(int modifiedBy)
    {
        IsDeleted = true;
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}
