using Cuzdanim.Domain.Common;
using Cuzdanim.Domain.Enums;
using Cuzdanim.Domain.ValueObjects;

namespace Cuzdanim.Domain.Entities;

public class Budget : BaseAuditableEntity
{
    public Guid UserId { get; private set; }
    public Guid CategoryId { get; private set; }
    public string Name { get; private set; }
    public Money Amount { get; private set; }
    public DateRange Period { get; private set; }

    // Bildirim ayarları
    public bool AlertWhenExceeded { get; private set; } = true;
    public decimal AlertThresholdPercentage { get; private set; } = 80; // %80'e ulaşınca uyar

    // Durum
    public bool IsActive { get; private set; } = true;

    // İlişkiler
    public virtual User User { get; private set; }
    public virtual Category Category { get; private set; }

    private Budget() { }

    public static Budget Create(
        Guid userId,
        Guid categoryId,
        string name,
        Money amount,
        DateRange period)
    {
        var budget = new Budget
        {
            UserId = userId,
            CategoryId = categoryId,
            Name = name,
            Amount = amount,
            Period = period
        };

        return budget;
    }

    public void Update(string name, Money amount, DateRange period)
    {
        Name = name;
        Amount = amount;
        Period = period;
        MarkAsUpdated();
    }

    public void SetAlertThreshold(decimal percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentException("Uyarı eşiği %0 ile %100 arasında olmalı");

        AlertThresholdPercentage = percentage;
        MarkAsUpdated();
    }

    public void Deactivate()
    {
        IsActive = false;
        MarkAsUpdated();
    }

    public void Activate()
    {
        IsActive = true;
        MarkAsUpdated();
    }

    public bool IsCurrentPeriod(DateTime date)
    {
        return Period.Contains(date);
    }

    public bool ShouldAlert(decimal spentAmount)
    {
        if (!AlertWhenExceeded) return false;

        var threshold = Amount.Amount * (AlertThresholdPercentage / 100);
        return spentAmount >= threshold;
    }
}