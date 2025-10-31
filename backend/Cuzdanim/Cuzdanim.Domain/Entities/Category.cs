using Cuzdanim.Domain.Common;
using Cuzdanim.Domain.Enums;

namespace Cuzdanim.Domain.Entities;

public class Category : BaseAuditableEntity
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public TransactionType TransactionType { get; private set; } 
    public CategoryType Type { get; private set; }
    public string? Icon { get; private set; }
    public string? Color { get; private set; }
    public bool IsDefault { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Guid? ParentCategoryId { get; private set; }

    // İlişkiler
    public virtual User User { get; private set; }
    public virtual Category? ParentCategory { get; private set; }
    public virtual ICollection<Category> SubCategories { get; private set; } = new List<Category>();
    public virtual ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();
    public virtual ICollection<Budget> Budgets { get; private set; } = new List<Budget>();

    private Category() { }

    public static Category Create(
        Guid userId,
        string name,
        TransactionType transactionType, 
        CategoryType type,
        string? icon = null,
        string? color = null,
        bool isDefault = false,
        Guid? parentCategoryId = null)
    {
        var category = new Category
        {
            UserId = userId,
            Name = name,
            TransactionType = transactionType, 
            Type = type,
            Icon = icon,
            Color = color,
            IsDefault = isDefault,
            ParentCategoryId = parentCategoryId
        };

        return category;
    }

    public void Update(string name, string? icon, string? color)
    {
        if (IsDefault)
            throw new InvalidOperationException("Varsayılan kategoriler değiştirilemez");

        Name = name;
        Icon = icon;
        Color = color;
        MarkAsUpdated();
    }

    public void Deactivate()
    {
        if (IsDefault)
            throw new InvalidOperationException("Varsayılan kategoriler devre dışı bırakılamaz");

        IsActive = false;
        MarkAsUpdated();
    }

    public void Activate()
    {
        IsActive = true;
        MarkAsUpdated();
    }
}