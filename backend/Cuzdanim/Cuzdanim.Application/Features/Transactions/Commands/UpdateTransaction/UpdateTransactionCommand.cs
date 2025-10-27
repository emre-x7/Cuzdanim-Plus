using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.Enums;
using MediatR;

namespace Cuzdanim.Application.Features.Transactions.Commands.UpdateTransaction;

public class UpdateTransactionCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}