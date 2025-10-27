using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Accounts.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Accounts.Queries.GetAccountsByUser;

public class GetAccountsByUserQuery : IRequest<Result<List<AccountDto>>>
{
    public Guid UserId { get; set; }
}