namespace Cuzdanim.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public Guid? CreatedBy { get; protected set; }
    public Guid? UpdatedBy { get; protected set; }

    public void SetCreatedBy(Guid userId)
    {
        CreatedBy = userId;
    }

    public void SetUpdatedBy(Guid userId)
    {
        UpdatedBy = userId;
        MarkAsUpdated();
    }
}