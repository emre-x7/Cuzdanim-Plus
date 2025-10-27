using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Domain.Entities;
using Cuzdanim.Domain.Enums;
using Cuzdanim.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Cuzdanim.Infrastructure.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Category>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.UserId == userId && c.IsActive)
            .Include(c => c.SubCategories)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> GetByTypeAsync(Guid userId, CategoryType type, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.UserId == userId && c.Type == type && c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> GetDefaultCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.IsDefault && c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }
}