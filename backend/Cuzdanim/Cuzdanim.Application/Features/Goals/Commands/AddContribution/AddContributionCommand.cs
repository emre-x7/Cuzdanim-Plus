using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.Enums;
using MediatR;

namespace Cuzdanim.Application.Features.Goals.Commands.AddContribution;

public class AddContributionCommand : IRequest<Result<Guid>>
{
    public Guid GoalId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
}