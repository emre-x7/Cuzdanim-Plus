using Cuzdanim.Application.Common.Models;
using MediatR;

namespace Cuzdanim.Application.Features.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IncludeInTotalBalance { get; set; }
}