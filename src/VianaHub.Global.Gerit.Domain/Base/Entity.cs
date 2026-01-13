namespace VianaHub.Global.Gerit.Domain.Base;

public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; protected set; }
    public Guid CreatedBy { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public Guid? UpdatedBy { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    protected Entity(Guid id, Guid createdBy)
    {
        Id = id;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetCreatedBy(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new InvalidOperationException("CreatedBy can only be set once.");

        CreatedBy = userId;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetModifiedBy(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new InvalidOperationException("UpdatedBy cannot be null or empty.");

        UpdatedBy = userId;
        UpdatedAt = DateTime.UtcNow;
    }

    // Equality baseada no Id
    public bool Equals(Entity other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;

        return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Entity);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity left, Entity right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }

    // Método para verificar se a entidade é transiente (ainda não foi persistida)
    public bool IsTransient()
    {
        return Id == Guid.Empty;
    }
}
