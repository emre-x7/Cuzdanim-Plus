using Cuzdanim.Domain.Common;

namespace Cuzdanim.Domain.Entities;

public class Family : BaseAuditableEntity
{
    public string Name { get; private set; }
    public Guid OwnerId { get; private set; }
    public string? Description { get; private set; }

    // İlişkiler
    public virtual User Owner { get; private set; }
    public virtual ICollection<FamilyMember> Members { get; private set; } = new List<FamilyMember>();
    public virtual ICollection<Goal> SharedGoals { get; private set; } = new List<Goal>();

    private Family() { }

    public static Family Create(Guid ownerId, string name, string? description = null)
    {
        var family = new Family
        {
            OwnerId = ownerId,
            Name = name,
            Description = description
        };

        return family;
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
        MarkAsUpdated();
    }
}