using Cuzdanim.Domain.Common;
using Cuzdanim.Domain.Enums;
using Cuzdanim.Domain.ValueObjects;
using System.Transactions;

namespace Cuzdanim.Domain.Entities;

public class Account : BaseAuditableEntity
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public AccountType Type { get; private set; }
    public Money Balance { get; private set; }
    public Money InitialBalance { get; private set; }

    // Banka bilgileri (opsiyonel)
    public string? BankName { get; private set; }
    public string? IBAN { get; private set; }
    public string? CardLastFourDigits { get; private set; }

    // Kredi kartı için
    public decimal? CreditLimit { get; private set; }
    public DateTime? BillingCycleDay { get; private set; }

    // Görünürlük
    public bool IsActive { get; private set; } = true;
    public bool IncludeInTotalBalance { get; private set; } = true;

    // İlişkiler
    public virtual User User { get; private set; }
    public virtual ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    private Account() { }

    public static Account Create(
        Guid userId,
        string name,
        AccountType type,
        Money initialBalance,
        string? bankName = null,
        string? iban = null)
    {
        var account = new Account
        {
            UserId = userId,
            Name = name,
            Type = type,
            Balance = new Money(initialBalance.Amount, initialBalance.Currency), 
            InitialBalance = new Money(initialBalance.Amount, initialBalance.Currency), 
            BankName = bankName,
            IBAN = iban
        };

        return account;
    }

    // Domain methods
    public void UpdateBalance(Money newBalance)
    {
        Balance = newBalance;
        MarkAsUpdated();
    }

    public void Deposit(Money amount)
    {
        if (amount.Currency != Balance.Currency)
            throw new InvalidOperationException("Para birimi uyuşmuyor");

        Balance += amount;
        MarkAsUpdated();
    }

    public void Withdraw(Money amount)
    {
        if (amount.Currency != Balance.Currency)
            throw new InvalidOperationException("Para birimi uyuşmuyor");

        // Kredi kartı için
        if (Type == AccountType.CreditCard)
        {
            if (CreditLimit.HasValue)
            {
                var totalDebt = Math.Abs(Balance.Amount) + amount.Amount;

                if (totalDebt > CreditLimit.Value)
                    throw new InvalidOperationException("Kredi kartı limitini aşıyor");
            }

            Balance = new Money(Balance.Amount - amount.Amount, Balance.Currency);
        }
        else
        {
            if (Balance.Amount < amount.Amount)
            {
                throw new InvalidOperationException("Yetersiz bakiye");
            }

            Balance = Balance - amount;
        }

        MarkAsUpdated();
    }

    public void SetCreditLimit(decimal limit)
    {
        if (Type != AccountType.CreditCard)
            throw new InvalidOperationException("Sadece kredi kartları için limit belirlenebilir");

        CreditLimit = limit;
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
    public void UpdateDetails(string name, bool isActive, bool includeInTotalBalance)
    {
        Name = name;
        IsActive = isActive;
        IncludeInTotalBalance = includeInTotalBalance;
        MarkAsUpdated();
    }
}