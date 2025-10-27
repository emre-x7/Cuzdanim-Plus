using Cuzdanim.Domain.Entities;
using Cuzdanim.Domain.Enums;

namespace Cuzdanim.Application.Common.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IReadOnlyList<Category>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Category>> GetByTypeAsync(Guid userId, CategoryType type, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Category>> GetDefaultCategoriesAsync(CancellationToken cancellationToken = default);
}