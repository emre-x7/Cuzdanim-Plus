using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Goals.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Goals.Queries.GetGoalsByUser;

public class GetGoalsByUserQuery : IRequest<Result<List<GoalDto>>>
{
    public Guid UserId { get; set; }
}