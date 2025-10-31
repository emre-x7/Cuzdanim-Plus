using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Categories.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Categories.Queries.GetCategoriesByUser;

public class GetCategoriesByUserQuery : IRequest<Result<List<CategoryDto>>>
{
    public Guid UserId { get; set; }
}