using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Domain.Entities;
using Cuzdanim.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Cuzdanim.Infrastructure.Repositories;

public class BudgetRepository : Repository<Budget>, IBudgetRepository
{
    public BudgetRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Budget>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Category)
            .Where(b => b.UserId == userId && b.IsActive)
            .OrderBy(b => b.Category.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Budget?> GetByCategoryAndPeriodAsync(
        Guid userId,
        Guid categoryId,
        DateTime date,
        CancellationToken cancellationToken = default)
    {
        //Önce veritabanından çek, sonra memory'de filtrele
        var budgets = await _dbSet
            .Include(b => b.Category)
            .Where(b => b.UserId == userId
                && b.CategoryId == categoryId
                && b.IsActive)
            .ToListAsync(cancellationToken);

        // Memory'de Period.Contains kontrolü yap
        return budgets.FirstOrDefault(b => b.Period.Contains(date));
    }
}