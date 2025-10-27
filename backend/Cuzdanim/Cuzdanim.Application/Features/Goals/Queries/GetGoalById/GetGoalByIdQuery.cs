using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Goals.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Goals.Queries.GetGoalById;

public class GetGoalByIdQuery : IRequest<Result<GoalDto>>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}