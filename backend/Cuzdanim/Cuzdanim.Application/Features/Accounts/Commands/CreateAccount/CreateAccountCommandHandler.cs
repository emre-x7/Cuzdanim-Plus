using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.Entities;
using Cuzdanim.Domain.ValueObjects;
using MediatR;

namespace Cuzdanim.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        // 1. Kullanıcı var mı kontrol et
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result<Guid>.Failure("Kullanıcı bulunamadı");
        }

        // 2. Money Value Object oluştur
        var initialBalance = new Money(request.InitialBalance, request.Currency);

        // 3. Account entity oluştur
        var account = Account.Create(
            request.UserId,
            request.Name,
            request.Type,
            initialBalance,
            request.BankName,
            request.IBAN
        );

        // 4. Kart bilgilerini set et
        if (!string.IsNullOrEmpty(request.CardLastFourDigits))
        {
            // Reflection ile private property set etme (Entity'de public setter olmadığı için)
            var property = typeof(Account).GetProperty("CardLastFourDigits");
            property?.SetValue(account, request.CardLastFourDigits);
        }

        // 5. Kredi kartı limitini set et
        if (request.CreditLimit.HasValue)
        {
            account.SetCreditLimit(request.CreditLimit.Value);
        }

        // 6. Veritabanına kaydet
        await _unitOfWork.Accounts.AddAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(account.Id, "Hesap başarıyla oluşturuldu");
    }
}