namespace Cuzdanim.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow; 
    public DateTime? UpdatedAt { get; protected set; }

    // Soft delete için
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow; 
    }

    public void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow; 
    }
}