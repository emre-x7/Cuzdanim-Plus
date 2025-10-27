using Cuzdanim.Domain.Entities;
using Cuzdanim.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cuzdanim.Infrastructure.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(a => a.Id);

        // Properties
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Type)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(a => a.BankName)
            .HasMaxLength(100);

        builder.Property(a => a.IBAN)
            .HasMaxLength(34);

        builder.Property(a => a.CardLastFourDigits)
            .HasMaxLength(4);

        // Money Value Object - Owned Type olarak
        builder.OwnsOne(a => a.Balance, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("BalanceAmount")
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("BalanceCurrency")
                .HasConversion<string>()
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.OwnsOne(a => a.InitialBalance, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("InitialBalanceAmount")
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("InitialBalanceCurrency")
                .HasConversion<string>()
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.Property(a => a.CreditLimit)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(a => a.UserId);
        builder.HasIndex(a => new { a.UserId, a.IsActive });

        // Query filter
        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}