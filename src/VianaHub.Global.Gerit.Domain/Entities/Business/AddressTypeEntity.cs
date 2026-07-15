using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa um Tipo de Endereço (Residencial, Comercial, etc.)
/// </summary>
public class AddressTypeEntity : Entity
{
    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public string? Code { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public ICollection<AddressTypeTranslationEntity> Translations { get; private set; }

    // Construtor protegido para o EF Core
    protected AddressTypeEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Tipo de Endereço
    /// </summary>
    public AddressTypeEntity(string name, string description, int createdBy)
        : this(name, description, code: null, createdBy)
    {
    }

    /// <summary>
    /// Construtor completo para criação de um novo Tipo de Endereço, incluindo o Code de identificação.
    /// </summary>
    public AddressTypeEntity(string name, string description, string code, int createdBy)
    {
        Name = name;
        Description = description;
        Code = code;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
        Translations = new List<AddressTypeTranslationEntity>();
    }

    public void Update(string name, string description, int modifiedBy)
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
