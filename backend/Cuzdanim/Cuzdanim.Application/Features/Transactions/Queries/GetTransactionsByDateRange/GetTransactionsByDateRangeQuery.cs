using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Transactions.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Transactions.Queries.GetTransactionsByDateRange;

public class GetTransactionsByDateRangeQuery : IRequest<Result<List<TransactionDto>>>
{
    public Guid UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? CategoryId { get; set; }
}