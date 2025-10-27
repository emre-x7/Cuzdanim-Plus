using Cuzdanim.Application.Common.Models;
using MediatR;

namespace Cuzdanim.Application.Features.Goals.Commands.DeleteGoal;

public class DeleteGoalCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}