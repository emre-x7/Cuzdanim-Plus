using Cuzdanim.Domain.Enums;

namespace Cuzdanim.Domain.ValueObjects;

public class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    public Money(decimal amount, Currency currency)
    {

        Amount = amount;
        Currency = currency;
    }

    // Copy constructor
    public Money(Money other)
    {
        Amount = other.Amount;
        Currency = other.Currency;
    }

    // Para birimi dönüşümü için (gelecekte exchange rate servisi ile)
    public Money ConvertTo(Currency targetCurrency, decimal exchangeRate)
    {
        var convertedAmount = Amount * exchangeRate;
        return new Money(convertedAmount, targetCurrency);
    }

    // Matematiksel işlemler
    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Farklı para birimlerini toplayamazsınız");

        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Money operator -(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Farklı para birimlerini çıkaramazsınız");

        return new Money(a.Amount - b.Amount, a.Currency);
    }

    // Eşitlik kontrolü
    public bool Equals(Money other)
    {
        if (other is null) return false;
        return Amount == other.Amount && Currency == other.Currency;
    }

    public override bool Equals(object obj) => Equals(obj as Money);
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);

    public override string ToString() => $"{Amount:N2} {Currency}";
}