using Cuzdanim.Domain.Common;
using Cuzdanim.Domain.Enums;
using Cuzdanim.Domain.ValueObjects;

namespace Cuzdanim.Domain.Entities;

public class RecurringTransaction : BaseAuditableEntity
{
    public Guid UserId { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }
    public TransactionType Type { get; private set; }
    public Money Amount { get; private set; }
    public string Description { get; private set; }

    // Tekrarlama ayarları
    public RecurrenceFrequency Frequency { get; private set; }
    public int Interval { get; private set; } = 1; // Her X günde/hafta/ay
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public DateTime NextOccurrence { get; private set; }

    // Bildirim ayarları
    public bool SendReminder { get; private set; } = true;
    public int ReminderDaysBefore { get; private set; } = 3;

    // Durum
    public bool IsActive { get; private set; } = true;

    // İlişkiler
    public virtual User User { get; private set; }
    public virtual Account Account { get; private set; }
    public virtual Category Category { get; private set; }
    public virtual ICollection<Transaction> GeneratedTransactions { get; private set; } = new List<Transaction>();

    private RecurringTransaction() { }

    public static RecurringTransaction Create(
        Guid userId,
        Guid accountId,
        Guid categoryId,
        TransactionType type,
        Money amount,
        string description,
        RecurrenceFrequency frequency,
        DateTime startDate,
        DateTime? endDate = null,
        int interval = 1)
    {
        var recurring = new RecurringTransaction
        {
            UserId = userId,
            AccountId = accountId,
            CategoryId = categoryId,
            Type = type,
            Amount = amount,
            Description = description,
            Frequency = frequency,
            Interval = interval,
            StartDate = startDate,
            EndDate = endDate,
            NextOccurrence = startDate
        };

        return recurring;
    }

    public void UpdateNextOccurrence()
    {
        NextOccurrence = Frequency switch
        {
            RecurrenceFrequency.Daily => NextOccurrence.AddDays(Interval),
            RecurrenceFrequency.Weekly => NextOccurrence.AddDays(7 * Interval),
            RecurrenceFrequency.Monthly => NextOccurrence.AddMonths(Interval),
            RecurrenceFrequency.Yearly => NextOccurrence.AddYears(Interval),
            _ => throw new InvalidOperationException("Geçersiz tekrarlama sıklığı")
        };

        MarkAsUpdated();
    }

    public void Pause()
    {
        IsActive = false;
        MarkAsUpdated();
    }

    public void Resume()
    {
        IsActive = true;
        MarkAsUpdated();
    }

    public void UpdateAmount(Money newAmount)
    {
        Amount = newAmount;
        MarkAsUpdated();
    }

    public bool ShouldGenerateTransaction(DateTime currentDate)
    {
        if (!IsActive) return false;
        if (EndDate.HasValue && currentDate > EndDate.Value) return false;
        return currentDate >= NextOccurrence;
    }
}