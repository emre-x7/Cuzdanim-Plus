using Cuzdanim.Domain.Entities;

namespace Cuzdanim.Application.Common.Interfaces;

public interface IRecurringTransactionRepository : IRepository<RecurringTransaction>
{
    Task<IReadOnlyList<RecurringTransaction>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RecurringTransaction>> GetDueTransactionsAsync(DateTime currentDate, CancellationToken cancellationToken = default);
}