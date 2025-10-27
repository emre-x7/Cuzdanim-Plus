using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using MediatR;

namespace Cuzdanim.Application.Features.Budgets.Commands.DeleteBudget;

public class DeleteBudgetCommandHandler : IRequestHandler<DeleteBudgetCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBudgetCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteBudgetCommand request, CancellationToken cancellationToken)
    {
        // 1. Bütçeyi bul
        var budget = await _unitOfWork.Budgets.GetByIdAsync(request.Id, cancellationToken);

        if (budget == null)
        {
            return Result<bool>.Failure("Bütçe bulunamadı");
        }

        // 2. Yetki kontrolü
        if (budget.UserId != request.UserId)
        {
            return Result<bool>.Failure("Bu bütçeyi silme yetkiniz yok");
        }

        // 3. Soft delete
        budget.MarkAsDeleted();

        // 4. Kaydet
        _unitOfWork.Budgets.Update(budget);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, "Bütçe başarıyla silindi");
    }
}