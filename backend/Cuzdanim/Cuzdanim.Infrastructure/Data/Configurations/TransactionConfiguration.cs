using Cuzdanim.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cuzdanim.Infrastructure.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(t => t.Id);

        // Properties
        builder.Property(t => t.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Property(t => t.Notes)
            .HasMaxLength(2000);

        builder.Property(t => t.Tags)
            .HasMaxLength(500);

        builder.Property(t => t.ReceiptUrl)
            .HasMaxLength(1000);

        builder.Property(t => t.TransactionDate)
            .IsRequired();

        // Money Value Object
        builder.OwnsOne(t => t.Amount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("Amount")
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasConversion<string>()
                .HasMaxLength(10)
                .IsRequired();
        });

        // Indexes (Performance için kritik!)
        builder.HasIndex(t => t.UserId);
        builder.HasIndex(t => t.AccountId);
        builder.HasIndex(t => t.CategoryId);
        builder.HasIndex(t => t.TransactionDate);
        builder.HasIndex(t => new { t.UserId, t.TransactionDate }); // Kullanıcı + tarih sorguları için
        builder.HasIndex(t => new { t.AccountId, t.TransactionDate }); // Hesap + tarih sorguları için

        // Query filter
        builder.HasQueryFilter(t => !t.IsDeleted);

        // İlişkiler
        builder.HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Restrict); // Hesap silinirken işlemler korunsun

        builder.HasOne(t => t.Category)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Transfer için hedef hesap (opsiyonel)
        builder.HasOne(t => t.ToAccount)
            .WithMany()
            .HasForeignKey(t => t.ToAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.RecurringTransaction)
            .WithMany(r => r.GeneratedTransactions)
            .HasForeignKey(t => t.RecurringTransactionId)
            .OnDelete(DeleteBehavior.SetNull); // Recurring silinirse, bağlantı kaldırılsın ama transaction kalsın
    }
}