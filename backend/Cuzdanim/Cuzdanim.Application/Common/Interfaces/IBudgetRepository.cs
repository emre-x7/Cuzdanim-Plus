using Cuzdanim.Domain.Entities;

namespace Cuzdanim.Application.Common.Interfaces;

public interface IBudgetRepository : IRepository<Budget>
{
    Task<IReadOnlyList<Budget>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Budget?> GetByCategoryAndPeriodAsync(Guid userId, Guid categoryId, DateTime date, CancellationToken cancellationToken = default);
}