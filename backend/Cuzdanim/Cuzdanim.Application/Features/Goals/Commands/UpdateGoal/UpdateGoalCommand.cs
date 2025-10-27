using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.Enums;
using MediatR;

namespace Cuzdanim.Application.Features.Goals.Commands.UpdateGoal;

public class UpdateGoalCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal TargetAmount { get; set; }
    public Currency Currency { get; set; }
    public DateTime TargetDate { get; set; }
}