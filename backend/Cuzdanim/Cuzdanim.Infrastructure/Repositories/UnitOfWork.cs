using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cuzdanim.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _currentTransaction;

    // Lazy initialization için private fields
    private IUserRepository? _userRepository;
    private IAccountRepository? _accountRepository;
    private ITransactionRepository? _transactionRepository;
    private ICategoryRepository? _categoryRepository;
    private IBudgetRepository? _budgetRepository;
    private IGoalRepository? _goalRepository;
    private IRecurringTransactionRepository? _recurringTransactionRepository;
    private IRefreshTokenRepository? _refreshTokenRepository; 

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    // Lazy initialization pattern
    public IUserRepository Users =>
        _userRepository ??= new UserRepository(_context);

    public IAccountRepository Accounts =>
        _accountRepository ??= new AccountRepository(_context);

    public ITransactionRepository Transactions =>
        _transactionRepository ??= new TransactionRepository(_context);

    public ICategoryRepository Categories =>
        _categoryRepository ??= new CategoryRepository(_context);

    public IBudgetRepository Budgets =>
        _budgetRepository ??= new BudgetRepository(_context);

    public IGoalRepository Goals =>
        _goalRepository ??= new GoalRepository(_context);

    public IRecurringTransactionRepository RecurringTransactions =>
        _recurringTransactionRepository ??= new RecurringTransactionRepository(_context);

    public IRefreshTokenRepository RefreshTokens =>
        _refreshTokenRepository ??= new RefreshTokenRepository(_context);

    // Transaction Management
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
        {
            throw new InvalidOperationException("Bir transaction zaten başlatılmış.");
        }

        _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _context.Dispose();
    }
}