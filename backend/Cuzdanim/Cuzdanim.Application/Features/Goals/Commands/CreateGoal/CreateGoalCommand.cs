using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.Enums;
using MediatR;

namespace Cuzdanim.Application.Features.Goals.Commands.CreateGoal;

public class CreateGoalCommand : IRequest<Result<Guid>>
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal TargetAmount { get; set; }
    public Currency Currency { get; set; } = Currency.TRY;
    public DateTime TargetDate { get; set; }
    public string? ImageUrl { get; set; }
    public string? Icon { get; set; }
}