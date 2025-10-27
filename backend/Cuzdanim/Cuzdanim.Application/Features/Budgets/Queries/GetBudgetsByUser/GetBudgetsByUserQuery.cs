using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Budgets.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Budgets.Queries.GetBudgetsByUser;

public class GetBudgetsByUserQuery : IRequest<Result<List<BudgetDto>>>
{
    public Guid UserId { get; set; }
}