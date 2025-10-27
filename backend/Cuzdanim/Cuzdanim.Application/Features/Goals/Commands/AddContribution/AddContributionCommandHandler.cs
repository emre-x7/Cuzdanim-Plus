using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.ValueObjects;
using MediatR;

namespace Cuzdanim.Application.Features.Goals.Commands.AddContribution;

public class AddContributionCommandHandler : IRequestHandler<AddContributionCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddContributionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(AddContributionCommand request, CancellationToken cancellationToken)
    {
        // 1. Hedefi bul
        var goal = await _unitOfWork.Goals.GetByIdAsync(request.GoalId, cancellationToken);

        if (goal == null)
        {
            return Result<Guid>.Failure("Hedef bulunamadı");
        }

        // 2. Yetki kontrolü
        if (goal.UserId != request.UserId)
        {
            return Result<Guid>.Failure("Bu hedefe katkı ekleme yetkiniz yok");
        }

        // 3. Money Value Object oluştur
        var amount = new Money(request.Amount, request.Currency);

        // 4. Katkı ekle (Domain method)
        try
        {
            goal.AddContribution(amount);
        }
        catch (InvalidOperationException ex)
        {
            return Result<Guid>.Failure(ex.Message);
        }

        // 5. Kaydet
        _unitOfWork.Goals.Update(goal);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var message = goal.Status == Domain.Enums.GoalStatus.Completed
            ? "Tebrikler! Hedef tamamlandı!"
            : "Katkı başarıyla eklendi";

        return Result<Guid>.Success(goal.Id, message);
    }
}