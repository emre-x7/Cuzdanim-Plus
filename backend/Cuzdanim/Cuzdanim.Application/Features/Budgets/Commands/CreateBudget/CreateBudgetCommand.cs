using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.Enums;
using MediatR;

namespace Cuzdanim.Application.Features.Budgets.Commands.CreateBudget;

public class CreateBudgetCommand : IRequest<Result<Guid>>
{
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public Currency Currency { get; set; } = Currency.TRY;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal AlertThresholdPercentage { get; set; } = 80;
}