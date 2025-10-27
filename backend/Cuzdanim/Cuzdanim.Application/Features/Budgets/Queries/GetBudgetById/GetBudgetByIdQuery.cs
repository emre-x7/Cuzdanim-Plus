using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Budgets.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Budgets.Queries.GetBudgetById;

public class GetBudgetByIdQuery : IRequest<Result<BudgetDto>>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}