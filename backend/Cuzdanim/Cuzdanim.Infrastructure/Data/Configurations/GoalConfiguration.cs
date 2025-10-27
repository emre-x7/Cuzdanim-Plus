using Cuzdanim.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cuzdanim.Infrastructure.Data.Configurations;

public class GoalConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.ToTable("Goals");

        builder.HasKey(g => g.Id);

        // Properties
        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(g => g.Description)
            .HasMaxLength(1000);

        builder.Property(g => g.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(g => g.TargetDate)
            .IsRequired();

        builder.Property(g => g.ImageUrl)
            .HasMaxLength(1000);

        builder.Property(g => g.Icon)
            .HasMaxLength(50);

        // Money Value Objects
        builder.OwnsOne(g => g.TargetAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("TargetAmount")
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("TargetCurrency")
                .HasConversion<string>()
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.OwnsOne(g => g.CurrentAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("CurrentAmount")
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("CurrentCurrency")
                .HasConversion<string>()
                .HasMaxLength(10)
                .IsRequired();
        });

        // Indexes
        builder.HasIndex(g => g.UserId);
        builder.HasIndex(g => g.FamilyId);
        builder.HasIndex(g => new { g.UserId, g.Status });

        // Query filter
        builder.HasQueryFilter(g => !g.IsDeleted);

        // İlişkiler
        builder.HasOne(g => g.User)
            .WithMany(u => u.Goals)
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(g => g.Family)
            .WithMany(f => f.SharedGoals)
            .HasForeignKey(g => g.FamilyId)
            .OnDelete(DeleteBehavior.SetNull); // Aile silinirse hedef kişiye ait kalır
    }
}