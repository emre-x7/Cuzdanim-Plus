using Cuzdanim.Domain.Entities;

namespace Cuzdanim.Application.Common.Interfaces;

public interface IAccountRepository : IRepository<Account>
{
    Task<IReadOnlyList<Account>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Account>> GetActiveAccountsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalBalanceAsync(Guid userId, CancellationToken cancellationToken = default);
}