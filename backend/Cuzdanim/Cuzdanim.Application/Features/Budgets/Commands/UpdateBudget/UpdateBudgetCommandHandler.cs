using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.ValueObjects;
using MediatR;

namespace Cuzdanim.Application.Features.Budgets.Commands.UpdateBudget;

public class UpdateBudgetCommandHandler : IRequestHandler<UpdateBudgetCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBudgetCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(UpdateBudgetCommand request, CancellationToken cancellationToken)
    {
        // 1. Bütçeyi bul
        var budget = await _unitOfWork.Budgets.GetByIdAsync(request.Id, cancellationToken);

        if (budget == null)
        {
            return Result<Guid>.Failure("Bütçe bulunamadı");
        }

        // 2. Yetki kontrolü
        if (budget.UserId != request.UserId)
        {
            return Result<Guid>.Failure("Bu bütçeyi güncelleme yetkiniz yok");
        }

        // 3. Value Objects oluştur
        var amount = new Money(request.Amount, request.Currency);
        var period = new DateRange(request.StartDate, request.EndDate);

        // 4. Bütçe güncelle (Domain method kullan)
        budget.Update(request.Name, amount, period);
        budget.SetAlertThreshold(request.AlertThresholdPercentage);

        // 5. Kaydet
        _unitOfWork.Budgets.Update(budget);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(budget.Id, "Bütçe başarıyla güncellendi");
    }
}