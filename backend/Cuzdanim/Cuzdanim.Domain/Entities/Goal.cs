using Cuzdanim.Domain.Common;
using Cuzdanim.Domain.Enums;
using Cuzdanim.Domain.ValueObjects;

namespace Cuzdanim.Domain.Entities;

public class Goal : BaseAuditableEntity
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Money TargetAmount { get; private set; }
    public Money CurrentAmount { get; private set; }
    public DateTime TargetDate { get; private set; }
    public GoalStatus Status { get; private set; }

    // Görsel
    public string? ImageUrl { get; private set; }
    public string? Icon { get; private set; }

    // Aile hedefi mi?
    public bool IsShared { get; private set; }
    public Guid? FamilyId { get; private set; }

    // İlişkiler
    public virtual User User { get; private set; }
    public virtual Family? Family { get; private set; }

    private Goal() { }

    public static Goal Create(
        Guid userId,
        string name,
        Money targetAmount,
        DateTime targetDate,
        string? description = null,
        bool isShared = false,
        Guid? familyId = null)
    {
        var goal = new Goal
        {
            UserId = userId,
            Name = name,
            Description = description,
            TargetAmount = targetAmount,
            CurrentAmount = new Money(0, targetAmount.Currency),
            TargetDate = targetDate,
            Status = GoalStatus.Active,
            IsShared = isShared,
            FamilyId = familyId
        };

        return goal;
    }

    public void AddContribution(Money amount)
    {
        if (amount.Currency != TargetAmount.Currency)
            throw new InvalidOperationException("Para birimi uyuşmuyor");

        if (Status != GoalStatus.Active)
            throw new InvalidOperationException("Sadece aktif hedeflere katkı yapılabilir");

        CurrentAmount += amount;

        if (CurrentAmount.Amount >= TargetAmount.Amount)
        {
            Status = GoalStatus.Completed;
        }

        MarkAsUpdated();
    }

    public void Update(string name, string? description, Money targetAmount, DateTime targetDate)
    {
        Name = name;
        Description = description;
        TargetAmount = targetAmount;
        TargetDate = targetDate;
        MarkAsUpdated();
    }

    public void Pause()
    {
        Status = GoalStatus.Paused;
        MarkAsUpdated();
    }

    public void Resume()
    {
        Status = GoalStatus.Active;
        MarkAsUpdated();
    }

    public void Cancel()
    {
        Status = GoalStatus.Cancelled;
        MarkAsUpdated();
    }

    // Computed properties
    public decimal ProgressPercentage
    {
        get
        {
            if (TargetAmount.Amount <= 0) return 0;

            var percentage = (CurrentAmount.Amount / TargetAmount.Amount) * 100;

            // Maksimum %100 
            return percentage > 100 ? 100 : percentage;
        }
    }

    public int DaysRemaining => (TargetDate.Date - DateTime.UtcNow.Date).Days;

    public Money RemainingAmount
    {
        get
        {
            // Hedef aşıldıysa kalan 0 
            if (CurrentAmount.Amount >= TargetAmount.Amount)
            {
                return new Money(0, TargetAmount.Currency);
            }

            return TargetAmount - CurrentAmount;
        }
    }
}