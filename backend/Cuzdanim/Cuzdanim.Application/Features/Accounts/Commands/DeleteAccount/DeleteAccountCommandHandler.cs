using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using MediatR;

namespace Cuzdanim.Application.Features.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAccountCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        // 1. Hesabı bul
        var account = await _unitOfWork.Accounts.GetByIdAsync(request.Id, cancellationToken);

        if (account == null)
        {
            return Result<bool>.Failure("Hesap bulunamadı");
        }

        // 2. Yetki kontrolü
        if (account.UserId != request.UserId)
        {
            return Result<bool>.Failure("Bu hesabı silme yetkiniz yok");
        }

        // 3. Soft delete (BaseEntity'deki method)
        account.MarkAsDeleted();

        // 4. Kaydet
        _unitOfWork.Accounts.Update(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, "Hesap başarıyla silindi");
    }
}