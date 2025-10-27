using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Domain.Entities;
using Cuzdanim.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Cuzdanim.Infrastructure.Repositories;

public class RecurringTransactionRepository : Repository<RecurringTransaction>, IRecurringTransactionRepository
{
    public RecurringTransactionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<RecurringTransaction>> GetActiveByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Account)  // Hesap bilgisi
            .Include(r => r.Category) // Kategori bilgisi
            .Where(r => r.UserId == userId && r.IsActive)
            .OrderBy(r => r.NextOccurrence) // Yakın olanlar önce
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RecurringTransaction>> GetDueTransactionsAsync(
        DateTime currentDate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Account)
            .Include(r => r.Category)
            .Include(r => r.User) // Bildirim göndermek için User bilgisi
            .Where(r => r.IsActive
                && r.NextOccurrence <= currentDate
                && (!r.EndDate.HasValue || r.EndDate.Value >= currentDate)) // EndDate yoksa veya geçmemişse
            .OrderBy(r => r.NextOccurrence)
            .ToListAsync(cancellationToken);
    }
}