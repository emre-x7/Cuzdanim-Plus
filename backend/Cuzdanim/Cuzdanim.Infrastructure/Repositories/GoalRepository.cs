using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Domain.Entities;
using Cuzdanim.Domain.Enums;
using Cuzdanim.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Cuzdanim.Infrastructure.Repositories;

public class GoalRepository : Repository<Goal>, IGoalRepository
{
    public GoalRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Goal>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(g => g.UserId == userId)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Goal>> GetByStatusAsync(Guid userId, GoalStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(g => g.UserId == userId && g.Status == status)
            .OrderBy(g => g.TargetDate) // Yakın hedefler önce
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Goal>> GetSharedGoalsAsync(Guid familyId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(g => g.User) // Hedefi oluşturan kullanıcı bilgisi
            .Where(g => g.FamilyId == familyId && g.IsShared)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}