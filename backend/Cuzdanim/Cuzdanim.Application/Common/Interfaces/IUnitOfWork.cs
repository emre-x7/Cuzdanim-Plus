namespace Cuzdanim.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // Repositories
    IUserRepository Users { get; }
    IAccountRepository Accounts { get; }
    ITransactionRepository Transactions { get; }
    ICategoryRepository Categories { get; }
    IBudgetRepository Budgets { get; }
    IGoalRepository Goals { get; }
    IRecurringTransactionRepository RecurringTransactions { get; }
    IRefreshTokenRepository RefreshTokens { get; }

    // Transaction Management
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}