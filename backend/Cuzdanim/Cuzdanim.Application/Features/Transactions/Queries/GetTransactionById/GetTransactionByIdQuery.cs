using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Transactions.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Transactions.Queries.GetTransactionById;

public class GetTransactionByIdQuery : IRequest<Result<TransactionDto>>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}