using Cuzdanim.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cuzdanim.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Account> Accounts { get; }
    DbSet<Transaction> Transactions { get; }
    DbSet<Budget> Budgets { get; }
    DbSet<Goal> Goals { get; }
    DbSet<Category> Categories { get; }
    DbSet<RecurringTransaction> RecurringTransactions { get; }
    DbSet<Family> Families { get; }
    DbSet<FamilyMember> FamilyMembers { get; }
    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}