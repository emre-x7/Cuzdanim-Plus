using Cuzdanim.Application.Common.Models;
using MediatR;

namespace Cuzdanim.Application.Features.Transactions.Commands.DeleteTransaction;

public class DeleteTransactionCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}