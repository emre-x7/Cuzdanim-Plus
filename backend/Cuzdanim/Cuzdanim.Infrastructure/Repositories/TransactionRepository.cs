using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Domain.Entities;
using Cuzdanim.Domain.Enums;
using Cuzdanim.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Cuzdanim.Infrastructure.Repositories;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Transaction>> GetByUserIdAsync(
        Guid userId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(t => t.Category)
            .Include(t => t.Account)
            .Where(t => t.UserId == userId);

        if (startDate.HasValue)
            query = query.Where(t => t.TransactionDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(t => t.TransactionDate <= endDate.Value);

        return await query
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Transaction>> GetByAccountIdAsync(
        Guid accountId,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.TransactionDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Transaction>> GetByCategoryIdAsync(
        Guid categoryId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(t => t.CategoryId == categoryId);

        if (startDate.HasValue)
            query = query.Where(t => t.TransactionDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(t => t.TransactionDate <= endDate.Value);

        return await query
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalIncomeAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var transactions = await _dbSet
            .Where(t => t.UserId == userId
                && t.Type == TransactionType.Income
                && t.TransactionDate >= startDate
                && t.TransactionDate <= endDate)
            .ToListAsync(cancellationToken);

        return transactions.Sum(t => t.Amount.Amount);
    }

    public async Task<decimal> GetTotalExpenseAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var transactions = await _dbSet
            .Where(t => t.UserId == userId
                && t.Type == TransactionType.Expense
                && t.TransactionDate >= startDate
                && t.TransactionDate <= endDate)
            .ToListAsync(cancellationToken);

        return transactions.Sum(t => t.Amount.Amount);
    }
}