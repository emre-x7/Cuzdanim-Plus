using Cuzdanim.Domain.Common;
using Cuzdanim.Domain.Enums;

namespace Cuzdanim.Domain.Entities;

public class FamilyMember : BaseAuditableEntity
{
    public Guid FamilyId { get; private set; }
    public Guid UserId { get; private set; }
    public FamilyRole Role { get; private set; }
    public DateTime JoinedAt { get; private set; }
    public bool IsActive { get; private set; } = true;

    // İlişkiler
    public virtual Family Family { get; private set; }
    public virtual User User { get; private set; }

    private FamilyMember() { }

    public static FamilyMember Create(Guid familyId, Guid userId, FamilyRole role)
    {
        var member = new FamilyMember
        {
            FamilyId = familyId,
            UserId = userId,
            Role = role,
            JoinedAt = DateTime.UtcNow
        };

        return member;
    }

    public void ChangeRole(FamilyRole newRole)
    {
        Role = newRole;
        MarkAsUpdated();
    }

    public void Leave()
    {
        IsActive = false;
        MarkAsUpdated();
    }
}