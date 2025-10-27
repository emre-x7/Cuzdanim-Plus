using Cuzdanim.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cuzdanim.Infrastructure.Data.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.ToTable("Budgets");

        builder.HasKey(b => b.Id);

        // Properties
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.AlertThresholdPercentage)
            .HasPrecision(5, 2);

        // Money Value Object
        builder.OwnsOne(b => b.Amount, money =>
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

        // DateRange Value Object
        builder.OwnsOne(b => b.Period, period =>
        {
            period.Property(p => p.StartDate)
                .HasColumnName("PeriodStartDate")
                .IsRequired();

            period.Property(p => p.EndDate)
                .HasColumnName("PeriodEndDate")
                .IsRequired();
        });

        // Indexes
        builder.HasIndex(b => b.UserId);
        builder.HasIndex(b => b.CategoryId);
        builder.HasIndex(b => new { b.UserId, b.IsActive });

        // Query filter
        builder.HasQueryFilter(b => !b.IsDeleted);

        // İlişkiler
        builder.HasOne(b => b.User)
            .WithMany(u => u.Budgets)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Category)
            .WithMany(c => c.Budgets)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}