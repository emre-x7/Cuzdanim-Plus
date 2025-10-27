namespace Cuzdanim.Application.Common.Interfaces;

public interface ICategorySeederService
{
    Task SeedDefaultCategoriesForUserAsync(Guid userId);
}