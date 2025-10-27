using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.ValueObjects;
using MediatR;

namespace Cuzdanim.Application.Features.Transactions.Commands.UpdateTransaction;

public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTransactionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        // 1. İşlemi bul
        var transaction = await _unitOfWork.Transactions.GetByIdAsync(request.Id, cancellationToken);

        if (transaction == null)
        {
            return Result<Guid>.Failure("İşlem bulunamadı");
        }

        // 2. Yetki kontrolü
        if (transaction.UserId != request.UserId)
        {
            return Result<Guid>.Failure("Bu işlemi güncelleme yetkiniz yok");
        }

        // 3. Kategori kontrolü
        var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            return Result<Guid>.Failure("Kategori bulunamadı");
        }

        // 4. Money Value Object oluştur
        var amount = new Money(request.Amount, request.Currency);

        // 5. Transaction güncelle (Domain method kullan)
        transaction.Update(
            request.CategoryId,
            amount,
            request.TransactionDate,
            request.Description,
            request.Notes
        );

        // 6. Kaydet
        _unitOfWork.Transactions.Update(transaction);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(transaction.Id, "İşlem başarıyla güncellendi");
    }
}