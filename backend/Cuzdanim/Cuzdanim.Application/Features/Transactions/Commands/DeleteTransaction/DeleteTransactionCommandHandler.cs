using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using MediatR;

namespace Cuzdanim.Application.Features.Transactions.Commands.DeleteTransaction;

public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTransactionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        // 1. İşlemi bul
        var transaction = await _unitOfWork.Transactions.GetByIdAsync(request.Id, cancellationToken);

        if (transaction == null)
        {
            return Result<bool>.Failure("İşlem bulunamadı");
        }

        // 2. Yetki kontrolü
        if (transaction.UserId != request.UserId)
        {
            return Result<bool>.Failure("Bu işlemi silme yetkiniz yok");
        }

        // 3. Soft delete
        transaction.MarkAsDeleted();

        // 4. Kaydet
        _unitOfWork.Transactions.Update(transaction);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, "İşlem başarıyla silindi");
    }
}