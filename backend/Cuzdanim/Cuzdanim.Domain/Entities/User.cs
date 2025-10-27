using Cuzdanim.Domain.Common;
using Cuzdanim.Domain.Enums;
using System.Security.Principal;
using System.Transactions;

namespace Cuzdanim.Domain.Entities;

public class User : BaseAuditableEntity
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? PhoneNumber { get; private set; }
    public DateTime? DateOfBirth { get; private set; }

    // Email doğrulama
    public bool IsEmailVerified { get; private set; }
    public string? EmailVerificationToken { get; private set; }

    // Şifre sıfırlama
    public string? PasswordResetToken { get; private set; }
    public DateTime? PasswordResetTokenExpiry { get; private set; }

    // Kullanıcı tercihleri
    public Currency PreferredCurrency { get; private set; } = Currency.TRY;
    public string? ProfilePictureUrl { get; private set; }

    // İlişkiler
    public virtual ICollection<Account> Accounts { get; private set; } = new List<Account>();
    public virtual ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();
    public virtual ICollection<Budget> Budgets { get; private set; } = new List<Budget>();
    public virtual ICollection<Goal> Goals { get; private set; } = new List<Goal>();
    public virtual ICollection<FamilyMember> FamilyMemberships { get; private set; } = new List<FamilyMember>();

    // Constructor (EF Core için paramtresiz gerekli)
    private User() { }

    // Factory method (Domain-driven design pattern)
    public static User Create(string email, string passwordHash, string firstName, string lastName)
    {
        var user = new User
        {
            Email = email.ToLowerInvariant(),
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName,
            IsEmailVerified = false,
            EmailVerificationToken = Guid.NewGuid().ToString()
        };

        return user;
    }

    // Domain methods
    public void UpdateProfile(string firstName, string lastName, string phoneNumber, DateTime? dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;

        DateOfBirth = dateOfBirth.HasValue
            ? DateTime.SpecifyKind(dateOfBirth.Value, DateTimeKind.Utc)
            : null;

        MarkAsUpdated();
    }

    public void VerifyEmail()
    {
        IsEmailVerified = true;
        EmailVerificationToken = null;
        MarkAsUpdated();
    }

    public void InitiatePasswordReset()
    {
        PasswordResetToken = Guid.NewGuid().ToString();
        PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(24);
        MarkAsUpdated();
    }

    public void ResetPassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        PasswordResetToken = null;
        PasswordResetTokenExpiry = null;
        MarkAsUpdated();
    }

    public void ChangePreferredCurrency(Currency currency)
    {
        PreferredCurrency = currency;
        MarkAsUpdated();
    }

    public string FullName => $"{FirstName} {LastName}";
}