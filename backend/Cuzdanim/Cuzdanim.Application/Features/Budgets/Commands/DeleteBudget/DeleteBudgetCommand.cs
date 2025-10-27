using Cuzdanim.Application.Common.Models;
using MediatR;

namespace Cuzdanim.Application.Features.Budgets.Commands.DeleteBudget;

public class DeleteBudgetCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}