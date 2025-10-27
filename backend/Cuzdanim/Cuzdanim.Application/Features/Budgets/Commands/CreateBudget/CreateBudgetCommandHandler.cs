using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.Entities;
using Cuzdanim.Domain.ValueObjects;
using MediatR;

namespace Cuzdanim.Application.Features.Budgets.Commands.CreateBudget;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBudgetCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        // 1. Kullanıcı kontrolü
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result<Guid>.Failure("Kullanıcı bulunamadı");
        }

        // 2. Kategori kontrolü
        var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            return Result<Guid>.Failure("Kategori bulunamadı");
        }

        // 3. Aynı kategori ve dönem için bütçe var mı kontrol et
        var existingBudget = await _unitOfWork.Budgets.GetByCategoryAndPeriodAsync(
            request.UserId,
            request.CategoryId,
            request.StartDate,
            cancellationToken);

        if (existingBudget != null)
        {
            return Result<Guid>.Failure("Bu kategori için seçilen dönemde zaten bir bütçe tanımlı");
        }

        // 4. Value Objects oluştur
        var amount = new Money(request.Amount, request.Currency);
        var period = new DateRange(request.StartDate, request.EndDate);

        // 5. Budget entity oluştur
        var budget = Budget.Create(
            request.UserId,
            request.CategoryId,
            request.Name,
            amount,
            period
        );

        // 6. Uyarı eşiğini set et
        budget.SetAlertThreshold(request.AlertThresholdPercentage);

        // 7. Veritabanına kaydet
        await _unitOfWork.Budgets.AddAsync(budget, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(budget.Id, "Bütçe başarıyla oluşturuldu");
    }
}