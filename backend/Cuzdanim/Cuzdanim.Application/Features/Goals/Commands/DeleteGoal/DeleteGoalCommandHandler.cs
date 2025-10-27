using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using MediatR;

namespace Cuzdanim.Application.Features.Goals.Commands.DeleteGoal;

public class DeleteGoalCommandHandler : IRequestHandler<DeleteGoalCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGoalCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteGoalCommand request, CancellationToken cancellationToken)
    {
        // 1. Hedefi bul
        var goal = await _unitOfWork.Goals.GetByIdAsync(request.Id, cancellationToken);

        if (goal == null)
        {
            return Result<bool>.Failure("Hedef bulunamadı");
        }

        // 2. Yetki kontrolü
        if (goal.UserId != request.UserId)
        {
            return Result<bool>.Failure("Bu hedefi silme yetkiniz yok");
        }

        // 3. Soft delete
        goal.MarkAsDeleted();

        // 4. Kaydet
        _unitOfWork.Goals.Update(goal);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, "Hedef başarıyla silindi");
    }
}