using Cuzdanim.Domain.Common;
using Cuzdanim.Domain.Enums;
using Cuzdanim.Domain.ValueObjects;

namespace Cuzdanim.Domain.Entities;

public class Transaction : BaseAuditableEntity
{
    public Guid UserId { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }
    public TransactionType Type { get; private set; }
    public Money Amount { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public string? Description { get; private set; }
    public string? Notes { get; private set; }

    // Transfer için (hesaplar arası)
    public Guid? ToAccountId { get; private set; }

    // Tekrarlayan işlem mi?
    public bool IsRecurring { get; private set; }
    public Guid? RecurringTransactionId { get; private set; }

    // Etiketler (virgülle ayrılmış)
    public string? Tags { get; private set; }

    // Makbuz/Fatura
    public string? ReceiptUrl { get; private set; }

    // AI tarafından kategorize edildi mi?
    public bool IsAutoCategorized { get; private set; }

    // İlişkiler
    public virtual User User { get; private set; }
    public virtual Account Account { get; private set; }
    public virtual Account? ToAccount { get; private set; }
    public virtual Category Category { get; private set; }
    public virtual RecurringTransaction? RecurringTransaction { get; private set; }

    private Transaction() { }

    public static Transaction CreateExpense(
        Guid userId,
        Guid accountId,
        Guid categoryId,
        Money amount,
        DateTime transactionDate,
        string? description = null,
        string? notes = null)
    {
        var transaction = new Transaction
        {
            UserId = userId,
            AccountId = accountId,
            CategoryId = categoryId,
            Type = TransactionType.Expense,
            Amount = amount,
            TransactionDate = transactionDate,
            Description = description,
            Notes = notes
        };

        return transaction;
    }

    public static Transaction CreateIncome(
        Guid userId,
        Guid accountId,
        Guid categoryId,
        Money amount,
        DateTime transactionDate,
        string? description = null,
        string? notes = null)
    {
        var transaction = new Transaction
        {
            UserId = userId,
            AccountId = accountId,
            CategoryId = categoryId,
            Type = TransactionType.Income,
            Amount = amount,
            TransactionDate = transactionDate,
            Description = description,
            Notes = notes
        };

        return transaction;
    }

    public static Transaction CreateTransfer(
        Guid userId,
        Guid fromAccountId,
        Guid toAccountId,
        Money amount,
        DateTime transactionDate,
        string? description = null)
    {
        if (fromAccountId == toAccountId)
            throw new InvalidOperationException("Aynı hesaba transfer yapılamaz");

        var transaction = new Transaction
        {
            UserId = userId,
            AccountId = fromAccountId,
            ToAccountId = toAccountId,
            CategoryId = Guid.Empty, // Transfer'de kategori yok
            Type = TransactionType.Transfer,
            Amount = amount,
            TransactionDate = transactionDate,
            Description = description ?? "Hesaplar arası transfer"
        };

        return transaction;
    }

    public void Update(
        Guid categoryId,
        Money amount,
        DateTime transactionDate,
        string? description,
        string? notes)
    {
        CategoryId = categoryId;
        Amount = amount;
        TransactionDate = transactionDate;
        Description = description;
        Notes = notes;
        MarkAsUpdated();
    }

    public void AttachReceipt(string receiptUrl)
    {
        ReceiptUrl = receiptUrl;
        MarkAsUpdated();
    }

    public void AddTags(params string[] tags)
    {
        var existingTags = Tags?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
            ?? new List<string>();

        existingTags.AddRange(tags);
        Tags = string.Join(",", existingTags.Distinct());
        MarkAsUpdated();
    }

    public void MarkAsAutoCategorized()
    {
        IsAutoCategorized = true;
        MarkAsUpdated();
    }

    public void LinkToRecurring(Guid recurringTransactionId)
    {
        IsRecurring = true;
        RecurringTransactionId = recurringTransactionId;
        MarkAsUpdated();
    }
}