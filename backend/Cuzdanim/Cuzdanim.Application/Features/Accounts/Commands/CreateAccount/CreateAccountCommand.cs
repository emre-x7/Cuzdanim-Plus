using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.Enums;
using MediatR;

namespace Cuzdanim.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommand : IRequest<Result<Guid>>
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public AccountType Type { get; set; }
    public decimal InitialBalance { get; set; }
    public Currency Currency { get; set; } = Currency.TRY;
    public string? BankName { get; set; }
    public string? IBAN { get; set; }
    public string? CardLastFourDigits { get; set; }
    public decimal? CreditLimit { get; set; }
}