using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.Entities;
using Cuzdanim.Domain.Enums;
using Cuzdanim.Domain.ValueObjects;
using MediatR;

namespace Cuzdanim.Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransactionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        // 1. Kullanıcı kontrolü
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result<Guid>.Failure("Kullanıcı bulunamadı");
        }

        // 2. Hesap kontrolü ve yetki kontrolü
        var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId, cancellationToken);
        if (account == null)
        {
            return Result<Guid>.Failure("Hesap bulunamadı");
        }

        if (account.UserId != request.UserId)
        {
            return Result<Guid>.Failure("Bu hesaba erişim yetkiniz yok");
        }

        // 3. Kategori kontrolü
        var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            return Result<Guid>.Failure("Kategori bulunamadı");
        }

        // 4. Money Value Object oluştur
        var amount = new Money(request.Amount, request.Currency);

        // 5. Transaction tipine göre entity oluştur
        Transaction transaction;

        if (request.Type == TransactionType.Expense)
        {
            transaction = Transaction.CreateExpense(
                request.UserId,
                request.AccountId,
                request.CategoryId,
                amount,
                request.TransactionDate,
                request.Description,
                request.Notes
            );

            // Hesap bakiyesinden düş
            try
            {
                account.Withdraw(amount);
            }
            catch (InvalidOperationException ex)
            {
                return Result<Guid>.Failure(ex.Message);
            }
        }
        else if (request.Type == TransactionType.Income)
        {
            transaction = Transaction.CreateIncome(
                request.UserId,
                request.AccountId,
                request.CategoryId,
                amount,
                request.TransactionDate,
                request.Description,
                request.Notes
            );

            // Hesap bakiyesine ekle
            account.Deposit(amount);
        }
        else
        {
            return Result<Guid>.Failure("Transfer işlemleri için ayrı endpoint kullanınız");
        }

        // 6. Makbuz URL'si varsa ekle
        if (!string.IsNullOrEmpty(request.ReceiptUrl))
        {
            transaction.AttachReceipt(request.ReceiptUrl);
        }

        // 7. Etiketler varsa ekle
        if (request.Tags != null && request.Tags.Any())
        {
            transaction.AddTags(request.Tags.ToArray());
        }

        // 8. Transaction başlat (hesap bakiyesi + transaction atomik olmalı)
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _unitOfWork.Transactions.AddAsync(transaction, cancellationToken);
            _unitOfWork.Accounts.Update(account);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return Result<Guid>.Success(transaction.Id, "İşlem başarıyla kaydedildi");
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}