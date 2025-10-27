using Cuzdanim.Domain.Entities;
using Cuzdanim.Domain.Enums;

namespace Cuzdanim.Application.Common.Interfaces;

public interface IGoalRepository : IRepository<Goal>
{
    Task<IReadOnlyList<Goal>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Goal>> GetByStatusAsync(Guid userId, GoalStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Goal>> GetSharedGoalsAsync(Guid familyId, CancellationToken cancellationToken = default);
}