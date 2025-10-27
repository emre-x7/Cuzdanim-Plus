using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.ValueObjects;
using MediatR;

namespace Cuzdanim.Application.Features.Goals.Commands.UpdateGoal;

public class UpdateGoalCommandHandler : IRequestHandler<UpdateGoalCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGoalCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(UpdateGoalCommand request, CancellationToken cancellationToken)
    {
        // 1. Hedefi bul
        var goal = await _unitOfWork.Goals.GetByIdAsync(request.Id, cancellationToken);

        if (goal == null)
        {
            return Result<Guid>.Failure("Hedef bulunamadı");
        }

        // 2. Yetki kontrolü
        if (goal.UserId != request.UserId)
        {
            return Result<Guid>.Failure("Bu hedefi güncelleme yetkiniz yok");
        }

        // 3. Money Value Object oluştur
        var targetAmount = new Money(request.TargetAmount, request.Currency);

        // 4. Goal güncelle (Domain method kullan)
        goal.Update(request.Name, request.Description, targetAmount, request.TargetDate);

        // 5. Kaydet
        _unitOfWork.Goals.Update(goal);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(goal.Id, "Hedef başarıyla güncellendi");
    }
}