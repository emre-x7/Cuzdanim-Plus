using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Domain.Entities;
using Cuzdanim.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Cuzdanim.Infrastructure.Repositories;

public class AccountRepository : Repository<Account>, IAccountRepository
{
    public AccountRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Account>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Account>> GetActiveAccountsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.UserId == userId && a.IsActive)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalBalanceAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var accounts = await _dbSet
            .Where(a => a.UserId == userId && a.IsActive && a.IncludeInTotalBalance)
            .ToListAsync(cancellationToken);

        // Memory'de toplama yap (Money Value Object kullanarak)
        return accounts.Sum(a => a.Balance.Amount);
    }
}