using Cuzdanim.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cuzdanim.Infrastructure.Data.Configurations;

public class FamilyMemberConfiguration : IEntityTypeConfiguration<FamilyMember>
{
    public void Configure(EntityTypeBuilder<FamilyMember> builder)
    {
        builder.ToTable("FamilyMembers");

        builder.HasKey(fm => fm.Id);

        builder.Property(fm => fm.Role)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(fm => fm.JoinedAt)
            .IsRequired();

        // Indexes
        builder.HasIndex(fm => fm.FamilyId);
        builder.HasIndex(fm => fm.UserId);
        builder.HasIndex(fm => new { fm.FamilyId, fm.UserId }).IsUnique(); // Aynı kullanıcı aynı aileye 2 kez eklenemez

        // Query filter
        builder.HasQueryFilter(fm => !fm.IsDeleted);

        // İlişkiler
        builder.HasOne(fm => fm.Family)
            .WithMany(f => f.Members)
            .HasForeignKey(fm => fm.FamilyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(fm => fm.User)
            .WithMany(u => u.FamilyMemberships)
            .HasForeignKey(fm => fm.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}