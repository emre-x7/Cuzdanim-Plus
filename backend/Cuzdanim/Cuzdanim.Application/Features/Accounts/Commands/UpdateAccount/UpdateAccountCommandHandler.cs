using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using MediatR;

namespace Cuzdanim.Application.Features.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAccountCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        // 1. Hesabı bul
        var account = await _unitOfWork.Accounts.GetByIdAsync(request.Id, cancellationToken);

        if (account == null)
        {
            return Result<Guid>.Failure("Hesap bulunamadı");
        }

        // 2. Yetki kontrolü (sadece kendi hesabını güncelleyebilir)
        if (account.UserId != request.UserId)
        {
            return Result<Guid>.Failure("Bu hesabı güncelleme yetkiniz yok");
        }

        // 3. Hesap bilgilerini güncelle (Domain method kullanarak olabilir, ama şimdilik basit)
        // Account entity'de UpdateDetails() methodu eklenebilir
        // Şimdilik reflection ile (geçici çözüm)
        var nameProperty = typeof(Domain.Entities.Account).GetProperty("Name");
        nameProperty?.SetValue(account, request.Name);

        var isActiveProperty = typeof(Domain.Entities.Account).GetProperty("IsActive");
        isActiveProperty?.SetValue(account, request.IsActive);

        var includeProperty = typeof(Domain.Entities.Account).GetProperty("IncludeInTotalBalance");
        includeProperty?.SetValue(account, request.IncludeInTotalBalance);

        account.MarkAsUpdated();

        // 4. Kaydet
        _unitOfWork.Accounts.Update(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(account.Id, "Hesap başarıyla güncellendi");
    }
}