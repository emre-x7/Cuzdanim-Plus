using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.Enums;
using MediatR;

namespace Cuzdanim.Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommand : IRequest<Result<Guid>>
{
    public Guid UserId { get; set; }
    public Guid AccountId { get; set; }
    public Guid CategoryId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; } = Currency.TRY;
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? ReceiptUrl { get; set; }
    public List<string>? Tags { get; set; }
}